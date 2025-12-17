namespace BikeRental.Application.Contracts.Bike;

/// <summary>
/// Service interface for managing bike entities in the rental system.
/// Extends the base CRUD operations with bike-specific functionality.
/// </summary>
public interface IBikeService : IApplicationService<BikeDto, BikeCreateUpdateDto, int>
{
    /// <summary>
    /// Retrieves all bikes belonging to a specific model
    /// </summary>
    /// <param name="modelId">Identifier of the bike model</param>
    /// <returns>List of bike DTOs belonging to the specified model</returns>
    public Task<IList<BikeDto>> GetBikesByModelAsync(int modelId);
}