using WorldCup2026.Application.DTOs.Match;

namespace WorldCup2026.Application.DTOs.Dashboard;

/// <summary>
/// DTO for dashboard/homepage data
/// </summary>
public class DashboardDto
{
    public TournamentStatsDto Stats { get; set; } = new();
    public List<MatchDto> UpcomingMatches { get; set; } = new();
    public List<MatchDto> RecentResults { get; set; } = new();
    public List<MatchDto> TodayMatches { get; set; } = new();
}

/// <summary>
/// Tournament statistics
/// </summary>
public class TournamentStatsDto
{
    public int TotalTeams { get; set; }
    public int TotalGroups { get; set; }
    public int TotalMatches { get; set; }
    public int CompletedMatches { get; set; }
    public int UpcomingMatches { get; set; }
    public int TotalGoals { get; set; }
    public int TotalStadiums { get; set; }
}

// Made with Bob