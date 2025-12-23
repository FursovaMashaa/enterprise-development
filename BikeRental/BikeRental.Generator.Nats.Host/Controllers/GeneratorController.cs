using BikeRental.Application.Contracts.Rental;
using BikeRental.Generator.Nats.Host.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace BikeRental.Generator.Nats.Host.Controllers;

/// <summary>
/// Controller for initiating message exchange through a message broker
/// </summary>
/// <param name="logger">Logger instance</param>
/// <param name="producerService">Service for sending messages to NATS</param>
[Route("api/[controller]")]
[ApiController]
public class GeneratorController(ILogger<GeneratorController> logger, IProducerService producerService) : ControllerBase
{
    /// <summary>
    /// Method for sending messages through the broker
    /// </summary>
    /// <param name="batchSize">Size of the message batch to send</param>
    /// <param name="payloadLimit">Total number of messages to send</param>
    /// <param name="waitTime">Delay in seconds between batch sends</param>
    /// <returns>List of generated rental contracts</returns>
    [HttpGet]
    [ProducesResponseType(200)]
    [ProducesResponseType(500)]
    public async Task<ActionResult<List<RentalCreateUpdateDto>>> Get([FromQuery] int batchSize, [FromQuery] int payloadLimit, [FromQuery] int waitTime)
    {
        logger.LogInformation("Generating {limit} contracts via {batchSize} batches and {waitTime}s delay", payloadLimit, batchSize, waitTime);
        try
        {
            var list = new List<RentalCreateUpdateDto>(payloadLimit);
            var counter = 0;
            while (counter < payloadLimit)
            {
                var batch = RentalGenerator.GenerateBatch(batchSize);
                await producerService.SendAsync(batch);
                logger.LogInformation("Batch of {batchSize} items has been sent", batchSize);
                await Task.Delay(waitTime * 1000);
                counter += batchSize;
                list.AddRange(batch);
            }

            logger.LogInformation("{method} method of {controller} executed successfully", nameof(Get), GetType().Name);
            return Ok(list);
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "An exception happened during {method} method of {controller}", nameof(Get), GetType().Name);
            return StatusCode(500, $"{ex.Message}\n\r{ex.InnerException?.Message}");
        }
    }
}