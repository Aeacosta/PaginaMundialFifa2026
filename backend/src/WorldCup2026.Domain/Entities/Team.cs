using WorldCup2026.Domain.Enums;

namespace WorldCup2026.Domain.Entities;

/// <summary>
/// Represents a national team participating in the World Cup
/// </summary>
public class Team : BaseEntity
{
    /// <summary>
    /// Name of the team (e.g., "Argentina", "Brazil")
    /// </summary>
    public string Name { get; set; } = string.Empty;

    /// <summary>
    /// ISO 3-letter country code (e.g., "ARG", "BRA")
    /// </summary>
    public string Code { get; set; } = string.Empty;

    /// <summary>
    /// URL to the team's flag image
    /// </summary>
    public string? FlagUrl { get; set; }

    /// <summary>
    /// Group ID this team belongs to (nullable for teams not yet assigned)
    /// </summary>
    public int? GroupId { get; set; }

    /// <summary>
    /// FIFA confederation the team belongs to
    /// </summary>
    public Confederation Confederation { get; set; }

    /// <summary>
    /// Current FIFA ranking (optional)
    /// </summary>
    public int? FifaRanking { get; set; }

    // Navigation properties

    /// <summary>
    /// Group this team belongs to
    /// </summary>
    public Group? Group { get; set; }

    /// <summary>
    /// Matches where this team is the home team
    /// </summary>
    public ICollection<Match> HomeMatches { get; set; } = new List<Match>();

    /// <summary>
    /// Matches where this team is the away team
    /// </summary>
    public ICollection<Match> AwayMatches { get; set; } = new List<Match>();

    /// <summary>
    /// Standing record for this team
    /// </summary>
    public Standing? Standing { get; set; }

    /// <summary>
    /// Match results where this team won
    /// </summary>
    public ICollection<MatchResult> WonMatches { get; set; } = new List<MatchResult>();
}

// Made with Bob
