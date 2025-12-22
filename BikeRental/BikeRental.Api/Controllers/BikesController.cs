using Microsoft.AspNetCore.Mvc;
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
    /// <summary>
    /// Retrieves all rental records for a specific bike
    /// </summary>
    /// <param name="id">Bike identifier</param>
    /// <returns>List of rental records for the specified bike</returns>
    [HttpGet("{id}/rentals")]
    [ProducesResponseType(typeof(IList<RentalDto>), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<ActionResult<IList<RentalDto>>> GetRentals(int id)
    {
        logger.LogInformation("Getting rentals for bike {BikeId}", id);

        try
        {
            var rentals = await rentalService.GetRentalsByBikeAsync(id);
            return rentals?.Any() == true ? Ok(rentals) : NoContent();
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Error getting rentals for bike {BikeId}", id);
            return StatusCode(500, "Internal server error");
        }
    }
}