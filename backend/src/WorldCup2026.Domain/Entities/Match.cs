using WorldCup2026.Domain.Enums;

namespace WorldCup2026.Domain.Entities;

/// <summary>
/// Represents a match between two teams
/// </summary>
public class Match : BaseEntity
{
    /// <summary>
    /// ID of the home team
    /// </summary>
    public int HomeTeamId { get; set; }

    /// <summary>
    /// ID of the away team
    /// </summary>
    public int AwayTeamId { get; set; }

    /// <summary>
    /// ID of the stadium where the match is played
    /// </summary>
    public int StadiumId { get; set; }

    /// <summary>
    /// Date and time when the match is scheduled
    /// </summary>
    public DateTime MatchDate { get; set; }

    /// <summary>
    /// Phase of the tournament (GroupStage, RoundOf16, etc.)
    /// </summary>
    public MatchPhase Phase { get; set; }

    /// <summary>
    /// Round number (for group stage: 1, 2, or 3)
    /// </summary>
    public int? Round { get; set; }

    /// <summary>
    /// Group ID (only for group stage matches)
    /// </summary>
    public int? GroupId { get; set; }

    /// <summary>
    /// Current status of the match
    /// </summary>
    public MatchStatus Status { get; set; }

    // Navigation properties

    /// <summary>
    /// Home team
    /// </summary>
    public Team HomeTeam { get; set; } = null!;

    /// <summary>
    /// Away team
    /// </summary>
    public Team AwayTeam { get; set; } = null!;

    /// <summary>
    /// Stadium where the match is played
    /// </summary>
    public Stadium Stadium { get; set; } = null!;

    /// <summary>
    /// Group (only for group stage matches)
    /// </summary>
    public Group? Group { get; set; }

    /// <summary>
    /// Result of the match (if finished)
    /// </summary>
    public MatchResult? Result { get; set; }
}

// Made with Bob
