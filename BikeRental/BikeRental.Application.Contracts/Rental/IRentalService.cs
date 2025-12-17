namespace BikeRental.Application.Contracts.Rental;

/// <summary>
/// Service interface for managing rental transactions in the bike rental system.
/// Extends the base CRUD operations with rental-specific query functionality.
/// </summary>
public interface IRentalService : IApplicationService<RentalDto, RentalCreateUpdateDto, int>
{
    /// <summary>
    /// Retrieves all rental transactions for a specific renter
    /// </summary>
    /// <param name="renterId">Identifier of the renter</param>
    /// <returns>List of rental DTOs associated with the specified renter</returns>
    public Task<IList<RentalDto>> GetRentalsByRenterAsync(int renterId);

    /// <summary>
    /// Retrieves all rental transactions for a specific bike
    /// </summary>
    /// <param name="bikeId">Identifier of the bike</param>
    /// <returns>List of rental DTOs associated with the specified bike</returns>
    public Task<IList<RentalDto>> GetRentalsByBikeAsync(int bikeId);
}