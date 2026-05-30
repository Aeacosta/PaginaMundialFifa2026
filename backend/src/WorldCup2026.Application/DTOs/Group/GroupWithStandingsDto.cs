using WorldCup2026.Application.DTOs.Standing;

namespace WorldCup2026.Application.DTOs.Group;

/// <summary>
/// DTO for Group with standings information
/// </summary>
public class GroupWithStandingsDto
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string? Description { get; set; }
    public List<StandingDto> Standings { get; set; } = new();
}

// Made with Bob