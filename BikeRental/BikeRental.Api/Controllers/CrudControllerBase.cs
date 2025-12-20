using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
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
    protected readonly ILogger _logger = logger;
    protected readonly IApplicationService<TDto, TCreateUpdateDto, TKey> _service = service;

    /// <summary>
    /// Retrieves all entities
    /// </summary>
    /// <returns>List of all entities</returns>
    [HttpGet]
    public virtual async Task<ActionResult<List<TDto>>> GetAll()
    {
        var items = await _service.GetAll();
        return Ok(items.ToList());
    }

    /// <summary>
    /// Retrieves a specific entity by its identifier
    /// </summary>
    /// <param name="id">Entity identifier</param>
    /// <returns>The requested entity</returns>
    /// <response code="200">Entity found and returned</response>
    /// <response code="404">Entity not found</response>
    [HttpGet("{id}")]
    public virtual async Task<ActionResult<TDto>> GetById(TKey id)
    {
        var item = await _service.Get(id);
        if (item == null)
            return NotFound();

        return Ok(item);
    }

    /// <summary>
    /// Creates a new entity
    /// </summary>
    /// <param name="dto">Data for creating the entity</param>
    /// <returns>The newly created entity</returns>
    /// <response code="201">Entity created successfully</response>
    [HttpPost]
    public virtual async Task<ActionResult<TDto>> Create([FromBody] TCreateUpdateDto dto)
    {
        var item = await _service.Create(dto);
        var id = GetIdValue(item);
        return CreatedAtAction(nameof(GetById), new { id }, item);
    }

    /// <summary>
    /// Updates an existing entity
    /// </summary>
    /// <param name="id">Identifier of the entity to update</param>
    /// <param name="dto">Updated data for the entity</param>
    /// <returns>The updated entity</returns>
    /// <response code="200">Entity updated successfully</response>
    /// <response code="404">Entity not found</response>
    [HttpPut("{id}")]
    public virtual async Task<ActionResult<TDto>> Update(TKey id, [FromBody] TCreateUpdateDto dto)
    {
        try
        {
            var item = await _service.Update(dto, id);
            return Ok(item);
        }
        catch (KeyNotFoundException)
        {
            return NotFound();
        }
    }

    /// <summary>
    /// Deletes an entity by its identifier
    /// </summary>
    /// <param name="id">Identifier of the entity to delete</param>
    /// <returns>Result of the delete operation</returns>
    /// <response code="200">Entity deleted successfully</response>
    /// <response code="404">Entity not found</response>
    [HttpDelete("{id}")]
    public virtual async Task<IActionResult> Delete(TKey id)
    {
        try
        {
            var result = await _service.Delete(id);
            if (result)
                return Ok();
            else
                return NotFound();
        }
        catch (KeyNotFoundException)
        {
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