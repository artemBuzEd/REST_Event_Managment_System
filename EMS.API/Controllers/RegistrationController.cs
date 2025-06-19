using EMS.BLL.DTOs.Request.Registration;
using EMS.BLL.Services.Contracts;
using Microsoft.AspNetCore.Mvc;

namespace EMS.API.Controllers;

[Route("api/[controller]")]
[ApiController]
public class RegistrationController : ControllerBase
{
    private readonly IRegistrationService _registrationService;

    public RegistrationController(IRegistrationService registrationService)
    {
        _registrationService = registrationService;
    }

    [HttpGet("{id:int}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetById(int id)
    {
        var registration = await _registrationService.GetByIdAsync(id);
        return Ok(registration);
    }

    [HttpGet("by-date")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> GetByDate([FromQuery] DateTime date)
    {
        var registration = await _registrationService.GetByRegistrationDateAsync(date);
        return Ok(registration);
    }

    [HttpGet("by-specific-event")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> GetBySpecificEvent([FromQuery] int eventId)
    {
        var registrations = await _registrationService.GetAllRegistrationsOnSpecificEventIdAsync(eventId);
        if (!registrations.Any())
            return NotFound($"There is no registrations on event with id {eventId}");
        return Ok(registrations);
    }

    [HttpPost]
    [ProducesResponseType(StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> Create([FromBody] RegistrationCreateRequestDTO dto, CancellationToken cancellationToken)
    {
        var registration = await _registrationService.CreateAsync(dto, cancellationToken);
        return CreatedAtAction(nameof(GetById), new { id = registration.Id }, registration);
    }

    [HttpPut("{id:int}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> Update(int id, [FromBody] RegistrationUpdateRequestDTO dto,
        CancellationToken cancellationToken)
    { 
        await _registrationService.UpdateAsync(id, dto, cancellationToken);
        return NoContent();
    }

    [HttpDelete("{id:int}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> Delete(int id, CancellationToken cancellationToken)
    {
        await _registrationService.DeleteAsync(id, cancellationToken);
        return NoContent();
    }
}