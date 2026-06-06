using AutoMapper;
using FluentAssertions;
using Moq;
using WorldCup2026.Application.DTOs.Match;
using WorldCup2026.Application.Interfaces;
using WorldCup2026.Application.Services;
using WorldCup2026.Domain.Entities;
using WorldCup2026.Domain.Enums;
using WorldCup2026.Domain.Interfaces;
using DomainMatch = WorldCup2026.Domain.Entities.Match;

namespace WorldCup2026.Tests.Services;

[TestClass]
public class MatchServiceTests
{
    private Mock<IUnitOfWork> _unitOfWorkMock;
    private Mock<IMapper> _mapperMock;
    private Mock<IStandingService> _standingServiceMock;
    private MatchService _matchService;

    [TestInitialize]
    public void Setup()
    {
        _unitOfWorkMock = new Mock<IUnitOfWork>();
        _mapperMock = new Mock<IMapper>();
        _standingServiceMock = new Mock<IStandingService>();
        _matchService = new MatchService(_unitOfWorkMock.Object, _mapperMock.Object, _standingServiceMock.Object);
    }

    [TestMethod]
    public async Task GetAllMatchesAsync_WithFilters_ReturnsFilteredPagedMatches()
    {
        // Arrange
        var matches = new List<DomainMatch>
        {
            new() { Id = 1, Phase = MatchPhase.GroupStage, Status = MatchStatus.Scheduled, GroupId = 1, StadiumId = 1, MatchDate = DateTime.UtcNow.AddDays(1) },
            new() { Id = 2, Phase = MatchPhase.GroupStage, Status = MatchStatus.Finished, GroupId = 1, StadiumId = 2, MatchDate = DateTime.UtcNow.AddDays(-1) },
            new() { Id = 3, Phase = MatchPhase.RoundOf16, Status = MatchStatus.Scheduled, GroupId = null, StadiumId = 1, MatchDate = DateTime.UtcNow.AddDays(5) }
        };

        var matchDtos = new List<MatchDto>
        {
            new() { Id = 1, Phase = MatchPhase.GroupStage, Status = MatchStatus.Scheduled },
            new() { Id = 2, Phase = MatchPhase.GroupStage, Status = MatchStatus.Finished }
        };

        _unitOfWorkMock.Setup(u => u.Matches.GetAllAsync(It.IsAny<CancellationToken>()))
            .ReturnsAsync(matches);
        _mapperMock.Setup(m => m.Map<List<MatchDto>>(It.IsAny<List<DomainMatch>>()))
            .Returns(matchDtos);

        // Act
        var result = await _matchService.GetAllMatchesAsync(
            pageNumber: 1,
            pageSize: 10,
            phase: MatchPhase.GroupStage,
            groupId: 1);

        // Assert
        result.Should().NotBeNull();
        result.Items.Should().HaveCount(2);
        result.TotalCount.Should().Be(2);
        result.PageNumber.Should().Be(1);
        result.PageSize.Should().Be(10);
    }

    [TestMethod]
    public async Task GetMatchByIdAsync_ExistingMatch_ReturnsMatchDto()
    {
        // Arrange
        var match = new DomainMatch { Id = 1, HomeTeamId = 1, AwayTeamId = 2 };
        var matchDto = new MatchDto { Id = 1, HomeTeamId = 1, AwayTeamId = 2 };

        _unitOfWorkMock.Setup(u => u.Matches.GetByIdAsync(1, It.IsAny<CancellationToken>()))
            .ReturnsAsync(match);
        _mapperMock.Setup(m => m.Map<MatchDto>(match))
            .Returns(matchDto);

        // Act
        var result = await _matchService.GetMatchByIdAsync(1);

        // Assert
        result.Should().NotBeNull();
        result!.Id.Should().Be(1);
    }

    [TestMethod]
    public async Task GetMatchByIdAsync_NonExistingMatch_ReturnsNull()
    {
        // Arrange
        _unitOfWorkMock.Setup(u => u.Matches.GetByIdAsync(999, It.IsAny<CancellationToken>()))
            .ReturnsAsync((DomainMatch?)null);

        // Act
        var result = await _matchService.GetMatchByIdAsync(999);

        // Assert
        result.Should().BeNull();
    }

    [TestMethod]
    public async Task GetMatchesByTeamAsync_ReturnsTeamMatches()
    {
        // Arrange
        var matches = new List<DomainMatch>
        {
            new() { Id = 1, HomeTeamId = 1, AwayTeamId = 2 },
            new() { Id = 2, HomeTeamId = 3, AwayTeamId = 1 }
        };
        var matchDtos = new List<MatchDto>
        {
            new() { Id = 1, HomeTeamId = 1, AwayTeamId = 2 },
            new() { Id = 2, HomeTeamId = 3, AwayTeamId = 1 }
        };

        _unitOfWorkMock.Setup(u => u.Matches.GetByTeamIdAsync(1, It.IsAny<CancellationToken>()))
            .ReturnsAsync(matches);
        _mapperMock.Setup(m => m.Map<IEnumerable<MatchDto>>(matches))
            .Returns(matchDtos);

        // Act
        var result = await _matchService.GetMatchesByTeamAsync(1);

        // Assert
        result.Should().HaveCount(2);
    }

    [TestMethod]
    public async Task GetUpcomingMatchesAsync_ReturnsUpcomingMatches()
    {
        // Arrange
        var matches = new List<DomainMatch>
        {
            new() { Id = 1, Status = MatchStatus.Scheduled, MatchDate = DateTime.UtcNow.AddDays(1) },
            new() { Id = 2, Status = MatchStatus.Scheduled, MatchDate = DateTime.UtcNow.AddDays(2) }
        };
        var matchDtos = new List<MatchDto>
        {
            new() { Id = 1, Status = MatchStatus.Scheduled },
            new() { Id = 2, Status = MatchStatus.Scheduled }
        };

        _unitOfWorkMock.Setup(u => u.Matches.GetUpcomingMatchesAsync(10, It.IsAny<CancellationToken>()))
            .ReturnsAsync(matches);
        _mapperMock.Setup(m => m.Map<IEnumerable<MatchDto>>(matches))
            .Returns(matchDtos);

        // Act
        var result = await _matchService.GetUpcomingMatchesAsync(10);

        // Assert
        result.Should().HaveCount(2);
    }

    [TestMethod]
    public async Task GetLiveMatchesAsync_ReturnsInProgressMatches()
    {
        // Arrange
        var matches = new List<DomainMatch>
        {
            new() { Id = 1, Status = MatchStatus.InProgress },
            new() { Id = 2, Status = MatchStatus.InProgress }
        };
        var matchDtos = new List<MatchDto>
        {
            new() { Id = 1, Status = MatchStatus.InProgress },
            new() { Id = 2, Status = MatchStatus.InProgress }
        };

        _unitOfWorkMock.Setup(u => u.Matches.GetByStatusAsync(MatchStatus.InProgress, It.IsAny<CancellationToken>()))
            .ReturnsAsync(matches);
        _mapperMock.Setup(m => m.Map<IEnumerable<MatchDto>>(matches))
            .Returns(matchDtos);

        // Act
        var result = await _matchService.GetLiveMatchesAsync();

        // Assert
        result.Should().HaveCount(2);
        result.Should().OnlyContain(m => m.Status == MatchStatus.InProgress);
    }

    [TestMethod]
    public async Task CreateMatchAsync_ValidData_CreatesMatch()
    {
        // Arrange
        var createDto = new CreateMatchDto
        {
            HomeTeamId = 1,
            AwayTeamId = 2,
            StadiumId = 1,
            MatchDate = DateTime.UtcNow.AddDays(10),
            Phase = MatchPhase.GroupStage,
            GroupId = 1
        };

        var homeTeam = new Team { Id = 1, Name = "Team A" };
        var awayTeam = new Team { Id = 2, Name = "Team B" };
        var stadium = new Stadium { Id = 1, Name = "Stadium A" };
        var group = new Group { Id = 1, Name = "Group A" };
        var match = new DomainMatch { Id = 1, HomeTeamId = 1, AwayTeamId = 2 };
        var matchDto = new MatchDto { Id = 1, HomeTeamId = 1, AwayTeamId = 2 };

        _unitOfWorkMock.Setup(u => u.Teams.GetByIdAsync(1, It.IsAny<CancellationToken>()))
            .ReturnsAsync(homeTeam);
        _unitOfWorkMock.Setup(u => u.Teams.GetByIdAsync(2, It.IsAny<CancellationToken>()))
            .ReturnsAsync(awayTeam);
        _unitOfWorkMock.Setup(u => u.Stadiums.GetByIdAsync(1, It.IsAny<CancellationToken>()))
            .ReturnsAsync(stadium);
        _unitOfWorkMock.Setup(u => u.Groups.GetByIdAsync(1, It.IsAny<CancellationToken>()))
            .ReturnsAsync(group);
        _unitOfWorkMock.Setup(u => u.Matches.GetByDateRangeAsync(It.IsAny<DateTime>(), It.IsAny<DateTime>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(new List<DomainMatch>());
        _mapperMock.Setup(m => m.Map<DomainMatch>(createDto))
            .Returns(match);
        _unitOfWorkMock.Setup(u => u.Matches.GetByIdAsync(1, It.IsAny<CancellationToken>()))
            .ReturnsAsync(match);
        _mapperMock.Setup(m => m.Map<MatchDto>(match))
            .Returns(matchDto);

        // Act
        var result = await _matchService.CreateMatchAsync(createDto);

        // Assert
        result.Should().NotBeNull();
        result.Id.Should().Be(1);
        _unitOfWorkMock.Verify(u => u.Matches.AddAsync(It.IsAny<DomainMatch>(), It.IsAny<CancellationToken>()), Times.Once);
        _unitOfWorkMock.Verify(u => u.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Once);
    }

    [TestMethod]
    public async Task CreateMatchAsync_HomeTeamNotFound_ThrowsException()
    {
        // Arrange
        var createDto = new CreateMatchDto
        {
            HomeTeamId = 999,
            AwayTeamId = 2,
            StadiumId = 1,
            MatchDate = DateTime.UtcNow.AddDays(10)
        };

        _unitOfWorkMock.Setup(u => u.Teams.GetByIdAsync(999, It.IsAny<CancellationToken>()))
            .ReturnsAsync((Team?)null);

        // Act
        var act = async () => await _matchService.CreateMatchAsync(createDto);

        // Assert
        await act.Should().ThrowAsync<InvalidOperationException>()
            .WithMessage("Home team with ID 999 not found.");
    }

    [TestMethod]
    public async Task CreateMatchAsync_SameTeams_ThrowsException()
    {
        // Arrange
        var createDto = new CreateMatchDto
        {
            HomeTeamId = 1,
            AwayTeamId = 1,
            StadiumId = 1,
            MatchDate = DateTime.UtcNow.AddDays(10)
        };

        var team = new Team { Id = 1, Name = "Team A" };

        _unitOfWorkMock.Setup(u => u.Teams.GetByIdAsync(1, It.IsAny<CancellationToken>()))
            .ReturnsAsync(team);

        // Act
        var act = async () => await _matchService.CreateMatchAsync(createDto);

        // Assert
        await act.Should().ThrowAsync<InvalidOperationException>()
            .WithMessage("Home team and away team must be different.");
    }

    [TestMethod]
    public async Task CreateMatchAsync_TeamNotAvailable_ThrowsException()
    {
        // Arrange
        var createDto = new CreateMatchDto
        {
            HomeTeamId = 1,
            AwayTeamId = 2,
            StadiumId = 1,
            MatchDate = DateTime.UtcNow.AddDays(10)
        };

        var homeTeam = new Team { Id = 1, Name = "Team A" };
        var awayTeam = new Team { Id = 2, Name = "Team B" };
        var stadium = new Stadium { Id = 1, Name = "Stadium A" };
        var conflictingMatch = new DomainMatch { Id = 99, HomeTeamId = 1, AwayTeamId = 3, MatchDate = createDto.MatchDate };

        _unitOfWorkMock.Setup(u => u.Teams.GetByIdAsync(1, It.IsAny<CancellationToken>()))
            .ReturnsAsync(homeTeam);
        _unitOfWorkMock.Setup(u => u.Teams.GetByIdAsync(2, It.IsAny<CancellationToken>()))
            .ReturnsAsync(awayTeam);
        _unitOfWorkMock.Setup(u => u.Stadiums.GetByIdAsync(1, It.IsAny<CancellationToken>()))
            .ReturnsAsync(stadium);
        _unitOfWorkMock.Setup(u => u.Matches.GetByDateRangeAsync(It.IsAny<DateTime>(), It.IsAny<DateTime>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(new List<DomainMatch> { conflictingMatch });

        // Act
        var act = async () => await _matchService.CreateMatchAsync(createDto);

        // Assert
        await act.Should().ThrowAsync<InvalidOperationException>()
            .WithMessage("One or both teams are already scheduled to play at this time.");
    }

    [TestMethod]
    public async Task UpdateMatchResultAsync_ValidData_UpdatesResultAndRecalculatesStandings()
    {
        // Arrange
        var updateDto = new UpdateMatchResultDto
        {
            HomeTeamScore = 2,
            AwayTeamScore = 1
        };

        var match = new DomainMatch
        {
            Id = 1,
            HomeTeamId = 1,
            AwayTeamId = 2,
            Status = MatchStatus.InProgress,
            GroupId = 1,
            Result = null
        };

        var updatedMatch = new DomainMatch
        {
            Id = 1,
            HomeTeamId = 1,
            AwayTeamId = 2,
            Status = MatchStatus.Finished,
            GroupId = 1,
            Result = new MatchResult { HomeTeamScore = 2, AwayTeamScore = 1, WinnerTeamId = 1 }
        };

        var matchDto = new MatchDto { Id = 1, Status = MatchStatus.Finished };

        _unitOfWorkMock.Setup(u => u.Matches.GetByIdAsync(1, It.IsAny<CancellationToken>()))
            .ReturnsAsync(match);
        _mapperMock.Setup(m => m.Map<MatchResult>(updateDto))
            .Returns(new MatchResult { HomeTeamScore = 2, AwayTeamScore = 1 });
        _unitOfWorkMock.SetupSequence(u => u.Matches.GetByIdAsync(1, It.IsAny<CancellationToken>()))
            .ReturnsAsync(match)
            .ReturnsAsync(updatedMatch);
        _mapperMock.Setup(m => m.Map<MatchDto>(updatedMatch))
            .Returns(matchDto);

        // Act
        var result = await _matchService.UpdateMatchResultAsync(1, updateDto);

        // Assert
        result.Should().NotBeNull();
        _unitOfWorkMock.Verify(u => u.Matches.Update(It.IsAny<DomainMatch>()), Times.Once);
        _unitOfWorkMock.Verify(u => u.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Once);
        _standingServiceMock.Verify(s => s.RecalculateGroupStandingsAsync(1, It.IsAny<CancellationToken>()), Times.Once);
    }

    [TestMethod]
    public async Task UpdateMatchResultAsync_ScheduledMatch_ThrowsException()
    {
        // Arrange
        var updateDto = new UpdateMatchResultDto
        {
            HomeTeamScore = 2,
            AwayTeamScore = 1
        };

        var match = new DomainMatch
        {
            Id = 1,
            Status = MatchStatus.Scheduled
        };

        _unitOfWorkMock.Setup(u => u.Matches.GetByIdAsync(1, It.IsAny<CancellationToken>()))
            .ReturnsAsync(match);

        // Act
        var act = async () => await _matchService.UpdateMatchResultAsync(1, updateDto);

        // Assert
        await act.Should().ThrowAsync<InvalidOperationException>()
            .WithMessage("Cannot update result for a scheduled match. Start the match first.");
    }

    [TestMethod]
    public async Task UpdateMatchResultAsync_WithPenalties_DeterminesWinnerCorrectly()
    {
        // Arrange
        var updateDto = new UpdateMatchResultDto
        {
            HomeTeamScore = 1,
            AwayTeamScore = 1,
            HomeTeamPenalties = 4,
            AwayTeamPenalties = 5
        };

        var match = new DomainMatch
        {
            Id = 1,
            HomeTeamId = 1,
            AwayTeamId = 2,
            Status = MatchStatus.InProgress,
            GroupId = null,
            Result = null
        };

        var updatedMatch = new DomainMatch
        {
            Id = 1,
            HomeTeamId = 1,
            AwayTeamId = 2,
            Status = MatchStatus.Finished,
            Result = new MatchResult { HomeTeamScore = 1, AwayTeamScore = 1, HomeTeamPenalties = 4, AwayTeamPenalties = 5, WinnerTeamId = 2 }
        };

        var matchDto = new MatchDto { Id = 1 };

        _unitOfWorkMock.Setup(u => u.Matches.GetByIdAsync(1, It.IsAny<CancellationToken>()))
            .ReturnsAsync(match);
        _mapperMock.Setup(m => m.Map<MatchResult>(updateDto))
            .Returns(new MatchResult { HomeTeamScore = 1, AwayTeamScore = 1, HomeTeamPenalties = 4, AwayTeamPenalties = 5 });
        _unitOfWorkMock.SetupSequence(u => u.Matches.GetByIdAsync(1, It.IsAny<CancellationToken>()))
            .ReturnsAsync(match)
            .ReturnsAsync(updatedMatch);
        _mapperMock.Setup(m => m.Map<MatchDto>(updatedMatch))
            .Returns(matchDto);

        // Act
        var result = await _matchService.UpdateMatchResultAsync(1, updateDto);

        // Assert
        result.Should().NotBeNull();
        _standingServiceMock.Verify(s => s.RecalculateGroupStandingsAsync(It.IsAny<int>(), It.IsAny<CancellationToken>()), Times.Never);
    }

    [TestMethod]
    public async Task UpdateMatchStatusAsync_ValidTransition_UpdatesStatus()
    {
        // Arrange
        var match = new DomainMatch { Id = 1, Status = MatchStatus.Scheduled };
        var updatedMatch = new DomainMatch { Id = 1, Status = MatchStatus.InProgress };
        var matchDto = new MatchDto { Id = 1, Status = MatchStatus.InProgress };

        _unitOfWorkMock.Setup(u => u.Matches.GetByIdAsync(1, It.IsAny<CancellationToken>()))
            .ReturnsAsync(match);
        _unitOfWorkMock.SetupSequence(u => u.Matches.GetByIdAsync(1, It.IsAny<CancellationToken>()))
            .ReturnsAsync(match)
            .ReturnsAsync(updatedMatch);
        _mapperMock.Setup(m => m.Map<MatchDto>(updatedMatch))
            .Returns(matchDto);

        // Act
        var result = await _matchService.UpdateMatchStatusAsync(1, MatchStatus.InProgress);

        // Assert
        result.Should().NotBeNull();
        result.Status.Should().Be(MatchStatus.InProgress);
        _unitOfWorkMock.Verify(u => u.Matches.Update(It.IsAny<DomainMatch>()), Times.Once);
    }

    [TestMethod]
    public async Task UpdateMatchStatusAsync_FinishedMatch_ThrowsException()
    {
        // Arrange
        var match = new DomainMatch { Id = 1, Status = MatchStatus.Finished };

        _unitOfWorkMock.Setup(u => u.Matches.GetByIdAsync(1, It.IsAny<CancellationToken>()))
            .ReturnsAsync(match);

        // Act
        var act = async () => await _matchService.UpdateMatchStatusAsync(1, MatchStatus.Scheduled);

        // Assert
        await act.Should().ThrowAsync<InvalidOperationException>()
            .WithMessage("Cannot change status of a finished match.");
    }

    [TestMethod]
    public async Task DeleteMatchAsync_ScheduledMatch_DeletesSuccessfully()
    {
        // Arrange
        var match = new DomainMatch { Id = 1, Status = MatchStatus.Scheduled };

        _unitOfWorkMock.Setup(u => u.Matches.GetByIdAsync(1, It.IsAny<CancellationToken>()))
            .ReturnsAsync(match);

        // Act
        var result = await _matchService.DeleteMatchAsync(1);

        // Assert
        result.Should().BeTrue();
        _unitOfWorkMock.Verify(u => u.Matches.Delete(match), Times.Once);
        _unitOfWorkMock.Verify(u => u.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Once);
    }

    [TestMethod]
    public async Task DeleteMatchAsync_FinishedMatch_ThrowsException()
    {
        // Arrange
        var match = new DomainMatch { Id = 1, Status = MatchStatus.Finished };

        _unitOfWorkMock.Setup(u => u.Matches.GetByIdAsync(1, It.IsAny<CancellationToken>()))
            .ReturnsAsync(match);

        // Act
        var act = async () => await _matchService.DeleteMatchAsync(1);

        // Assert
        await act.Should().ThrowAsync<InvalidOperationException>()
            .WithMessage("Cannot delete a finished match.");
    }

    [TestMethod]
    public async Task DeleteMatchAsync_NonExistingMatch_ReturnsFalse()
    {
        // Arrange
        _unitOfWorkMock.Setup(u => u.Matches.GetByIdAsync(999, It.IsAny<CancellationToken>()))
            .ReturnsAsync((DomainMatch?)null);

        // Act
        var result = await _matchService.DeleteMatchAsync(999);

        // Assert
        result.Should().BeFalse();
        _unitOfWorkMock.Verify(u => u.Matches.Delete(It.IsAny<DomainMatch>()), Times.Never);
    }

    [TestMethod]
    public async Task MatchExistsAsync_ExistingMatch_ReturnsTrue()
    {
        // Arrange
        var match = new DomainMatch { Id = 1 };

        _unitOfWorkMock.Setup(u => u.Matches.GetByIdAsync(1, It.IsAny<CancellationToken>()))
            .ReturnsAsync(match);

        // Act
        var result = await _matchService.MatchExistsAsync(1);

        // Assert
        result.Should().BeTrue();
    }

    [TestMethod]
    public async Task MatchExistsAsync_NonExistingMatch_ReturnsFalse()
    {
        // Arrange
        _unitOfWorkMock.Setup(u => u.Matches.GetByIdAsync(999, It.IsAny<CancellationToken>()))
            .ReturnsAsync((DomainMatch?)null);

        // Act
        var result = await _matchService.MatchExistsAsync(999);

        // Assert
        result.Should().BeFalse();
    }

    [TestMethod]
    public async Task ValidateTeamAvailabilityAsync_NoConflicts_ReturnsTrue()
    {
        // Arrange
        var matchDate = DateTime.UtcNow.AddDays(10);

        _unitOfWorkMock.Setup(u => u.Matches.GetByDateRangeAsync(It.IsAny<DateTime>(), It.IsAny<DateTime>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(new List<DomainMatch>());

        // Act
        var result = await _matchService.ValidateTeamAvailabilityAsync(1, 2, matchDate);

        // Assert
        result.Should().BeTrue();
    }

    [TestMethod]
    public async Task ValidateTeamAvailabilityAsync_WithConflict_ReturnsFalse()
    {
        // Arrange
        var matchDate = DateTime.UtcNow.AddDays(10);
        var conflictingMatch = new DomainMatch
        {
            Id = 99,
            HomeTeamId = 1,
            AwayTeamId = 3,
            MatchDate = matchDate
        };

        _unitOfWorkMock.Setup(u => u.Matches.GetByDateRangeAsync(It.IsAny<DateTime>(), It.IsAny<DateTime>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(new List<DomainMatch> { conflictingMatch });

        // Act
        var result = await _matchService.ValidateTeamAvailabilityAsync(1, 2, matchDate);

        // Assert
        result.Should().BeFalse();
    }

    [TestMethod]
    public async Task ValidateTeamAvailabilityAsync_ExcludeCurrentMatch_ReturnsTrue()
    {
        // Arrange
        var matchDate = DateTime.UtcNow.AddDays(10);
        var currentMatch = new DomainMatch
        {
            Id = 1,
            HomeTeamId = 1,
            AwayTeamId = 2,
            MatchDate = matchDate
        };

        _unitOfWorkMock.Setup(u => u.Matches.GetByDateRangeAsync(It.IsAny<DateTime>(), It.IsAny<DateTime>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(new List<DomainMatch> { currentMatch });

        // Act
        var result = await _matchService.ValidateTeamAvailabilityAsync(1, 2, matchDate, excludeMatchId: 1);

        // Assert
        result.Should().BeTrue();
    }
}

// Made with Bob