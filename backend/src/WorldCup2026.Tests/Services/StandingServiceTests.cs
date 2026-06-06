using AutoMapper;
using FluentAssertions;
using Moq;
using WorldCup2026.Application.DTOs.Standing;
using WorldCup2026.Application.Services;
using WorldCup2026.Domain.Entities;
using WorldCup2026.Domain.Interfaces;

namespace WorldCup2026.Tests.Services;

[TestClass]
public class StandingServiceTests
{
    private Mock<IUnitOfWork> _unitOfWorkMock;
    private Mock<IMapper> _mapperMock;
    private StandingService _standingService;

    [TestInitialize]
    public void Setup()
    {
        _unitOfWorkMock = new Mock<IUnitOfWork>();
        _mapperMock = new Mock<IMapper>();
        _standingService = new StandingService(_unitOfWorkMock.Object, _mapperMock.Object);
    }

    [TestMethod]
    public async Task GetStandingsByGroupAsync_ValidGroup_ReturnsStandings()
    {
        // Arrange
        var groupId = 1;
        var group = new Group { Id = groupId, Name = "Group A" };
        var standings = new List<Standing>
        {
            new() { Id = 1, GroupId = groupId, TeamId = 1, Position = 1, Points = 9 },
            new() { Id = 2, GroupId = groupId, TeamId = 2, Position = 2, Points = 6 },
            new() { Id = 3, GroupId = groupId, TeamId = 3, Position = 3, Points = 3 }
        };
        var standingDtos = new List<StandingDto>
        {
            new() { Id = 1, GroupId = groupId, TeamId = 1, Position = 1, Points = 9 },
            new() { Id = 2, GroupId = groupId, TeamId = 2, Position = 2, Points = 6 },
            new() { Id = 3, GroupId = groupId, TeamId = 3, Position = 3, Points = 3 }
        };

        _unitOfWorkMock.Setup(u => u.Groups.GetByIdAsync(groupId, It.IsAny<CancellationToken>()))
            .ReturnsAsync(group);
        _unitOfWorkMock.Setup(u => u.Standings.GetByGroupIdAsync(groupId, It.IsAny<CancellationToken>()))
            .ReturnsAsync(standings);
        _mapperMock.Setup(m => m.Map<IEnumerable<StandingDto>>(standings))
            .Returns(standingDtos);

        // Act
        var result = await _standingService.GetStandingsByGroupAsync(groupId);

        // Assert
        result.Should().NotBeNull();
        result.Should().HaveCount(3);
        result.Should().BeInAscendingOrder(s => s.Position);
    }

    [TestMethod]
    public async Task GetStandingsByGroupAsync_NonExistingGroup_ThrowsException()
    {
        // Arrange
        var groupId = 999;

        _unitOfWorkMock.Setup(u => u.Groups.GetByIdAsync(groupId, It.IsAny<CancellationToken>()))
            .ReturnsAsync((Group?)null);

        // Act
        var act = async () => await _standingService.GetStandingsByGroupAsync(groupId);

        // Assert
        await act.Should().ThrowAsync<InvalidOperationException>()
            .WithMessage($"Group with ID {groupId} not found.");
    }

    [TestMethod]
    public async Task GetStandingByTeamAsync_ExistingTeam_ReturnsStanding()
    {
        // Arrange
        var teamId = 1;
        var team = new Team { Id = teamId, Name = "Brazil" };
        var standing = new Standing { Id = 1, TeamId = teamId, GroupId = 1, Position = 1, Points = 9 };
        var standingDto = new StandingDto { Id = 1, TeamId = teamId, GroupId = 1, Position = 1, Points = 9 };

        _unitOfWorkMock.Setup(u => u.Teams.GetByIdAsync(teamId, It.IsAny<CancellationToken>()))
            .ReturnsAsync(team);
        _unitOfWorkMock.Setup(u => u.Standings.GetByTeamIdAsync(teamId, It.IsAny<CancellationToken>()))
            .ReturnsAsync(standing);
        _mapperMock.Setup(m => m.Map<StandingDto>(standing))
            .Returns(standingDto);

        // Act
        var result = await _standingService.GetStandingByTeamAsync(teamId);

        // Assert
        result.Should().NotBeNull();
        result!.TeamId.Should().Be(teamId);
        result.Position.Should().Be(1);
    }

    [TestMethod]
    public async Task GetStandingByTeamAsync_TeamWithoutStanding_ReturnsNull()
    {
        // Arrange
        var teamId = 1;
        var team = new Team { Id = teamId, Name = "Brazil" };

        _unitOfWorkMock.Setup(u => u.Teams.GetByIdAsync(teamId, It.IsAny<CancellationToken>()))
            .ReturnsAsync(team);
        _unitOfWorkMock.Setup(u => u.Standings.GetByTeamIdAsync(teamId, It.IsAny<CancellationToken>()))
            .ReturnsAsync((Standing?)null);

        // Act
        var result = await _standingService.GetStandingByTeamAsync(teamId);

        // Assert
        result.Should().BeNull();
    }

    [TestMethod]
    public async Task GetStandingByTeamAsync_NonExistingTeam_ThrowsException()
    {
        // Arrange
        var teamId = 999;

        _unitOfWorkMock.Setup(u => u.Teams.GetByIdAsync(teamId, It.IsAny<CancellationToken>()))
            .ReturnsAsync((Team?)null);

        // Act
        var act = async () => await _standingService.GetStandingByTeamAsync(teamId);

        // Assert
        await act.Should().ThrowAsync<InvalidOperationException>()
            .WithMessage($"Team with ID {teamId} not found.");
    }

    [TestMethod]
    public async Task RecalculateGroupStandingsAsync_ValidGroup_RecalculatesSuccessfully()
    {
        // Arrange
        var groupId = 1;
        var group = new Group { Id = groupId, Name = "Group A" };

        _unitOfWorkMock.Setup(u => u.Groups.GetByIdAsync(groupId, It.IsAny<CancellationToken>()))
            .ReturnsAsync(group);
        _unitOfWorkMock.Setup(u => u.Standings.UpdateGroupStandingsAsync(groupId, It.IsAny<CancellationToken>()))
            .Returns(Task.CompletedTask);

        // Act
        await _standingService.RecalculateGroupStandingsAsync(groupId);

        // Assert
        _unitOfWorkMock.Verify(u => u.Standings.UpdateGroupStandingsAsync(groupId, It.IsAny<CancellationToken>()), Times.Once);
        _unitOfWorkMock.Verify(u => u.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Once);
    }

    [TestMethod]
    public async Task RecalculateGroupStandingsAsync_NonExistingGroup_ThrowsException()
    {
        // Arrange
        var groupId = 999;

        _unitOfWorkMock.Setup(u => u.Groups.GetByIdAsync(groupId, It.IsAny<CancellationToken>()))
            .ReturnsAsync((Group?)null);

        // Act
        var act = async () => await _standingService.RecalculateGroupStandingsAsync(groupId);

        // Assert
        await act.Should().ThrowAsync<InvalidOperationException>()
            .WithMessage($"Group with ID {groupId} not found.");
        _unitOfWorkMock.Verify(u => u.Standings.UpdateGroupStandingsAsync(It.IsAny<int>(), It.IsAny<CancellationToken>()), Times.Never);
    }

    [TestMethod]
    public async Task RecalculateAllStandingsAsync_RecalculatesAllGroups()
    {
        // Arrange
        var groups = new List<Group>
        {
            new() { Id = 1, Name = "Group A" },
            new() { Id = 2, Name = "Group B" },
            new() { Id = 3, Name = "Group C" }
        };

        _unitOfWorkMock.Setup(u => u.Groups.GetAllAsync(It.IsAny<CancellationToken>()))
            .ReturnsAsync(groups);
        _unitOfWorkMock.Setup(u => u.Standings.UpdateGroupStandingsAsync(It.IsAny<int>(), It.IsAny<CancellationToken>()))
            .Returns(Task.CompletedTask);

        // Act
        await _standingService.RecalculateAllStandingsAsync();

        // Assert
        _unitOfWorkMock.Verify(u => u.Standings.UpdateGroupStandingsAsync(1, It.IsAny<CancellationToken>()), Times.Once);
        _unitOfWorkMock.Verify(u => u.Standings.UpdateGroupStandingsAsync(2, It.IsAny<CancellationToken>()), Times.Once);
        _unitOfWorkMock.Verify(u => u.Standings.UpdateGroupStandingsAsync(3, It.IsAny<CancellationToken>()), Times.Once);
        _unitOfWorkMock.Verify(u => u.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Once);
    }

    [TestMethod]
    public async Task GetTopTeamsAsync_ReturnsTopTeamsOrderedCorrectly()
    {
        // Arrange
        var allStandings = new List<Standing>
        {
            new() { Id = 1, TeamId = 1, Points = 9, GoalDifference = 5, GoalsFor = 8 },
            new() { Id = 2, TeamId = 2, Points = 9, GoalDifference = 4, GoalsFor = 7 },
            new() { Id = 3, TeamId = 3, Points = 7, GoalDifference = 3, GoalsFor = 6 },
            new() { Id = 4, TeamId = 4, Points = 6, GoalDifference = 2, GoalsFor = 5 },
            new() { Id = 5, TeamId = 5, Points = 4, GoalDifference = 1, GoalsFor = 4 }
        };

        var topStandingDtos = new List<StandingDto>
        {
            new() { Id = 1, TeamId = 1, Points = 9, GoalDifference = 5, GoalsFor = 8 },
            new() { Id = 2, TeamId = 2, Points = 9, GoalDifference = 4, GoalsFor = 7 },
            new() { Id = 3, TeamId = 3, Points = 7, GoalDifference = 3, GoalsFor = 6 }
        };

        _unitOfWorkMock.Setup(u => u.Standings.GetAllAsync(It.IsAny<CancellationToken>()))
            .ReturnsAsync(allStandings);
        _mapperMock.Setup(m => m.Map<IEnumerable<StandingDto>>(It.IsAny<List<Standing>>()))
            .Returns(topStandingDtos);

        // Act
        var result = await _standingService.GetTopTeamsAsync(count: 3);

        // Assert
        result.Should().NotBeNull();
        result.Should().HaveCount(3);
        // First team should have highest points and goal difference
        var resultList = result.ToList();
        resultList[0].Points.Should().Be(9);
        resultList[0].GoalDifference.Should().Be(5);
    }

    [TestMethod]
    public async Task GetTopTeamsAsync_DefaultCount_Returns16Teams()
    {
        // Arrange
        var allStandings = Enumerable.Range(1, 20)
            .Select(i => new Standing
            {
                Id = i,
                TeamId = i,
                Points = 20 - i,
                GoalDifference = 10 - i,
                GoalsFor = 15 - i
            })
            .ToList();

        var topStandingDtos = Enumerable.Range(1, 16)
            .Select(i => new StandingDto
            {
                Id = i,
                TeamId = i,
                Points = 20 - i,
                GoalDifference = 10 - i,
                GoalsFor = 15 - i
            })
            .ToList();

        _unitOfWorkMock.Setup(u => u.Standings.GetAllAsync(It.IsAny<CancellationToken>()))
            .ReturnsAsync(allStandings);
        _mapperMock.Setup(m => m.Map<IEnumerable<StandingDto>>(It.IsAny<List<Standing>>()))
            .Returns(topStandingDtos);

        // Act
        var result = await _standingService.GetTopTeamsAsync();

        // Assert
        result.Should().HaveCount(16);
    }

    [TestMethod]
    public async Task GetQualifiedTeamsFromGroupAsync_ValidGroup_ReturnsTopTeams()
    {
        // Arrange
        var groupId = 1;
        var group = new Group { Id = groupId, Name = "Group A" };
        var standings = new List<Standing>
        {
            new() { Id = 1, GroupId = groupId, TeamId = 1, Position = 1, Points = 9 },
            new() { Id = 2, GroupId = groupId, TeamId = 2, Position = 2, Points = 6 },
            new() { Id = 3, GroupId = groupId, TeamId = 3, Position = 3, Points = 3 },
            new() { Id = 4, GroupId = groupId, TeamId = 4, Position = 4, Points = 0 }
        };

        var qualifiedDtos = new List<StandingDto>
        {
            new() { Id = 1, GroupId = groupId, TeamId = 1, Position = 1, Points = 9 },
            new() { Id = 2, GroupId = groupId, TeamId = 2, Position = 2, Points = 6 }
        };

        _unitOfWorkMock.Setup(u => u.Groups.GetByIdAsync(groupId, It.IsAny<CancellationToken>()))
            .ReturnsAsync(group);
        _unitOfWorkMock.Setup(u => u.Standings.GetByGroupIdAsync(groupId, It.IsAny<CancellationToken>()))
            .ReturnsAsync(standings);
        _mapperMock.Setup(m => m.Map<IEnumerable<StandingDto>>(It.IsAny<List<Standing>>()))
            .Returns(qualifiedDtos);

        // Act
        var result = await _standingService.GetQualifiedTeamsFromGroupAsync(groupId, count: 2);

        // Assert
        result.Should().NotBeNull();
        result.Should().HaveCount(2);
        var resultList = result.ToList();
        resultList[0].Position.Should().Be(1);
        resultList[1].Position.Should().Be(2);
    }

    [TestMethod]
    public async Task GetQualifiedTeamsFromGroupAsync_CustomCount_ReturnsCorrectNumber()
    {
        // Arrange
        var groupId = 1;
        var group = new Group { Id = groupId, Name = "Group A" };
        var standings = new List<Standing>
        {
            new() { Id = 1, GroupId = groupId, TeamId = 1, Position = 1, Points = 9 },
            new() { Id = 2, GroupId = groupId, TeamId = 2, Position = 2, Points = 6 },
            new() { Id = 3, GroupId = groupId, TeamId = 3, Position = 3, Points = 3 },
            new() { Id = 4, GroupId = groupId, TeamId = 4, Position = 4, Points = 0 }
        };

        var qualifiedDtos = new List<StandingDto>
        {
            new() { Id = 1, GroupId = groupId, TeamId = 1, Position = 1, Points = 9 },
            new() { Id = 2, GroupId = groupId, TeamId = 2, Position = 2, Points = 6 },
            new() { Id = 3, GroupId = groupId, TeamId = 3, Position = 3, Points = 3 }
        };

        _unitOfWorkMock.Setup(u => u.Groups.GetByIdAsync(groupId, It.IsAny<CancellationToken>()))
            .ReturnsAsync(group);
        _unitOfWorkMock.Setup(u => u.Standings.GetByGroupIdAsync(groupId, It.IsAny<CancellationToken>()))
            .ReturnsAsync(standings);
        _mapperMock.Setup(m => m.Map<IEnumerable<StandingDto>>(It.IsAny<List<Standing>>()))
            .Returns(qualifiedDtos);

        // Act
        var result = await _standingService.GetQualifiedTeamsFromGroupAsync(groupId, count: 3);

        // Assert
        result.Should().HaveCount(3);
    }

    [TestMethod]
    public async Task GetQualifiedTeamsFromGroupAsync_NonExistingGroup_ThrowsException()
    {
        // Arrange
        var groupId = 999;

        _unitOfWorkMock.Setup(u => u.Groups.GetByIdAsync(groupId, It.IsAny<CancellationToken>()))
            .ReturnsAsync((Group?)null);

        // Act
        var act = async () => await _standingService.GetQualifiedTeamsFromGroupAsync(groupId);

        // Assert
        await act.Should().ThrowAsync<InvalidOperationException>()
            .WithMessage($"Group with ID {groupId} not found.");
    }
}

// Made with Bob