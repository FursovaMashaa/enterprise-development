using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using BikeRental.Application.Contracts.Bike;
using BikeRental.Application.Contracts.BikeModel;

namespace BikeRental.Api.Controllers; 

[ApiController]
[Route("api/BikeModels")] 
public class BikeModelsControllers : CrudControllerBase<BikeModelDto, BikeModelCreateUpdateDto, int>
{
    private readonly IBikeModelService _modelService; 

    public BikeModelsControllers(
        IBikeModelService modelService, 
        ILogger<BikeModelsControllers> logger)
        : base(modelService, logger)
    {
        _modelService = modelService;
    }

    [HttpGet("{id}/bikes")]
    public async Task<ActionResult<IList<BikeDto>>> GetBikes(int id)
    {
        try
        {
            var bikes = await _modelService.GetBikesAsync(id); 
            return bikes?.Any() == true ? Ok(bikes) : NoContent();
        }
        catch (Exception ex)
        {
             _logger.LogError(ex, "Ошибка при получении велосипедов для модели {ModelId}", id);
            return StatusCode(500, "Внутренняя ошибка сервера");
        }
    }
}