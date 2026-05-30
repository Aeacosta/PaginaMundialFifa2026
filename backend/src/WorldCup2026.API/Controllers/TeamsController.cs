using FluentValidation;
using Microsoft.AspNetCore.Mvc;
using WorldCup2026.Application.DTOs.Team;
using WorldCup2026.Application.Interfaces;
using WorldCup2026.Domain.Enums;

namespace WorldCup2026.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class TeamsController : ControllerBase
{
    private readonly ITeamService _teamService;
    private readonly IValidator<CreateTeamDto> _createValidator;
    private readonly IValidator<UpdateTeamDto> _updateValidator;

    public TeamsController(
        ITeamService teamService,
        IValidator<CreateTeamDto> createValidator,
        IValidator<UpdateTeamDto> updateValidator)
    {
        _teamService = teamService;
        _createValidator = createValidator;
        _updateValidator = updateValidator;
    }

    /// <summary>
    /// Get all teams with pagination and filtering
    /// </summary>
    [HttpGet]
    public async Task<IActionResult> GetAllTeams(
        [FromQuery] int page = 1,
        [FromQuery] int pageSize = 10,
        [FromQuery] Confederation? confederation = null,
        [FromQuery] int? groupId = null,
        [FromQuery] string? search = null,
        CancellationToken cancellationToken = default)
    {
        var result = await _teamService.GetAllTeamsAsync(
            page, pageSize, confederation, groupId, search, cancellationToken);
        return Ok(result);
    }

    /// <summary>
    /// Get team by ID
    /// </summary>
    [HttpGet("{id}")]
    public async Task<IActionResult> GetTeamById(int id, CancellationToken cancellationToken = default)
    {
        var team = await _teamService.GetTeamByIdAsync(id, cancellationToken);
        if (team == null)
            return NotFound(new { message = $"Team with ID {id} not found" });

        return Ok(team);
    }

    /// <summary>
    /// Get team by FIFA code
    /// </summary>
    [HttpGet("code/{code}")]
    public async Task<IActionResult> GetTeamByCode(string code, CancellationToken cancellationToken = default)
    {
        var team = await _teamService.GetTeamByCodeAsync(code, cancellationToken);
        if (team == null)
            return NotFound(new { message = $"Team with code {code} not found" });

        return Ok(team);
    }

    /// <summary>
    /// Get teams by group
    /// </summary>
    [HttpGet("group/{groupId}")]
    public async Task<IActionResult> GetTeamsByGroup(int groupId, CancellationToken cancellationToken = default)
    {
        var teams = await _teamService.GetTeamsByGroupAsync(groupId, cancellationToken);
        return Ok(teams);
    }

    /// <summary>
    /// Get teams by confederation
    /// </summary>
    [HttpGet("confederation/{confederation}")]
    public async Task<IActionResult> GetTeamsByConfederation(
        Confederation confederation,
        CancellationToken cancellationToken = default)
    {
        var teams = await _teamService.GetTeamsByConfederationAsync(confederation, cancellationToken);
        return Ok(teams);
    }

    /// <summary>
    /// Create a new team
    /// </summary>
    [HttpPost]
    public async Task<IActionResult> CreateTeam(
        [FromBody] CreateTeamDto dto,
        CancellationToken cancellationToken = default)
    {
        var validationResult = await _createValidator.ValidateAsync(dto, cancellationToken);
        if (!validationResult.IsValid)
            return BadRequest(validationResult.Errors);

        var team = await _teamService.CreateTeamAsync(dto, cancellationToken);
        return CreatedAtAction(nameof(GetTeamById), new { id = team.Id }, team);
    }

    /// <summary>
    /// Update an existing team
    /// </summary>
    [HttpPut("{id}")]
    public async Task<IActionResult> UpdateTeam(
        int id,
        [FromBody] UpdateTeamDto dto,
        CancellationToken cancellationToken = default)
    {
        var validationResult = await _updateValidator.ValidateAsync(dto, cancellationToken);
        if (!validationResult.IsValid)
            return BadRequest(validationResult.Errors);

        var team = await _teamService.UpdateTeamAsync(id, dto, cancellationToken);
        if (team == null)
            return NotFound(new { message = $"Team with ID {id} not found" });

        return Ok(team);
    }

    /// <summary>
    /// Delete a team
    /// </summary>
    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteTeam(int id, CancellationToken cancellationToken = default)
    {
        var result = await _teamService.DeleteTeamAsync(id, cancellationToken);
        if (!result)
            return NotFound(new { message = $"Team with ID {id} not found" });

        return NoContent();
    }

    /// <summary>
    /// Check if team exists
    /// </summary>
    [HttpHead("{id}")]
    public async Task<IActionResult> TeamExists(int id, CancellationToken cancellationToken = default)
    {
        var exists = await _teamService.TeamExistsAsync(id, cancellationToken);
        return exists ? Ok() : NotFound();
    }
}

// Made with Bob
