using AutoMapper;
using BikeRental.Application.Contracts;
using BikeRental.Application.Contracts.Bike;
using BikeRental.Application.Contracts.Renter;
using BikeRental.Domain;
using BikeRental.Domain.Enums;
using BikeRental.Domain.Models;

namespace BikeRental.Application.Services;

/// <summary>
/// Service implementation for analytics and reporting operations in the bike rental system.
/// Provides methods for generating business intelligence reports and statistical analysis.
/// </summary>
public class AnalyticsService(
    IRepository<Bike, int> bikeRepository,
    IRepository<Rental, int> rentalRepository,
    IRepository<BikeModel, int> modelRepository,
    IRepository<Renter, int> renterRepository,
    IMapper mapper
) : IAnalyticsService
{
    /// <summary>
    /// Retrieves all sport-type bikes from the inventory
    /// </summary>
    /// <returns>List of sport bike DTOs</returns>
    public async Task<IList<BikeDto>> GetAllSportBikesAsync()
    {
        var bikes = await bikeRepository.ReadAll();
        var models = await modelRepository.ReadAll();
        
        // Find sport model IDs
        var sportModelIds = models
            .Where(m => m.BikeType == BikeType.Sport)
            .Select(m => m.Id)
            .ToList();
            
        var sportBikes = bikes
            .Where(b => sportModelIds.Contains(b.ModelId))
            .ToList(); 
            
        return mapper.Map<List<BikeDto>>(sportBikes);
    }

    /// <summary>
    /// Calculates the top 5 bike models by total rental revenue
    /// </summary>
    /// <returns>List of model ID and revenue pairs, sorted by revenue descending</returns>
    public async Task<IList<KeyValuePair<int, decimal>>> GetTopFiveModelsByRevenueAsync()
    {
        var rentals = await rentalRepository.ReadAll();
        var bikes = await bikeRepository.ReadAll();
        var models = await modelRepository.ReadAll();

        var result = rentals
            .Select(r => new 
            {
                Rental = r,
                Bike = bikes.FirstOrDefault(b => b.Id == r.BikeId),
                Model = models.FirstOrDefault(m => m.Id == bikes.FirstOrDefault(b => b.Id == r.BikeId)?.ModelId)
            })
            .Where(x => x.Model != null)
            .GroupBy(x => x.Model!.Id)  
            .Select(g => new
            {
                ModelId = g.Key, 
                Revenue = g.Sum(x => x.Rental.DurationHours * x.Model!.PricePerHour)
            })
            .OrderByDescending(x => x.Revenue)
            .ThenBy(x => x.ModelId)
            .Take(5)
            .Select(x => new KeyValuePair<int, decimal>(x.ModelId, Math.Round(x.Revenue, 2)))
            .ToList();

        return result;
    }

    /// <summary>
    /// Calculates the top 5 bike models by total rental duration
    /// </summary>
    /// <returns>List of model ID and total hours pairs, sorted by hours descending</returns>
    public async Task<IList<KeyValuePair<int, int>>> GetTopFiveModelsByTotalDurationAsync()
    {
        var rentals = await rentalRepository.ReadAll();
        var bikes = await bikeRepository.ReadAll();

        return rentals
            .Select(r => new 
            {
                Rental = r,
                Bike = bikes.FirstOrDefault(b => b.Id == r.BikeId)
            })
            .Where(x => x.Bike != null)
            .GroupBy(x => x.Bike!.ModelId)
            .Select(g => new KeyValuePair<int, int>(
                g.Key, 
                g.Sum(x => x.Rental.DurationHours)
            ))
            .OrderByDescending(x => x.Value)
            .ThenBy(x => x.Key)
            .Take(5)
            .ToList();
    }

    /// <summary>
    /// Calculates minimum, maximum, and average rental durations
    /// </summary>
    /// <returns>Tuple containing min, max, and average rental hours</returns>
    public async Task<(int Min, int Max, double Avg)> GetMinMaxAvgRentDurationAsync()
    {
        var rentals = await rentalRepository.ReadAll();
        if (!rentals.Any())
            return (0, 0, 0);

        var durations = rentals.Select(r => r.DurationHours).ToArray();
        return (durations.Min(), durations.Max(), Math.Round(durations.Average(), 2));
    }

    /// <summary>
    /// Calculates total rental hours for a specific bike category
    /// </summary>
    /// <param name="type">Bike type identifier (as integer corresponding to BikeType enum)</param>
    /// <returns>Total rental hours for the specified bike category</returns>
    public async Task<int> GetTotalRentalTimeByTypeAsync(int type)
    {
        var bikeType = (BikeType)type;
        var rentals = await rentalRepository.ReadAll();
        var bikes = await bikeRepository.ReadAll();
        var models = await modelRepository.ReadAll();

        // Find model IDs of the specified type
        var modelIds = models
            .Where(m => m.BikeType == bikeType)
            .Select(m => m.Id)
            .ToList();

        // Find bikes of those models
        var bikeIds = bikes
            .Where(b => modelIds.Contains(b.ModelId))
            .Select(b => b.Id)
            .ToList();

        // Sum rentals of those bikes
        return rentals
            .Where(r => bikeIds.Contains(r.BikeId))
            .Sum(r => r.DurationHours);
    }

    /// <summary>
    /// Identifies top clients based on rental frequency (clients with maximum rental count)
    /// </summary>
    /// <returns>List of renter DTOs and their rental counts for top-performing clients</returns>
    public async Task<IList<KeyValuePair<RenterDto, int>>> GetTopClientsByRentalCountAsync()
    {
        var rentals = await rentalRepository.ReadAll();
        var renters = await renterRepository.ReadAll();
        
        if (!rentals.Any())
            return [];

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
                mapper.Map<RenterDto>(x.Renter),
                x.Count
            ))
            .ToList();

        return leaders;
    }
}