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
public class BikeModelsController : CrudControllerBase<BikeModelDto, BikeModelCreateUpdateDto, int>
{
    private readonly IBikeModelService _modelService;

    /// <summary>
    /// Initializes a new instance of the <see cref="BikeModelsController"/> class
    /// </summary>
    /// <param name="modelService">Bike model service</param>
    /// <param name="logger">Logger</param>
    public BikeModelsController(
        IBikeModelService modelService,
        ILogger<BikeModelsController> logger)
        : base(modelService, logger)
    {
        _modelService = modelService;
    }

    /// <summary>
    /// Retrieves all bikes associated with a specific bike model
    /// </summary>
    /// <param name="id">Bike model identifier</param>
    /// <returns>List of bikes belonging to the specified model</returns>
    /// <response code="200">Success - Returns list of bikes</response>
    /// <response code="204">No bikes found for this model</response>
    /// <response code="500">Internal server error</response>
    [HttpGet("{id}/bikes")]
    [ProducesResponseType(typeof(IList<BikeDto>), 200)]
    [ProducesResponseType(204)]
    [ProducesResponseType(500)]
    public async Task<ActionResult<IList<BikeDto>>> GetBikes(int id)
    {
        try
        {
            var bikes = await _modelService.GetBikesAsync(id);
            return bikes?.Any() == true ? Ok(bikes) : NoContent();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error retrieving bikes for model {ModelId}", id);
            return StatusCode(500, "Internal server error");
        }
    }
}