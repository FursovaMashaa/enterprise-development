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
public class RentalsController : CrudControllerBase<RentalDto, RentalCreateUpdateDto, int>
{
    /// <summary>
    /// Initializes a new instance of the <see cref="RentalsController"/> class
    /// </summary>
    /// <param name="service">Rental service providing CRUD operations</param>
    /// <param name="logger">Logger for tracking rental operations</param>
    public RentalsController(
        IRentalService service,
        ILogger<RentalsController> logger)
        : base(service, logger)
    {
    }
}