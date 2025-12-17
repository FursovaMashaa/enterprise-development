using AutoMapper;
using BikeRental.Application.Contracts.Rental;
using BikeRental.Application.Contracts.Renter;
using BikeRental.Domain;
using BikeRental.Domain.Models;

namespace BikeRental.Application.Services;

/// <summary>
/// Service implementation for managing renter entities in the bike rental system.
/// Provides CRUD operations and rental-related functionality for renters.
/// </summary>
public class RenterService : IRenterService
{
    private readonly IRepository<Renter, int> _renterRepository;
    private readonly IRepository<Rental, int> _rentRepository;
    private readonly IMapper _mapper;

    /// <summary>
    /// Initializes a new instance of the <see cref="RenterService"/> class
    /// </summary>
    /// <param name="renterRepository">Repository for renter entities</param>
    /// <param name="rentRepository">Repository for rental entities</param>
    /// <param name="mapper">AutoMapper instance for object mapping</param>
    public RenterService(
        IRepository<Renter, int> renterRepository, 
        IRepository<Rental, int> rentRepository,  
        IMapper mapper)
    {
        _renterRepository = renterRepository;
        _rentRepository = rentRepository;
        _mapper = mapper;
    }

    /// <summary>
    /// Creates a new renter
    /// </summary>
    /// <param name="dto">Data for creating the renter</param>
    /// <returns>The created renter DTO</returns>
    public async Task<RenterDto> Create(RenterCreateUpdateDto dto)
    {
        var renter = _mapper.Map<Renter>(dto);
        var created = await _renterRepository.Create(renter);
        return _mapper.Map<RenterDto>(created);
    }

    /// <summary>
    /// Deletes a renter by its identifier
    /// </summary>
    /// <param name="id">Renter identifier</param>
    /// <returns>True if deletion was successful, false otherwise</returns>
    public async Task<bool> Delete(int id)
    {
        return await _renterRepository.Delete(id);
    }

    /// <summary>
    /// Retrieves a specific renter by its identifier
    /// </summary>
    /// <param name="id">Renter identifier</param>
    /// <returns>The renter DTO or null if not found</returns>
    public async Task<RenterDto?> Get(int id)
    {
        var renter = await _renterRepository.Read(id);
        return renter != null ? _mapper.Map<RenterDto>(renter) : null;
    }

    /// <summary>
    /// Retrieves all renters
    /// </summary>
    /// <returns>List of all renter DTOs</returns>
    public async Task<IList<RenterDto>> GetAll()
    {
        var renters = await _renterRepository.ReadAll();
        return _mapper.Map<List<RenterDto>>(renters);
    }

    /// <summary>
    /// Updates an existing renter
    /// </summary>
    /// <param name="dto">Updated data for the renter</param>
    /// <param name="id">Identifier of the renter to update</param>
    /// <returns>The updated renter DTO</returns>
    /// <exception cref="KeyNotFoundException">Thrown when the renter is not found</exception>
    public async Task<RenterDto> Update(RenterCreateUpdateDto dto, int id)
    {
        var renter = await _renterRepository.Read(id);
        if (renter == null)
            throw new KeyNotFoundException($"Renter with ID {id} not found.");

        _mapper.Map(dto, renter);
        var updated = await _renterRepository.Update(renter);
        return _mapper.Map<RenterDto>(updated);
    }

    /// <summary>
    /// Retrieves all rental records for a specific renter
    /// </summary>
    /// <param name="renterId">Renter identifier</param>
    /// <returns>List of rental DTOs associated with the specified renter</returns>
    public async Task<IList<RentalDto>> GetRenterRentalsAsync(int renterId)
    {
        var rents = await _rentRepository.ReadAll();
        var filtered = rents.Where(r => r.Renter.Id == renterId).ToList();
        return _mapper.Map<List<RentalDto>>(filtered);
    }

    /// <summary>
    /// Retrieves the count of rental transactions for a specific renter
    /// </summary>
    /// <param name="renterId">Renter identifier</param>
    /// <returns>Total number of rental transactions for the specified renter</returns>
    public async Task<int> GetRenterRentalAsync(int renterId)
    {
        var rents = await _rentRepository.ReadAll();
        return rents.Count(r => r.Renter.Id == renterId);
    }
}