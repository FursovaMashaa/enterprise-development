namespace BikeRental.Application.Contracts.Bike;

public interface IBikeService : IApplicationService<BikeDto, BikeCreateUpdateDto, int>
{
    public Task<IList<BikeDto>> GetBikesByModelAsync(int modelId);
}