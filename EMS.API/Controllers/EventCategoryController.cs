using EMS.BLL.DTOs.Request;
using EMS.BLL.Services.Contracts;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace EMS.API.Controllers;

[Authorize]
[Route("api/[controller]")]
[ApiController]
public class EventCategoryController : ControllerBase
{
    private readonly IEventCategoryService _eventCategoryService;

    public EventCategoryController(IEventCategoryService eventCategoryService)
    {
        _eventCategoryService = eventCategoryService;
    }

    [HttpGet]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> GetAllFull()
    {
        var eventCategories = await _eventCategoryService.GetAllFullAsync();
        return Ok(eventCategories);
    }

    [HttpGet("mini")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> GetAllMini()
    {
        var eventCategories = await _eventCategoryService.GetAllMiniAsync();
        return Ok(eventCategories);
    }
    
    [HttpGet("{id:int}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetById(int id)
    {
        var eventCategory = await _eventCategoryService.GetByIdAsync(id);
        return Ok(eventCategory);
    }

    [HttpGet("by-name")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetByName([FromQuery] string name)
    {
        var eventCategory = await _eventCategoryService.GetByNameAsync(name);
        return Ok(eventCategory);
    }
    
    [Authorize(Roles = "Organizer")]
    [HttpPost]
    [ProducesResponseType(StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> Create([FromBody] EventCategoryCreateRequestDTO dto, CancellationToken cancellationToken)
    {
        var eventCategory = await _eventCategoryService.CreateAsync(dto, cancellationToken);
        return CreatedAtAction(nameof(GetById), new { id = eventCategory.Id }, eventCategory);
    }

    [HttpDelete("{id:int}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> Delete(int id, CancellationToken cancellationToken)
    {
        await _eventCategoryService.DeleteAsync(id, cancellationToken);
        return NoContent();
    }
}