namespace WorldCup2026.Domain.Entities;

/// <summary>
/// Represents a group in the World Cup group stage
/// </summary>
public class Group : BaseEntity
{
    /// <summary>
    /// Name of the group (e.g., "Group A", "Group B")
    /// </summary>
    public string Name { get; set; } = string.Empty;

    /// <summary>
    /// Optional description of the group
    /// </summary>
    public string? Description { get; set; }

    // Navigation properties

    /// <summary>
    /// Teams in this group
    /// </summary>
    public ICollection<Team> Teams { get; set; } = new List<Team>();

    /// <summary>
    /// Matches in this group
    /// </summary>
    public ICollection<Match> Matches { get; set; } = new List<Match>();

    /// <summary>
    /// Standings for this group
    /// </summary>
    public ICollection<Standing> Standings { get; set; } = new List<Standing>();
}

// Made with Bob
