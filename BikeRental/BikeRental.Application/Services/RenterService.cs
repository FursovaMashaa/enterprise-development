using AutoMapper;
using BikeRental.Application.Contracts.Rental;
using BikeRental.Application.Contracts.Renter;
using BikeRental.Domain;
using BikeRental.Domain.Models;

namespace BikeRental.Application.Services;

public class RenterService : IRenterService
{

    private readonly IRepository<Renter, int> _renterRepository;
    private readonly IRepository<Rental, int> _rentRepository;
    private readonly IMapper _mapper;

    public RenterService(
        IRepository<Renter, int> renterRepository, 
        IRepository<Rental, int> rentRepository,  
        IMapper mapper)
    {
        _renterRepository = renterRepository;
        _rentRepository = rentRepository;
        _mapper = mapper;
    }

    public async Task<RenterDto> Create(RenterCreateUpdateDto dto)
    {
        var renter = _mapper.Map<Renter>(dto);
        var created = await _renterRepository.Create(renter);
        return _mapper.Map<RenterDto>(created);
    }

    public async Task<bool> Delete(int id)
    {
        return await _renterRepository.Delete(id);
    }

    public async Task<RenterDto?> Get(int id)
    {
        var renter = await _renterRepository.Read(id);
        return renter != null ? _mapper.Map<RenterDto>(renter) : null;
    }

    public async Task<IList<RenterDto>> GetAll()
    {
        var renters = await _renterRepository.ReadAll();
        return _mapper.Map<List<RenterDto>>(renters);
    }

    public async Task<RenterDto> Update(RenterCreateUpdateDto dto, int id)
    {
        var renter = await _renterRepository.Read(id);
        if (renter == null)
            throw new KeyNotFoundException($"Renter with ID {id} not found.");

        _mapper.Map(dto, renter);
        var updated = await _renterRepository.Update(renter);
        return _mapper.Map<RenterDto>(updated);
    }

    public async Task<IList<RentalDto>> GetRenterRentalsAsync(int renterId)
    {
        var rents = await _rentRepository.ReadAll();
        var filtered = rents.Where(r => r.Renter.Id == renterId).ToList();
        return _mapper.Map<List<RentalDto>>(filtered);
    }

    public async Task<int> GetRenterRentalAsync(int renterId)
    {
        var rents = await _rentRepository.ReadAll();
        return rents.Count(r => r.Renter.Id == renterId);
    }
}