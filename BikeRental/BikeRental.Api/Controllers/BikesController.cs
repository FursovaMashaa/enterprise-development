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
public class BikesController(
    IBikeService bikeService,
    IRentalService rentalService,
    ILogger<BikesController> logger
) : CrudControllerBase<BikeDto, BikeCreateUpdateDto, int>(bikeService, logger)
{
    private readonly IBikeService _bikeService = bikeService;
    private readonly IRentalService _rentalService = rentalService;

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