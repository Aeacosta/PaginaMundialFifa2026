using AutoMapper;
using FluentAssertions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using WorldCup2026.Application.DTOs.Team;
using WorldCup2026.Application.Services;
using WorldCup2026.Domain.Entities;
using WorldCup2026.Domain.Enums;
using WorldCup2026.Domain.Interfaces;
using DomainMatch = WorldCup2026.Domain.Entities.Match;

namespace WorldCup2026.Tests.Services;

[TestClass]
public class TeamServiceTests
{
    private Mock<IUnitOfWork> _unitOfWorkMock;
    private Mock<IMapper> _mapperMock;
    private TeamService _teamService;

    [TestInitialize]
    public void Setup()
    {
        _unitOfWorkMock = new Mock<IUnitOfWork>();
        _mapperMock = new Mock<IMapper>();
        _teamService = new TeamService(_unitOfWorkMock.Object, _mapperMock.Object);
    }

    [TestMethod]
    public async Task GetAllTeamsAsync_ShouldReturnPagedTeams()
    {
        // Arrange
        var teams = new List<Team>
        {
            new Team { Id = 1, Name = "Argentina", Code = "ARG", Confederation = Confederation.CONMEBOL },
            new Team { Id = 2, Name = "Brazil", Code = "BRA", Confederation = Confederation.CONMEBOL }
        };

        var teamDtos = new List<TeamDto>
        {
            new TeamDto { Id = 1, Name = "Argentina", Code = "ARG", Confederation = Confederation.CONMEBOL },
            new TeamDto { Id = 2, Name = "Brazil", Code = "BRA", Confederation = Confederation.CONMEBOL }
        };

        _unitOfWorkMock.Setup(u => u.Teams.GetAllAsync(It.IsAny<CancellationToken>())).ReturnsAsync(teams);
        _mapperMock.Setup(m => m.Map<List<TeamDto>>(It.IsAny<List<Team>>())).Returns(teamDtos);

        // Act
        var result = await _teamService.GetAllTeamsAsync();

        // Assert
        result.Should().NotBeNull();
        result.Items.Should().HaveCount(2);
        result.TotalCount.Should().Be(2);
        result.PageNumber.Should().Be(1);
        result.PageSize.Should().Be(10);
        _unitOfWorkMock.Verify(u => u.Teams.GetAllAsync(It.IsAny<CancellationToken>()), Times.Once);
    }

    [TestMethod]
    public async Task GetTeamByIdAsync_WithValidId_ShouldReturnTeam()
    {
        // Arrange
        var teamId = 1;
        var team = new Team { Id = teamId, Name = "Argentina", Code = "ARG", Confederation = Confederation.CONMEBOL };
        var teamDto = new TeamDto { Id = teamId, Name = "Argentina", Code = "ARG", Confederation = Confederation.CONMEBOL };

        _unitOfWorkMock.Setup(u => u.Teams.GetByIdAsync(teamId, It.IsAny<CancellationToken>())).ReturnsAsync(team);
        _mapperMock.Setup(m => m.Map<TeamDto>(team)).Returns(teamDto);

        // Act
        var result = await _teamService.GetTeamByIdAsync(teamId);

        // Assert
        result.Should().NotBeNull();
        result.Should().BeEquivalentTo(teamDto);
        _unitOfWorkMock.Verify(u => u.Teams.GetByIdAsync(teamId, It.IsAny<CancellationToken>()), Times.Once);
    }

    [TestMethod]
    public async Task GetTeamByIdAsync_WithInvalidId_ShouldReturnNull()
    {
        // Arrange
        var teamId = 999;
        _unitOfWorkMock.Setup(u => u.Teams.GetByIdAsync(teamId, It.IsAny<CancellationToken>())).ReturnsAsync((Team)null);

        // Act
        var result = await _teamService.GetTeamByIdAsync(teamId);

        // Assert
        result.Should().BeNull();
        _unitOfWorkMock.Verify(u => u.Teams.GetByIdAsync(teamId, It.IsAny<CancellationToken>()), Times.Once);
    }

    [TestMethod]
    public async Task CreateTeamAsync_WithValidData_ShouldCreateTeam()
    {
        // Arrange
        var createDto = new CreateTeamDto
        {
            Name = "Argentina",
            Code = "ARG",
            Confederation = Confederation.CONMEBOL,
            FifaRanking = 1,
            GroupId = 1
        };

        var team = new Team
        {
            Name = "Argentina",
            Code = "ARG",
            Confederation = Confederation.CONMEBOL,
            FifaRanking = 1,
            GroupId = 1
        };

        var createdTeam = new Team
        {
            Id = 1,
            Name = "Argentina",
            Code = "ARG",
            Confederation = Confederation.CONMEBOL,
            FifaRanking = 1,
            GroupId = 1
        };

        var group = new Group { Id = 1, Name = "Group A" };

        var teamDto = new TeamDto
        {
            Id = 1,
            Name = "Argentina",
            Code = "ARG",
            Confederation = Confederation.CONMEBOL,
            FifaRanking = 1,
            GroupId = 1
        };

        _unitOfWorkMock.Setup(u => u.Teams.FindAsync(It.IsAny<System.Linq.Expressions.Expression<Func<Team, bool>>>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(new List<Team>());
        _unitOfWorkMock.Setup(u => u.Groups.GetByIdAsync(1, It.IsAny<CancellationToken>())).ReturnsAsync(group);
        _mapperMock.Setup(m => m.Map<Team>(createDto)).Returns(team);
        _unitOfWorkMock.Setup(u => u.Teams.AddAsync(It.IsAny<Team>(), It.IsAny<CancellationToken>())).ReturnsAsync(team);
        _unitOfWorkMock.Setup(u => u.SaveChangesAsync(It.IsAny<CancellationToken>())).ReturnsAsync(1);
        _unitOfWorkMock.Setup(u => u.Teams.GetByIdAsync(It.IsAny<int>(), It.IsAny<CancellationToken>())).ReturnsAsync(createdTeam);
        _mapperMock.Setup(m => m.Map<TeamDto>(createdTeam)).Returns(teamDto);

        // Act
        var result = await _teamService.CreateTeamAsync(createDto);

        // Assert
        result.Should().NotBeNull();
        result.Should().BeEquivalentTo(teamDto);
        _unitOfWorkMock.Verify(u => u.Teams.AddAsync(It.IsAny<Team>(), It.IsAny<CancellationToken>()), Times.Once);
        _unitOfWorkMock.Verify(u => u.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Once);
    }

    [TestMethod]
    public async Task UpdateTeamAsync_WithValidData_ShouldUpdateTeam()
    {
        // Arrange
        var teamId = 1;
        var updateDto = new UpdateTeamDto
        {
            Name = "Argentina Updated",
            Code = "ARG",
            Confederation = Confederation.CONMEBOL,
            FifaRanking = 2,
            GroupId = 1
        };

        var existingTeam = new Team
        {
            Id = teamId,
            Name = "Argentina",
            Code = "ARG",
            Confederation = Confederation.CONMEBOL,
            FifaRanking = 1,
            GroupId = 1
        };

        var updatedTeam = new Team
        {
            Id = teamId,
            Name = "Argentina Updated",
            Code = "ARG",
            Confederation = Confederation.CONMEBOL,
            FifaRanking = 2,
            GroupId = 1
        };

        var group = new Group { Id = 1, Name = "Group A" };

        var teamDto = new TeamDto
        {
            Id = teamId,
            Name = "Argentina Updated",
            Code = "ARG",
            Confederation = Confederation.CONMEBOL,
            FifaRanking = 2,
            GroupId = 1
        };

        _unitOfWorkMock.Setup(u => u.Teams.GetByIdAsync(teamId, It.IsAny<CancellationToken>())).ReturnsAsync(existingTeam);
        _unitOfWorkMock.Setup(u => u.Teams.FindAsync(It.IsAny<System.Linq.Expressions.Expression<Func<Team, bool>>>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(new List<Team>());
        _unitOfWorkMock.Setup(u => u.Groups.GetByIdAsync(1, It.IsAny<CancellationToken>())).ReturnsAsync(group);
        _mapperMock.Setup(m => m.Map(updateDto, existingTeam)).Returns(updatedTeam);
        _unitOfWorkMock.Setup(u => u.Teams.Update(It.IsAny<Team>()));
        _unitOfWorkMock.Setup(u => u.SaveChangesAsync(It.IsAny<CancellationToken>())).ReturnsAsync(1);
        _unitOfWorkMock.SetupSequence(u => u.Teams.GetByIdAsync(teamId, It.IsAny<CancellationToken>()))
            .ReturnsAsync(existingTeam)
            .ReturnsAsync(updatedTeam);
        _mapperMock.Setup(m => m.Map<TeamDto>(updatedTeam)).Returns(teamDto);

        // Act
        var result = await _teamService.UpdateTeamAsync(teamId, updateDto);

        // Assert
        result.Should().NotBeNull();
        result.Should().BeEquivalentTo(teamDto);
        _unitOfWorkMock.Verify(u => u.Teams.Update(It.IsAny<Team>()), Times.Once);
        _unitOfWorkMock.Verify(u => u.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Once);
    }

    [TestMethod]
    public async Task UpdateTeamAsync_WithInvalidId_ShouldThrowException()
    {
        // Arrange
        var teamId = 999;
        var updateDto = new UpdateTeamDto { Name = "Test", Code = "TST", Confederation = Confederation.UEFA };
        _unitOfWorkMock.Setup(u => u.Teams.GetByIdAsync(teamId, It.IsAny<CancellationToken>())).ReturnsAsync((Team)null);

        // Act & Assert
        var act = async () => await _teamService.UpdateTeamAsync(teamId, updateDto);
        await act.Should().ThrowAsync<InvalidOperationException>();
        
        _unitOfWorkMock.Verify(u => u.Teams.Update(It.IsAny<Team>()), Times.Never);
        _unitOfWorkMock.Verify(u => u.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Never);
    }

    [TestMethod]
    public async Task DeleteTeamAsync_WithValidId_ShouldReturnTrue()
    {
        // Arrange
        var teamId = 1;
        var team = new Team { Id = teamId, Name = "Argentina" };
        _unitOfWorkMock.Setup(u => u.Teams.GetByIdAsync(teamId, It.IsAny<CancellationToken>())).ReturnsAsync(team);
        _unitOfWorkMock.Setup(u => u.Matches.FindAsync(It.IsAny<System.Linq.Expressions.Expression<Func<DomainMatch, bool>>>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(new List<DomainMatch>());
        _unitOfWorkMock.Setup(u => u.Teams.Delete(team));
        _unitOfWorkMock.Setup(u => u.SaveChangesAsync(It.IsAny<CancellationToken>())).ReturnsAsync(1);

        // Act
        var result = await _teamService.DeleteTeamAsync(teamId);

        // Assert
        result.Should().BeTrue();
        _unitOfWorkMock.Verify(u => u.Teams.Delete(team), Times.Once);
        _unitOfWorkMock.Verify(u => u.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Once);
    }

    [TestMethod]
    public async Task DeleteTeamAsync_WithInvalidId_ShouldReturnFalse()
    {
        // Arrange
        var teamId = 999;
        _unitOfWorkMock.Setup(u => u.Teams.GetByIdAsync(teamId, It.IsAny<CancellationToken>())).ReturnsAsync((Team)null);

        // Act
        var result = await _teamService.DeleteTeamAsync(teamId);

        // Assert
        result.Should().BeFalse();
        _unitOfWorkMock.Verify(u => u.Teams.Delete(It.IsAny<Team>()), Times.Never);
        _unitOfWorkMock.Verify(u => u.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Never);
    }

    [TestMethod]
    public async Task GetTeamsByConfederationAsync_ShouldReturnTeamsFromConfederation()
    {
        // Arrange
        var confederation = Confederation.CONMEBOL;
        var teams = new List<Team>
        {
            new Team { Id = 1, Name = "Argentina", Confederation = Confederation.CONMEBOL },
            new Team { Id = 2, Name = "Brazil", Confederation = Confederation.CONMEBOL }
        };

        var teamDtos = new List<TeamDto>
        {
            new TeamDto { Id = 1, Name = "Argentina", Confederation = Confederation.CONMEBOL },
            new TeamDto { Id = 2, Name = "Brazil", Confederation = Confederation.CONMEBOL }
        };

        _unitOfWorkMock.Setup(u => u.Teams.GetByConfederationAsync(confederation, It.IsAny<CancellationToken>()))
            .ReturnsAsync(teams);
        _mapperMock.Setup(m => m.Map<IEnumerable<TeamDto>>(teams)).Returns(teamDtos);

        // Act
        var result = await _teamService.GetTeamsByConfederationAsync(confederation);

        // Assert
        result.Should().NotBeNull();
        result.Should().HaveCount(2);
        result.Should().BeEquivalentTo(teamDtos);
    }

    [TestMethod]
    public async Task GetTeamsByGroupAsync_ShouldReturnTeamsInGroup()
    {
        // Arrange
        var groupId = 1;
        var teams = new List<Team>
        {
            new Team { Id = 1, Name = "Argentina", GroupId = groupId },
            new Team { Id = 2, Name = "Brazil", GroupId = groupId }
        };

        var teamDtos = new List<TeamDto>
        {
            new TeamDto { Id = 1, Name = "Argentina", GroupId = groupId },
            new TeamDto { Id = 2, Name = "Brazil", GroupId = groupId }
        };

        _unitOfWorkMock.Setup(u => u.Teams.GetByGroupIdAsync(groupId, It.IsAny<CancellationToken>())).ReturnsAsync(teams);
        _mapperMock.Setup(m => m.Map<IEnumerable<TeamDto>>(teams)).Returns(teamDtos);

        // Act
        var result = await _teamService.GetTeamsByGroupAsync(groupId);

        // Assert
        result.Should().NotBeNull();
        result.Should().HaveCount(2);
        result.Should().BeEquivalentTo(teamDtos);
    }
}

// Made with Bob
