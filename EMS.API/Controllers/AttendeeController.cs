using EMS.BLL.DTOs.Request;
using EMS.BLL.DTOs.Request.Attendee;
using EMS.BLL.Services.Contracts;
using EMS.DAL.EF.Entities.HelpModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace EMS.API.Controllers;

[Authorize]
[Route("api/[controller]")]
[ApiController]
public class AttendeeController : ControllerBase
{
    private readonly IAttendeeService _attendeeService;

    public AttendeeController(IAttendeeService attendeeService)
    {
        _attendeeService = attendeeService;
    }

    [HttpGet("paginated")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetPaginated([FromQuery] AttendeeParameters parameters)
    {
        var result = await _attendeeService.GetAllPaginatedAsync(parameters);
        return Ok(result);
    }
    
    [HttpGet]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> GetAll()
    {
        var attendees = await _attendeeService.GetAllAsync();
        return Ok(attendees);
    }

    [HttpGet("{id}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetById(string id)
    {
        var attendee = await _attendeeService.GetByIdAsync(id);
        return Ok(attendee);
    }
    
    [Authorize(Roles = "Attendee")]
    [HttpPost]
    [ProducesResponseType(StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> Create([FromBody] AttendeeCreateRequestDTO dto, CancellationToken cancellationToken)
    {
        var attendee = await _attendeeService.CreateAsync(dto, cancellationToken);
        return CreatedAtAction(nameof(GetById), new { id = attendee.Id }, attendee);
    }
    
    [Authorize(Roles = "Attendee")]
    [HttpPut("{id}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> Update(string id, [FromBody] AttendeeUpdateRequestDTO dto,
        CancellationToken cancellationToken)
    {
        await _attendeeService.UpdateAsync(id, dto, cancellationToken);
        return NoContent();
    }
    
    [Authorize(Roles = "Attendee")]
    [HttpDelete("{id}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> Delete(string id, CancellationToken cancellationToken)
    {
        await _attendeeService.DeleteAsync(id, cancellationToken);
        return NoContent();
    }
    
    
}