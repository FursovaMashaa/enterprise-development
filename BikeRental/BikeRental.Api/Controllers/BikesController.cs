using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using BikeRental.Application.Contracts.Bike;
using BikeRental.Application.Contracts.Rental;

namespace BikeRental.Api.Controllers; // Исправлено пространство имен

[ApiController]
[Route("api/[controller]")]
public class BikesController : CrudControllerBase<BikeDto, BikeCreateUpdateDto, int>{
    private readonly IBikeService _bikeService;
    private readonly IRentalService _rentalService;

    public BikesController(
        IBikeService bikeService,
        IRentalService rentalService,
        ILogger<BikesController> logger)
        : base(bikeService, logger)
    {
        _bikeService = bikeService;
        _rentalService = rentalService;
    }

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