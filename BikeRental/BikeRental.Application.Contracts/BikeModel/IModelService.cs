using BikeRental.Application.Contracts.Bike;

namespace BikeRental.Application.Contracts.BikeModel;

public interface IBikeModelService : IApplicationService<BikeModelDto, BikeModelCreateUpdateDto, int>
{
    public Task<IList<BikeDto>> GetBikesAsync(int modelId);
}