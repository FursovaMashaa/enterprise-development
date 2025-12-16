using BikeRental.Application.Contracts.Bike;
using BikeRental.Application.Contracts.Renter;

namespace BikeRental.Application.Contracts;

public interface IAnalyticsService
{
    public Task<IList<BikeDto>> GetAllSportBikesAsync();

    public Task<IList<KeyValuePair<int, decimal>>> GetTopFiveModelsByRevenueAsync();

    public Task<IList<KeyValuePair<int, int>>> GetTopFiveModelsByTotalDurationAsync();

    public Task<(int Min, int Max, double Avg)> GetMinMaxAvgRentDurationAsync();

    public Task<int> GetTotalRentalTimeByTypeAsync(int type);

    public Task<IList<KeyValuePair<RenterDto, int>>> GetTopClientsByRentalCountAsync();
}