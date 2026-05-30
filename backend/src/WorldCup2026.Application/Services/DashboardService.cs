using AutoMapper;
using WorldCup2026.Application.DTOs.Dashboard;
using WorldCup2026.Application.DTOs.Match;
using WorldCup2026.Application.Interfaces;
using WorldCup2026.Domain.Enums;
using WorldCup2026.Domain.Interfaces;

namespace WorldCup2026.Application.Services;

/// <summary>
/// Service implementation for dashboard and statistics operations
/// </summary>
public class DashboardService : IDashboardService
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;

    public DashboardService(IUnitOfWork unitOfWork, IMapper mapper)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
    }

    public async Task<DashboardDto> GetDashboardDataAsync(CancellationToken cancellationToken = default)
    {
        // Get tournament stats
        var stats = await GetTournamentStatsAsync(cancellationToken);

        // Get upcoming matches
        var upcomingMatches = await _unitOfWork.Matches.GetUpcomingMatchesAsync(5, cancellationToken);
        var upcomingMatchDtos = _mapper.Map<List<MatchDto>>(upcomingMatches);

        // Get recent matches
        var recentMatches = await _unitOfWork.Matches.GetRecentResultsAsync(5, cancellationToken);
        var recentMatchDtos = _mapper.Map<List<MatchDto>>(recentMatches);

        // Get today's matches
        var todayMatches = await _unitOfWork.Matches.GetTodayMatchesAsync(cancellationToken);
        var todayMatchDtos = _mapper.Map<List<MatchDto>>(todayMatches);

        return new DashboardDto
        {
            Stats = stats,
            UpcomingMatches = upcomingMatchDtos,
            RecentResults = recentMatchDtos,
            TodayMatches = todayMatchDtos
        };
    }

    public async Task<TournamentStatsDto> GetTournamentStatsAsync(CancellationToken cancellationToken = default)
    {
        // Get all entities
        var teams = await _unitOfWork.Teams.GetAllAsync(cancellationToken);
        var groups = await _unitOfWork.Groups.GetAllAsync(cancellationToken);
        var matches = await _unitOfWork.Matches.GetAllAsync(cancellationToken);
        var stadiums = await _unitOfWork.Stadiums.GetAllAsync(cancellationToken);

        // Calculate statistics
        var totalTeams = teams.Count();
        var totalGroups = groups.Count();
        var totalMatches = matches.Count();
        var totalStadiums = stadiums.Count();

        var completedMatches = matches.Count(m => m.Status == MatchStatus.Finished);
        var upcomingMatches = matches.Count(m => m.Status == MatchStatus.Scheduled);

        // Calculate total goals from finished matches
        var finishedMatches = matches.Where(m => m.Status == MatchStatus.Finished && m.Result != null);
        var totalGoals = finishedMatches.Sum(m => m.Result!.HomeTeamScore + m.Result.AwayTeamScore);

        return new TournamentStatsDto
        {
            TotalTeams = totalTeams,
            TotalGroups = totalGroups,
            TotalMatches = totalMatches,
            TotalStadiums = totalStadiums,
            CompletedMatches = completedMatches,
            UpcomingMatches = upcomingMatches,
            TotalGoals = totalGoals
        };
    }

    public async Task<IEnumerable<TopScorerDto>> GetTopScorersAsync(int count = 10, CancellationToken cancellationToken = default)
    {
        // Note: This is a placeholder implementation
        // In a real application, you would have a Player entity and track individual player goals
        // For now, we'll return an empty list as we don't have player data in our current model
        
        // TODO: Implement player tracking and goal scoring statistics
        // This would require:
        // 1. Player entity
        // 2. Goal entity (linking player, match, minute)
        // 3. Repository methods to query player statistics
        
        return await Task.FromResult(Enumerable.Empty<TopScorerDto>());
    }

    public async Task<IEnumerable<TeamPerformanceDto>> GetTopTeamsByWinsAsync(int count = 10, CancellationToken cancellationToken = default)
    {
        var standings = await _unitOfWork.Standings.GetAllAsync(cancellationToken);
        
        var topTeams = standings
            .OrderByDescending(s => s.Won)
            .ThenByDescending(s => s.Points)
            .ThenByDescending(s => s.GoalDifference)
            .Take(count)
            .Select(s => new TeamPerformanceDto
            {
                TeamId = s.TeamId,
                TeamName = s.Team.Name,
                TeamCode = s.Team.Code,
                GroupName = s.Group.Name,
                MatchesPlayed = s.Played,
                Wins = s.Won,
                Draws = s.Drawn,
                Losses = s.Lost,
                GoalsFor = s.GoalsFor,
                GoalsAgainst = s.GoalsAgainst,
                GoalDifference = s.GoalDifference,
                Points = s.Points,
                WinPercentage = s.Played > 0 ? Math.Round((decimal)s.Won / s.Played * 100, 2) : 0
            })
            .ToList();

        return topTeams;
    }

    public async Task<IEnumerable<TeamPerformanceDto>> GetTopTeamsByGoalDifferenceAsync(int count = 10, CancellationToken cancellationToken = default)
    {
        var standings = await _unitOfWork.Standings.GetAllAsync(cancellationToken);
        
        var topTeams = standings
            .OrderByDescending(s => s.GoalDifference)
            .ThenByDescending(s => s.Points)
            .ThenByDescending(s => s.GoalsFor)
            .Take(count)
            .Select(s => new TeamPerformanceDto
            {
                TeamId = s.TeamId,
                TeamName = s.Team.Name,
                TeamCode = s.Team.Code,
                GroupName = s.Group.Name,
                MatchesPlayed = s.Played,
                Wins = s.Won,
                Draws = s.Drawn,
                Losses = s.Lost,
                GoalsFor = s.GoalsFor,
                GoalsAgainst = s.GoalsAgainst,
                GoalDifference = s.GoalDifference,
                Points = s.Points,
                WinPercentage = s.Played > 0 ? Math.Round((decimal)s.Won / s.Played * 100, 2) : 0
            })
            .ToList();

        return topTeams;
    }

    public async Task<IEnumerable<TeamPerformanceDto>> GetTopTeamsByPointsAsync(int count = 10, CancellationToken cancellationToken = default)
    {
        var standings = await _unitOfWork.Standings.GetAllAsync(cancellationToken);
        
        var topTeams = standings
            .OrderByDescending(s => s.Points)
            .ThenByDescending(s => s.GoalDifference)
            .ThenByDescending(s => s.GoalsFor)
            .Take(count)
            .Select(s => new TeamPerformanceDto
            {
                TeamId = s.TeamId,
                TeamName = s.Team.Name,
                TeamCode = s.Team.Code,
                GroupName = s.Group.Name,
                MatchesPlayed = s.Played,
                Wins = s.Won,
                Draws = s.Drawn,
                Losses = s.Lost,
                GoalsFor = s.GoalsFor,
                GoalsAgainst = s.GoalsAgainst,
                GoalDifference = s.GoalDifference,
                Points = s.Points,
                WinPercentage = s.Played > 0 ? Math.Round((decimal)s.Won / s.Played * 100, 2) : 0
            })
            .ToList();

        return topTeams;
    }
}

// Made with Bob
