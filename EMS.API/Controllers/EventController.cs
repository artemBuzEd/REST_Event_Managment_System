using EMS.BLL.DTOs.Request;
using EMS.BLL.Services.Contracts;
using Microsoft.AspNetCore.Mvc;

namespace EMS.API.Controllers;
[Route("api/[controller]")]
[ApiController]
public class EventController : ControllerBase
{
    private readonly IEventService _eventService;

    public EventController(IEventService eventService)
    {
        _eventService = eventService;
    }

    [HttpGet]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> GetAll()
    {
        var events = await _eventService.GetAllAsync();
        return Ok(events);
    }

    [HttpGet("{id:int}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetById(int id)
    {
        var events = await _eventService.GetByIdAsync(id);
        return Ok(events);
    }

    [HttpGet("by-date-range")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetByDateRange([FromQuery] DateTime startDate, [FromQuery] DateTime endDate)
    {
        if (startDate > endDate)
            return BadRequest("Start date must be earlier than or equal to end date.");
        var events = await _eventService.GetByDateRangeAsync(startDate, endDate);
        if (!events.Any())
            return NotFound("No events found within the specified date range.");
        return Ok(events);
    }

    [HttpGet("detailed/{id:int}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetDetailedEventById(int id)
    {
        var events = await _eventService.GetDetailedEventByIdAsync(id);
        return Ok(events);
    }

    [HttpGet("detailed/organizer/{id:int}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetDetailedEventBySpecificOrganizerId(int id)
    {
        var events = await _eventService.GetDetailedEventSpecificOrganizerIdAsync(id);
        if (!events.Any())
            return NotFound($"No events found within this organizer id: {id}");
        return Ok(events);
    }

    [HttpPost]
    [ProducesResponseType(StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> Create([FromBody] EventCreateRequestDTO dto, CancellationToken cancellationToken)
    {
        var events = await _eventService.CreateAsync(dto, cancellationToken);
        return CreatedAtAction(nameof(GetById), new { id = events.Id }, events);
    }

    [HttpPut("{id:int}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> Update(int id, [FromBody] EventUpdateRequestDTO dto, CancellationToken cancellationToken)
    {
        await _eventService.UpdateAsync(id, dto, cancellationToken);
        return NoContent();
    }

    [HttpDelete("{id:int}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> Delete(int id, CancellationToken cancellationToken)
    {
        await _eventService.DeleteAsync(id, cancellationToken);
        return NoContent();
    }
}