using EMS.BLL.DTOs.Request;
using EMS.BLL.Services.Contracts;
using EMS.DAL.EF.Entities.HelpModels;
using Microsoft.AspNetCore.Mvc;

namespace EMS.API.Controllers;

[Route("api/[controller]")]
[ApiController]
public class VenueController : ControllerBase
{
    private readonly IVenueService _venueService;

    public VenueController(IVenueService venueService)
    {
        _venueService = venueService;
    }

    [HttpGet]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetAll()
    {
        var venues = await _venueService.GetAllAsync();
        return Ok(venues);
    }

    [HttpGet("paginated")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> GetPaginated([FromQuery] VenueParameters parameters)
    {
        var result = await _venueService.GetAllPaginatedAsync(parameters);
        return Ok(result);
    }
    
    [HttpGet("{id:int}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> GetById(int id)
    {
        var venues = await _venueService.GetByIdAsync(id);
        return Ok(venues);
    }

    [HttpGet("by-city")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetAllByCityNameAsync([FromQuery] string cityName)
    {
        var venues = await _venueService.GetByCityAsync(cityName);
        return Ok(venues);
    }

    [HttpGet("by-capacity-range")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetAllByCapacityRangeAsync([FromQuery] int startCapacity,[FromQuery] int endCapacity)
    {
        var venues = await _venueService.GetAllByCapacityRangeAsync(startCapacity, endCapacity);
        return Ok(venues);
    }

    [HttpPost]
    [ProducesResponseType(StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> Create([FromBody] VenueCreateRequestDTO dto,
        CancellationToken cancellationToken)
    {
        var venues = await _venueService.CreateAsync(dto, cancellationToken);
        return CreatedAtAction(nameof(GetById), new { id = venues.Id }, venues);
    }

    [HttpPut("{id:int}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> Update(int id, [FromBody] VenueUpdateRequestDTO dto, CancellationToken cancellationToken)
    {
        await _venueService.UpdateAsync(id, dto, cancellationToken);
        return NoContent();
    }

    [HttpDelete("{id:int}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> Delete(int id, CancellationToken cancellationToken)
    {
        await _venueService.DeleteAsync(id, cancellationToken);
        return NoContent();
    }
}