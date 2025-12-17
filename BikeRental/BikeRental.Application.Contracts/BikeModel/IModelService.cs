using BikeRental.Application.Contracts.Bike;

namespace BikeRental.Application.Contracts.BikeModel;

/// <summary>
/// Service interface for managing bike model entities in the rental system.
/// Extends the base CRUD operations with bike model-specific functionality.
/// </summary>
public interface IBikeModelService : IApplicationService<BikeModelDto, BikeModelCreateUpdateDto, int>
{
    /// <summary>
    /// Retrieves all bikes belonging to a specific bike model
    /// </summary>
    /// <param name="modelId">Identifier of the bike model</param>
    /// <returns>List of bike DTOs belonging to the specified model</returns>
    public Task<IList<BikeDto>> GetBikesAsync(int modelId);
}