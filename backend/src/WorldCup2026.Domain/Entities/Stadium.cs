namespace WorldCup2026.Domain.Entities;

/// <summary>
/// Represents a stadium hosting World Cup matches
/// </summary>
public class Stadium : BaseEntity
{
    /// <summary>
    /// Name of the stadium
    /// </summary>
    public string Name { get; set; } = string.Empty;

    /// <summary>
    /// City where the stadium is located
    /// </summary>
    public string City { get; set; } = string.Empty;

    /// <summary>
    /// Country where the stadium is located (USA, Mexico, or Canada)
    /// </summary>
    public string Country { get; set; } = string.Empty;

    /// <summary>
    /// Seating capacity of the stadium
    /// </summary>
    public int Capacity { get; set; }

    /// <summary>
    /// URL to stadium image
    /// </summary>
    public string? ImageUrl { get; set; }

    /// <summary>
    /// URL to country flag image
    /// </summary>
    public string? FlagUrl { get; set; }

    /// <summary>
    /// Geographic latitude coordinate
    /// </summary>
    public decimal? Latitude { get; set; }

    /// <summary>
    /// Geographic longitude coordinate
    /// </summary>
    public decimal? Longitude { get; set; }

    // Navigation properties

    /// <summary>
    /// Matches hosted at this stadium
    /// </summary>
    public ICollection<Match> Matches { get; set; } = new List<Match>();
}

// Made with Bob
