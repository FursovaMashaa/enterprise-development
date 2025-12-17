using AutoMapper;
using BikeRental.Application.Contracts.Bike;
using BikeRental.Domain;
using BikeRental.Domain.Models;

namespace BikeRental.Application.Services;

/// <summary>
/// Service implementation for managing bikes in the rental system.
/// Provides CRUD operations and specialized queries for bike entities.
/// </summary>
public class BikeService : IBikeService
{
    private readonly IRepository<Bike, int> _bikeRepository;   
    private readonly IRepository<BikeModel, int> _modelRepository;
    private readonly IMapper _mapper;

    /// <summary>
    /// Initializes a new instance of the <see cref="BikeService"/> class
    /// </summary>
    /// <param name="bikeRepository">Repository for bike entities</param>
    /// <param name="modelRepository">Repository for bike model entities</param>
    /// <param name="mapper">AutoMapper instance for object mapping</param>
    public BikeService(
        IRepository<Bike, int> bikeRepository,    
        IRepository<BikeModel, int> modelRepository,
        IMapper mapper)
    {
        _bikeRepository = bikeRepository;
        _modelRepository = modelRepository;
        _mapper = mapper;
    }

    /// <summary>
    /// Creates a new bike
    /// </summary>
    /// <param name="dto">Data for creating the bike</param>
    /// <returns>The created bike DTO</returns>
    /// <exception cref="ArgumentException">Thrown when the specified model does not exist</exception>
    public async Task<BikeDto> Create(BikeCreateUpdateDto dto)
    {
        var model = await _modelRepository.Read(dto.ModelId); 
        if (model == null)
            throw new ArgumentException($"Model with id {dto.ModelId} not found");

        var bike = _mapper.Map<Bike>(dto);
        bike.Model = model;

        var created = await _bikeRepository.Create(bike);
        return _mapper.Map<BikeDto>(created);
    }

    /// <summary>
    /// Deletes a bike by its identifier
    /// </summary>
    /// <param name="id">Bike identifier</param>
    /// <returns>True if deletion was successful, false otherwise</returns>
    public async Task<bool> Delete(int id)
    {
        return await _bikeRepository.Delete(id);
    }

    /// <summary>
    /// Retrieves a specific bike by its identifier
    /// </summary>
    /// <param name="id">Bike identifier</param>
    /// <returns>The bike DTO or null if not found</returns>
    public async Task<BikeDto?> Get(int id)
    {
        var bike = await _bikeRepository.Read(id);
        return bike != null ? _mapper.Map<BikeDto>(bike) : null;
    }

    /// <summary>
    /// Retrieves all bikes
    /// </summary>
    /// <returns>List of all bike DTOs</returns>
    public async Task<IList<BikeDto>> GetAll()
    {
        var bikes = await _bikeRepository.ReadAll();
        return _mapper.Map<List<BikeDto>>(bikes);
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
        var bike = await _bikeRepository.Read(id);
        if (bike == null)
            throw new KeyNotFoundException($"Bike {id} not found");

        var model = await _modelRepository.Read(dto.ModelId); 
        if (model == null)
            throw new ArgumentException($"Model with id {dto.ModelId} not found");

        _mapper.Map(dto, bike);
        bike.Model = model;

        var updated = await _bikeRepository.Update(bike);
        return _mapper.Map<BikeDto>(updated);
    }

    /// <summary>
    /// Retrieves bikes filtered by model
    /// </summary>
    /// <param name="modelId">Bike model identifier</param>
    /// <returns>List of bike DTOs belonging to the specified model</returns>
    public async Task<IList<BikeDto>> GetBikesByModelAsync(int modelId) 
    {
        var bikes = await _bikeRepository.ReadAll();
        var filtered = bikes.Where(b => b.Model?.Id == modelId).ToList(); 
        return _mapper.Map<List<BikeDto>>(filtered);
    }
}