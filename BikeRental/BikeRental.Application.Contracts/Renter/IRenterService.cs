namespace BikeRental.Application.Contracts.Renter;

/// <summary>
/// Service interface for managing renter entities in the bike rental system.
/// Extends the base CRUD operations with renter-specific functionality.
/// </summary>
public interface IRenterService : IApplicationService<RenterDto, RenterCreateUpdateDto, int>
{
    /// <summary>
    /// Retrieves the total number of rental transactions for a specific renter
    /// </summary>
    /// <param name="renterId">Identifier of the renter</param>
    /// <returns>Count of rental transactions associated with the specified renter</returns>
    public Task<int> GetRenterRentalAsync(int renterId);
}