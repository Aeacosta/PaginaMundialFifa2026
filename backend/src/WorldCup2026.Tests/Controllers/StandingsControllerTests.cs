using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Moq;
using WorldCup2026.API.Controllers;
using WorldCup2026.Application.DTOs.Standing;
using WorldCup2026.Application.Interfaces;

namespace WorldCup2026.Tests.Controllers;

[TestClass]
public class StandingsControllerTests
{
    private Mock<IStandingService> _standingServiceMock;
    private Mock<ILogger<StandingsController>> _loggerMock;
    private StandingsController _controller;

    [TestInitialize]
    public void Setup()
    {
        _standingServiceMock = new Mock<IStandingService>();
        _loggerMock = new Mock<ILogger<StandingsController>>();
        _controller = new StandingsController(_standingServiceMock.Object, _loggerMock.Object);
    }

    #region GetGroupStandings Tests

    [TestMethod]
    public async Task GetGroupStandings_ExistingGroup_ReturnsOkWithStandings()
    {
        // Arrange
        var standings = new List<StandingDto>
        {
            new StandingDto { Id = 1, GroupId = 1, TeamId = 1, TeamName = "Team A", Points = 9, Position = 1 },
            new StandingDto { Id = 2, GroupId = 1, TeamId = 2, TeamName = "Team B", Points = 6, Position = 2 },
            new StandingDto { Id = 3, GroupId = 1, TeamId = 3, TeamName = "Team C", Points = 3, Position = 3 },
            new StandingDto { Id = 4, GroupId = 1, TeamId = 4, TeamName = "Team D", Points = 0, Position = 4 }
        };

        _standingServiceMock.Setup(s => s.GetStandingsByGroupAsync(1, It.IsAny<CancellationToken>()))
            .ReturnsAsync(standings);

        // Act
        var result = await _controller.GetGroupStandings(1);

        // Assert
        var okResult = result.Result as OkObjectResult;
        Assert.IsNotNull(okResult);
        Assert.AreEqual(200, okResult.StatusCode);

        var returnedStandings = okResult.Value as IEnumerable<StandingDto>;
        Assert.IsNotNull(returnedStandings);
        Assert.AreEqual(4, returnedStandings.Count());
    }

    [TestMethod]
    public async Task GetGroupStandings_NonExistingGroup_ReturnsNotFound()
    {
        // Arrange
        _standingServiceMock.Setup(s => s.GetStandingsByGroupAsync(999, It.IsAny<CancellationToken>()))
            .ReturnsAsync(new List<StandingDto>());

        // Act
        var result = await _controller.GetGroupStandings(999);

        // Assert
        var notFoundResult = result.Result as NotFoundObjectResult;
        Assert.IsNotNull(notFoundResult);
        Assert.AreEqual(404, notFoundResult.StatusCode);
    }

    [TestMethod]
    public async Task GetGroupStandings_NullStandings_ReturnsNotFound()
    {
        // Arrange
        _standingServiceMock.Setup(s => s.GetStandingsByGroupAsync(1, It.IsAny<CancellationToken>()))
            .ReturnsAsync((IEnumerable<StandingDto>?)null);

        // Act
        var result = await _controller.GetGroupStandings(1);

        // Assert
        var notFoundResult = result.Result as NotFoundObjectResult;
        Assert.IsNotNull(notFoundResult);
        Assert.AreEqual(404, notFoundResult.StatusCode);
    }

    #endregion

    #region GetAllStandings Tests

    [TestMethod]
    public async Task GetAllStandings_ReturnsOkWithAllStandings()
    {
        // Arrange
        var standings = new List<StandingDto>();
        for (int i = 1; i <= 48; i++)
        {
            standings.Add(new StandingDto
            {
                Id = i,
                GroupId = (i - 1) / 4 + 1,
                TeamId = i,
                TeamName = $"Team {i}",
                Points = 9 - (i % 4) * 3,
                Position = (i - 1) % 4 + 1
            });
        }

        _standingServiceMock.Setup(s => s.GetTopTeamsAsync(48, It.IsAny<CancellationToken>()))
            .ReturnsAsync(standings);

        // Act
        var result = await _controller.GetAllStandings();

        // Assert
        var okResult = result.Result as OkObjectResult;
        Assert.IsNotNull(okResult);
        Assert.AreEqual(200, okResult.StatusCode);

        var returnedStandings = okResult.Value as IEnumerable<StandingDto>;
        Assert.IsNotNull(returnedStandings);
        Assert.AreEqual(48, returnedStandings.Count());
    }

    #endregion

    #region GetTeamStanding Tests

    [TestMethod]
    public async Task GetTeamStanding_ExistingTeam_ReturnsOkWithStanding()
    {
        // Arrange
        var standing = new StandingDto
        {
            Id = 1,
            GroupId = 1,
            TeamId = 1,
            TeamName = "Team A",
            Points = 9,
            Position = 1,
            Played = 3,
            Won = 3,
            Drawn = 0,
            Lost = 0
        };

        _standingServiceMock.Setup(s => s.GetStandingByTeamAsync(1, It.IsAny<CancellationToken>()))
            .ReturnsAsync(standing);

        // Act
        var result = await _controller.GetTeamStanding(1);

        // Assert
        var okResult = result.Result as OkObjectResult;
        Assert.IsNotNull(okResult);
        Assert.AreEqual(200, okResult.StatusCode);

        var returnedStanding = okResult.Value as StandingDto;
        Assert.IsNotNull(returnedStanding);
        Assert.AreEqual(1, returnedStanding.TeamId);
        Assert.AreEqual("Team A", returnedStanding.TeamName);
        Assert.AreEqual(9, returnedStanding.Points);
    }

    [TestMethod]
    public async Task GetTeamStanding_NonExistingTeam_ReturnsNotFound()
    {
        // Arrange
        _standingServiceMock.Setup(s => s.GetStandingByTeamAsync(999, It.IsAny<CancellationToken>()))
            .ReturnsAsync((StandingDto?)null);

        // Act
        var result = await _controller.GetTeamStanding(999);

        // Assert
        var notFoundResult = result.Result as NotFoundObjectResult;
        Assert.IsNotNull(notFoundResult);
        Assert.AreEqual(404, notFoundResult.StatusCode);
    }

    #endregion

    #region RecalculateGroupStandings Tests

    [TestMethod]
    public async Task RecalculateGroupStandings_ValidGroup_ReturnsOk()
    {
        // Arrange
        _standingServiceMock.Setup(s => s.RecalculateGroupStandingsAsync(1, It.IsAny<CancellationToken>()))
            .Returns(Task.CompletedTask);

        // Act
        var result = await _controller.RecalculateGroupStandings(1);

        // Assert
        var okResult = result as OkObjectResult;
        Assert.IsNotNull(okResult);
        Assert.AreEqual(200, okResult.StatusCode);

        _standingServiceMock.Verify(s => s.RecalculateGroupStandingsAsync(1, It.IsAny<CancellationToken>()), Times.Once);
    }

    #endregion

    #region RecalculateAllStandings Tests

    [TestMethod]
    public async Task RecalculateAllStandings_ReturnsOk()
    {
        // Arrange
        _standingServiceMock.Setup(s => s.RecalculateAllStandingsAsync(It.IsAny<CancellationToken>()))
            .Returns(Task.CompletedTask);

        // Act
        var result = await _controller.RecalculateAllStandings();

        // Assert
        var okResult = result as OkObjectResult;
        Assert.IsNotNull(okResult);
        Assert.AreEqual(200, okResult.StatusCode);

        _standingServiceMock.Verify(s => s.RecalculateAllStandingsAsync(It.IsAny<CancellationToken>()), Times.Once);
    }

    #endregion

    #region GetQualifiedTeams Tests

    [TestMethod]
    public async Task GetQualifiedTeams_ExistingGroup_ReturnsOkWithTopTwoTeams()
    {
        // Arrange
        var qualifiedTeams = new List<StandingDto>
        {
            new StandingDto { Id = 1, GroupId = 1, TeamId = 1, TeamName = "Team A", Points = 9, Position = 1 },
            new StandingDto { Id = 2, GroupId = 1, TeamId = 2, TeamName = "Team B", Points = 6, Position = 2 }
        };

        _standingServiceMock.Setup(s => s.GetQualifiedTeamsFromGroupAsync(1, 2, It.IsAny<CancellationToken>()))
            .ReturnsAsync(qualifiedTeams);

        // Act
        var result = await _controller.GetQualifiedTeams(1);

        // Assert
        var okResult = result.Result as OkObjectResult;
        Assert.IsNotNull(okResult);
        Assert.AreEqual(200, okResult.StatusCode);

        var returnedTeams = okResult.Value as IEnumerable<StandingDto>;
        Assert.IsNotNull(returnedTeams);
        Assert.AreEqual(2, returnedTeams.Count());
        Assert.AreEqual(1, returnedTeams.First().Position);
        Assert.AreEqual(2, returnedTeams.Last().Position);
    }

    [TestMethod]
    public async Task GetQualifiedTeams_NonExistingGroup_ReturnsNotFound()
    {
        // Arrange
        _standingServiceMock.Setup(s => s.GetQualifiedTeamsFromGroupAsync(999, 2, It.IsAny<CancellationToken>()))
            .ReturnsAsync(new List<StandingDto>());

        // Act
        var result = await _controller.GetQualifiedTeams(999);

        // Assert
        var notFoundResult = result.Result as NotFoundObjectResult;
        Assert.IsNotNull(notFoundResult);
        Assert.AreEqual(404, notFoundResult.StatusCode);
    }

    [TestMethod]
    public async Task GetQualifiedTeams_NullResult_ReturnsNotFound()
    {
        // Arrange
        _standingServiceMock.Setup(s => s.GetQualifiedTeamsFromGroupAsync(1, 2, It.IsAny<CancellationToken>()))
            .ReturnsAsync((IEnumerable<StandingDto>?)null);

        // Act
        var result = await _controller.GetQualifiedTeams(1);

        // Assert
        var notFoundResult = result.Result as NotFoundObjectResult;
        Assert.IsNotNull(notFoundResult);
        Assert.AreEqual(404, notFoundResult.StatusCode);
    }

    #endregion

    #region GetStandingsSummary Tests

    [TestMethod]
    public async Task GetStandingsSummary_ReturnsOkWithSummary()
    {
        // Arrange
        var standings = new List<StandingDto>
        {
            new StandingDto { Id = 1, GroupId = 1, TeamId = 1, TeamName = "Team A", Points = 9, Played = 3, GoalsFor = 10, GoalsAgainst = 2, GoalDifference = 8 },
            new StandingDto { Id = 2, GroupId = 1, TeamId = 2, TeamName = "Team B", Points = 6, Played = 3, GoalsFor = 8, GoalsAgainst = 4, GoalDifference = 4 },
            new StandingDto { Id = 3, GroupId = 1, TeamId = 3, TeamName = "Team C", Points = 3, Played = 3, GoalsFor = 5, GoalsAgainst = 7, GoalDifference = -2 },
            new StandingDto { Id = 4, GroupId = 1, TeamId = 4, TeamName = "Team D", Points = 0, Played = 3, GoalsFor = 2, GoalsAgainst = 12, GoalDifference = -10 },
            new StandingDto { Id = 5, GroupId = 2, TeamId = 5, TeamName = "Team E", Points = 7, Played = 3, GoalsFor = 7, GoalsAgainst = 3, GoalDifference = 4 }
        };

        _standingServiceMock.Setup(s => s.GetTopTeamsAsync(48, It.IsAny<CancellationToken>()))
            .ReturnsAsync(standings);

        // Act
        var result = await _controller.GetStandingsSummary();

        // Assert
        var okResult = result.Result as OkObjectResult;
        Assert.IsNotNull(okResult);
        Assert.AreEqual(200, okResult.StatusCode);

        // Verify the summary structure
        var summary = okResult.Value;
        Assert.IsNotNull(summary);

        // Use reflection to check properties
        var summaryType = summary.GetType();
        var totalTeamsProperty = summaryType.GetProperty("TotalTeams");
        var totalGroupsProperty = summaryType.GetProperty("TotalGroups");
        var totalMatchesProperty = summaryType.GetProperty("TotalMatches");
        var totalGoalsProperty = summaryType.GetProperty("TotalGoals");

        Assert.IsNotNull(totalTeamsProperty);
        Assert.IsNotNull(totalGroupsProperty);
        Assert.IsNotNull(totalMatchesProperty);
        Assert.IsNotNull(totalGoalsProperty);

        Assert.AreEqual(5, totalTeamsProperty.GetValue(summary));
        Assert.AreEqual(2, totalGroupsProperty.GetValue(summary));
        Assert.AreEqual(15, totalMatchesProperty.GetValue(summary)); // 3+3+3+3+3
        Assert.AreEqual(32, totalGoalsProperty.GetValue(summary)); // 10+8+5+2+7
    }

    #endregion
}

// Made with Bob
