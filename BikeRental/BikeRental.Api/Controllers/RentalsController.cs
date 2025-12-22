using Microsoft.AspNetCore.Mvc;
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
    // Этот контроллер не имеет дополнительных методов,
    // поэтому просто передает параметры в базовый класс
    // Все CRUD методы документированы в базовом классе через [ProducesResponseType]
}