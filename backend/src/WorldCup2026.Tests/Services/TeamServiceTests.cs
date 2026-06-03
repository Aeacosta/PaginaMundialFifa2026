using AutoMapper;
using FluentAssertions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using WorldCup2026.Application.DTOs.Team;
using WorldCup2026.Application.Services;
using WorldCup2026.Domain.Entities;
using WorldCup2026.Domain.Enums;
using WorldCup2026.Domain.Interfaces;

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
    public async Task GetAllAsync_ShouldReturnAllTeams()
    {
        // Arrange
        var teams = new List<Team>
        {
            new Team { Id = 1, Name = "Argentina", Code = "ARG", Confederation = Confederation.CONMEBOL },
            new Team { Id = 2, Name = "Brazil", Code = "BRA", Confederation = Confederation.CONMEBOL }
        };

        var teamDtos = new List<TeamDto>
        {
            new TeamDto { Id = 1, Name = "Argentina", Code = "ARG", Confederation = "CONMEBOL" },
            new TeamDto { Id = 2, Name = "Brazil", Code = "BRA", Confederation = "CONMEBOL" }
        };

        _unitOfWorkMock.Setup(u => u.Teams.GetAllAsync()).ReturnsAsync(teams);
        _mapperMock.Setup(m => m.Map<IEnumerable<TeamDto>>(teams)).Returns(teamDtos);

        // Act
        var result = await _teamService.GetAllAsync();

        // Assert
        result.Should().NotBeNull();
        result.Should().HaveCount(2);
        result.Should().BeEquivalentTo(teamDtos);
        _unitOfWorkMock.Verify(u => u.Teams.GetAllAsync(), Times.Once);
    }

    [TestMethod]
    public async Task GetByIdAsync_WithValidId_ShouldReturnTeam()
    {
        // Arrange
        var teamId = 1;
        var team = new Team { Id = teamId, Name = "Argentina", Code = "ARG", Confederation = Confederation.CONMEBOL };
        var teamDto = new TeamDto { Id = teamId, Name = "Argentina", Code = "ARG", Confederation = "CONMEBOL" };

        _unitOfWorkMock.Setup(u => u.Teams.GetByIdAsync(teamId)).ReturnsAsync(team);
        _mapperMock.Setup(m => m.Map<TeamDto>(team)).Returns(teamDto);

        // Act
        var result = await _teamService.GetByIdAsync(teamId);

        // Assert
        result.Should().NotBeNull();
        result.Should().BeEquivalentTo(teamDto);
        _unitOfWorkMock.Verify(u => u.Teams.GetByIdAsync(teamId), Times.Once);
    }

    [TestMethod]
    public async Task GetByIdAsync_WithInvalidId_ShouldReturnNull()
    {
        // Arrange
        var teamId = 999;
        _unitOfWorkMock.Setup(u => u.Teams.GetByIdAsync(teamId)).ReturnsAsync((Team)null);

        // Act
        var result = await _teamService.GetByIdAsync(teamId);

        // Assert
        result.Should().BeNull();
        _unitOfWorkMock.Verify(u => u.Teams.GetByIdAsync(teamId), Times.Once);
    }

    [TestMethod]
    public async Task CreateAsync_WithValidData_ShouldCreateTeam()
    {
        // Arrange
        var createDto = new CreateTeamDto
        {
            Name = "Argentina",
            Code = "ARG",
            Confederation = "CONMEBOL",
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

        var teamDto = new TeamDto
        {
            Id = 1,
            Name = "Argentina",
            Code = "ARG",
            Confederation = "CONMEBOL",
            FifaRanking = 1,
            GroupId = 1
        };

        _mapperMock.Setup(m => m.Map<Team>(createDto)).Returns(team);
        _unitOfWorkMock.Setup(u => u.Teams.AddAsync(team)).ReturnsAsync(createdTeam);
        _unitOfWorkMock.Setup(u => u.SaveChangesAsync()).ReturnsAsync(1);
        _mapperMock.Setup(m => m.Map<TeamDto>(createdTeam)).Returns(teamDto);

        // Act
        var result = await _teamService.CreateAsync(createDto);

        // Assert
        result.Should().NotBeNull();
        result.Should().BeEquivalentTo(teamDto);
        _unitOfWorkMock.Verify(u => u.Teams.AddAsync(It.IsAny<Team>()), Times.Once);
        _unitOfWorkMock.Verify(u => u.SaveChangesAsync(), Times.Once);
    }

    [TestMethod]
    public async Task UpdateAsync_WithValidData_ShouldUpdateTeam()
    {
        // Arrange
        var teamId = 1;
        var updateDto = new UpdateTeamDto
        {
            Name = "Argentina Updated",
            Code = "ARG",
            Confederation = "CONMEBOL",
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

        var teamDto = new TeamDto
        {
            Id = teamId,
            Name = "Argentina Updated",
            Code = "ARG",
            Confederation = "CONMEBOL",
            FifaRanking = 2,
            GroupId = 1
        };

        _unitOfWorkMock.Setup(u => u.Teams.GetByIdAsync(teamId)).ReturnsAsync(existingTeam);
        _mapperMock.Setup(m => m.Map(updateDto, existingTeam)).Returns(updatedTeam);
        _unitOfWorkMock.Setup(u => u.Teams.Update(updatedTeam));
        _unitOfWorkMock.Setup(u => u.SaveChangesAsync()).ReturnsAsync(1);
        _mapperMock.Setup(m => m.Map<TeamDto>(updatedTeam)).Returns(teamDto);

        // Act
        var result = await _teamService.UpdateAsync(teamId, updateDto);

        // Assert
        result.Should().NotBeNull();
        result.Should().BeEquivalentTo(teamDto);
        _unitOfWorkMock.Verify(u => u.Teams.Update(It.IsAny<Team>()), Times.Once);
        _unitOfWorkMock.Verify(u => u.SaveChangesAsync(), Times.Once);
    }

    [TestMethod]
    public async Task UpdateAsync_WithInvalidId_ShouldReturnNull()
    {
        // Arrange
        var teamId = 999;
        var updateDto = new UpdateTeamDto { Name = "Test" };
        _unitOfWorkMock.Setup(u => u.Teams.GetByIdAsync(teamId)).ReturnsAsync((Team)null);

        // Act
        var result = await _teamService.UpdateAsync(teamId, updateDto);

        // Assert
        result.Should().BeNull();
        _unitOfWorkMock.Verify(u => u.Teams.Update(It.IsAny<Team>()), Times.Never);
        _unitOfWorkMock.Verify(u => u.SaveChangesAsync(), Times.Never);
    }

    [TestMethod]
    public async Task DeleteAsync_WithValidId_ShouldReturnTrue()
    {
        // Arrange
        var teamId = 1;
        var team = new Team { Id = teamId, Name = "Argentina" };
        _unitOfWorkMock.Setup(u => u.Teams.GetByIdAsync(teamId)).ReturnsAsync(team);
        _unitOfWorkMock.Setup(u => u.Teams.Delete(team));
        _unitOfWorkMock.Setup(u => u.SaveChangesAsync()).ReturnsAsync(1);

        // Act
        var result = await _teamService.DeleteAsync(teamId);

        // Assert
        result.Should().BeTrue();
        _unitOfWorkMock.Verify(u => u.Teams.Delete(team), Times.Once);
        _unitOfWorkMock.Verify(u => u.SaveChangesAsync(), Times.Once);
    }

    [TestMethod]
    public async Task DeleteAsync_WithInvalidId_ShouldReturnFalse()
    {
        // Arrange
        var teamId = 999;
        _unitOfWorkMock.Setup(u => u.Teams.GetByIdAsync(teamId)).ReturnsAsync((Team)null);

        // Act
        var result = await _teamService.DeleteAsync(teamId);

        // Assert
        result.Should().BeFalse();
        _unitOfWorkMock.Verify(u => u.Teams.Delete(It.IsAny<Team>()), Times.Never);
        _unitOfWorkMock.Verify(u => u.SaveChangesAsync(), Times.Never);
    }

    [TestMethod]
    public async Task GetByConfederationAsync_ShouldReturnTeamsFromConfederation()
    {
        // Arrange
        var confederation = "CONMEBOL";
        var teams = new List<Team>
        {
            new Team { Id = 1, Name = "Argentina", Confederation = Confederation.CONMEBOL },
            new Team { Id = 2, Name = "Brazil", Confederation = Confederation.CONMEBOL }
        };

        var teamDtos = new List<TeamDto>
        {
            new TeamDto { Id = 1, Name = "Argentina", Confederation = "CONMEBOL" },
            new TeamDto { Id = 2, Name = "Brazil", Confederation = "CONMEBOL" }
        };

        _unitOfWorkMock.Setup(u => u.Teams.GetByConfederationAsync(Confederation.CONMEBOL))
            .ReturnsAsync(teams);
        _mapperMock.Setup(m => m.Map<IEnumerable<TeamDto>>(teams)).Returns(teamDtos);

        // Act
        var result = await _teamService.GetByConfederationAsync(confederation);

        // Assert
        result.Should().NotBeNull();
        result.Should().HaveCount(2);
        result.Should().BeEquivalentTo(teamDtos);
    }

    [TestMethod]
    public async Task GetByGroupIdAsync_ShouldReturnTeamsInGroup()
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

        _unitOfWorkMock.Setup(u => u.Teams.GetByGroupIdAsync(groupId)).ReturnsAsync(teams);
        _mapperMock.Setup(m => m.Map<IEnumerable<TeamDto>>(teams)).Returns(teamDtos);

        // Act
        var result = await _teamService.GetByGroupIdAsync(groupId);

        // Assert
        result.Should().NotBeNull();
        result.Should().HaveCount(2);
        result.Should().BeEquivalentTo(teamDtos);
    }
}

// Made with Bob
