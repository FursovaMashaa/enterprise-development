using BikeRental.Application.Contracts.Rental;
using BikeRental.Generator.Nats.Host.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace BikeRental.Generator.Nats.Host.Controllers;

[Route("api/[controller]")]
[ApiController]
public class GeneratorController(ILogger<GeneratorController> logger, IProducerService producerService) : ControllerBase
{

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