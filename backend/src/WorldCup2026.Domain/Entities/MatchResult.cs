namespace WorldCup2026.Domain.Entities;

/// <summary>
/// Represents the result of a completed match
/// </summary>
public class MatchResult : BaseEntity
{
    /// <summary>
    /// ID of the match this result belongs to
    /// </summary>
    public int MatchId { get; set; }

    /// <summary>
    /// Goals scored by the home team in regular time
    /// </summary>
    public int HomeTeamScore { get; set; }

    /// <summary>
    /// Goals scored by the away team in regular time
    /// </summary>
    public int AwayTeamScore { get; set; }

    /// <summary>
    /// Goals scored by the home team in penalty shootout (knockout only)
    /// </summary>
    public int? HomeTeamPenalties { get; set; }

    /// <summary>
    /// Goals scored by the away team in penalty shootout (knockout only)
    /// </summary>
    public int? AwayTeamPenalties { get; set; }

    /// <summary>
    /// ID of the winning team (null for draws in group stage)
    /// </summary>
    public int? WinnerTeamId { get; set; }

    /// <summary>
    /// Match highlights and key moments description
    /// </summary>
    public string? Highlights { get; set; }

    // Navigation properties

    /// <summary>
    /// Match this result belongs to
    /// </summary>
    public Match Match { get; set; } = null!;

    /// <summary>
    /// Winning team (null for draws)
    /// </summary>
    public Team? WinnerTeam { get; set; }
}

// Made with Bob
