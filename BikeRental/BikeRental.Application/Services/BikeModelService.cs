using AutoMapper;
using BikeRental.Application.Contracts.Bike;
using BikeRental.Application.Contracts.BikeModel;
using BikeRental.Domain;
using BikeRental.Domain.Enums;
using BikeRental.Domain.Models;

namespace BikeRental.Application.Services;

/// <summary>
/// Service implementation for managing bike models in the rental system.
/// Provides CRUD operations and specialized queries for bike model entities.
/// </summary>
public class BikeModelService(
    IRepository<BikeModel, int> modelRepository,
    IRepository<Bike, int> bikeRepository, 
    IMapper mapper
) : IBikeModelService
{
    /// <summary>
    /// Creates a new bike model
    /// </summary>
    /// <param name="dto">Data for creating the bike model</param>
    /// <returns>The created bike model DTO</returns>
    public async Task<BikeModelDto> Create(BikeModelCreateUpdateDto dto)
    {
    var allModels = await modelRepository.ReadAll();
    var maxId = allModels.Any() ? allModels.Max(m => m.Id) : 0;
    
    var model = mapper.Map<BikeModel>(dto);
    model.Id = maxId + 1; 

    var created = await modelRepository.Create(model);
    return mapper.Map<BikeModelDto>(created);
    }

    /// <summary>
    /// Deletes a bike model by its identifier
    /// </summary>
    /// <param name="id">Bike model identifier</param>
    /// <returns>True if deletion was successful, false otherwise</returns>
    public async Task<bool> Delete(int id) 
    {
        return await modelRepository.Delete(id);
    }

    /// <summary>
    /// Retrieves a specific bike model by its identifier
    /// </summary>
    /// <param name="id">Bike model identifier</param>
    /// <returns>The bike model DTO or null if not found</returns>
    public async Task<BikeModelDto?> Get(int id) 
    {
        var model = await modelRepository.Read(id);
        return model != null ? mapper.Map<BikeModelDto>(model) : null;
    }

    /// <summary>
    /// Retrieves all bike models
    /// </summary>
    /// <returns>List of all bike model DTOs</returns>
    public async Task<IList<BikeModelDto>> GetAll()
    {
        var models = await modelRepository.ReadAll();
        return mapper.Map<List<BikeModelDto>>(models);
    }

    /// <summary>
    /// Updates an existing bike model
    /// </summary>
    /// <param name="dto">Updated data for the bike model</param>
    /// <param name="id">Identifier of the bike model to update</param>
    /// <returns>The updated bike model DTO</returns>
    /// <exception cref="KeyNotFoundException">Thrown when the bike model is not found</exception>
    public async Task<BikeModelDto> Update(BikeModelCreateUpdateDto dto, int id) 
    {
        var model = await modelRepository.Read(id) ?? throw new KeyNotFoundException($"BikeModel {id} not found");

        mapper.Map(dto, model);
        var updated = await modelRepository.Update(model);
        return mapper.Map<BikeModelDto>(updated);
    }

    /// <summary>
    /// Retrieves bike models filtered by bike type
    /// </summary>
    /// <param name="bikeType">Type of bike to filter by</param>
    /// <returns>List of bike model DTOs of the specified type</returns>
    public async Task<IList<BikeModelDto>> GetModelsByTypeAsync(BikeType bikeType)
    {
        var models = await modelRepository.ReadAll();
        var filtered = models.Where(m => m.BikeType == bikeType).ToList();
        return mapper.Map<List<BikeModelDto>>(filtered);
    }

    /// <summary>
    /// Retrieves bike models filtered by model year
    /// </summary>
    /// <param name="modelYear">Year to filter by (null returns all models)</param>
    /// <returns>List of bike model DTOs from the specified year</returns>
    public async Task<IList<BikeModelDto>> GetModelsByYearAsync(int? modelYear) 
    {
        var models = await modelRepository.ReadAll();
        var filtered = models.Where(m => m.ModelYear == modelYear).ToList();
        return mapper.Map<List<BikeModelDto>>(filtered);
    }

    /// <summary>
    /// Retrieves all bikes belonging to a specific model
    /// </summary>
    /// <param name="modelId">Bike model identifier</param>
    /// <returns>List of bike DTOs belonging to the specified model</returns>
    public async Task<IList<BikeDto>> GetBikesAsync(int modelId) 
    {
        var bikes = await bikeRepository.ReadAll();
        var filtered = bikes.Where(b => b.ModelId == modelId).ToList();
        return mapper.Map<List<BikeDto>>(filtered);
    }
}