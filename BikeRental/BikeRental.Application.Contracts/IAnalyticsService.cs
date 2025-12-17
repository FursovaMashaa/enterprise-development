using BikeRental.Application.Contracts.Bike;
using BikeRental.Application.Contracts.Renter;

namespace BikeRental.Application.Contracts;

/// <summary>
/// Service interface for business intelligence and analytics operations in the bike rental system.
/// Provides methods for generating various reports and statistical analyses.
/// </summary>
public interface IAnalyticsService
{
    /// <summary>
    /// Retrieves all sport-type bikes from the inventory
    /// </summary>
    /// <returns>List of sport bike DTOs</returns>
    public Task<IList<BikeDto>> GetAllSportBikesAsync();

    /// <summary>
    /// Calculates the top 5 bike models by total rental revenue
    /// </summary>
    /// <returns>List of model ID and revenue pairs, sorted by revenue descending</returns>
    public Task<IList<KeyValuePair<int, decimal>>> GetTopFiveModelsByRevenueAsync();

    /// <summary>
    /// Calculates the top 5 bike models by total rental duration
    /// </summary>
    /// <returns>List of model ID and total hours pairs, sorted by hours descending</returns>
    public Task<IList<KeyValuePair<int, int>>> GetTopFiveModelsByTotalDurationAsync();

    /// <summary>
    /// Calculates minimum, maximum, and average rental durations
    /// </summary>
    /// <returns>Tuple containing min, max, and average rental hours</returns>
    public Task<(int Min, int Max, double Avg)> GetMinMaxAvgRentDurationAsync();

    /// <summary>
    /// Calculates total rental hours for a specific bike category
    /// </summary>
    /// <param name="type">Bike type identifier (0: Sport, 1: Mountain, 2: Road, 3: Hybrid)</param>
    /// <returns>Total rental hours for the specified bike category</returns>
    public Task<int> GetTotalRentalTimeByTypeAsync(int type);

    /// <summary>
    /// Identifies top clients based on rental frequency (clients with maximum rental count)
    /// </summary>
    /// <returns>List of renter DTOs and their rental counts for top-performing clients</returns>
    public Task<IList<KeyValuePair<RenterDto, int>>> GetTopClientsByRentalCountAsync();
}