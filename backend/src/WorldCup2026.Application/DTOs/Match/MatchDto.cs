using WorldCup2026.Domain.Enums;

namespace WorldCup2026.Application.DTOs.Match;

/// <summary>
/// DTO for Match entity
/// </summary>
public class MatchDto
{
    public int Id { get; set; }
    public int HomeTeamId { get; set; }
    public string HomeTeamName { get; set; } = string.Empty;
    public string HomeTeamCode { get; set; } = string.Empty;
    public string? HomeTeamFlagUrl { get; set; }
    public int AwayTeamId { get; set; }
    public string AwayTeamName { get; set; } = string.Empty;
    public string AwayTeamCode { get; set; } = string.Empty;
    public string? AwayTeamFlagUrl { get; set; }
    public int StadiumId { get; set; }
    public string StadiumName { get; set; } = string.Empty;
    public string StadiumCity { get; set; } = string.Empty;
    public string StadiumCountry { get; set; } = string.Empty;
    public DateTime MatchDate { get; set; }
    public MatchPhase Phase { get; set; }
    public string PhaseName { get; set; } = string.Empty;
    public int? Round { get; set; }
    public int? GroupId { get; set; }
    public string? GroupName { get; set; }
    public MatchStatus Status { get; set; }
    public string StatusName { get; set; } = string.Empty;
    public MatchResultDto? Result { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime UpdatedAt { get; set; }
}

// Made with Bob