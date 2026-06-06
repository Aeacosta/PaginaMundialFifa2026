using AutoMapper;
using FluentAssertions;
using Moq;
using WorldCup2026.Application.DTOs.Dashboard;
using WorldCup2026.Application.DTOs.Match;
using WorldCup2026.Application.Services;
using WorldCup2026.Domain.Entities;
using WorldCup2026.Domain.Enums;
using WorldCup2026.Domain.Interfaces;
using DomainMatch = WorldCup2026.Domain.Entities.Match;

namespace WorldCup2026.Tests.Services;

[TestClass]
public class DashboardServiceTests
{
    private Mock<IUnitOfWork> _unitOfWorkMock;
    private Mock<IMapper> _mapperMock;
    private DashboardService _dashboardService;

    [TestInitialize]
    public void Setup()
    {
        _unitOfWorkMock = new Mock<IUnitOfWork>();
        _mapperMock = new Mock<IMapper>();
        _dashboardService = new DashboardService(_unitOfWorkMock.Object, _mapperMock.Object);
    }

    [TestMethod]
    public async Task GetDashboardDataAsync_ReturnsCompleteDashboardData()
    {
        // Arrange
        var teams = new List<Team> { new() { Id = 1 }, new() { Id = 2 } };
        var groups = new List<Group> { new() { Id = 1 } };
        var matches = new List<DomainMatch>
        {
            new() { Id = 1, Status = MatchStatus.Finished, Result = new MatchResult { HomeTeamScore = 2, AwayTeamScore = 1 } },
            new() { Id = 2, Status = MatchStatus.Scheduled }
        };
        var stadiums = new List<Stadium> { new() { Id = 1 } };

        var upcomingMatches = new List<DomainMatch> { new() { Id = 2, Status = MatchStatus.Scheduled } };
        var recentMatches = new List<DomainMatch> { new() { Id = 1, Status = MatchStatus.Finished } };
        var todayMatches = new List<DomainMatch>();

        var upcomingMatchDtos = new List<MatchDto> { new() { Id = 2 } };
        var recentMatchDtos = new List<MatchDto> { new() { Id = 1 } };
        var todayMatchDtos = new List<MatchDto>();

        _unitOfWorkMock.Setup(u => u.Teams.GetAllAsync(It.IsAny<CancellationToken>()))
            .ReturnsAsync(teams);
        _unitOfWorkMock.Setup(u => u.Groups.GetAllAsync(It.IsAny<CancellationToken>()))
            .ReturnsAsync(groups);
        _unitOfWorkMock.Setup(u => u.Matches.GetAllAsync(It.IsAny<CancellationToken>()))
            .ReturnsAsync(matches);
        _unitOfWorkMock.Setup(u => u.Stadiums.GetAllAsync(It.IsAny<CancellationToken>()))
            .ReturnsAsync(stadiums);
        _unitOfWorkMock.Setup(u => u.Matches.GetUpcomingMatchesAsync(5, It.IsAny<CancellationToken>()))
            .ReturnsAsync(upcomingMatches);
        _unitOfWorkMock.Setup(u => u.Matches.GetRecentResultsAsync(5, It.IsAny<CancellationToken>()))
            .ReturnsAsync(recentMatches);
        _unitOfWorkMock.Setup(u => u.Matches.GetTodayMatchesAsync(It.IsAny<CancellationToken>()))
            .ReturnsAsync(todayMatches);

        _mapperMock.Setup(m => m.Map<List<MatchDto>>(upcomingMatches))
            .Returns(upcomingMatchDtos);
        _mapperMock.Setup(m => m.Map<List<MatchDto>>(recentMatches))
            .Returns(recentMatchDtos);
        _mapperMock.Setup(m => m.Map<List<MatchDto>>(todayMatches))
            .Returns(todayMatchDtos);

        // Act
        var result = await _dashboardService.GetDashboardDataAsync();

        // Assert
        result.Should().NotBeNull();
        result.Stats.Should().NotBeNull();
        result.Stats.TotalTeams.Should().Be(2);
        result.Stats.TotalGroups.Should().Be(1);
        result.Stats.TotalMatches.Should().Be(2);
        result.Stats.CompletedMatches.Should().Be(1);
        result.Stats.UpcomingMatches.Should().Be(1);
        result.Stats.TotalGoals.Should().Be(3);
        result.UpcomingMatches.Should().HaveCount(1);
        result.RecentResults.Should().HaveCount(1);
        result.TodayMatches.Should().BeEmpty();
    }

    [TestMethod]
    public async Task GetTournamentStatsAsync_CalculatesCorrectStatistics()
    {
        // Arrange
        var teams = Enumerable.Range(1, 48).Select(i => new Team { Id = i }).ToList();
        var groups = Enumerable.Range(1, 12).Select(i => new Group { Id = i }).ToList();
        var stadiums = Enumerable.Range(1, 16).Select(i => new Stadium { Id = i }).ToList();
        
        var matches = new List<DomainMatch>
        {
            new() { Id = 1, Status = MatchStatus.Finished, Result = new MatchResult { HomeTeamScore = 3, AwayTeamScore = 1 } },
            new() { Id = 2, Status = MatchStatus.Finished, Result = new MatchResult { HomeTeamScore = 2, AwayTeamScore = 2 } },
            new() { Id = 3, Status = MatchStatus.Scheduled },
            new() { Id = 4, Status = MatchStatus.Scheduled },
            new() { Id = 5, Status = MatchStatus.InProgress }
        };

        _unitOfWorkMock.Setup(u => u.Teams.GetAllAsync(It.IsAny<CancellationToken>()))
            .ReturnsAsync(teams);
        _unitOfWorkMock.Setup(u => u.Groups.GetAllAsync(It.IsAny<CancellationToken>()))
            .ReturnsAsync(groups);
        _unitOfWorkMock.Setup(u => u.Matches.GetAllAsync(It.IsAny<CancellationToken>()))
            .ReturnsAsync(matches);
        _unitOfWorkMock.Setup(u => u.Stadiums.GetAllAsync(It.IsAny<CancellationToken>()))
            .ReturnsAsync(stadiums);

        // Act
        var result = await _dashboardService.GetTournamentStatsAsync();

        // Assert
        result.Should().NotBeNull();
        result.TotalTeams.Should().Be(48);
        result.TotalGroups.Should().Be(12);
        result.TotalMatches.Should().Be(5);
        result.TotalStadiums.Should().Be(16);
        result.CompletedMatches.Should().Be(2);
        result.UpcomingMatches.Should().Be(2);
        result.TotalGoals.Should().Be(8); // 3+1+2+2 = 8
    }

    [TestMethod]
    public async Task GetTournamentStatsAsync_NoMatches_ReturnsZeroGoals()
    {
        // Arrange
        var teams = new List<Team> { new() { Id = 1 } };
        var groups = new List<Group> { new() { Id = 1 } };
        var matches = new List<DomainMatch>();
        var stadiums = new List<Stadium> { new() { Id = 1 } };

        _unitOfWorkMock.Setup(u => u.Teams.GetAllAsync(It.IsAny<CancellationToken>()))
            .ReturnsAsync(teams);
        _unitOfWorkMock.Setup(u => u.Groups.GetAllAsync(It.IsAny<CancellationToken>()))
            .ReturnsAsync(groups);
        _unitOfWorkMock.Setup(u => u.Matches.GetAllAsync(It.IsAny<CancellationToken>()))
            .ReturnsAsync(matches);
        _unitOfWorkMock.Setup(u => u.Stadiums.GetAllAsync(It.IsAny<CancellationToken>()))
            .ReturnsAsync(stadiums);

        // Act
        var result = await _dashboardService.GetTournamentStatsAsync();

        // Assert
        result.TotalGoals.Should().Be(0);
        result.CompletedMatches.Should().Be(0);
        result.UpcomingMatches.Should().Be(0);
    }

    [TestMethod]
    public async Task GetTopScorersAsync_ReturnsEmptyList()
    {
        // Act
        var result = await _dashboardService.GetTopScorersAsync(10);

        // Assert
        result.Should().NotBeNull();
        result.Should().BeEmpty();
    }

    [TestMethod]
    public async Task GetTopTeamsByWinsAsync_ReturnsTeamsOrderedByWins()
    {
        // Arrange
        var standings = new List<Standing>
        {
            new()
            {
                Id = 1,
                TeamId = 1,
                Won = 3,
                Points = 9,
                GoalDifference = 5,
                Played = 3,
                Team = new Team { Id = 1, Name = "Brazil", Code = "BRA" },
                Group = new Group { Id = 1, Name = "Group A" },
                GoalsFor = 8,
                GoalsAgainst = 3,
                Drawn = 0,
                Lost = 0
            },
            new()
            {
                Id = 2,
                TeamId = 2,
                Won = 2,
                Points = 6,
                GoalDifference = 3,
                Played = 3,
                Team = new Team { Id = 2, Name = "Argentina", Code = "ARG" },
                Group = new Group { Id = 1, Name = "Group A" },
                GoalsFor = 5,
                GoalsAgainst = 2,
                Drawn = 0,
                Lost = 1
            },
            new()
            {
                Id = 3,
                TeamId = 3,
                Won = 1,
                Points = 3,
                GoalDifference = 0,
                Played = 3,
                Team = new Team { Id = 3, Name = "Uruguay", Code = "URU" },
                Group = new Group { Id = 1, Name = "Group A" },
                GoalsFor = 3,
                GoalsAgainst = 3,
                Drawn = 0,
                Lost = 2
            }
        };

        _unitOfWorkMock.Setup(u => u.Standings.GetAllAsync(It.IsAny<CancellationToken>()))
            .ReturnsAsync(standings);

        // Act
        var result = await _dashboardService.GetTopTeamsByWinsAsync(3);

        // Assert
        result.Should().NotBeNull();
        var resultList = result.ToList();
        resultList.Should().HaveCount(3);
        resultList[0].TeamName.Should().Be("Brazil");
        resultList[0].Wins.Should().Be(3);
        resultList[0].WinPercentage.Should().Be(100);
        resultList[1].TeamName.Should().Be("Argentina");
        resultList[1].Wins.Should().Be(2);
        resultList[2].TeamName.Should().Be("Uruguay");
        resultList[2].Wins.Should().Be(1);
    }

    [TestMethod]
    public async Task GetTopTeamsByWinsAsync_LimitCount_ReturnsCorrectNumber()
    {
        // Arrange
        var standings = Enumerable.Range(1, 10)
            .Select(i => new Standing
            {
                Id = i,
                TeamId = i,
                Won = 10 - i,
                Points = (10 - i) * 3,
                GoalDifference = 10 - i,
                Played = 3,
                Team = new Team { Id = i, Name = $"Team {i}", Code = $"T{i:D2}" },
                Group = new Group { Id = 1, Name = "Group A" },
                GoalsFor = 5,
                GoalsAgainst = 2,
                Drawn = 0,
                Lost = i - 1
            })
            .ToList();

        _unitOfWorkMock.Setup(u => u.Standings.GetAllAsync(It.IsAny<CancellationToken>()))
            .ReturnsAsync(standings);

        // Act
        var result = await _dashboardService.GetTopTeamsByWinsAsync(5);

        // Assert
        result.Should().HaveCount(5);
    }

    [TestMethod]
    public async Task GetTopTeamsByGoalDifferenceAsync_ReturnsTeamsOrderedByGoalDifference()
    {
        // Arrange
        var standings = new List<Standing>
        {
            new()
            {
                Id = 1,
                TeamId = 1,
                Won = 2,
                Points = 6,
                GoalDifference = 8,
                Played = 3,
                Team = new Team { Id = 1, Name = "Germany", Code = "GER" },
                Group = new Group { Id = 1, Name = "Group A" },
                GoalsFor = 10,
                GoalsAgainst = 2,
                Drawn = 0,
                Lost = 1
            },
            new()
            {
                Id = 2,
                TeamId = 2,
                Won = 3,
                Points = 9,
                GoalDifference = 5,
                Played = 3,
                Team = new Team { Id = 2, Name = "Spain", Code = "ESP" },
                Group = new Group { Id = 1, Name = "Group A" },
                GoalsFor = 7,
                GoalsAgainst = 2,
                Drawn = 0,
                Lost = 0
            }
        };

        _unitOfWorkMock.Setup(u => u.Standings.GetAllAsync(It.IsAny<CancellationToken>()))
            .ReturnsAsync(standings);

        // Act
        var result = await _dashboardService.GetTopTeamsByGoalDifferenceAsync(2);

        // Assert
        var resultList = result.ToList();
        resultList.Should().HaveCount(2);
        resultList[0].TeamName.Should().Be("Germany");
        resultList[0].GoalDifference.Should().Be(8);
        resultList[1].TeamName.Should().Be("Spain");
        resultList[1].GoalDifference.Should().Be(5);
    }

    [TestMethod]
    public async Task GetTopTeamsByPointsAsync_ReturnsTeamsOrderedByPoints()
    {
        // Arrange
        var standings = new List<Standing>
        {
            new()
            {
                Id = 1,
                TeamId = 1,
                Won = 3,
                Points = 9,
                GoalDifference = 5,
                Played = 3,
                Team = new Team { Id = 1, Name = "France", Code = "FRA" },
                Group = new Group { Id = 1, Name = "Group A" },
                GoalsFor = 8,
                GoalsAgainst = 3,
                Drawn = 0,
                Lost = 0
            },
            new()
            {
                Id = 2,
                TeamId = 2,
                Won = 2,
                Points = 7,
                GoalDifference = 4,
                Played = 3,
                Team = new Team { Id = 2, Name = "England", Code = "ENG" },
                Group = new Group { Id = 1, Name = "Group A" },
                GoalsFor = 6,
                GoalsAgainst = 2,
                Drawn = 1,
                Lost = 0
            },
            new()
            {
                Id = 3,
                TeamId = 3,
                Won = 1,
                Points = 4,
                GoalDifference = 1,
                Played = 3,
                Team = new Team { Id = 3, Name = "Italy", Code = "ITA" },
                Group = new Group { Id = 1, Name = "Group A" },
                GoalsFor = 4,
                GoalsAgainst = 3,
                Drawn = 1,
                Lost = 1
            }
        };

        _unitOfWorkMock.Setup(u => u.Standings.GetAllAsync(It.IsAny<CancellationToken>()))
            .ReturnsAsync(standings);

        // Act
        var result = await _dashboardService.GetTopTeamsByPointsAsync(3);

        // Assert
        var resultList = result.ToList();
        resultList.Should().HaveCount(3);
        resultList[0].TeamName.Should().Be("France");
        resultList[0].Points.Should().Be(9);
        resultList[1].TeamName.Should().Be("England");
        resultList[1].Points.Should().Be(7);
        resultList[2].TeamName.Should().Be("Italy");
        resultList[2].Points.Should().Be(4);
    }

    [TestMethod]
    public async Task GetTopTeamsByPointsAsync_TieBreaker_UsesGoalDifference()
    {
        // Arrange
        var standings = new List<Standing>
        {
            new()
            {
                Id = 1,
                TeamId = 1,
                Won = 2,
                Points = 6,
                GoalDifference = 3,
                GoalsFor = 5,
                Played = 3,
                Team = new Team { Id = 1, Name = "Team A", Code = "TEA" },
                Group = new Group { Id = 1, Name = "Group A" },
                GoalsAgainst = 2,
                Drawn = 0,
                Lost = 1
            },
            new()
            {
                Id = 2,
                TeamId = 2,
                Won = 2,
                Points = 6,
                GoalDifference = 5,
                GoalsFor = 7,
                Played = 3,
                Team = new Team { Id = 2, Name = "Team B", Code = "TEB" },
                Group = new Group { Id = 1, Name = "Group A" },
                GoalsAgainst = 2,
                Drawn = 0,
                Lost = 1
            }
        };

        _unitOfWorkMock.Setup(u => u.Standings.GetAllAsync(It.IsAny<CancellationToken>()))
            .ReturnsAsync(standings);

        // Act
        var result = await _dashboardService.GetTopTeamsByPointsAsync(2);

        // Assert
        var resultList = result.ToList();
        resultList[0].TeamName.Should().Be("Team B"); // Higher goal difference
        resultList[0].GoalDifference.Should().Be(5);
        resultList[1].TeamName.Should().Be("Team A");
        resultList[1].GoalDifference.Should().Be(3);
    }

    [TestMethod]
    public async Task GetTopTeamsByWinsAsync_CalculatesWinPercentageCorrectly()
    {
        // Arrange
        var standings = new List<Standing>
        {
            new()
            {
                Id = 1,
                TeamId = 1,
                Won = 2,
                Points = 6,
                GoalDifference = 3,
                Played = 4,
                Team = new Team { Id = 1, Name = "Team A", Code = "TEA" },
                Group = new Group { Id = 1, Name = "Group A" },
                GoalsFor = 5,
                GoalsAgainst = 2,
                Drawn = 0,
                Lost = 2
            }
        };

        _unitOfWorkMock.Setup(u => u.Standings.GetAllAsync(It.IsAny<CancellationToken>()))
            .ReturnsAsync(standings);

        // Act
        var result = await _dashboardService.GetTopTeamsByWinsAsync(1);

        // Assert
        var resultList = result.ToList();
        resultList[0].WinPercentage.Should().Be(50); // 2 wins out of 4 matches = 50%
    }

    [TestMethod]
    public async Task GetTopTeamsByWinsAsync_NoMatchesPlayed_ReturnsZeroWinPercentage()
    {
        // Arrange
        var standings = new List<Standing>
        {
            new()
            {
                Id = 1,
                TeamId = 1,
                Won = 0,
                Points = 0,
                GoalDifference = 0,
                Played = 0,
                Team = new Team { Id = 1, Name = "Team A", Code = "TEA" },
                Group = new Group { Id = 1, Name = "Group A" },
                GoalsFor = 0,
                GoalsAgainst = 0,
                Drawn = 0,
                Lost = 0
            }
        };

        _unitOfWorkMock.Setup(u => u.Standings.GetAllAsync(It.IsAny<CancellationToken>()))
            .ReturnsAsync(standings);

        // Act
        var result = await _dashboardService.GetTopTeamsByWinsAsync(1);

        // Assert
        var resultList = result.ToList();
        resultList[0].WinPercentage.Should().Be(0);
    }
}

// Made with Bob