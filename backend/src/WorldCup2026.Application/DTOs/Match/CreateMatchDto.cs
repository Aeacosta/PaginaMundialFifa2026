using WorldCup2026.Domain.Enums;

namespace WorldCup2026.Application.DTOs.Match;

/// <summary>
/// DTO for creating a new match
/// </summary>
public class CreateMatchDto
{
    public int HomeTeamId { get; set; }
    public int AwayTeamId { get; set; }
    public int StadiumId { get; set; }
    public DateTime MatchDate { get; set; }
    public MatchPhase Phase { get; set; }
    public int? Round { get; set; }
    public int? GroupId { get; set; }
}

// Made with Bob