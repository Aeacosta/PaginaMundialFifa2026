namespace WorldCup2026.Infrastructure.Data.Seeding.Models;

/// <summary>
/// Root model for match seed data from JSON
/// </summary>
public class MatchSeedData
{
    public List<MatchData> Matches { get; set; } = new();
}

/// <summary>
/// Individual match data from JSON
/// </summary>
public class MatchData
{
    public string? HomeTeamCode { get; set; }
    public string? AwayTeamCode { get; set; }
    public string? StadiumName { get; set; }
    public string? GroupName { get; set; }
    public string Phase { get; set; } = "GroupStage";
    public int Round { get; set; }
    public DateTime MatchDate { get; set; }
    public string Status { get; set; } = "Scheduled";
    public MatchResultData? Result { get; set; }
}

/// <summary>
/// Match result data from JSON
/// </summary>
public class MatchResultData
{
    public int HomeTeamScore { get; set; }
    public int AwayTeamScore { get; set; }
    public int? HomeTeamPenalties { get; set; }
    public int? AwayTeamPenalties { get; set; }
    public int? HomeTeamExtraTimeScore { get; set; }
    public int? AwayTeamExtraTimeScore { get; set; }
}

// Made with Bob