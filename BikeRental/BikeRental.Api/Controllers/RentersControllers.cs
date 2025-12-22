using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using BikeRental.Application.Contracts.Rental;
using BikeRental.Application.Contracts.Renter;

namespace BikeRental.Api.Controllers;

/// <summary>
/// Controller for managing renter entities in the bike rental system.
/// Provides CRUD operations for renters and access to their rental history.
/// </summary>
[ApiController]
[Route("api/[controller]")]
public class RentersController(
    IRenterService renterService,
    IRentalService rentalService,
    ILogger<RentersController> logger
) : CrudControllerBase<RenterDto, RenterCreateUpdateDto, int>(renterService, logger)
{
    /// <summary>
    /// Retrieves all rental records for a specific renter
    /// </summary>
    /// <param name="id">Renter identifier</param>
    /// <returns>List of rental records associated with the specified renter</returns>
    [HttpGet("{id}/rentals")]
    [ProducesResponseType(typeof(IList<RentalDto>), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<ActionResult<IList<RentalDto>>> GetRentals(int id)
    {
        logger.LogInformation("Getting rentals for renter {RenterId}", id);

        try
        {
            var rentals = await rentalService.GetRentalsByRenterAsync(id);
            return rentals?.Any() == true ? Ok(rentals) : NoContent();
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Error getting rentals for renter {RenterId}", id);
            return StatusCode(500, "Internal server error");
        }
    }
}