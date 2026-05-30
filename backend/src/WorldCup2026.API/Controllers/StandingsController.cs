using Microsoft.AspNetCore.Mvc;
using WorldCup2026.Application.DTOs.Standing;
using WorldCup2026.Application.Interfaces;

namespace WorldCup2026.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class StandingsController : ControllerBase
{
    private readonly IStandingService _standingService;
    private readonly ILogger<StandingsController> _logger;

    public StandingsController(IStandingService standingService, ILogger<StandingsController> logger)
    {
        _standingService = standingService;
        _logger = logger;
    }

    /// <summary>
    /// Get standings for a specific group
    /// </summary>
    [HttpGet("group/{groupId}")]
    [ProducesResponseType(typeof(IEnumerable<StandingDto>), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<IEnumerable<StandingDto>>> GetGroupStandings(
        int groupId,
        CancellationToken cancellationToken = default)
    {
        _logger.LogInformation("Getting standings for group ID: {GroupId}", groupId);

        var standings = await _standingService.GetGroupStandingsAsync(groupId, cancellationToken);

        if (standings == null || !standings.Any())
        {
            _logger.LogWarning("No standings found for group ID: {GroupId}", groupId);
            return NotFound(new { message = $"No standings found for group {groupId}" });
        }

        return Ok(standings);
    }

    /// <summary>
    /// Get all group stage standings
    /// </summary>
    [HttpGet("all")]
    [ProducesResponseType(typeof(IEnumerable<StandingDto>), StatusCodes.Status200OK)]
    public async Task<ActionResult<IEnumerable<StandingDto>>> GetAllStandings(
        CancellationToken cancellationToken = default)
    {
        _logger.LogInformation("Getting all group stage standings");

        var standings = await _standingService.GetAllStandingsAsync(cancellationToken);
        return Ok(standings);
    }

    /// <summary>
    /// Get standings for a specific team
    /// </summary>
    [HttpGet("team/{teamId}")]
    [ProducesResponseType(typeof(StandingDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<StandingDto>> GetTeamStanding(
        int teamId,
        CancellationToken cancellationToken = default)
    {
        _logger.LogInformation("Getting standing for team ID: {TeamId}", teamId);

        var standing = await _standingService.GetTeamStandingAsync(teamId, cancellationToken);

        if (standing == null)
        {
            _logger.LogWarning("Standing not found for team ID: {TeamId}", teamId);
            return NotFound(new { message = $"Standing not found for team {teamId}" });
        }

        return Ok(standing);
    }

    /// <summary>
    /// Recalculate standings for a specific group
    /// </summary>
    [HttpPost("group/{groupId}/recalculate")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> RecalculateGroupStandings(
        int groupId,
        CancellationToken cancellationToken = default)
    {
        _logger.LogInformation("Recalculating standings for group ID: {GroupId}", groupId);

        await _standingService.RecalculateGroupStandingsAsync(groupId, cancellationToken);

        return Ok(new { message = $"Standings recalculated for group {groupId}" });
    }

    /// <summary>
    /// Recalculate all group stage standings
    /// </summary>
    [HttpPost("recalculate-all")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public async Task<IActionResult> RecalculateAllStandings(
        CancellationToken cancellationToken = default)
    {
        _logger.LogInformation("Recalculating all group stage standings");

        await _standingService.RecalculateAllStandingsAsync(cancellationToken);

        return Ok(new { message = "All group stage standings recalculated successfully" });
    }

    /// <summary>
    /// Get qualified teams from a group (top 2)
    /// </summary>
    [HttpGet("group/{groupId}/qualified")]
    [ProducesResponseType(typeof(IEnumerable<StandingDto>), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<IEnumerable<StandingDto>>> GetQualifiedTeams(
        int groupId,
        CancellationToken cancellationToken = default)
    {
        _logger.LogInformation("Getting qualified teams for group ID: {GroupId}", groupId);

        var standings = await _standingService.GetGroupStandingsAsync(groupId, cancellationToken);

        if (standings == null || !standings.Any())
        {
            _logger.LogWarning("No standings found for group ID: {GroupId}", groupId);
            return NotFound(new { message = $"No standings found for group {groupId}" });
        }

        // Return top 2 teams (qualified for knockout stage)
        var qualifiedTeams = standings.Take(2);
        return Ok(qualifiedTeams);
    }

    /// <summary>
    /// Get standings summary with statistics
    /// </summary>
    [HttpGet("summary")]
    [ProducesResponseType(typeof(object), StatusCodes.Status200OK)]
    public async Task<ActionResult<object>> GetStandingsSummary(
        CancellationToken cancellationToken = default)
    {
        _logger.LogInformation("Getting standings summary");

        var allStandings = await _standingService.GetAllStandingsAsync(cancellationToken);

        var summary = new
        {
            TotalTeams = allStandings.Count(),
            TotalGroups = allStandings.Select(s => s.GroupId).Distinct().Count(),
            TotalMatches = allStandings.Sum(s => s.Played),
            TotalGoals = allStandings.Sum(s => s.GoalsFor),
            TopScorers = allStandings
                .OrderByDescending(s => s.GoalsFor)
                .Take(5)
                .Select(s => new { s.TeamName, s.GoalsFor }),
            BestDefense = allStandings
                .Where(s => s.Played > 0)
                .OrderBy(s => s.GoalsAgainst)
                .Take(5)
                .Select(s => new { s.TeamName, s.GoalsAgainst }),
            TopTeams = allStandings
                .OrderByDescending(s => s.Points)
                .ThenByDescending(s => s.GoalDifference)
                .Take(10)
                .Select(s => new { s.TeamName, s.Points, s.GoalDifference })
        };

        return Ok(summary);
    }
}

// Made with Bob
