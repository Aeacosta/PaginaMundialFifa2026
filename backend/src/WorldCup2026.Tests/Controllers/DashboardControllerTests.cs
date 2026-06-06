using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Moq;
using WorldCup2026.API.Controllers;
using WorldCup2026.Application.DTOs.Dashboard;
using WorldCup2026.Application.DTOs.Match;
using WorldCup2026.Application.Interfaces;
using WorldCup2026.Domain.Enums;

namespace WorldCup2026.Tests.Controllers;

[TestClass]
public class DashboardControllerTests
{
    private Mock<IDashboardService> _dashboardServiceMock;
    private Mock<ILogger<DashboardController>> _loggerMock;
    private DashboardController _controller;

    [TestInitialize]
    public void Setup()
    {
        _dashboardServiceMock = new Mock<IDashboardService>();
        _loggerMock = new Mock<ILogger<DashboardController>>();
        _controller = new DashboardController(_dashboardServiceMock.Object, _loggerMock.Object);
    }

    #region GetDashboard Tests

    [TestMethod]
    public async Task GetDashboard_ReturnsOkWithDashboardData()
    {
        // Arrange
        var dashboard = new DashboardDto
        {
            Stats = new TournamentStatsDto
            {
                TotalTeams = 48,
                TotalGroups = 12,
                TotalMatches = 104,
                CompletedMatches = 50,
                UpcomingMatches = 54,
                TotalGoals = 150
            },
            UpcomingMatches = new List<MatchDto>
            {
                new MatchDto { Id = 1, HomeTeamName = "Team A", AwayTeamName = "Team B" },
                new MatchDto { Id = 2, HomeTeamName = "Team C", AwayTeamName = "Team D" }
            },
            RecentResults = new List<MatchDto>
            {
                new MatchDto { Id = 3, HomeTeamName = "Team E", AwayTeamName = "Team F" },
                new MatchDto { Id = 4, HomeTeamName = "Team G", AwayTeamName = "Team H" }
            },
            TodayMatches = new List<MatchDto>
            {
                new MatchDto { Id = 5, HomeTeamName = "Team I", AwayTeamName = "Team J" }
            }
        };

        _dashboardServiceMock.Setup(s => s.GetDashboardDataAsync(It.IsAny<CancellationToken>()))
            .ReturnsAsync(dashboard);

        // Act
        var result = await _controller.GetDashboard();

        // Assert
        var okResult = result.Result as OkObjectResult;
        Assert.IsNotNull(okResult);
        Assert.AreEqual(200, okResult.StatusCode);

        var returnedDashboard = okResult.Value as DashboardDto;
        Assert.IsNotNull(returnedDashboard);
        Assert.AreEqual(48, returnedDashboard.Stats.TotalTeams);
        Assert.AreEqual(104, returnedDashboard.Stats.TotalMatches);
    }

    #endregion

    #region GetStatistics Tests

    [TestMethod]
    public async Task GetStatistics_ReturnsOkWithStatistics()
    {
        // Arrange
        var stats = new TournamentStatsDto
        {
            TotalTeams = 48,
            TotalGroups = 12,
            TotalStadiums = 16,
            TotalMatches = 104,
            CompletedMatches = 50,
            UpcomingMatches = 54,
            TotalGoals = 150
        };

        _dashboardServiceMock.Setup(s => s.GetTournamentStatsAsync(It.IsAny<CancellationToken>()))
            .ReturnsAsync(stats);

        // Act
        var result = await _controller.GetStatistics();

        // Assert
        var okResult = result.Result as OkObjectResult;
        Assert.IsNotNull(okResult);
        Assert.AreEqual(200, okResult.StatusCode);

        var returnedStats = okResult.Value as TournamentStatsDto;
        Assert.IsNotNull(returnedStats);
        Assert.AreEqual(48, returnedStats.TotalTeams);
        Assert.AreEqual(16, returnedStats.TotalStadiums);
        Assert.AreEqual(150, returnedStats.TotalGoals);
    }

    #endregion

    #region GetUpcomingMatches Tests

    [TestMethod]
    public async Task GetUpcomingMatches_WithDefaultCount_ReturnsOkWithMatches()
    {
        // Arrange
        var dashboard = new DashboardDto
        {
            Stats = new TournamentStatsDto(),
            UpcomingMatches = new List<MatchDto>
            {
                new MatchDto { Id = 1, HomeTeamName = "Team A", AwayTeamName = "Team B" },
                new MatchDto { Id = 2, HomeTeamName = "Team C", AwayTeamName = "Team D" },
                new MatchDto { Id = 3, HomeTeamName = "Team E", AwayTeamName = "Team F" },
                new MatchDto { Id = 4, HomeTeamName = "Team G", AwayTeamName = "Team H" },
                new MatchDto { Id = 5, HomeTeamName = "Team I", AwayTeamName = "Team J" },
                new MatchDto { Id = 6, HomeTeamName = "Team K", AwayTeamName = "Team L" }
            },
            RecentResults = new List<MatchDto>(),
            TodayMatches = new List<MatchDto>()
        };

        _dashboardServiceMock.Setup(s => s.GetDashboardDataAsync(It.IsAny<CancellationToken>()))
            .ReturnsAsync(dashboard);

        // Act
        var result = await _controller.GetUpcomingMatches();

        // Assert
        var okResult = result.Result as OkObjectResult;
        Assert.IsNotNull(okResult);
        Assert.AreEqual(200, okResult.StatusCode);

        var matches = okResult.Value as IEnumerable<object>;
        Assert.IsNotNull(matches);
        Assert.AreEqual(5, matches.Count());
    }

    [TestMethod]
    public async Task GetUpcomingMatches_WithCustomCount_ReturnsSpecifiedNumber()
    {
        // Arrange
        var dashboard = new DashboardDto
        {
            Stats = new TournamentStatsDto(),
            UpcomingMatches = new List<MatchDto>
            {
                new MatchDto { Id = 1 }, new MatchDto { Id = 2 }, new MatchDto { Id = 3 },
                new MatchDto { Id = 4 }, new MatchDto { Id = 5 }, new MatchDto { Id = 6 }
            },
            RecentResults = new List<MatchDto>(),
            TodayMatches = new List<MatchDto>()
        };

        _dashboardServiceMock.Setup(s => s.GetDashboardDataAsync(It.IsAny<CancellationToken>()))
            .ReturnsAsync(dashboard);

        // Act
        var result = await _controller.GetUpcomingMatches(count: 3);

        // Assert
        var okResult = result.Result as OkObjectResult;
        Assert.IsNotNull(okResult);
        var matches = okResult.Value as IEnumerable<object>;
        Assert.IsNotNull(matches);
        Assert.AreEqual(3, matches.Count());
    }

    #endregion

    #region GetRecentResults Tests

    [TestMethod]
    public async Task GetRecentResults_WithDefaultCount_ReturnsOkWithResults()
    {
        // Arrange
        var dashboard = new DashboardDto
        {
            Stats = new TournamentStatsDto(),
            UpcomingMatches = new List<MatchDto>(),
            RecentResults = new List<MatchDto>
            {
                new MatchDto { Id = 1, HomeTeamName = "Team A", AwayTeamName = "Team B", Status = MatchStatus.Finished },
                new MatchDto { Id = 2, HomeTeamName = "Team C", AwayTeamName = "Team D", Status = MatchStatus.Finished },
                new MatchDto { Id = 3, HomeTeamName = "Team E", AwayTeamName = "Team F", Status = MatchStatus.Finished },
                new MatchDto { Id = 4, HomeTeamName = "Team G", AwayTeamName = "Team H", Status = MatchStatus.Finished },
                new MatchDto { Id = 5, HomeTeamName = "Team I", AwayTeamName = "Team J", Status = MatchStatus.Finished },
                new MatchDto { Id = 6, HomeTeamName = "Team K", AwayTeamName = "Team L", Status = MatchStatus.Finished }
            },
            TodayMatches = new List<MatchDto>()
        };

        _dashboardServiceMock.Setup(s => s.GetDashboardDataAsync(It.IsAny<CancellationToken>()))
            .ReturnsAsync(dashboard);

        // Act
        var result = await _controller.GetRecentResults();

        // Assert
        var okResult = result.Result as OkObjectResult;
        Assert.IsNotNull(okResult);
        Assert.AreEqual(200, okResult.StatusCode);

        var results = okResult.Value as IEnumerable<object>;
        Assert.IsNotNull(results);
        Assert.AreEqual(5, results.Count());
    }

    #endregion

    #region GetTodayMatches Tests

    [TestMethod]
    public async Task GetTodayMatches_ReturnsOkWithTodayMatches()
    {
        // Arrange
        var dashboard = new DashboardDto
        {
            Stats = new TournamentStatsDto(),
            UpcomingMatches = new List<MatchDto>(),
            RecentResults = new List<MatchDto>(),
            TodayMatches = new List<MatchDto>
            {
                new MatchDto { Id = 1, HomeTeamName = "Team A", AwayTeamName = "Team B" },
                new MatchDto { Id = 2, HomeTeamName = "Team C", AwayTeamName = "Team D" }
            }
        };

        _dashboardServiceMock.Setup(s => s.GetDashboardDataAsync(It.IsAny<CancellationToken>()))
            .ReturnsAsync(dashboard);

        // Act
        var result = await _controller.GetTodayMatches();

        // Assert
        var okResult = result.Result as OkObjectResult;
        Assert.IsNotNull(okResult);
        Assert.AreEqual(200, okResult.StatusCode);

        var matches = okResult.Value as IEnumerable<object>;
        Assert.IsNotNull(matches);
        Assert.AreEqual(2, matches.Count());
    }

    #endregion

    #region GetTopScorers Tests

    [TestMethod]
    public async Task GetTopScorers_WithDefaultCount_ReturnsOkWithScorers()
    {
        // Arrange
        var topScorers = new List<TopScorerDto>
        {
            new TopScorerDto { TeamId = 1, TeamName = "Team A", Goals = 10 },
            new TopScorerDto { TeamId = 2, TeamName = "Team B", Goals = 8 },
            new TopScorerDto { TeamId = 3, TeamName = "Team C", Goals = 7 }
        };

        _dashboardServiceMock.Setup(s => s.GetTopScorersAsync(10, It.IsAny<CancellationToken>()))
            .ReturnsAsync(topScorers);

        // Act
        var result = await _controller.GetTopScorers();

        // Assert
        var okResult = result.Result as OkObjectResult;
        Assert.IsNotNull(okResult);
        Assert.AreEqual(200, okResult.StatusCode);

        var scorers = okResult.Value as IEnumerable<TopScorerDto>;
        Assert.IsNotNull(scorers);
        Assert.AreEqual(3, scorers.Count());
        Assert.AreEqual(10, scorers.First().Goals);
    }

    #endregion

    #region GetTopTeamsByWins Tests

    [TestMethod]
    public async Task GetTopTeamsByWins_ReturnsOkWithTopTeams()
    {
        // Arrange
        var topTeams = new List<TeamPerformanceDto>
        {
            new TeamPerformanceDto { TeamId = 1, TeamName = "Team A", Wins = 5, Points = 15 },
            new TeamPerformanceDto { TeamId = 2, TeamName = "Team B", Wins = 4, Points = 12 }
        };

        _dashboardServiceMock.Setup(s => s.GetTopTeamsByWinsAsync(10, It.IsAny<CancellationToken>()))
            .ReturnsAsync(topTeams);

        // Act
        var result = await _controller.GetTopTeamsByWins();

        // Assert
        var okResult = result.Result as OkObjectResult;
        Assert.IsNotNull(okResult);
        var teams = okResult.Value as IEnumerable<TeamPerformanceDto>;
        Assert.IsNotNull(teams);
        Assert.AreEqual(2, teams.Count());
        Assert.AreEqual(5, teams.First().Wins);
    }

    #endregion

    #region GetTopTeamsByGoalDifference Tests

    [TestMethod]
    public async Task GetTopTeamsByGoalDifference_ReturnsOkWithTopTeams()
    {
        // Arrange
        var topTeams = new List<TeamPerformanceDto>
        {
            new TeamPerformanceDto { TeamId = 1, TeamName = "Team A", GoalDifference = 10 },
            new TeamPerformanceDto { TeamId = 2, TeamName = "Team B", GoalDifference = 8 }
        };

        _dashboardServiceMock.Setup(s => s.GetTopTeamsByGoalDifferenceAsync(10, It.IsAny<CancellationToken>()))
            .ReturnsAsync(topTeams);

        // Act
        var result = await _controller.GetTopTeamsByGoalDifference();

        // Assert
        var okResult = result.Result as OkObjectResult;
        Assert.IsNotNull(okResult);
        var teams = okResult.Value as IEnumerable<TeamPerformanceDto>;
        Assert.IsNotNull(teams);
        Assert.AreEqual(2, teams.Count());
        Assert.AreEqual(10, teams.First().GoalDifference);
    }

    #endregion

    #region GetTopTeamsByPoints Tests

    [TestMethod]
    public async Task GetTopTeamsByPoints_ReturnsOkWithTopTeams()
    {
        // Arrange
        var topTeams = new List<TeamPerformanceDto>
        {
            new TeamPerformanceDto { TeamId = 1, TeamName = "Team A", Points = 15 },
            new TeamPerformanceDto { TeamId = 2, TeamName = "Team B", Points = 12 },
            new TeamPerformanceDto { TeamId = 3, TeamName = "Team C", Points = 10 }
        };

        _dashboardServiceMock.Setup(s => s.GetTopTeamsByPointsAsync(10, It.IsAny<CancellationToken>()))
            .ReturnsAsync(topTeams);

        // Act
        var result = await _controller.GetTopTeamsByPoints();

        // Assert
        var okResult = result.Result as OkObjectResult;
        Assert.IsNotNull(okResult);
        var teams = okResult.Value as IEnumerable<TeamPerformanceDto>;
        Assert.IsNotNull(teams);
        Assert.AreEqual(3, teams.Count());
        Assert.AreEqual(15, teams.First().Points);
    }

    #endregion

    #region GetTournamentProgress Tests

    [TestMethod]
    public async Task GetTournamentProgress_ReturnsOkWithProgressData()
    {
        // Arrange
        var stats = new TournamentStatsDto
        {
            TotalMatches = 104,
            CompletedMatches = 52,
            UpcomingMatches = 52,
            TotalGoals = 156
        };

        _dashboardServiceMock.Setup(s => s.GetTournamentStatsAsync(It.IsAny<CancellationToken>()))
            .ReturnsAsync(stats);

        // Act
        var result = await _controller.GetTournamentProgress();

        // Assert
        var okResult = result.Result as OkObjectResult;
        Assert.IsNotNull(okResult);
        Assert.AreEqual(200, okResult.StatusCode);

        var progress = okResult.Value;
        Assert.IsNotNull(progress);

        // Use reflection to check properties
        var progressType = progress.GetType();
        var totalMatchesProperty = progressType.GetProperty("TotalMatches");
        var completedMatchesProperty = progressType.GetProperty("CompletedMatches");
        var completionPercentageProperty = progressType.GetProperty("CompletionPercentage");

        Assert.IsNotNull(totalMatchesProperty);
        Assert.IsNotNull(completedMatchesProperty);
        Assert.IsNotNull(completionPercentageProperty);

        Assert.AreEqual(104, totalMatchesProperty.GetValue(progress));
        Assert.AreEqual(52, completedMatchesProperty.GetValue(progress));
        Assert.AreEqual(50.0, completionPercentageProperty.GetValue(progress));
    }

    #endregion

    #region GetQuickFacts Tests

    [TestMethod]
    public async Task GetQuickFacts_ReturnsOkWithQuickFacts()
    {
        // Arrange
        var stats = new TournamentStatsDto
        {
            TotalTeams = 48,
            TotalGroups = 12,
            TotalStadiums = 16,
            TotalMatches = 104,
            CompletedMatches = 50,
            UpcomingMatches = 54,
            TotalGoals = 150
        };

        _dashboardServiceMock.Setup(s => s.GetTournamentStatsAsync(It.IsAny<CancellationToken>()))
            .ReturnsAsync(stats);

        // Act
        var result = await _controller.GetQuickFacts();

        // Assert
        var okResult = result.Result as OkObjectResult;
        Assert.IsNotNull(okResult);
        Assert.AreEqual(200, okResult.StatusCode);

        var quickFacts = okResult.Value;
        Assert.IsNotNull(quickFacts);

        // Use reflection to check properties
        var factsType = quickFacts.GetType();
        var totalTeamsProperty = factsType.GetProperty("TotalTeams");
        var totalGoalsProperty = factsType.GetProperty("TotalGoals");

        Assert.IsNotNull(totalTeamsProperty);
        Assert.IsNotNull(totalGoalsProperty);

        Assert.AreEqual(48, totalTeamsProperty.GetValue(quickFacts));
        Assert.AreEqual(150, totalGoalsProperty.GetValue(quickFacts));
    }

    #endregion

    #region GetTournamentOverview Tests

    [TestMethod]
    public async Task GetTournamentOverview_ReturnsOkWithOverview()
    {
        // Arrange
        var dashboard = new DashboardDto
        {
            Stats = new TournamentStatsDto { TotalTeams = 48, TotalMatches = 104 },
            UpcomingMatches = new List<MatchDto> { new MatchDto { Id = 1 }, new MatchDto { Id = 2 } },
            RecentResults = new List<MatchDto> { new MatchDto { Id = 3 } },
            TodayMatches = new List<MatchDto> { new MatchDto { Id = 4 }, new MatchDto { Id = 5 } }
        };

        var topScorers = new List<TopScorerDto>
        {
            new TopScorerDto { TeamId = 1, TeamName = "Team A", Goals = 10 }
        };

        var topTeams = new List<TeamPerformanceDto>
        {
            new TeamPerformanceDto { TeamId = 1, TeamName = "Team A", Points = 15 }
        };

        _dashboardServiceMock.Setup(s => s.GetDashboardDataAsync(It.IsAny<CancellationToken>()))
            .ReturnsAsync(dashboard);
        _dashboardServiceMock.Setup(s => s.GetTopScorersAsync(5, It.IsAny<CancellationToken>()))
            .ReturnsAsync(topScorers);
        _dashboardServiceMock.Setup(s => s.GetTopTeamsByPointsAsync(5, It.IsAny<CancellationToken>()))
            .ReturnsAsync(topTeams);

        // Act
        var result = await _controller.GetTournamentOverview();

        // Assert
        var okResult = result.Result as OkObjectResult;
        Assert.IsNotNull(okResult);
        Assert.AreEqual(200, okResult.StatusCode);

        var overview = okResult.Value;
        Assert.IsNotNull(overview);

        // Use reflection to check properties
        var overviewType = overview.GetType();
        var upcomingMatchesCountProperty = overviewType.GetProperty("UpcomingMatchesCount");
        var todayMatchesCountProperty = overviewType.GetProperty("TodayMatchesCount");

        Assert.IsNotNull(upcomingMatchesCountProperty);
        Assert.IsNotNull(todayMatchesCountProperty);

        Assert.AreEqual(2, upcomingMatchesCountProperty.GetValue(overview));
        Assert.AreEqual(2, todayMatchesCountProperty.GetValue(overview));
    }

    #endregion
}

// Made with Bob
