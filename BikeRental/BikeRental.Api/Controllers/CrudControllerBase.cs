using Microsoft.AspNetCore.Mvc;
using BikeRental.Application.Contracts;

namespace BikeRental.Api.Controllers;

/// <summary>
/// Base controller providing CRUD (Create, Read, Update, Delete) operations for entities.
/// This abstract class can be inherited by controllers to provide standard CRUD functionality.
/// </summary>
/// <typeparam name="TDto">DTO type for entity representation</typeparam>
/// <typeparam name="TCreateUpdateDto">DTO type for create/update operations</typeparam>
/// <typeparam name="TKey">Type of the entity's primary key</typeparam>
[ApiController]
[Route("api/[controller]")]
public abstract class CrudControllerBase<TDto, TCreateUpdateDto, TKey>(
    IApplicationService<TDto, TCreateUpdateDto, TKey> service,
    ILogger logger
) : ControllerBase
    where TDto : class
    where TCreateUpdateDto : class
{
    /// <summary>
    /// Retrieves all entities
    /// </summary>
    /// <returns>List of all entities</returns>
    [HttpGet]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public virtual async Task<ActionResult<List<TDto>>> GetAll()
    {
        logger.LogInformation("Getting all entities of type {Type}", typeof(TDto).Name);
        var items = await service.GetAll();
        logger.LogInformation("Retrieved {Count} entities of type {Type}", items.Count, typeof(TDto).Name);
        return Ok(items.ToList());
    }

    /// <summary>
    /// Retrieves a specific entity by its identifier
    /// </summary>
    /// <param name="id">Entity identifier</param>
    /// <returns>The requested entity</returns>
    [HttpGet("{id}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public virtual async Task<ActionResult<TDto>> GetById(TKey id)
    {
        logger.LogInformation("Getting entity of type {Type} with id {Id}", typeof(TDto).Name, id);
        var item = await service.Get(id);
        if (item == null)
        {
            logger.LogWarning("Entity of type {Type} with id {Id} not found", typeof(TDto).Name, id);
            return NotFound();
        }

        logger.LogInformation("Entity of type {Type} with id {Id} found", typeof(TDto).Name, id);
        return Ok(item);
    }

    /// <summary>
    /// Creates a new entity
    /// </summary>
    /// <param name="dto">Data for creating the entity</param>
    /// <returns>The newly created entity</returns>
    [HttpPost]
    [ProducesResponseType(StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public virtual async Task<ActionResult<TDto>> Create([FromBody] TCreateUpdateDto dto)
    {
        logger.LogInformation("Creating new entity of type {Type}", typeof(TDto).Name);
        var item = await service.Create(dto);
        var id = GetIdValue(item);
        logger.LogInformation("Created entity of type {Type} with id {Id}", typeof(TDto).Name, id);
        return CreatedAtAction(nameof(GetById), new { id }, item);
    }

    /// <summary>
    /// Updates an existing entity
    /// </summary>
    /// <param name="id">Identifier of the entity to update</param>
    /// <param name="dto">Updated data for the entity</param>
    /// <returns>The updated entity</returns>
    [HttpPut("{id}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public virtual async Task<ActionResult<TDto>> Update(TKey id, [FromBody] TCreateUpdateDto dto)
    {
        logger.LogInformation("Updating entity of type {Type} with id {Id}", typeof(TDto).Name, id);
        try
        {
            var item = await service.Update(dto, id);
            logger.LogInformation("Updated entity of type {Type} with id {Id}", typeof(TDto).Name, id);
            return Ok(item);
        }
        catch (KeyNotFoundException)
        {
            logger.LogWarning("Entity of type {Type} with id {Id} not found for update", typeof(TDto).Name, id);
            return NotFound();
        }
        catch (ArgumentException ex)
        {
            logger.LogWarning(ex, "Bad request for updating entity of type {Type} with id {Id}", typeof(TDto).Name, id);
            return BadRequest(ex.Message);
        }
    }

    /// <summary>
    /// Deletes an entity by its identifier
    /// </summary>
    /// <param name="id">Identifier of the entity to delete</param>
    /// <returns>Result of the delete operation</returns>
    [HttpDelete("{id}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public virtual async Task<IActionResult> Delete(TKey id)
    {
        logger.LogInformation("Deleting entity of type {Type} with id {Id}", typeof(TDto).Name, id);
        try
        {
            var result = await service.Delete(id);
            if (result)
            {
                logger.LogInformation("Deleted entity of type {Type} with id {Id}", typeof(TDto).Name, id);
                return Ok();
            }
            else
            {
                logger.LogWarning("Entity of type {Type} with id {Id} not found for deletion", typeof(TDto).Name, id);
                return NotFound();
            }
        }
        catch (KeyNotFoundException)
        {
            logger.LogWarning("Entity of type {Type} with id {Id} not found for deletion (KeyNotFoundException)", typeof(TDto).Name, id);
            return NotFound();
        }
    }

    /// <summary>
    /// Extracts the Id value from a DTO object using reflection
    /// </summary>
    /// <param name="dto">DTO object</param>
    /// <returns>Id value or null if not found</returns>
    private object? GetIdValue(TDto dto)
    {
        var prop = dto.GetType().GetProperty("Id");
        return prop?.GetValue(dto);
    }
}