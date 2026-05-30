using FluentValidation;
using Microsoft.AspNetCore.Mvc;
using WorldCup2026.Application.DTOs.Stadium;
using WorldCup2026.Application.Interfaces;

namespace WorldCup2026.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class StadiumsController : ControllerBase
{
    private readonly IStadiumService _stadiumService;
    private readonly IValidator<CreateStadiumDto> _createValidator;
    private readonly IValidator<UpdateStadiumDto> _updateValidator;

    public StadiumsController(
        IStadiumService stadiumService,
        IValidator<CreateStadiumDto> createValidator,
        IValidator<UpdateStadiumDto> updateValidator)
    {
        _stadiumService = stadiumService;
        _createValidator = createValidator;
        _updateValidator = updateValidator;
    }

    /// <summary>
    /// Get all stadiums with pagination and filtering
    /// </summary>
    [HttpGet]
    public async Task<IActionResult> GetAllStadiums(
        [FromQuery] int page = 1,
        [FromQuery] int pageSize = 10,
        [FromQuery] string? city = null,
        [FromQuery] string? country = null,
        [FromQuery] string? search = null,
        CancellationToken cancellationToken = default)
    {
        var result = await _stadiumService.GetAllStadiumsAsync(
            page, pageSize, city, country, search, cancellationToken);
        return Ok(result);
    }

    /// <summary>
    /// Get stadium by ID
    /// </summary>
    [HttpGet("{id}")]
    public async Task<IActionResult> GetStadiumById(int id, CancellationToken cancellationToken = default)
    {
        var stadium = await _stadiumService.GetStadiumByIdAsync(id, cancellationToken);
        if (stadium == null)
            return NotFound(new { message = $"Stadium with ID {id} not found" });

        return Ok(stadium);
    }

    /// <summary>
    /// Get stadiums by city
    /// </summary>
    [HttpGet("city/{city}")]
    public async Task<IActionResult> GetStadiumsByCity(string city, CancellationToken cancellationToken = default)
    {
        var stadiums = await _stadiumService.GetStadiumsByCityAsync(city, cancellationToken);
        return Ok(stadiums);
    }

    /// <summary>
    /// Get stadiums by country
    /// </summary>
    [HttpGet("country/{country}")]
    public async Task<IActionResult> GetStadiumsByCountry(string country, CancellationToken cancellationToken = default)
    {
        var stadiums = await _stadiumService.GetStadiumsByCountryAsync(country, cancellationToken);
        return Ok(stadiums);
    }

    /// <summary>
    /// Create a new stadium
    /// </summary>
    [HttpPost]
    public async Task<IActionResult> CreateStadium(
        [FromBody] CreateStadiumDto dto,
        CancellationToken cancellationToken = default)
    {
        var validationResult = await _createValidator.ValidateAsync(dto, cancellationToken);
        if (!validationResult.IsValid)
            return BadRequest(validationResult.Errors);

        var stadium = await _stadiumService.CreateStadiumAsync(dto, cancellationToken);
        return CreatedAtAction(nameof(GetStadiumById), new { id = stadium.Id }, stadium);
    }

    /// <summary>
    /// Update an existing stadium
    /// </summary>
    [HttpPut("{id}")]
    public async Task<IActionResult> UpdateStadium(
        int id,
        [FromBody] UpdateStadiumDto dto,
        CancellationToken cancellationToken = default)
    {
        var validationResult = await _updateValidator.ValidateAsync(dto, cancellationToken);
        if (!validationResult.IsValid)
            return BadRequest(validationResult.Errors);

        // Map UpdateStadiumDto to CreateStadiumDto (they have the same properties)
        var createDto = new CreateStadiumDto
        {
            Name = dto.Name,
            City = dto.City,
            Country = dto.Country,
            Capacity = dto.Capacity,
            Latitude = dto.Latitude,
            Longitude = dto.Longitude
        };
        
        var stadium = await _stadiumService.UpdateStadiumAsync(id, createDto, cancellationToken);
        if (stadium == null)
            return NotFound(new { message = $"Stadium with ID {id} not found" });

        return Ok(stadium);
    }

    /// <summary>
    /// Delete a stadium
    /// </summary>
    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteStadium(int id, CancellationToken cancellationToken = default)
    {
        var result = await _stadiumService.DeleteStadiumAsync(id, cancellationToken);
        if (!result)
            return NotFound(new { message = $"Stadium with ID {id} not found" });

        return NoContent();
    }

    /// <summary>
    /// Check if stadium exists
    /// </summary>
    [HttpHead("{id}")]
    public async Task<IActionResult> StadiumExists(int id, CancellationToken cancellationToken = default)
    {
        var exists = await _stadiumService.StadiumExistsAsync(id, cancellationToken);
        return exists ? Ok() : NotFound();
    }

    /// <summary>
    /// Check if stadium has scheduled matches
    /// </summary>
    [HttpGet("{id}/has-matches")]
    public async Task<IActionResult> HasScheduledMatches(int id, CancellationToken cancellationToken = default)
    {
        var hasMatches = await _stadiumService.HasScheduledMatchesAsync(id, cancellationToken);
        return Ok(new { stadiumId = id, hasScheduledMatches = hasMatches });
    }
}

// Made with Bob
