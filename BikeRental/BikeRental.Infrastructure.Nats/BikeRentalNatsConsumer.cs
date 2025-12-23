using BikeRental.Application.Contracts.Rental;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
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
    IConfiguration configuration,
    ILogger<BikeRentalNatsConsumer> logger
) : BackgroundService
{
    private readonly string _streamName = configuration.GetSection("Nats")["StreamName"] ?? "bikerental"; 
    private readonly string _subjectName = configuration.GetSection("Nats")["SubjectName"] ?? "bikerental.events"; 
    
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

            while (!stoppingToken.IsCancellationRequested)
            {
                await foreach (var message in consumer.ConsumeAsync<IList<RentalCreateUpdateDto>>(
                    cancellationToken: stoppingToken))
                {
                    if (message.Data is null) continue;

                    using var scope = scopeFactory.CreateScope();
                    var rentalService = scope.ServiceProvider.GetRequiredService<IRentalService>();
                    
                    foreach (var rental in message.Data)
                    {
                        await rentalService.Create(rental);
                    }
                    
                    logger.LogInformation(
                        "Successfully consumed {Count} contracts from stream '{Stream}'", 
                        message.Data.Count, _streamName
                    );
                }
            }
        }
        catch (OperationCanceledException)
        {
            logger.LogInformation("Consumer stopped");
        }
        catch (Exception ex)
        {
            logger.LogError(
                ex, 
                "Exception occured during receiving rental contracts from {subject}/{stream}", 
                _subjectName, _streamName
            );
        }
    }
}