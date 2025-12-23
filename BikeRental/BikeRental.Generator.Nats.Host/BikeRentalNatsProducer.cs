using BikeRental.Application.Contracts.Rental;
using BikeRental.Generator.Nats.Host.Interfaces;
using NATS.Client.Core;
using NATS.Net;
using System.Text.Json;

namespace BikeRental.Generator.Nats.Host;

/// <summary>
/// Implementation for sending rental contracts to NATS JetStream
/// Connects to NATS, ensures stream exists, and publishes batches of rental contracts
/// </summary>
/// <param name="configuration">Application configuration for retrieving NATS settings</param>
/// <param name="connection">NATS connection instance</param>
/// <param name="logger">Logger for tracking operations and errors</param>
public class BikeRentalNatsProducer(
    IConfiguration configuration,
    INatsConnection connection,
    ILogger<BikeRentalNatsProducer> logger
) : IProducerService
{
    private readonly string _streamName = configuration.GetSection("Nats")["StreamName"] 
        ?? throw new KeyNotFoundException("StreamName section of Nats is missing");
    private readonly string _subjectName = configuration.GetSection("Nats")["SubjectName"] 
        ?? throw new KeyNotFoundException("SubjectName section of Nats is missing");

    /// <summary>
    /// Sends a batch of rental contracts to NATS JetStream
    /// Establishes connection, creates or updates the stream, and publishes the batch
    /// </summary>
    /// <param name="batch">The collection of rental contracts to send</param>
    /// <returns>A task that represents the asynchronous send operation</returns>
    /// <exception cref="KeyNotFoundException">Thrown when NATS configuration is missing</exception>
    /// <exception cref="NatsException">Thrown when NATS operations fail</exception>
    public async Task SendAsync(IList<RentalCreateUpdateDto> batch)
    {
        try
        {
            await connection.ConnectAsync();
            var context = connection.CreateJetStreamContext();
            var stream = await context.CreateOrUpdateStreamAsync(new NATS.Client.JetStream.Models.StreamConfig(_streamName, [_subjectName]));
            logger.LogInformation("Establishing a stream {stream} with subject {subject}", _streamName, _subjectName);

            await context.PublishAsync(_subjectName, JsonSerializer.SerializeToUtf8Bytes(batch));                        
            logger.LogInformation("Sent a batch of {count} contracts to {subject} of {stream}", batch.Count, _subjectName, _streamName);
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Exception occured during sending a batch of {count} contracts to {stream}/{subject}", batch.Count, _streamName, _subjectName);
        }
    }
}