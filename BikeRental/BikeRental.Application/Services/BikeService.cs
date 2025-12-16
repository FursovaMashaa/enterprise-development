using AutoMapper;
using BikeRental.Application.Contracts.Bike;
using BikeRental.Domain;
using BikeRental.Domain.Models;

namespace BikeRental.Application.Services;

public class BikeService : IBikeService
{
    private readonly IRepository<Bike, int> _bikeRepository;   
    private readonly IRepository<BikeModel, int> _modelRepository;
    private readonly IMapper _mapper;

    public BikeService(
        IRepository<Bike, int> bikeRepository,    
        IRepository<BikeModel, int> modelRepository,
        IMapper mapper)
    {
        _bikeRepository = bikeRepository;
        _modelRepository = modelRepository;
        _mapper = mapper;
    }

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

    public async Task<bool> Delete(int id)
    {
        return await _bikeRepository.Delete(id);
    }

    public async Task<BikeDto?> Get(int id)
    {
        var bike = await _bikeRepository.Read(id);
        return bike != null ? _mapper.Map<BikeDto>(bike) : null;
    }

    public async Task<IList<BikeDto>> GetAll()
    {
        var bikes = await _bikeRepository.ReadAll();
        return _mapper.Map<List<BikeDto>>(bikes);
    }

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

    public async Task<IList<BikeDto>> GetBikesByModelAsync(int modelId) 
    {
        var bikes = await _bikeRepository.ReadAll();
        var filtered = bikes.Where(b => b.Model.Id == modelId).ToList(); 
        return _mapper.Map<List<BikeDto>>(filtered);
    }
}