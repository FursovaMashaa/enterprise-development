using AutoMapper;
using BikeRental.Application.Contracts.Bike;
using BikeRental.Application.Contracts.BikeModel;
using BikeRental.Domain;
using BikeRental.Domain.Enums;
using BikeRental.Domain.Models;

namespace BikeRental.Application.Services;

public class BikeModelService : IBikeModelService
{
    private readonly IRepository<BikeModel, int> _modelRepository;
    private readonly IRepository<Bike, int> _bikeRepository; 
    private readonly IMapper _mapper;

    public BikeModelService(
        IRepository<BikeModel, int> modelRepository,
        IRepository<Bike, int> bikeRepository, 
        IMapper mapper)
    {
        _modelRepository = modelRepository;
        _bikeRepository = bikeRepository;
        _mapper = mapper;
    }

    public async Task<BikeModelDto> Create(BikeModelCreateUpdateDto dto)
    {
        var model = _mapper.Map<BikeModel>(dto);
        var created = await _modelRepository.Create(model);
        return _mapper.Map<BikeModelDto>(created);
    }

    public async Task<bool> Delete(int id) 
    {
        return await _modelRepository.Delete(id);
    }

    public async Task<BikeModelDto?> Get(int id) 
    {
        var model = await _modelRepository.Read(id);
        return model != null ? _mapper.Map<BikeModelDto>(model) : null;
    }

    public async Task<IList<BikeModelDto>> GetAll()
    {
        var models = await _modelRepository.ReadAll();
        return _mapper.Map<List<BikeModelDto>>(models);
    }

    public async Task<BikeModelDto> Update(BikeModelCreateUpdateDto dto, int id) 
    {
        var model = await _modelRepository.Read(id);
        if (model == null)
            throw new KeyNotFoundException($"BikeModel {id} not found");

        _mapper.Map(dto, model);
        var updated = await _modelRepository.Update(model);
        return _mapper.Map<BikeModelDto>(updated);
    }

    public async Task<IList<BikeModelDto>> GetModelsByTypeAsync(BikeType bikeType)
    {
        var models = await _modelRepository.ReadAll();
        var filtered = models.Where(m => m.BikeType == bikeType).ToList();
        return _mapper.Map<List<BikeModelDto>>(filtered);
    }

    public async Task<IList<BikeModelDto>> GetModelsByYearAsync(int? modelYear) 
    {
        var models = await _modelRepository.ReadAll();
        var filtered = models.Where(m => m.ModelYear == modelYear).ToList();
        return _mapper.Map<List<BikeModelDto>>(filtered);
    }

    public async Task<IList<BikeDto>> GetBikesAsync(int modelId) 
    {
        var bikes = await _bikeRepository.ReadAll();
        var filtered = bikes.Where(b => b.ModelId == modelId).ToList();
        return _mapper.Map<List<BikeDto>>(filtered);
    }
}