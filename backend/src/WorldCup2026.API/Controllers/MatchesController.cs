using Microsoft.AspNetCore.Mvc;
using WorldCup2026.Application.DTOs.Common;
using WorldCup2026.Application.DTOs.Match;
using WorldCup2026.Application.Interfaces;
using WorldCup2026.Domain.Enums;

namespace WorldCup2026.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class MatchesController : ControllerBase
{
    private readonly IMatchService _matchService;
    private readonly ILogger<MatchesController> _logger;

    public MatchesController(IMatchService matchService, ILogger<MatchesController> logger)
    {
        _matchService = matchService;
        _logger = logger;
    }

    /// <summary>
    /// Get all matches with optional filtering and pagination
    /// </summary>
    [HttpGet]
    [ProducesResponseType(typeof(PagedResult<MatchDto>), StatusCodes.Status200OK)]
    public async Task<ActionResult<PagedResult<MatchDto>>> GetMatches(
        [FromQuery] int? groupId = null,
        [FromQuery] int? teamId = null,
        [FromQuery] int? stadiumId = null,
        [FromQuery] MatchPhase? phase = null,
        [FromQuery] MatchStatus? status = null,
        [FromQuery] DateTime? dateFrom = null,
        [FromQuery] DateTime? dateTo = null,
        [FromQuery] int pageNumber = 1,
        [FromQuery] int pageSize = 10,
        CancellationToken cancellationToken = default)
    {
        _logger.LogInformation("Getting matches with filters - GroupId: {GroupId}, TeamId: {TeamId}, Phase: {Phase}, Status: {Status}",
            groupId, teamId, phase, status);

        var result = await _matchService.GetAllMatchesAsync(
            pageNumber, pageSize, phase, status, groupId, stadiumId, dateFrom, dateTo,
            cancellationToken);

        return Ok(result);
    }

    /// <summary>
    /// Get a specific match by ID
    /// </summary>
    [HttpGet("{id}")]
    [ProducesResponseType(typeof(MatchDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<MatchDto>> GetMatch(int id, CancellationToken cancellationToken = default)
    {
        _logger.LogInformation("Getting match with ID: {MatchId}", id);

        var match = await _matchService.GetMatchByIdAsync(id, cancellationToken);

        if (match == null)
        {
            _logger.LogWarning("Match with ID {MatchId} not found", id);
            return NotFound(new { message = $"Match with ID {id} not found" });
        }

        return Ok(match);
    }

    /// <summary>
    /// Get matches by group
    /// </summary>
    [HttpGet("group/{groupId}")]
    [ProducesResponseType(typeof(IEnumerable<MatchDto>), StatusCodes.Status200OK)]
    public async Task<ActionResult<IEnumerable<MatchDto>>> GetMatchesByGroup(
        int groupId,
        CancellationToken cancellationToken = default)
    {
        _logger.LogInformation("Getting matches for group ID: {GroupId}", groupId);

        var matches = await _matchService.GetMatchesByGroupAsync(groupId, cancellationToken);
        return Ok(matches);
    }

    /// <summary>
    /// Get matches by team
    /// </summary>
    [HttpGet("team/{teamId}")]
    [ProducesResponseType(typeof(IEnumerable<MatchDto>), StatusCodes.Status200OK)]
    public async Task<ActionResult<IEnumerable<MatchDto>>> GetMatchesByTeam(
        int teamId,
        CancellationToken cancellationToken = default)
    {
        _logger.LogInformation("Getting matches for team ID: {TeamId}", teamId);

        var matches = await _matchService.GetMatchesByTeamAsync(teamId, cancellationToken);
        return Ok(matches);
    }

    /// <summary>
    /// Get matches by phase
    /// </summary>
    [HttpGet("phase/{phase}")]
    [ProducesResponseType(typeof(IEnumerable<MatchDto>), StatusCodes.Status200OK)]
    public async Task<ActionResult<IEnumerable<MatchDto>>> GetMatchesByPhase(
        MatchPhase phase,
        CancellationToken cancellationToken = default)
    {
        _logger.LogInformation("Getting matches for phase: {Phase}", phase);

        var matches = await _matchService.GetMatchesByPhaseAsync(phase, cancellationToken);
        return Ok(matches);
    }

    /// <summary>
    /// Get matches by status
    /// </summary>
    [HttpGet("status/{status}")]
    [ProducesResponseType(typeof(IEnumerable<MatchDto>), StatusCodes.Status200OK)]
    public async Task<ActionResult<IEnumerable<MatchDto>>> GetMatchesByStatus(
        MatchStatus status,
        CancellationToken cancellationToken = default)
    {
        _logger.LogInformation("Getting matches for status: {Status}", status);

        var matches = await _matchService.GetMatchesByStatusAsync(status, cancellationToken);
        return Ok(matches);
    }

    /// <summary>
    /// Get upcoming matches
    /// </summary>
    [HttpGet("upcoming")]
    [ProducesResponseType(typeof(IEnumerable<MatchDto>), StatusCodes.Status200OK)]
    public async Task<ActionResult<IEnumerable<MatchDto>>> GetUpcomingMatches(
        [FromQuery] int count = 10,
        CancellationToken cancellationToken = default)
    {
        _logger.LogInformation("Getting {Count} upcoming matches", count);

        var matches = await _matchService.GetUpcomingMatchesAsync(count, cancellationToken);
        return Ok(matches);
    }

    /// <summary>
    /// Get recent matches
    /// </summary>
    [HttpGet("recent")]
    [ProducesResponseType(typeof(IEnumerable<MatchDto>), StatusCodes.Status200OK)]
    public async Task<ActionResult<IEnumerable<MatchDto>>> GetRecentMatches(
        [FromQuery] int count = 10,
        CancellationToken cancellationToken = default)
    {
        _logger.LogInformation("Getting {Count} recent matches", count);

        var matches = await _matchService.GetRecentMatchesAsync(count, cancellationToken);
        return Ok(matches);
    }

    /// <summary>
    /// Create a new match
    /// </summary>
    [HttpPost]
    [ProducesResponseType(typeof(MatchDto), StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<ActionResult<MatchDto>> CreateMatch(
        [FromBody] CreateMatchDto createMatchDto,
        CancellationToken cancellationToken = default)
    {
        _logger.LogInformation("Creating new match between teams {HomeTeamId} and {AwayTeamId}",
            createMatchDto.HomeTeamId, createMatchDto.AwayTeamId);

        var match = await _matchService.CreateMatchAsync(createMatchDto, cancellationToken);

        return CreatedAtAction(nameof(GetMatch), new { id = match.Id }, match);
    }

    /// <summary>
    /// Update an existing match
    /// </summary>
    [HttpPut("{id}")]
    [ProducesResponseType(typeof(MatchDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<MatchDto>> UpdateMatch(
        int id,
        [FromBody] UpdateMatchDto updateMatchDto,
        CancellationToken cancellationToken = default)
    {
        _logger.LogInformation("Updating match with ID: {MatchId}", id);

        // Note: There's no UpdateMatchAsync in the service interface
        // Matches should be updated through UpdateMatchResultAsync or UpdateMatchStatusAsync
        // For now, we'll return a message indicating this endpoint is not implemented
        _logger.LogWarning("UpdateMatch endpoint called but not implemented in service");
        return BadRequest(new { message = "To update a match, use the /result endpoint to update the result or /status endpoint to update the status" });
    }

    /// <summary>
    /// Update match result
    /// </summary>
    [HttpPut("{id}/result")]
    [ProducesResponseType(typeof(MatchDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<MatchDto>> UpdateMatchResult(
        int id,
        [FromBody] UpdateMatchResultDto resultDto,
        CancellationToken cancellationToken = default)
    {
        _logger.LogInformation("Updating result for match ID: {MatchId}", id);

        var match = await _matchService.UpdateMatchResultAsync(id, resultDto, cancellationToken);

        if (match == null)
        {
            _logger.LogWarning("Match with ID {MatchId} not found for result update", id);
            return NotFound(new { message = $"Match with ID {id} not found" });
        }

        return Ok(match);
    }

    /// <summary>
    /// Update match status
    /// </summary>
    [HttpPut("{id}/status")]
    [ProducesResponseType(typeof(MatchDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<MatchDto>> UpdateMatchStatus(
        int id,
        [FromBody] MatchStatus status,
        CancellationToken cancellationToken = default)
    {
        _logger.LogInformation("Updating status for match ID: {MatchId} to {Status}", id, status);

        var match = await _matchService.UpdateMatchStatusAsync(id, status, cancellationToken);

        if (match == null)
        {
            _logger.LogWarning("Match with ID {MatchId} not found for status update", id);
            return NotFound(new { message = $"Match with ID {id} not found" });
        }

        return Ok(match);
    }

    /// <summary>
    /// Delete a match
    /// </summary>
    [HttpDelete("{id}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> DeleteMatch(int id, CancellationToken cancellationToken = default)
    {
        _logger.LogInformation("Deleting match with ID: {MatchId}", id);

        var result = await _matchService.DeleteMatchAsync(id, cancellationToken);

        if (!result)
        {
            _logger.LogWarning("Match with ID {MatchId} not found for deletion", id);
            return NotFound(new { message = $"Match with ID {id} not found" });
        }

        return NoContent();
    }

    /// <summary>
    /// Check if a match exists
    /// </summary>
    [HttpHead("{id}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> MatchExists(int id, CancellationToken cancellationToken = default)
    {
        var exists = await _matchService.MatchExistsAsync(id, cancellationToken);
        return exists ? Ok() : NotFound();
    }
}

// Made with Bob
