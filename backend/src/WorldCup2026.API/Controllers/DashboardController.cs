using Microsoft.AspNetCore.Mvc;
using WorldCup2026.Application.DTOs.Dashboard;
using WorldCup2026.Application.Interfaces;

namespace WorldCup2026.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class DashboardController : ControllerBase
{
    private readonly IDashboardService _dashboardService;
    private readonly ILogger<DashboardController> _logger;

    public DashboardController(IDashboardService dashboardService, ILogger<DashboardController> logger)
    {
        _dashboardService = dashboardService;
        _logger = logger;
    }

    /// <summary>
    /// Get complete dashboard data with all statistics
    /// </summary>
    [HttpGet]
    [ProducesResponseType(typeof(DashboardDto), StatusCodes.Status200OK)]
    public async Task<ActionResult<DashboardDto>> GetDashboard(CancellationToken cancellationToken = default)
    {
        _logger.LogInformation("Getting complete dashboard data");

        var dashboard = await _dashboardService.GetDashboardDataAsync(cancellationToken);
        return Ok(dashboard);
    }

    /// <summary>
    /// Get tournament statistics
    /// </summary>
    [HttpGet("statistics")]
    [ProducesResponseType(typeof(TournamentStatsDto), StatusCodes.Status200OK)]
    public async Task<ActionResult<TournamentStatsDto>> GetStatistics(
        CancellationToken cancellationToken = default)
    {
        _logger.LogInformation("Getting tournament statistics");

        var statistics = await _dashboardService.GetTournamentStatsAsync(cancellationToken);
        return Ok(statistics);
    }

    /// <summary>
    /// Get upcoming matches for dashboard
    /// </summary>
    [HttpGet("upcoming-matches")]
    [ProducesResponseType(typeof(IEnumerable<object>), StatusCodes.Status200OK)]
    public async Task<ActionResult<IEnumerable<object>>> GetUpcomingMatches(
        [FromQuery] int count = 5,
        CancellationToken cancellationToken = default)
    {
        _logger.LogInformation("Getting {Count} upcoming matches for dashboard", count);

        var dashboard = await _dashboardService.GetDashboardDataAsync(cancellationToken);
        var upcomingMatches = dashboard.UpcomingMatches.Take(count);

        return Ok(upcomingMatches);
    }

    /// <summary>
    /// Get recent results for dashboard
    /// </summary>
    [HttpGet("recent-results")]
    [ProducesResponseType(typeof(IEnumerable<object>), StatusCodes.Status200OK)]
    public async Task<ActionResult<IEnumerable<object>>> GetRecentResults(
        [FromQuery] int count = 5,
        CancellationToken cancellationToken = default)
    {
        _logger.LogInformation("Getting {Count} recent results for dashboard", count);

        var dashboard = await _dashboardService.GetDashboardDataAsync(cancellationToken);
        var recentResults = dashboard.RecentResults.Take(count);

        return Ok(recentResults);
    }

    /// <summary>
    /// Get today's matches
    /// </summary>
    [HttpGet("today-matches")]
    [ProducesResponseType(typeof(IEnumerable<object>), StatusCodes.Status200OK)]
    public async Task<ActionResult<IEnumerable<object>>> GetTodayMatches(
        CancellationToken cancellationToken = default)
    {
        _logger.LogInformation("Getting today's matches");

        var dashboard = await _dashboardService.GetDashboardDataAsync(cancellationToken);
        return Ok(dashboard.TodayMatches);
    }

    /// <summary>
    /// Get top scorers
    /// </summary>
    [HttpGet("top-scorers")]
    [ProducesResponseType(typeof(IEnumerable<TopScorerDto>), StatusCodes.Status200OK)]
    public async Task<ActionResult<IEnumerable<TopScorerDto>>> GetTopScorers(
        [FromQuery] int count = 10,
        CancellationToken cancellationToken = default)
    {
        _logger.LogInformation("Getting top {Count} scorers", count);

        var topScorers = await _dashboardService.GetTopScorersAsync(count, cancellationToken);
        return Ok(topScorers);
    }

    /// <summary>
    /// Get top teams by wins
    /// </summary>
    [HttpGet("top-teams/wins")]
    [ProducesResponseType(typeof(IEnumerable<TeamPerformanceDto>), StatusCodes.Status200OK)]
    public async Task<ActionResult<IEnumerable<TeamPerformanceDto>>> GetTopTeamsByWins(
        [FromQuery] int count = 10,
        CancellationToken cancellationToken = default)
    {
        _logger.LogInformation("Getting top {Count} teams by wins", count);

        var topTeams = await _dashboardService.GetTopTeamsByWinsAsync(count, cancellationToken);
        return Ok(topTeams);
    }

    /// <summary>
    /// Get top teams by goal difference
    /// </summary>
    [HttpGet("top-teams/goal-difference")]
    [ProducesResponseType(typeof(IEnumerable<TeamPerformanceDto>), StatusCodes.Status200OK)]
    public async Task<ActionResult<IEnumerable<TeamPerformanceDto>>> GetTopTeamsByGoalDifference(
        [FromQuery] int count = 10,
        CancellationToken cancellationToken = default)
    {
        _logger.LogInformation("Getting top {Count} teams by goal difference", count);

        var topTeams = await _dashboardService.GetTopTeamsByGoalDifferenceAsync(count, cancellationToken);
        return Ok(topTeams);
    }

    /// <summary>
    /// Get top teams by points
    /// </summary>
    [HttpGet("top-teams/points")]
    [ProducesResponseType(typeof(IEnumerable<TeamPerformanceDto>), StatusCodes.Status200OK)]
    public async Task<ActionResult<IEnumerable<TeamPerformanceDto>>> GetTopTeamsByPoints(
        [FromQuery] int count = 10,
        CancellationToken cancellationToken = default)
    {
        _logger.LogInformation("Getting top {Count} teams by points", count);

        var topTeams = await _dashboardService.GetTopTeamsByPointsAsync(count, cancellationToken);
        return Ok(topTeams);
    }

    /// <summary>
    /// Get tournament progress summary
    /// </summary>
    [HttpGet("progress")]
    [ProducesResponseType(typeof(object), StatusCodes.Status200OK)]
    public async Task<ActionResult<object>> GetTournamentProgress(
        CancellationToken cancellationToken = default)
    {
        _logger.LogInformation("Getting tournament progress");

        var statistics = await _dashboardService.GetTournamentStatsAsync(cancellationToken);

        var progress = new
        {
            TotalMatches = statistics.TotalMatches,
            CompletedMatches = statistics.CompletedMatches,
            UpcomingMatches = statistics.UpcomingMatches,
            CompletionPercentage = statistics.TotalMatches > 0
                ? Math.Round((double)statistics.CompletedMatches / statistics.TotalMatches * 100, 2)
                : 0,
            TotalGoals = statistics.TotalGoals,
            AverageGoalsPerMatch = statistics.CompletedMatches > 0
                ? Math.Round((double)statistics.TotalGoals / statistics.CompletedMatches, 2)
                : 0
        };

        return Ok(progress);
    }

    /// <summary>
    /// Get quick facts for dashboard
    /// </summary>
    [HttpGet("quick-facts")]
    [ProducesResponseType(typeof(object), StatusCodes.Status200OK)]
    public async Task<ActionResult<object>> GetQuickFacts(
        CancellationToken cancellationToken = default)
    {
        _logger.LogInformation("Getting quick facts");

        var statistics = await _dashboardService.GetTournamentStatsAsync(cancellationToken);

        var quickFacts = new
        {
            TotalTeams = statistics.TotalTeams,
            TotalGroups = statistics.TotalGroups,
            TotalStadiums = statistics.TotalStadiums,
            TotalMatches = statistics.TotalMatches,
            CompletedMatches = statistics.CompletedMatches,
            UpcomingMatches = statistics.UpcomingMatches,
            TotalGoals = statistics.TotalGoals
        };

        return Ok(quickFacts);
    }

    /// <summary>
    /// Get comprehensive tournament overview
    /// </summary>
    [HttpGet("overview")]
    [ProducesResponseType(typeof(object), StatusCodes.Status200OK)]
    public async Task<ActionResult<object>> GetTournamentOverview(
        CancellationToken cancellationToken = default)
    {
        _logger.LogInformation("Getting tournament overview");

        var dashboard = await _dashboardService.GetDashboardDataAsync(cancellationToken);
        var topScorers = await _dashboardService.GetTopScorersAsync(5, cancellationToken);
        var topTeams = await _dashboardService.GetTopTeamsByPointsAsync(5, cancellationToken);

        var overview = new
        {
            Statistics = dashboard.Stats,
            UpcomingMatchesCount = dashboard.UpcomingMatches.Count,
            RecentResultsCount = dashboard.RecentResults.Count,
            TodayMatchesCount = dashboard.TodayMatches.Count,
            TopScorers = topScorers,
            TopTeams = topTeams
        };

        return Ok(overview);
    }
}

// Made with Bob
