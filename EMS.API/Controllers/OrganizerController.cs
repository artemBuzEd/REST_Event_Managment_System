using EMS.BLL.DTOs.Request;
using EMS.BLL.Services.Contracts;
using EMS.DAL.EF.Entities;
using EMS.DAL.EF.Entities.HelpModels;
using Microsoft.AspNetCore.Mvc;

namespace EMS.API.Controllers;
[Route("api/[controller]")]
[ApiController]
public class OrganizerController : ControllerBase
{
    private readonly IOrganizerService _organizerService;

    public OrganizerController(IOrganizerService organizerService)
    {
        _organizerService = organizerService;
    }

    [HttpGet]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> GetAll()
    {
        var organizers = await _organizerService.GetAllAsync();
        return Ok(organizers);
    }

    [HttpGet("paginated")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> GetPaginated([FromQuery] OrganizerParameters parameters)
    {
        var result = await _organizerService.GetAllPaginatedAsync(parameters);
        return Ok(result);
    }
    
    [HttpGet("{id:int}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetById(int id)
    {
        var organizer = await _organizerService.GetByIdAsync(id);
        return Ok(organizer);
    }

    [HttpPost]
    [ProducesResponseType(StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> Create([FromBody] OrganizerCreateRequestDTO dto, CancellationToken cancellationToken)
    {
        var organizer = await _organizerService.CreateAsync(dto, cancellationToken);
        return CreatedAtAction(nameof(GetById), new { id = organizer.Id }, organizer);
    }

    [HttpPut("{id:int}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> Update(int id, [FromBody] OrganizerUpdateRequestDTO dto,
        CancellationToken cancellationToken)
    { 
        await _organizerService.UpdateAsync(id, dto, cancellationToken);
        return NoContent();
    }

    [HttpDelete("{id:int}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> Delete(int id, CancellationToken cancellationToken)
    {
        await _organizerService.DeleteAsync(id, cancellationToken);
        return NoContent();
    }
    
}