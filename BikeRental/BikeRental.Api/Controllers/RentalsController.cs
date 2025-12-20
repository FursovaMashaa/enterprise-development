using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using BikeRental.Application.Contracts.Rental;

namespace BikeRental.Api.Controllers;

/// <summary>
/// Controller for managing bike rental operations.
/// Provides CRUD functionality for rental records in the system.
/// </summary>
[ApiController]
[Route("api/[controller]")]
public class RentalsController(
    IRentalService service,
    ILogger<RentalsController> logger
) : CrudControllerBase<RentalDto, RentalCreateUpdateDto, int>(service, logger)
{
    
}