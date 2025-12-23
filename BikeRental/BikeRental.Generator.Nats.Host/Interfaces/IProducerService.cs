using BikeRental.Application.Contracts.Rental;

namespace BikeRental.Generator.Nats.Host.Interfaces;

/// <summary>
/// Defines the contract for a service that sends batches of rental contracts to a message broker
/// </summary>
public interface IProducerService
{
    /// <summary>
    /// Sends a batch of rental contracts to the configured message broker
    /// </summary>
    /// <param name="batch">The collection of rental contracts to send</param>
    /// <returns>A task that represents the asynchronous send operation</returns>
    public Task SendAsync(IList<RentalCreateUpdateDto> batch);
}