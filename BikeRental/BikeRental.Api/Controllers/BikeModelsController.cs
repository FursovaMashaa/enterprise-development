using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using BikeRental.Application.Contracts.Bike;
using BikeRental.Application.Contracts.BikeModel;

namespace BikeRental.Api.Controllers;

/// <summary>
/// Controller for managing bike models.
/// Provides CRUD operations for bike models and related functionality.
/// </summary>
[ApiController]
[Route("api/BikeModels")]
public class BikeModelsController(
    IBikeModelService modelService,
    ILogger<BikeModelsController> logger
) : CrudControllerBase<BikeModelDto, BikeModelCreateUpdateDto, int>(modelService, logger)
{
    /// <summary>
    /// Retrieves all bikes associated with a specific bike model
    /// </summary>
    /// <param name="id">Bike model identifier</param>
    /// <returns>List of bikes belonging to the specified model</returns>
    [HttpGet("{id}/bikes")]
    [ProducesResponseType(typeof(IList<BikeDto>), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<ActionResult<IList<BikeDto>>> GetBikes(int id)
    {
        try
        {
            var bikes = await modelService.GetBikesAsync(id);
            return bikes?.Any() == true ? Ok(bikes) : NoContent();
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Error retrieving bikes for model {ModelId}", id);
            return StatusCode(500, "Internal server error");
        }
    }
}