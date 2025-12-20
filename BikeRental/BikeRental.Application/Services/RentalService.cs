using AutoMapper;
using BikeRental.Application.Contracts.Rental;
using BikeRental.Domain;
using BikeRental.Domain.Models;

namespace BikeRental.Application.Services;

/// <summary>
/// Service implementation for managing bike rental operations.
/// Provides CRUD operations and various queries for rental transactions.
/// </summary>
public class RentalService(
    IRepository<Rental, int> rentalRepository, 
    IRepository<Bike, int> bikeRepository,     
    IRepository<Renter, int> renterRepository, 
    IMapper mapper
) : IRentalService
{
    private readonly IRepository<Rental, int> _rentalRepository = rentalRepository;
    private readonly IRepository<Bike, int> _bikeRepository = bikeRepository;
    private readonly IRepository<Renter, int> _renterRepository = renterRepository;
    private readonly IMapper _mapper = mapper;

    /// <summary>
    /// Creates a new rental transaction
    /// </summary>
    /// <param name="dto">Data for creating the rental</param>
    /// <returns>The created rental DTO</returns>
    /// <exception cref="ArgumentException">Thrown when bike or renter does not exist</exception>
    public async Task<RentalDto> Create(RentalCreateUpdateDto dto)
    {
        var bike = await _bikeRepository.Read(dto.BikeId); 
        if (bike == null)
            throw new ArgumentException($"Bike with id {dto.BikeId} not found");

        var renter = await _renterRepository.Read(dto.RenterId); 
        if (renter == null)
            throw new ArgumentException($"Renter with id {dto.RenterId} not found");

        var rental = _mapper.Map<Rental>(dto);
        rental.BikeId = dto.BikeId;
        rental.RenterId = dto.RenterId;

        var created = await _rentalRepository.Create(rental);
        return _mapper.Map<RentalDto>(created);
    }

    /// <summary>
    /// Deletes a rental by its identifier
    /// </summary>
    /// <param name="id">Rental identifier</param>
    /// <returns>True if deletion was successful, false otherwise</returns>
    public async Task<bool> Delete(int id)
    {
        return await _rentalRepository.Delete(id);
    }

    /// <summary>
    /// Retrieves a specific rental by its identifier
    /// </summary>
    /// <param name="id">Rental identifier</param>
    /// <returns>The rental DTO or null if not found</returns>
    public async Task<RentalDto?> Get(int id)
    {
        var rental = await _rentalRepository.Read(id);
        return rental != null ? _mapper.Map<RentalDto>(rental) : null;
    }

    /// <summary>
    /// Retrieves all rentals
    /// </summary>
    /// <returns>List of all rental DTOs</returns>
    public async Task<IList<RentalDto>> GetAll()
    {
        var rentals = await _rentalRepository.ReadAll();
        return _mapper.Map<List<RentalDto>>(rentals);
    }

    /// <summary>
    /// Updates an existing rental
    /// </summary>
    /// <param name="dto">Updated data for the rental</param>
    /// <param name="id">Identifier of the rental to update</param>
    /// <returns>The updated rental DTO</returns>
    /// <exception cref="KeyNotFoundException">Thrown when the rental is not found</exception>
    /// <exception cref="ArgumentException">Thrown when bike or renter does not exist</exception>
    public async Task<RentalDto> Update(RentalCreateUpdateDto dto, int id)
    {
        var rental = await _rentalRepository.Read(id);
        if (rental == null)
            throw new KeyNotFoundException($"Rental {id} not found");

        var bike = await _bikeRepository.Read(dto.BikeId); 
        if (bike == null)
            throw new ArgumentException($"Bike with id {dto.BikeId} not found");

        var renter = await _renterRepository.Read(dto.RenterId); 
        if (renter == null)
            throw new ArgumentException($"Renter with id {dto.RenterId} not found");

        _mapper.Map(dto, rental);
        rental.BikeId = dto.BikeId;
        rental.RenterId = dto.RenterId;

        var updated = await _rentalRepository.Update(rental);
        return _mapper.Map<RentalDto>(updated);
    }

    /// <summary>
    /// Retrieves all rentals for a specific renter
    /// </summary>
    /// <param name="renterId">Renter identifier</param>
    /// <returns>List of rental DTOs for the specified renter</returns>
    public async Task<IList<RentalDto>> GetRentalsByRenterAsync(int renterId)
    {
        var rentals = await _rentalRepository.ReadAll();
        var filtered = rentals.Where(r => r.RenterId == renterId).ToList();
        return _mapper.Map<List<RentalDto>>(filtered);
    }

    /// <summary>
    /// Retrieves all currently active rentals
    /// </summary>
    /// <returns>List of active rental DTOs</returns>
    public async Task<IList<RentalDto>> GetActiveRentalsAsync()
    {
        var rentals = await _rentalRepository.ReadAll();
        var active = rentals
            .Where(r => r.StartTime.AddHours(r.DurationHours) > DateTime.UtcNow)
            .ToList();
        return _mapper.Map<List<RentalDto>>(active);
    }

    /// <summary>
    /// Retrieves rentals within a specific time period
    /// </summary>
    /// <param name="startDate">Start date of the period</param>
    /// <param name="endDate">End date of the period</param>
    /// <returns>List of rental DTOs within the specified period</returns>
    public async Task<IList<RentalDto>> GetRentalsByPeriodAsync(DateTime startDate, DateTime endDate)
    {
        var rentals = await _rentalRepository.ReadAll();
        var filtered = rentals
            .Where(r => r.StartTime >= startDate && r.StartTime <= endDate)
            .ToList();
        return _mapper.Map<List<RentalDto>>(filtered);
    }

    /// <summary>
    /// Retrieves all rentals for a specific bike
    /// </summary>
    /// <param name="bikeId">Bike identifier</param>
    /// <returns>List of rental DTOs for the specified bike</returns>
    public async Task<IList<RentalDto>> GetRentalsByBikeAsync(int bikeId)
    {
        var rentals = await _rentalRepository.ReadAll();
        var filtered = rentals.Where(r => r.BikeId == bikeId).ToList();
        return _mapper.Map<List<RentalDto>>(filtered);
    }
}