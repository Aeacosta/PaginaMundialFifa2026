namespace WorldCup2026.Domain.Entities;

/// <summary>
/// Represents a team's standing in a group
/// </summary>
public class Standing : BaseEntity
{
    /// <summary>
    /// ID of the team
    /// </summary>
    public int TeamId { get; set; }

    /// <summary>
    /// ID of the group
    /// </summary>
    public int GroupId { get; set; }

    /// <summary>
    /// Number of matches played
    /// </summary>
    public int Played { get; set; }

    /// <summary>
    /// Number of matches won
    /// </summary>
    public int Won { get; set; }

    /// <summary>
    /// Number of matches drawn
    /// </summary>
    public int Drawn { get; set; }

    /// <summary>
    /// Number of matches lost
    /// </summary>
    public int Lost { get; set; }

    /// <summary>
    /// Goals scored by the team
    /// </summary>
    public int GoalsFor { get; set; }

    /// <summary>
    /// Goals conceded by the team
    /// </summary>
    public int GoalsAgainst { get; set; }

    /// <summary>
    /// Goal difference (GoalsFor - GoalsAgainst)
    /// </summary>
    public int GoalDifference { get; set; }

    /// <summary>
    /// Total points earned (3 for win, 1 for draw, 0 for loss)
    /// </summary>
    public int Points { get; set; }

    /// <summary>
    /// Current position in the group (1-4)
    /// </summary>
    public int Position { get; set; }

    // Navigation properties

    /// <summary>
    /// Team this standing belongs to
    /// </summary>
    public Team Team { get; set; } = null!;

    /// <summary>
    /// Group this standing belongs to
    /// </summary>
    public Group Group { get; set; } = null!;
}

// Made with Bob