using FluentValidation;
using FluentValidation.Results;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Moq;
using WorldCup2026.API.Controllers;
using WorldCup2026.Application.DTOs.Common;
using WorldCup2026.Application.DTOs.Match;
using WorldCup2026.Application.Interfaces;
using WorldCup2026.Domain.Enums;

namespace WorldCup2026.Tests.Controllers;

[TestClass]
public class MatchesControllerTests
{
    private Mock<IMatchService> _matchServiceMock;
    private Mock<ILogger<MatchesController>> _loggerMock;
    private MatchesController _controller;

    [TestInitialize]
    public void Setup()
    {
        _matchServiceMock = new Mock<IMatchService>();
        _loggerMock = new Mock<ILogger<MatchesController>>();
        _controller = new MatchesController(_matchServiceMock.Object, _loggerMock.Object);
    }

    #region GetMatches Tests

    [TestMethod]
    public async Task GetMatches_WithNoFilters_ReturnsOkWithPagedResult()
    {
        // Arrange
        var matches = new List<MatchDto>
        {
            new MatchDto { Id = 1, HomeTeamId = 1, AwayTeamId = 2, Phase = MatchPhase.GroupStage },
            new MatchDto { Id = 2, HomeTeamId = 3, AwayTeamId = 4, Phase = MatchPhase.GroupStage }
        };
        var pagedResult = new PagedResult<MatchDto>
        {
            Items = matches,
            TotalCount = 2,
            PageNumber = 1,
            PageSize = 10
        };

        _matchServiceMock.Setup(s => s.GetAllMatchesAsync(
            1, 10, null, null, null, null, null, null, It.IsAny<CancellationToken>()))
            .ReturnsAsync(pagedResult);

        // Act
        var result = await _controller.GetMatches();

        // Assert
        var okResult = result.Result as OkObjectResult;
        Assert.IsNotNull(okResult);
        Assert.AreEqual(200, okResult.StatusCode);

        var returnedResult = okResult.Value as PagedResult<MatchDto>;
        Assert.IsNotNull(returnedResult);
        Assert.AreEqual(2, returnedResult.Items.Count());
        Assert.AreEqual(2, returnedResult.TotalCount);
    }

    [TestMethod]
    public async Task GetMatches_WithFilters_ReturnsFilteredMatches()
    {
        // Arrange
        var matches = new List<MatchDto>
        {
            new MatchDto { Id = 1, HomeTeamId = 1, AwayTeamId = 2, Phase = MatchPhase.GroupStage, Status = MatchStatus.Scheduled }
        };
        var pagedResult = new PagedResult<MatchDto>
        {
            Items = matches,
            TotalCount = 1,
            PageNumber = 1,
            PageSize = 10
        };

        _matchServiceMock.Setup(s => s.GetAllMatchesAsync(
            1, 10, MatchPhase.GroupStage, MatchStatus.Scheduled, 1, null, null, null, It.IsAny<CancellationToken>()))
            .ReturnsAsync(pagedResult);

        // Act
        var result = await _controller.GetMatches(
            groupId: 1,
            phase: MatchPhase.GroupStage,
            status: MatchStatus.Scheduled);

        // Assert
        var okResult = result.Result as OkObjectResult;
        Assert.IsNotNull(okResult);
        var returnedResult = okResult.Value as PagedResult<MatchDto>;
        Assert.IsNotNull(returnedResult);
        Assert.AreEqual(1, returnedResult.Items.Count());
    }

    #endregion

    #region GetMatch Tests

    [TestMethod]
    public async Task GetMatch_ExistingMatch_ReturnsOkWithMatch()
    {
        // Arrange
        var match = new MatchDto { Id = 1, HomeTeamId = 1, AwayTeamId = 2 };
        _matchServiceMock.Setup(s => s.GetMatchByIdAsync(1, It.IsAny<CancellationToken>()))
            .ReturnsAsync(match);

        // Act
        var result = await _controller.GetMatch(1);

        // Assert
        var okResult = result.Result as OkObjectResult;
        Assert.IsNotNull(okResult);
        Assert.AreEqual(200, okResult.StatusCode);

        var returnedMatch = okResult.Value as MatchDto;
        Assert.IsNotNull(returnedMatch);
        Assert.AreEqual(1, returnedMatch.Id);
    }

    [TestMethod]
    public async Task GetMatch_NonExistingMatch_ReturnsNotFound()
    {
        // Arrange
        _matchServiceMock.Setup(s => s.GetMatchByIdAsync(999, It.IsAny<CancellationToken>()))
            .ReturnsAsync((MatchDto?)null);

        // Act
        var result = await _controller.GetMatch(999);

        // Assert
        var notFoundResult = result.Result as NotFoundObjectResult;
        Assert.IsNotNull(notFoundResult);
        Assert.AreEqual(404, notFoundResult.StatusCode);
    }

    #endregion

    #region GetMatchesByGroup Tests

    [TestMethod]
    public async Task GetMatchesByGroup_ExistingGroup_ReturnsMatches()
    {
        // Arrange
        var matches = new List<MatchDto>
        {
            new MatchDto { Id = 1, GroupId = 1, HomeTeamId = 1, AwayTeamId = 2 },
            new MatchDto { Id = 2, GroupId = 1, HomeTeamId = 3, AwayTeamId = 4 }
        };

        _matchServiceMock.Setup(s => s.GetMatchesByGroupAsync(1, It.IsAny<CancellationToken>()))
            .ReturnsAsync(matches);

        // Act
        var result = await _controller.GetMatchesByGroup(1);

        // Assert
        var okResult = result.Result as OkObjectResult;
        Assert.IsNotNull(okResult);
        Assert.AreEqual(200, okResult.StatusCode);

        var returnedMatches = okResult.Value as IEnumerable<MatchDto>;
        Assert.IsNotNull(returnedMatches);
        Assert.AreEqual(2, returnedMatches.Count());
    }

    #endregion

    #region GetMatchesByTeam Tests

    [TestMethod]
    public async Task GetMatchesByTeam_ExistingTeam_ReturnsMatches()
    {
        // Arrange
        var matches = new List<MatchDto>
        {
            new MatchDto { Id = 1, HomeTeamId = 1, AwayTeamId = 2 },
            new MatchDto { Id = 2, HomeTeamId = 3, AwayTeamId = 1 }
        };

        _matchServiceMock.Setup(s => s.GetMatchesByTeamAsync(1, It.IsAny<CancellationToken>()))
            .ReturnsAsync(matches);

        // Act
        var result = await _controller.GetMatchesByTeam(1);

        // Assert
        var okResult = result.Result as OkObjectResult;
        Assert.IsNotNull(okResult);
        var returnedMatches = okResult.Value as IEnumerable<MatchDto>;
        Assert.IsNotNull(returnedMatches);
        Assert.AreEqual(2, returnedMatches.Count());
    }

    #endregion

    #region GetMatchesByPhase Tests

    [TestMethod]
    public async Task GetMatchesByPhase_ValidPhase_ReturnsMatches()
    {
        // Arrange
        var matches = new List<MatchDto>
        {
            new MatchDto { Id = 1, Phase = MatchPhase.GroupStage },
            new MatchDto { Id = 2, Phase = MatchPhase.GroupStage }
        };

        _matchServiceMock.Setup(s => s.GetMatchesByPhaseAsync(MatchPhase.GroupStage, It.IsAny<CancellationToken>()))
            .ReturnsAsync(matches);

        // Act
        var result = await _controller.GetMatchesByPhase(MatchPhase.GroupStage);

        // Assert
        var okResult = result.Result as OkObjectResult;
        Assert.IsNotNull(okResult);
        var returnedMatches = okResult.Value as IEnumerable<MatchDto>;
        Assert.IsNotNull(returnedMatches);
        Assert.AreEqual(2, returnedMatches.Count());
        Assert.IsTrue(returnedMatches.All(m => m.Phase == MatchPhase.GroupStage));
    }

    #endregion

    #region GetMatchesByStatus Tests

    [TestMethod]
    public async Task GetMatchesByStatus_ValidStatus_ReturnsMatches()
    {
        // Arrange
        var matches = new List<MatchDto>
        {
            new MatchDto { Id = 1, Status = MatchStatus.Scheduled },
            new MatchDto { Id = 2, Status = MatchStatus.Scheduled }
        };

        _matchServiceMock.Setup(s => s.GetMatchesByStatusAsync(MatchStatus.Scheduled, It.IsAny<CancellationToken>()))
            .ReturnsAsync(matches);

        // Act
        var result = await _controller.GetMatchesByStatus(MatchStatus.Scheduled);

        // Assert
        var okResult = result.Result as OkObjectResult;
        Assert.IsNotNull(okResult);
        var returnedMatches = okResult.Value as IEnumerable<MatchDto>;
        Assert.IsNotNull(returnedMatches);
        Assert.AreEqual(2, returnedMatches.Count());
        Assert.IsTrue(returnedMatches.All(m => m.Status == MatchStatus.Scheduled));
    }

    #endregion

    #region GetUpcomingMatches Tests

    [TestMethod]
    public async Task GetUpcomingMatches_WithDefaultCount_ReturnsMatches()
    {
        // Arrange
        var matches = new List<MatchDto>
        {
            new MatchDto { Id = 1, MatchDate = DateTime.UtcNow.AddDays(1) },
            new MatchDto { Id = 2, MatchDate = DateTime.UtcNow.AddDays(2) }
        };

        _matchServiceMock.Setup(s => s.GetUpcomingMatchesAsync(10, It.IsAny<CancellationToken>()))
            .ReturnsAsync(matches);

        // Act
        var result = await _controller.GetUpcomingMatches();

        // Assert
        var okResult = result.Result as OkObjectResult;
        Assert.IsNotNull(okResult);
        var returnedMatches = okResult.Value as IEnumerable<MatchDto>;
        Assert.IsNotNull(returnedMatches);
        Assert.AreEqual(2, returnedMatches.Count());
    }

    [TestMethod]
    public async Task GetUpcomingMatches_WithCustomCount_ReturnsSpecifiedNumberOfMatches()
    {
        // Arrange
        var matches = new List<MatchDto>
        {
            new MatchDto { Id = 1, MatchDate = DateTime.UtcNow.AddDays(1) },
            new MatchDto { Id = 2, MatchDate = DateTime.UtcNow.AddDays(2) },
            new MatchDto { Id = 3, MatchDate = DateTime.UtcNow.AddDays(3) }
        };

        _matchServiceMock.Setup(s => s.GetUpcomingMatchesAsync(3, It.IsAny<CancellationToken>()))
            .ReturnsAsync(matches);

        // Act
        var result = await _controller.GetUpcomingMatches(count: 3);

        // Assert
        var okResult = result.Result as OkObjectResult;
        Assert.IsNotNull(okResult);
        var returnedMatches = okResult.Value as IEnumerable<MatchDto>;
        Assert.IsNotNull(returnedMatches);
        Assert.AreEqual(3, returnedMatches.Count());
    }

    #endregion

    #region GetRecentMatches Tests

    [TestMethod]
    public async Task GetRecentMatches_WithDefaultCount_ReturnsMatches()
    {
        // Arrange
        var matches = new List<MatchDto>
        {
            new MatchDto { Id = 1, MatchDate = DateTime.UtcNow.AddDays(-1), Status = MatchStatus.Finished },
            new MatchDto { Id = 2, MatchDate = DateTime.UtcNow.AddDays(-2), Status = MatchStatus.Finished }
        };

        _matchServiceMock.Setup(s => s.GetRecentMatchesAsync(10, It.IsAny<CancellationToken>()))
            .ReturnsAsync(matches);

        // Act
        var result = await _controller.GetRecentMatches();

        // Assert
        var okResult = result.Result as OkObjectResult;
        Assert.IsNotNull(okResult);
        var returnedMatches = okResult.Value as IEnumerable<MatchDto>;
        Assert.IsNotNull(returnedMatches);
        Assert.AreEqual(2, returnedMatches.Count());
    }

    [TestMethod]
    public async Task GetRecentMatches_WithCustomCount_ReturnsSpecifiedNumberOfMatches()
    {
        // Arrange
        var matches = new List<MatchDto>
        {
            new MatchDto { Id = 1, MatchDate = DateTime.UtcNow.AddDays(-1), Status = MatchStatus.Finished },
            new MatchDto { Id = 2, MatchDate = DateTime.UtcNow.AddDays(-2), Status = MatchStatus.Finished },
            new MatchDto { Id = 3, MatchDate = DateTime.UtcNow.AddDays(-3), Status = MatchStatus.Finished },
            new MatchDto { Id = 4, MatchDate = DateTime.UtcNow.AddDays(-4), Status = MatchStatus.Finished },
            new MatchDto { Id = 5, MatchDate = DateTime.UtcNow.AddDays(-5), Status = MatchStatus.Finished }
        };

        _matchServiceMock.Setup(s => s.GetRecentMatchesAsync(5, It.IsAny<CancellationToken>()))
            .ReturnsAsync(matches);

        // Act
        var result = await _controller.GetRecentMatches(count: 5);

        // Assert
        var okResult = result.Result as OkObjectResult;
        Assert.IsNotNull(okResult);
        var returnedMatches = okResult.Value as IEnumerable<MatchDto>;
        Assert.IsNotNull(returnedMatches);
        Assert.AreEqual(5, returnedMatches.Count());
    }

    #endregion

    #region CreateMatch Tests

    [TestMethod]
    public async Task CreateMatch_ValidData_ReturnsCreatedAtAction()
    {
        // Arrange
        var createDto = new CreateMatchDto
        {
            HomeTeamId = 1,
            AwayTeamId = 2,
            StadiumId = 1,
            MatchDate = DateTime.UtcNow.AddDays(30),
            Phase = MatchPhase.GroupStage,
            GroupId = 1
        };

        var createdMatch = new MatchDto
        {
            Id = 1,
            HomeTeamId = 1,
            AwayTeamId = 2,
            StadiumId = 1,
            MatchDate = createDto.MatchDate,
            Phase = MatchPhase.GroupStage,
            GroupId = 1,
            Status = MatchStatus.Scheduled
        };

        _matchServiceMock.Setup(s => s.CreateMatchAsync(createDto, It.IsAny<CancellationToken>()))
            .ReturnsAsync(createdMatch);

        // Act
        var result = await _controller.CreateMatch(createDto);

        // Assert
        var createdResult = result.Result as CreatedAtActionResult;
        Assert.IsNotNull(createdResult);
        Assert.AreEqual(201, createdResult.StatusCode);
        Assert.AreEqual(nameof(MatchesController.GetMatch), createdResult.ActionName);

        var returnedMatch = createdResult.Value as MatchDto;
        Assert.IsNotNull(returnedMatch);
        Assert.AreEqual(1, returnedMatch.Id);
        Assert.AreEqual(1, returnedMatch.HomeTeamId);
        Assert.AreEqual(2, returnedMatch.AwayTeamId);
    }

    #endregion

    #region UpdateMatch Tests

    [TestMethod]
    public async Task UpdateMatch_AnyRequest_ReturnsBadRequest()
    {
        // Arrange
        var updateDto = new UpdateMatchDto
        {
            HomeTeamId = Guid.NewGuid(),
            AwayTeamId = Guid.NewGuid(),
            StadiumId = Guid.NewGuid(),
            MatchDate = DateTime.UtcNow.AddDays(30),
            Phase = MatchPhase.GroupStage,
            Status = MatchStatus.Scheduled
        };

        // Act
        var result = await _controller.UpdateMatch(1, updateDto);

        // Assert
        var badRequestResult = result.Result as BadRequestObjectResult;
        Assert.IsNotNull(badRequestResult);
        Assert.AreEqual(400, badRequestResult.StatusCode);
    }

    #endregion

    #region UpdateMatchResult Tests

    [TestMethod]
    public async Task UpdateMatchResult_ExistingMatch_ReturnsOkWithUpdatedMatch()
    {
        // Arrange
        var resultDto = new UpdateMatchResultDto
        {
            HomeTeamScore = 2,
            AwayTeamScore = 1
        };

        var updatedMatch = new MatchDto
        {
            Id = 1,
            HomeTeamId = 1,
            AwayTeamId = 2,
            Status = MatchStatus.Finished,
            Result = new MatchResultDto
            {
                Id = 1,
                MatchId = 1,
                HomeTeamScore = 2,
                AwayTeamScore = 1
            }
        };

        _matchServiceMock.Setup(s => s.UpdateMatchResultAsync(1, resultDto, It.IsAny<CancellationToken>()))
            .ReturnsAsync(updatedMatch);

        // Act
        var result = await _controller.UpdateMatchResult(1, resultDto);

        // Assert
        var okResult = result.Result as OkObjectResult;
        Assert.IsNotNull(okResult);
        Assert.AreEqual(200, okResult.StatusCode);

        var returnedMatch = okResult.Value as MatchDto;
        Assert.IsNotNull(returnedMatch);
        Assert.AreEqual(1, returnedMatch.Id);
        Assert.AreEqual(MatchStatus.Finished, returnedMatch.Status);
        Assert.IsNotNull(returnedMatch.Result);
        Assert.AreEqual(2, returnedMatch.Result.HomeTeamScore);
        Assert.AreEqual(1, returnedMatch.Result.AwayTeamScore);
    }

    [TestMethod]
    public async Task UpdateMatchResult_NonExistingMatch_ReturnsNotFound()
    {
        // Arrange
        var resultDto = new UpdateMatchResultDto
        {
            HomeTeamScore = 2,
            AwayTeamScore = 1
        };

        _matchServiceMock.Setup(s => s.UpdateMatchResultAsync(999, resultDto, It.IsAny<CancellationToken>()))
            .ReturnsAsync((MatchDto?)null);

        // Act
        var result = await _controller.UpdateMatchResult(999, resultDto);

        // Assert
        var notFoundResult = result.Result as NotFoundObjectResult;
        Assert.IsNotNull(notFoundResult);
        Assert.AreEqual(404, notFoundResult.StatusCode);
    }

    #endregion

    #region DeleteMatch Tests

    [TestMethod]
    public async Task DeleteMatch_ExistingMatch_ReturnsNoContent()
    {
        // Arrange
        _matchServiceMock.Setup(s => s.DeleteMatchAsync(1, It.IsAny<CancellationToken>()))
            .ReturnsAsync(true);

        // Act
        var result = await _controller.DeleteMatch(1);

        // Assert
        var noContentResult = result as NoContentResult;
        Assert.IsNotNull(noContentResult);
        Assert.AreEqual(204, noContentResult.StatusCode);
    }

    [TestMethod]
    public async Task DeleteMatch_NonExistingMatch_ReturnsNotFound()
    {
        // Arrange
        _matchServiceMock.Setup(s => s.DeleteMatchAsync(999, It.IsAny<CancellationToken>()))
            .ReturnsAsync(false);

        // Act
        var result = await _controller.DeleteMatch(999);

        // Assert
        var notFoundResult = result as NotFoundObjectResult;
        Assert.IsNotNull(notFoundResult);
        Assert.AreEqual(404, notFoundResult.StatusCode);
    }

    #endregion

    #region MatchExists Tests

    [TestMethod]
    public async Task MatchExists_ExistingMatch_ReturnsOk()
    {
        // Arrange
        _matchServiceMock.Setup(s => s.MatchExistsAsync(1, It.IsAny<CancellationToken>()))
            .ReturnsAsync(true);

        // Act
        var result = await _controller.MatchExists(1);

        // Assert
        var okResult = result as OkResult;
        Assert.IsNotNull(okResult);
        Assert.AreEqual(200, okResult.StatusCode);
    }

    [TestMethod]
    public async Task MatchExists_NonExistingMatch_ReturnsNotFound()
    {
        // Arrange
        _matchServiceMock.Setup(s => s.MatchExistsAsync(999, It.IsAny<CancellationToken>()))
            .ReturnsAsync(false);

        // Act
        var result = await _controller.MatchExists(999);

        // Assert
        var notFoundResult = result as NotFoundResult;
        Assert.IsNotNull(notFoundResult);
        Assert.AreEqual(404, notFoundResult.StatusCode);
    }

    #endregion
}

// Made with Bob
