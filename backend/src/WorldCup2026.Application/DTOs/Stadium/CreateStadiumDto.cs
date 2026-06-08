namespace WorldCup2026.Application.DTOs.Stadium;

/// <summary>
/// DTO for creating a new stadium
/// </summary>
public class CreateStadiumDto
{
    public string Name { get; set; } = string.Empty;
    public string City { get; set; } = string.Empty;
    public string Country { get; set; } = string.Empty;
    public int Capacity { get; set; }
    public string? ImageUrl { get; set; }
    public string? FlagUrl { get; set; }
    public decimal? Latitude { get; set; }
    public decimal? Longitude { get; set; }
}

// Made with Bob