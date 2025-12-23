using BikeRental.Application.Contracts.Rental;

namespace BikeRental.Generator.Nats.Host.Interfaces;

public interface IProducerService
{
    public Task SendAsync(IList<RentalCreateUpdateDto> batch);
}