namespace BikeRental.Application.Contracts.Renter;

public interface IRenterService : IApplicationService<RenterDto, RenterCreateUpdateDto, int>
{
    public Task<int> GetRenterRentalAsync(int renterId);
}