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
    private readonly IMapper _mapper;

    public AnalyticsService(
        IRepository<Bike, int> bikeRepository,      
        IRepository<Rental, int> rentalRepository,  
        IMapper mapper)
    {
        _bikeRepository = bikeRepository;
        _rentalRepository = rentalRepository;
        _mapper = mapper;
    }

    public async Task<IList<BikeDto>> GetAllSportBikesAsync()
    {
        var bikes = await _bikeRepository.ReadAll();
        var sportBikes = bikes.Where(b => b.Model.BikeType == BikeType.Sport).ToList();
        return _mapper.Map<List<BikeDto>>(sportBikes);
    }

    public async Task<IList<KeyValuePair<int, decimal>>> GetTopFiveModelsByRevenueAsync()
    {
        var rentals = await _rentalRepository.ReadAll();

        var result = rentals
            .GroupBy(r => r.Bike.Model.Id) 
            .Select(g => new
            {
                ModelId = g.Key, 
                Revenue = g.Sum(r => r.DurationHours * r.Bike.Model.PricePerHour)
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

        return rentals
            .GroupBy(r => r.Bike.Model.Id)
            .Select(g => new KeyValuePair<int, int>(
                g.Key, 
                g.Sum(r => r.DurationHours) 
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
        return rentals
            .Where(r => r.Bike.Model.BikeType == bikeType)
            .Sum(r => r.DurationHours);
    }

    public async Task<IList<KeyValuePair<RenterDto, int>>> GetTopClientsByRentalCountAsync()
    {
        var rentals = await _rentalRepository.ReadAll();
        if (!rentals.Any())
            return new List<KeyValuePair<RenterDto, int>>();

        var grouped = rentals
            .GroupBy(r => r.Renter)
            .Select(g => new { Renter = g.Key, Count = g.Count() })
            .ToList();

        var maxCount = grouped.Max(g => g.Count);

        var leaders = grouped
            .Where(x => x.Count == maxCount)
            .Select(x => new KeyValuePair<RenterDto, int>(
                _mapper.Map<RenterDto>(x.Renter),
                x.Count
            ))
            .ToList();

        return leaders;
    }
}