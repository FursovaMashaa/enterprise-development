using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using BikeRental.Application.Contracts.Rental;
using BikeRental.Application.Contracts.Renter;

namespace BikeRental.Api.Controllers; 

[ApiController]
[Route("api/[controller]")]
public class RentersController : CrudControllerBase<RenterDto, RenterCreateUpdateDto, int>
{
    private readonly IRenterService _renterService;
    private readonly IRentalService _rentalService;

    public RentersController(
        IRenterService renterService,
        IRentalService rentalService,
        ILogger<RentersController> logger)
        : base(renterService, logger)
    {
        _renterService = renterService;
        _rentalService = rentalService;
    }

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