using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using BikeRental.Application.Contracts;

namespace BikeRental.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public abstract class CrudController<TDto, TCreateUpdateDto, TKey> : ControllerBase
    where TDto : class
    where TCreateUpdateDto : class
{
    protected readonly ILogger _logger;
    protected readonly IApplicationService<TDto, TCreateUpdateDto, TKey> _service;

    protected CrudController(IApplicationService<TDto, TCreateUpdateDto, TKey> service, ILogger logger)
    {
        _service = service;
        _logger = logger;
    }

    [HttpGet]
    public virtual async Task<ActionResult<List<TDto>>> GetAll()
    {
        var items = await _service.GetAll();
        return Ok(items.ToList());
    }

    [HttpGet("{id}")]
    public virtual async Task<ActionResult<TDto>> GetById(TKey id)
    {
        var item = await _service.Get(id);
        if (item == null)
            return NotFound();
        return Ok(item);
    }

    [HttpPost]
    public virtual async Task<ActionResult<TDto>> Create([FromBody] TCreateUpdateDto dto)
    {
        var item = await _service.Create(dto);
        var id = GetIdValue(item);
        return CreatedAtAction(nameof(GetById), new { id }, item);
    }

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

    private object? GetIdValue(TDto dto)
    {
        var prop = dto.GetType().GetProperty("Id");
        return prop?.GetValue(dto);
    }
}