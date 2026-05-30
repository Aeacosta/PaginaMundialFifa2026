using WorldCup2026.Domain.Enums;

namespace WorldCup2026.Application.DTOs.Team;

/// <summary>
/// DTO for Team entity
/// </summary>
public class TeamDto
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string Code { get; set; } = string.Empty;
    public string? FlagUrl { get; set; }
    public int? GroupId { get; set; }
    public string? GroupName { get; set; }
    public Confederation Confederation { get; set; }
    public string ConfederationName { get; set; } = string.Empty;
    public int? FifaRanking { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime UpdatedAt { get; set; }
}

// Made with Bob