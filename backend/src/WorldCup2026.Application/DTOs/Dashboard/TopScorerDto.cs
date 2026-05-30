namespace WorldCup2026.Application.DTOs.Dashboard;

/// <summary>
/// DTO for top scorer statistics
/// </summary>
public class TopScorerDto
{
    public int PlayerId { get; set; }
    public string PlayerName { get; set; } = string.Empty;
    public int TeamId { get; set; }
    public string TeamName { get; set; } = string.Empty;
    public string TeamCode { get; set; } = string.Empty;
    public int Goals { get; set; }
    public int MatchesPlayed { get; set; }
    public decimal GoalsPerMatch { get; set; }
}

// Made with Bob
