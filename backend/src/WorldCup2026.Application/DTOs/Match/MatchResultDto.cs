namespace WorldCup2026.Application.DTOs.Match;

/// <summary>
/// DTO for Match Result
/// </summary>
public class MatchResultDto
{
    public int Id { get; set; }
    public int MatchId { get; set; }
    public int HomeTeamScore { get; set; }
    public int AwayTeamScore { get; set; }
    public int? HomeTeamPenalties { get; set; }
    public int? AwayTeamPenalties { get; set; }
    public int? WinnerTeamId { get; set; }
    public string? WinnerTeamName { get; set; }
    public string? Highlights { get; set; }
}

// Made with Bob