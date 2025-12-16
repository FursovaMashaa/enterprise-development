using AutoMapper;
using BikeRental.Application.Contracts;
using BikeRental.Application.Contracts.Bike;
using BikeRental.Application.Contracts.Renter;
using BikeRental.Domain;
using BikeRental.Domain.Enums;
using BikeRental.Domain.Models;

namespace BikeRental.Application.Services;

public class AnalyticsService : IAnalyticsService
{
    private readonly IRepository<Bike, int> _bikeRepository;     
    private readonly IRepository<Rental, int> _rentalRepository;
    private readonly IRepository<BikeModel, int> _modelRepository;
    private readonly IRepository<Renter, int> _renterRepository;
    private readonly IMapper _mapper;

    public AnalyticsService(
        IRepository<Bike, int> bikeRepository,      
        IRepository<Rental, int> rentalRepository,
        IRepository<BikeModel, int> modelRepository,
        IRepository<Renter, int> renterRepository,
        IMapper mapper)
    {
        _bikeRepository = bikeRepository;
        _rentalRepository = rentalRepository;
        _modelRepository = modelRepository;
        _renterRepository = renterRepository;
        _mapper = mapper;
    }

    public async Task<IList<BikeDto>> GetAllSportBikesAsync()
    {
        var bikes = await _bikeRepository.ReadAll();
        var models = await _modelRepository.ReadAll();
        
        // Находим ID спортивных моделей
        var sportModelIds = models
            .Where(m => m.BikeType == BikeType.Sport)
            .Select(m => m.Id)
            .ToList();
            
        var sportBikes = bikes
            .Where(b => sportModelIds.Contains(b.ModelId))
            .ToList();
            
        return _mapper.Map<List<BikeDto>>(sportBikes);
    }

    public async Task<IList<KeyValuePair<int, decimal>>> GetTopFiveModelsByRevenueAsync()
    {
        var rentals = await _rentalRepository.ReadAll();
        var bikes = await _bikeRepository.ReadAll();
        var models = await _modelRepository.ReadAll();

        var result = rentals
            .Select(r => new 
            {
                Rental = r,
                Bike = bikes.FirstOrDefault(b => b.Id == r.BikeId),
                Model = models.FirstOrDefault(m => m.Id == bikes.FirstOrDefault(b => b.Id == r.BikeId)?.ModelId)
            })
            .Where(x => x.Model != null)
            .GroupBy(x => x.Model.Id)
            .Select(g => new
            {
                ModelId = g.Key, 
                Revenue = g.Sum(x => x.Rental.DurationHours * x.Model.PricePerHour)
            })
            .OrderByDescending(x => x.Revenue)
            .ThenBy(x => x.ModelId)
            .Take(5)
            .Select(x => new KeyValuePair<int, decimal>(x.ModelId, Math.Round(x.Revenue, 2)))
            .ToList();

        return result;
    }

    public async Task<IList<KeyValuePair<int, int>>> GetTopFiveModelsByTotalDurationAsync()
    {
        var rentals = await _rentalRepository.ReadAll();
        var bikes = await _bikeRepository.ReadAll();

        return rentals
            .Select(r => new 
            {
                Rental = r,
                Bike = bikes.FirstOrDefault(b => b.Id == r.BikeId)
            })
            .Where(x => x.Bike != null)
            .GroupBy(x => x.Bike.ModelId)
            .Select(g => new KeyValuePair<int, int>(
                g.Key, 
                g.Sum(x => x.Rental.DurationHours)
            ))
            .OrderByDescending(x => x.Value)
            .ThenBy(x => x.Key)
            .Take(5)
            .ToList();
    }

    public async Task<(int Min, int Max, double Avg)> GetMinMaxAvgRentDurationAsync()
    {
        var rentals = await _rentalRepository.ReadAll();
        if (!rentals.Any())
            return (0, 0, 0);

        var durations = rentals.Select(r => r.DurationHours).ToArray();
        return (durations.Min(), durations.Max(), Math.Round(durations.Average(), 2));
    }

    public async Task<int> GetTotalRentalTimeByTypeAsync(int type)
    {
        var bikeType = (BikeType)type;
        var rentals = await _rentalRepository.ReadAll();
        var bikes = await _bikeRepository.ReadAll();
        var models = await _modelRepository.ReadAll();

        // Находим ID моделей нужного типа
        var modelIds = models
            .Where(m => m.BikeType == bikeType)
            .Select(m => m.Id)
            .ToList();

        // Находим велосипеды этих моделей
        var bikeIds = bikes
            .Where(b => modelIds.Contains(b.ModelId))
            .Select(b => b.Id)
            .ToList();

        // Суммируем аренды этих велосипедов
        return rentals
            .Where(r => bikeIds.Contains(r.BikeId))
            .Sum(r => r.DurationHours);
    }

    public async Task<IList<KeyValuePair<RenterDto, int>>> GetTopClientsByRentalCountAsync()
    {
        var rentals = await _rentalRepository.ReadAll();
        var renters = await _renterRepository.ReadAll();
        
        if (!rentals.Any())
            return new List<KeyValuePair<RenterDto, int>>();

        var grouped = rentals
            .GroupBy(r => r.RenterId)
            .Select(g => new 
            { 
                RenterId = g.Key, 
                Count = g.Count() 
            })
            .ToList();

        var maxCount = grouped.Max(g => g.Count);

        var leaders = grouped
            .Where(x => x.Count == maxCount)
            .Select(x => new 
            {
                Renter = renters.FirstOrDefault(r => r.Id == x.RenterId),
                Count = x.Count
            })
            .Where(x => x.Renter != null)
            .Select(x => new KeyValuePair<RenterDto, int>(
                _mapper.Map<RenterDto>(x.Renter),
                x.Count
            ))
            .ToList();

        return leaders;
    }
}