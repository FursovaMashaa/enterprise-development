using BikeRental.Application.Contracts.Rental;
using BikeRental.Application.Contracts.Nats;
using BikeRental.Application.Contracts;
using BikeRental.Infrastructure.Nats.Deserializers;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using NATS.Client.Core;
using NATS.Client.JetStream.Models;
using NATS.Net;

namespace BikeRental.Infrastructure.Nats;

/// <summary>
/// Background service for processing bike rental messages from NATS JetStream.
/// </summary>
public class BikeRentalNatsConsumer(
    INatsConnection connection,
    IServiceScopeFactory scopeFactory,
    IOptions<NatsOptions> options,
    ILogger<BikeRentalNatsConsumer> logger
) : BackgroundService
{
    private readonly string _streamName = options.Value.StreamName; 
    private readonly string _subjectName = options.Value.SubjectName; 
    
    /// <summary>
    /// Consumes rental messages from NATS and processes them through the rental service.
    /// </summary>
    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        try
        {
            await connection.ConnectAsync();
            
            var context = connection.CreateJetStreamContext();
            
            await context.CreateOrUpdateStreamAsync(
                new StreamConfig(_streamName, [_subjectName]), 
                stoppingToken);
            
            logger.LogInformation("Stream '{Stream}' ready", _streamName);
            
            var consumer = await context.CreateConsumerAsync(
                _streamName, 
                new ConsumerConfig 
                { 
                    DeliverPolicy = ConsumerConfigDeliverPolicy.All,
                    AckPolicy = ConsumerConfigAckPolicy.Explicit
                }, 
                stoppingToken
            );
            
            logger.LogInformation("Consumer created for stream '{Stream}'", _streamName);

            await foreach (var message in consumer.ConsumeAsync(new BikeRentalPayloadDeserializer(), cancellationToken: stoppingToken))
            {
                if (message.Data is null)
                    continue;

                using var scope = scopeFactory.CreateScope();
                var rentalService = scope.ServiceProvider.GetRequiredService<IApplicationService<RentalDto, RentalCreateUpdateDto, int>>();

                foreach (var rental in message.Data)
                    await rentalService.Create(rental);

                await message.AckAsync(cancellationToken: stoppingToken);

                logger.LogInformation("Successfully consumed message from subject {subject} of stream {stream}", _subjectName, _streamName);
            }
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Consumer failed to start stream={stream} subject={subject} message={message}",
                _streamName, _subjectName, ex.Message);
        }
    }
}