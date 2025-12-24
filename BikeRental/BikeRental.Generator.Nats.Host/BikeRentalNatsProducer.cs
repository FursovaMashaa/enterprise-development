using BikeRental.Application.Contracts.Rental;
using BikeRental.Application.Contracts.Nats;
using BikeRental.Generator.Nats.Host.Interfaces;
using Microsoft.Extensions.Options;
using NATS.Client.Core;
using NATS.Client.JetStream.Models;
using System.Text.Json;
using NATS.Net;

namespace BikeRental.Generator.Nats.Host;

/// <summary>
/// Implementation for sending rental contracts to NATS JetStream
/// Connects to NATS, ensures stream exists, and publishes batches of rental contracts
/// </summary>
public class BikeRentalNatsProducer(
    IOptions<NatsOptions> options,
    INatsConnection connection,
    ILogger<BikeRentalNatsProducer> logger
) : IProducerService
{
    private readonly string _streamName = options.Value.StreamName;
    private readonly string _subjectName = options.Value.SubjectName;

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
            var stream = await context.CreateOrUpdateStreamAsync(new StreamConfig(_streamName, [_subjectName]));
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