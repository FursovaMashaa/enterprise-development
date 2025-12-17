using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using BikeRental.Application.Contracts.Bike;
using BikeRental.Application.Contracts.Rental;

namespace BikeRental.Api.Controllers;

/// <summary>
/// Controller for managing bikes in the rental system.
/// Provides CRUD operations for bikes and access to bike-related rental information.
/// </summary>
[ApiController]
[Route("api/[controller]")]
public class BikesController : CrudControllerBase<BikeDto, BikeCreateUpdateDto, int>
{
    private readonly IBikeService _bikeService;
    private readonly IRentalService _rentalService;

    /// <summary>
    /// Initializes a new instance of the <see cref="BikesController"/> class
    /// </summary>
    /// <param name="bikeService">Bike service for CRUD operations</param>
    /// <param name="rentalService">Rental service for accessing rental data</param>
    /// <param name="logger">Logger for tracking operations</param>
    public BikesController(
        IBikeService bikeService,
        IRentalService rentalService,
        ILogger<BikesController> logger)
        : base(bikeService, logger)
    {
        _bikeService = bikeService;
        _rentalService = rentalService;
    }

    /// <summary>
    /// Retrieves all rental records for a specific bike
    /// </summary>
    /// <param name="id">Bike identifier</param>
    /// <returns>List of rental records for the specified bike</returns>
    /// <response code="200">Success - Returns list of rental records</response>
    /// <response code="204">No rental records found for this bike</response>
    /// <response code="500">Internal server error</response>
    [HttpGet("{id}/rentals")]
    [ProducesResponseType(typeof(IList<RentalDto>), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<ActionResult<IList<RentalDto>>> GetRentals(int id)
    {
        _logger.LogInformation("Getting rentals for bike {BikeId}", id);

        try
        {
            var rentals = await _rentalService.GetRentalsByBikeAsync(id);
            return rentals?.Any() == true ? Ok(rentals) : NoContent();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error getting rentals for bike {BikeId}", id);
            return StatusCode(500, "Internal server error");
        }
    }
}