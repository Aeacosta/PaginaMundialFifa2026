namespace WorldCup2026.Domain.Enums;

/// <summary>
/// Represents the different phases of the World Cup tournament
/// </summary>
public enum MatchPhase
{
    /// <summary>
    /// Group stage matches (48 teams in 12 groups)
    /// </summary>
    GroupStage = 1,

    /// <summary>
    /// Round of 32 (32 teams, 16 matches)
    /// </summary>
    RoundOf32 = 2,

    /// <summary>
    /// Round of 16 (16 teams, 8 matches)
    /// </summary>
    RoundOf16 = 3,

    /// <summary>
    /// Quarter-finals (8 teams, 4 matches)
    /// </summary>
    QuarterFinals = 4,

    /// <summary>
    /// Semi-finals (4 teams, 2 matches)
    /// </summary>
    SemiFinals = 5,

    /// <summary>
    /// Third place match (2 teams, 1 match)
    /// </summary>
    ThirdPlace = 6,

    /// <summary>
    /// Final match (2 teams, 1 match)
    /// </summary>
    Final = 7
}

// Made with Bob
