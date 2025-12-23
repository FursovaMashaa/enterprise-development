using BikeRental.Application.Contracts.Rental;
using BikeRental.Generator.Nats.Host.Interfaces;
using NATS.Client.Core;
using NATS.Net;
using System.Text.Json;

namespace BikeRental.Generator.Nats.Host;

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