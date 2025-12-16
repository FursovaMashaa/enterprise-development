using AutoMapper;
using BikeRental.Application.Contracts.Rental;
using BikeRental.Domain;
using BikeRental.Domain.Models;

namespace BikeRental.Application.Services;

public class RentalService : IRentalService
{
    private readonly IRepository<Rental, int> _rentalRepository;
    private readonly IRepository<Bike, int> _bikeRepository;
    private readonly IRepository<Renter, int> _renterRepository;
    private readonly IMapper _mapper;

    public RentalService(
        IRepository<Rental, int> rentalRepository, 
        IRepository<Bike, int> bikeRepository,     
        IRepository<Renter, int> renterRepository, 
        IMapper mapper)
    {
        _rentalRepository = rentalRepository;
        _bikeRepository = bikeRepository;
        _renterRepository = renterRepository;
        _mapper = mapper;
    }

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

    public async Task<bool> Delete(int id)
    {
        return await _rentalRepository.Delete(id);
    }

    public async Task<RentalDto?> Get(int id)
    {
        var rental = await _rentalRepository.Read(id);
        return rental != null ? _mapper.Map<RentalDto>(rental) : null;
    }

    public async Task<IList<RentalDto>> GetAll()
    {
        var rentals = await _rentalRepository.ReadAll();
        return _mapper.Map<List<RentalDto>>(rentals);
    }

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

    public async Task<IList<RentalDto>> GetRentalsByRenterAsync(int renterId)
    {
        var rentals = await _rentalRepository.ReadAll();
        var filtered = rentals.Where(r => r.RenterId == renterId).ToList();
        return _mapper.Map<List<RentalDto>>(filtered);
    }

    public async Task<IList<RentalDto>> GetActiveRentalsAsync()
    {
        var rentals = await _rentalRepository.ReadAll();
        var active = rentals
            .Where(r => r.StartTime.AddHours(r.DurationHours) > DateTime.UtcNow)
            .ToList();
        return _mapper.Map<List<RentalDto>>(active);
    }

    public async Task<IList<RentalDto>> GetRentalsByPeriodAsync(DateTime startDate, DateTime endDate)
    {
        var rentals = await _rentalRepository.ReadAll();
        var filtered = rentals
            .Where(r => r.StartTime >= startDate && r.StartTime <= endDate)
            .ToList();
        return _mapper.Map<List<RentalDto>>(filtered);
    }

    public async Task<IList<RentalDto>> GetRentalsByBikeAsync(int bikeId)
    {
        var rentals = await _rentalRepository.ReadAll();
        var filtered = rentals.Where(r => r.BikeId == bikeId).ToList();
        return _mapper.Map<List<RentalDto>>(filtered);
    }
}