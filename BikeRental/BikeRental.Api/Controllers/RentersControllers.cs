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
    private readonly IRenterService _renterService = renterService;
    private readonly IRentalService _rentalService = rentalService;

    /// <summary>
    /// Retrieves all rental records for a specific renter
    /// </summary>
    /// <param name="id">Renter identifier</param>
    /// <returns>List of rental records associated with the specified renter</returns>
    /// <response code="200">Success - Returns list of rental records</response>
    /// <response code="204">No rental records found for this renter</response>
    /// <response code="500">Internal server error</response>
    [HttpGet("{id}/rentals")]
    [ProducesResponseType(typeof(IList<RentalDto>), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<ActionResult<IList<RentalDto>>> GetRentals(int id)
    {
        _logger.LogInformation("Getting rentals for renter {RenterId}", id);

        try
        {
            var rentals = await _rentalService.GetRentalsByRenterAsync(id);
            return rentals?.Any() == true ? Ok(rentals) : NoContent();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error getting rentals for renter {RenterId}", id);
            return StatusCode(500, "Internal server error");
        }
    }
}