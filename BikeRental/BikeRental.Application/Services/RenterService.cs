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
public class RenterService(
    IRepository<Renter, int> renterRepository, 
    IRepository<Rental, int> rentRepository,  
    IMapper mapper
) : IRenterService
{
    /// <summary>
    /// Creates a new renter
    /// </summary>
    /// <param name="dto">Data for creating the renter</param>
    /// <returns>The created renter DTO</returns>
    public async Task<RenterDto> Create(RenterCreateUpdateDto dto)
    {
        var allRenters = await renterRepository.ReadAll();
        var maxId = allRenters.Any() ? allRenters.Max(r => r.Id) : 0;
        
        var renter = mapper.Map<Renter>(dto);
        renter.Id = maxId + 1;

        var created = await renterRepository.Create(renter);
        return mapper.Map<RenterDto>(created);
    }

    /// <summary>
    /// Deletes a renter by its identifier
    /// </summary>
    /// <param name="id">Renter identifier</param>
    /// <returns>True if deletion was successful, false otherwise</returns>
    public async Task<bool> Delete(int id)
    {
        return await renterRepository.Delete(id);
    }

    /// <summary>
    /// Retrieves a specific renter by its identifier
    /// </summary>
    /// <param name="id">Renter identifier</param>
    /// <returns>The renter DTO or null if not found</returns>
    public async Task<RenterDto?> Get(int id)
    {
        var renter = await renterRepository.Read(id);
        return renter != null ? mapper.Map<RenterDto>(renter) : null;
    }

    /// <summary>
    /// Retrieves all renters
    /// </summary>
    /// <returns>List of all renter DTOs</returns>
    public async Task<IList<RenterDto>> GetAll()
    {
        var renters = await renterRepository.ReadAll();
        return mapper.Map<List<RenterDto>>(renters);
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
        var renter = await renterRepository.Read(id) ?? throw new KeyNotFoundException($"Renter with ID {id} not found.");

        mapper.Map(dto, renter);
        var updated = await renterRepository.Update(renter);
        return mapper.Map<RenterDto>(updated);
    }

    /// <summary>
    /// Retrieves all rental records for a specific renter
    /// </summary>
    /// <param name="renterId">Renter identifier</param>
    /// <returns>List of rental DTOs associated with the specified renter</returns>
    public async Task<IList<RentalDto>> GetRenterRentalsAsync(int renterId)
    {
        var rents = await rentRepository.ReadAll();
        var filtered = rents.Where(r => r.Renter?.Id == renterId).ToList();
        return mapper.Map<List<RentalDto>>(filtered);
    }

    /// <summary>
    /// Retrieves the count of rental transactions for a specific renter
    /// </summary>
    /// <param name="renterId">Renter identifier</param>
    /// <returns>Total number of rental transactions for the specified renter</returns>
    public async Task<int> GetRenterRentalAsync(int renterId)
    {
        var rents = await rentRepository.ReadAll();
        return rents.Count(r => r.Renter?.Id == renterId);
    }
}