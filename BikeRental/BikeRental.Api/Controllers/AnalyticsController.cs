using Microsoft.AspNetCore.Mvc;
using BikeRental.Application.Contracts;
using BikeRental.Application.Contracts.Bike;
using BikeRental.Application.Contracts.Renter;

namespace BikeRental.Api.Controllers;

/// <summary>
/// Controller for bike rental analytics.
/// Provides API endpoints for obtaining statistical data and analytical reports.
/// </summary>
[ApiController]
[Route("api/[controller]")]
public class AnalyticsController(
    IAnalyticsService analyticsService,
    ILogger<AnalyticsController> logger
) : ControllerBase
{
    /// <summary>
    /// Retrieves a list of all sport bikes available for rental
    /// </summary>
    /// <returns>List of sport bikes</returns>
    [HttpGet("sport-bikes")]
    [ProducesResponseType(typeof(IList<BikeDto>), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<ActionResult<IList<BikeDto>>> GetSportBikes()
    {
        try
        {
            var bikes = await analyticsService.GetAllSportBikesAsync();
            return Ok(bikes);
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Error getting sport bikes");
            return StatusCode(500, "Internal server error");
        }
    }

    /// <summary>
    /// Retrieves the top 5 bike models by total rental revenue
    /// </summary>
    /// <returns>List of models with total revenue</returns>
    [HttpGet("top-models-revenue")]
    [ProducesResponseType(typeof(IList<KeyValuePair<int, decimal>>), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<ActionResult<IList<KeyValuePair<int, decimal>>>> GetTopModelsByRevenue()
    {
        try
        {
            var models = await analyticsService.GetTopFiveModelsByRevenueAsync();
            return Ok(models);
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Error getting top models by revenue");
            return StatusCode(500, "Internal server error");
        }
    }

    /// <summary>
    /// Retrieves the top 5 bike models by total rental duration
    /// </summary>
    /// <returns>List of models with total rental hours</returns>
    [HttpGet("top-models-duration")]
    [ProducesResponseType(typeof(IList<KeyValuePair<int, int>>), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<ActionResult<IList<KeyValuePair<int, int>>>> GetTopModelsByDuration()
    {
        try
        {
            var models = await analyticsService.GetTopFiveModelsByTotalDurationAsync();
            return Ok(models);
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Error getting top models by duration");
            return StatusCode(500, "Internal server error");
        }
    }

    /// <summary>
    /// Retrieves rental duration statistics (minimum, maximum, and average)
    /// </summary>
    /// <returns>Object containing min, max, and average rental durations</returns>
    [HttpGet("rental-stats")]
    [ProducesResponseType(typeof(object), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<ActionResult<object>> GetRentalStats()
    {
        try
        {
            var stats = await analyticsService.GetMinMaxAvgRentDurationAsync();
            return Ok(new 
            { 
                MinDuration = stats.Min, 
                MaxDuration = stats.Max, 
                AvgDuration = stats.Avg 
            });
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Error getting rental statistics");
            return StatusCode(500, "Internal server error");
        }
    }

    /// <summary>
    /// Retrieves total rental hours for a specific bike category
    /// </summary>
    /// <param name="type">Bike type identifier (0: Sport, 1: Mountain, 2: Road, 3: Hybrid)</param>
    /// <returns>Total rental hours for the specified category</returns>
    [HttpGet("category-utilization/{type}")]
    [ProducesResponseType(typeof(int), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<ActionResult<int>> GetCategoryUtilization(int type)
    {
        try
        {
            var hours = await analyticsService.GetTotalRentalTimeByTypeAsync(type);
            return Ok(hours);
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Error getting category utilization for type {Type}", type);
            return StatusCode(500, "Internal server error");
        }
    }

    /// <summary>
    /// Retrieves top clients by number of rental transactions
    /// </summary>
    /// <returns>List of top clients with their rental counts</returns>
    [HttpGet("top-clients")]
    [ProducesResponseType(typeof(IList<KeyValuePair<RenterDto, int>>), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<ActionResult<IList<KeyValuePair<RenterDto, int>>>> GetTopClients()
    {
        try
        {
            var clients = await analyticsService.GetTopClientsByRentalCountAsync();
            return Ok(clients);
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Error getting top clients");
            return StatusCode(500, "Internal server error");
        }
    }
}