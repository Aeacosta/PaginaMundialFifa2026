using WorldCup2026.Domain.Enums;

namespace WorldCup2026.Application.DTOs.Match;

public class UpdateMatchDto
{
    public Guid HomeTeamId { get; set; }
    public Guid AwayTeamId { get; set; }
    public Guid StadiumId { get; set; }
    public DateTime MatchDate { get; set; }
    public MatchPhase Phase { get; set; }
    public MatchStatus Status { get; set; }
    public Guid? GroupId { get; set; }
}

// Made with Bob
