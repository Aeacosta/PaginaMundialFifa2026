namespace WorldCup2026.Application.DTOs.Match;

/// <summary>
/// DTO for updating match result
/// </summary>
public class UpdateMatchResultDto
{
    public int HomeTeamScore { get; set; }
    public int AwayTeamScore { get; set; }
    public int? HomeTeamPenalties { get; set; }
    public int? AwayTeamPenalties { get; set; }
}

// Made with Bob