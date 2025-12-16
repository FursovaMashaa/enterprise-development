namespace BikeRental.Application.Contracts.Rental;

public interface IRentalService : IApplicationService<RentalDto, RentalCreateUpdateDto, int>
{
    public Task<IList<RentalDto>> GetRentalsByRenterAsync(int renterId);

    public Task<IList<RentalDto>> GetRentalsByBikeAsync(int bikeId);
}