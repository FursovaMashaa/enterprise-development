using AutoMapper;
using BikeRental.Application.Contracts.Bike;
using BikeRental.Domain;
using BikeRental.Domain.Models;

namespace BikeRental.Application.Services;

/// <summary>
/// Service implementation for managing bikes in the rental system.
/// Provides CRUD operations and specialized queries for bike entities.
/// </summary>
public class BikeService(
    IRepository<Bike, int> bikeRepository,    
    IRepository<BikeModel, int> modelRepository,
    IMapper mapper
) : IBikeService
{
    /// <summary>
    /// Creates a new bike
    /// </summary>
    /// <param name="dto">Data for creating the bike</param>
    /// <returns>The created bike DTO</returns>
    /// <exception cref="ArgumentException">Thrown when the specified model does not exist</exception>
    public async Task<BikeDto> Create(BikeCreateUpdateDto dto)
    {
        var allBikes = await bikeRepository.ReadAll();
        var maxId = allBikes.Any() ? allBikes.Max(b => b.Id) : 0;
        
        var model = await modelRepository.Read(dto.ModelId) ?? throw new ArgumentException($"Model with id {dto.ModelId} not found");

        var bike = mapper.Map<Bike>(dto);
        bike.Id = maxId + 1;
        bike.Model = model;

        var created = await bikeRepository.Create(bike);
        return mapper.Map<BikeDto>(created);
    }

    /// <summary>
    /// Deletes a bike by its identifier
    /// </summary>
    /// <param name="id">Bike identifier</param>
    /// <returns>True if deletion was successful, false otherwise</returns>
    public async Task<bool> Delete(int id)
    {
        return await bikeRepository.Delete(id);
    }

    /// <summary>
    /// Retrieves a specific bike by its identifier
    /// </summary>
    /// <param name="id">Bike identifier</param>
    /// <returns>The bike DTO or null if not found</returns>
    public async Task<BikeDto?> Get(int id)
    {
        var bike = await bikeRepository.Read(id);
        return bike != null ? mapper.Map<BikeDto>(bike) : null;
    }

    /// <summary>
    /// Retrieves all bikes
    /// </summary>
    /// <returns>List of all bike DTOs</returns>
    public async Task<IList<BikeDto>> GetAll()
    {
        var bikes = await bikeRepository.ReadAll();
        return mapper.Map<List<BikeDto>>(bikes);
    }

    /// <summary>
    /// Updates an existing bike
    /// </summary>
    /// <param name="dto">Updated data for the bike</param>
    /// <param name="id">Identifier of the bike to update</param>
    /// <returns>The updated bike DTO</returns>
    /// <exception cref="KeyNotFoundException">Thrown when the bike is not found</exception>
    /// <exception cref="ArgumentException">Thrown when the specified model does not exist</exception>
    public async Task<BikeDto> Update(BikeCreateUpdateDto dto, int id)
    {
        var bike = await bikeRepository.Read(id) ?? throw new KeyNotFoundException($"Bike {id} not found");
    
        var model = await modelRepository.Read(dto.ModelId) ?? throw new ArgumentException($"Model with id {dto.ModelId} not found");

        mapper.Map(dto, bike);
        bike.Model = model;

        var updated = await bikeRepository.Update(bike);
        return mapper.Map<BikeDto>(updated);
    }

    /// <summary>
    /// Retrieves bikes filtered by model
    /// </summary>
    /// <param name="modelId">Bike model identifier</param>
    /// <returns>List of bike DTOs belonging to the specified model</returns>
    public async Task<IList<BikeDto>> GetBikesByModelAsync(int modelId) 
    {
        var bikes = await bikeRepository.ReadAll();
        var filtered = bikes.Where(b => b.Model?.Id == modelId).ToList(); 
        return mapper.Map<List<BikeDto>>(filtered);
    }
}