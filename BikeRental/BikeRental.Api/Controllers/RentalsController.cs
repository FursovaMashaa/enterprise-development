using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using BikeRental.Application.Contracts.Rental;

namespace BikeRental.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class RentalsController : CrudControllerBase<RentalDto, RentalCreateUpdateDto, int>
{
    public RentalsController(
        IRentalService service,
        ILogger<RentalsController> logger)
        : base(service, logger)
    {
        
    }
}