using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using BikeRental.Application.Contracts;
using BikeRental.Application.Contracts.Bike;
using BikeRental.Application.Contracts.Renter;

namespace BikeRental.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class AnalyticsController : ControllerBase
{
    private readonly ILogger<AnalyticsController> _logger;
    private readonly IAnalyticsService _analyticsService;

    public AnalyticsController(
        IAnalyticsService analyticsService,
        ILogger<AnalyticsController> logger)
    {
        _analyticsService = analyticsService;
        _logger = logger;
    }

    [HttpGet("sport-bikes")]
    public async Task<ActionResult<IList<BikeDto>>> GetSportBikes()
    {
        try
        {
            var bikes = await _analyticsService.GetAllSportBikesAsync();
            return Ok(bikes);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error getting sport bikes");
            return StatusCode(500, "Internal server error");
        }
    }

    [HttpGet("top-models-revenue")]
    public async Task<ActionResult<IList<KeyValuePair<int, decimal>>>> GetTopModelsByRevenue()
    {
        try
        {
            var models = await _analyticsService.GetTopFiveModelsByRevenueAsync();
            return Ok(models);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error getting top models by revenue");
            return StatusCode(500, "Internal server error");
        }
    }

    [HttpGet("top-models-duration")]
    public async Task<ActionResult<IList<KeyValuePair<int, int>>>> GetTopModelsByDuration()
    {
        try
        {
            var models = await _analyticsService.GetTopFiveModelsByTotalDurationAsync();
            return Ok(models);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error getting top models by duration");
            return StatusCode(500, "Internal server error");
        }
    }

    [HttpGet("rental-stats")]
    public async Task<ActionResult<object>> GetRentalStats()
    {
        try
        {
            var stats = await _analyticsService.GetMinMaxAvgRentDurationAsync();
            return Ok(new 
            { 
                MinDuration = stats.Min, 
                MaxDuration = stats.Max, 
                AvgDuration = stats.Avg 
            });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error getting rental statistics");
            return StatusCode(500, "Internal server error");
        }
    }

    [HttpGet("category-utilization/{type}")]
    public async Task<ActionResult<int>> GetCategoryUtilization(int type)
    {
        try
        {
            var hours = await _analyticsService.GetTotalRentalTimeByTypeAsync(type);
            return Ok(hours);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error getting category utilization for type {Type}", type);
            return StatusCode(500, "Internal server error");
        }
    }

    [HttpGet("top-clients")]
    public async Task<ActionResult<IList<KeyValuePair<RenterDto, int>>>> GetTopClients()
    {
        try
        {
            var clients = await _analyticsService.GetTopClientsByRentalCountAsync();
            return Ok(clients);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error getting top clients");
            return StatusCode(500, "Internal server error");
        }
    }
}