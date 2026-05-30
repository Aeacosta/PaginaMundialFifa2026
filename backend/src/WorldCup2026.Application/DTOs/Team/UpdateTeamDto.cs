using WorldCup2026.Domain.Enums;

namespace WorldCup2026.Application.DTOs.Team;

/// <summary>
/// DTO for updating an existing team
/// </summary>
public class UpdateTeamDto
{
    public string Name { get; set; } = string.Empty;
    public string Code { get; set; } = string.Empty;
    public string? FlagUrl { get; set; }
    public int? GroupId { get; set; }
    public Confederation Confederation { get; set; }
    public int? FifaRanking { get; set; }
}

// Made with Bob