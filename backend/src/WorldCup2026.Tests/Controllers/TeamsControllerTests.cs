using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using WorldCup2026.API.Controllers;
using WorldCup2026.Application.DTOs.Team;
using WorldCup2026.Application.Interfaces;

namespace WorldCup2026.Tests.Controllers;

[TestClass]
public class TeamsControllerTests
{
    private Mock<ITeamService> _teamServiceMock;
    private TeamsController _controller;

    [TestInitialize]
    public void Setup()
    {
        _teamServiceMock = new Mock<ITeamService>();
        _controller = new TeamsController(_teamServiceMock.Object);
    }

    [TestMethod]
    public async Task GetAll_ShouldReturnOkWithTeams()
    {
        // Arrange
        var teams = new List<TeamDto>
        {
            new TeamDto { Id = 1, Name = "Argentina", Code = "ARG" },
            new TeamDto { Id = 2, Name = "Brazil", Code = "BRA" }
        };
        _teamServiceMock.Setup(s => s.GetAllAsync()).ReturnsAsync(teams);

        // Act
        var result = await _controller.GetAll();

        // Assert
        result.Result.Should().BeOfType<OkObjectResult>();
        var okResult = result.Result as OkObjectResult;
        okResult.Value.Should().BeEquivalentTo(teams);
    }

    [TestMethod]
    public async Task GetById_WithValidId_ShouldReturnOkWithTeam()
    {
        // Arrange
        var teamId = 1;
        var team = new TeamDto { Id = teamId, Name = "Argentina", Code = "ARG" };
        _teamServiceMock.Setup(s => s.GetByIdAsync(teamId)).ReturnsAsync(team);

        // Act
        var result = await _controller.GetById(teamId);

        // Assert
        result.Result.Should().BeOfType<OkObjectResult>();
        var okResult = result.Result as OkObjectResult;
        okResult.Value.Should().BeEquivalentTo(team);
    }

    [TestMethod]
    public async Task GetById_WithInvalidId_ShouldReturnNotFound()
    {
        // Arrange
        var teamId = 999;
        _teamServiceMock.Setup(s => s.GetByIdAsync(teamId)).ReturnsAsync((TeamDto)null);

        // Act
        var result = await _controller.GetById(teamId);

        // Assert
        result.Result.Should().BeOfType<NotFoundResult>();
    }

    [TestMethod]
    public async Task Create_WithValidData_ShouldReturnCreatedAtAction()
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

        var createdTeam = new TeamDto
        {
            Id = 1,
            Name = "Argentina",
            Code = "ARG",
            Confederation = "CONMEBOL",
            FifaRanking = 1,
            GroupId = 1
        };

        _teamServiceMock.Setup(s => s.CreateAsync(createDto)).ReturnsAsync(createdTeam);

        // Act
        var result = await _controller.Create(createDto);

        // Assert
        result.Result.Should().BeOfType<CreatedAtActionResult>();
        var createdResult = result.Result as CreatedAtActionResult;
        createdResult.ActionName.Should().Be(nameof(_controller.GetById));
        createdResult.RouteValues["id"].Should().Be(createdTeam.Id);
        createdResult.Value.Should().BeEquivalentTo(createdTeam);
    }

    [TestMethod]
    public async Task Update_WithValidData_ShouldReturnOkWithUpdatedTeam()
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

        var updatedTeam = new TeamDto
        {
            Id = teamId,
            Name = "Argentina Updated",
            Code = "ARG",
            Confederation = "CONMEBOL",
            FifaRanking = 2,
            GroupId = 1
        };

        _teamServiceMock.Setup(s => s.UpdateAsync(teamId, updateDto)).ReturnsAsync(updatedTeam);

        // Act
        var result = await _controller.Update(teamId, updateDto);

        // Assert
        result.Result.Should().BeOfType<OkObjectResult>();
        var okResult = result.Result as OkObjectResult;
        okResult.Value.Should().BeEquivalentTo(updatedTeam);
    }

    [TestMethod]
    public async Task Update_WithInvalidId_ShouldReturnNotFound()
    {
        // Arrange
        var teamId = 999;
        var updateDto = new UpdateTeamDto { Name = "Test" };
        _teamServiceMock.Setup(s => s.UpdateAsync(teamId, updateDto)).ReturnsAsync((TeamDto)null);

        // Act
        var result = await _controller.Update(teamId, updateDto);

        // Assert
        result.Result.Should().BeOfType<NotFoundResult>();
    }

    [TestMethod]
    public async Task Delete_WithValidId_ShouldReturnNoContent()
    {
        // Arrange
        var teamId = 1;
        _teamServiceMock.Setup(s => s.DeleteAsync(teamId)).ReturnsAsync(true);

        // Act
        var result = await _controller.Delete(teamId);

        // Assert
        result.Should().BeOfType<NoContentResult>();
    }

    [TestMethod]
    public async Task Delete_WithInvalidId_ShouldReturnNotFound()
    {
        // Arrange
        var teamId = 999;
        _teamServiceMock.Setup(s => s.DeleteAsync(teamId)).ReturnsAsync(false);

        // Act
        var result = await _controller.Delete(teamId);

        // Assert
        result.Should().BeOfType<NotFoundResult>();
    }

    [TestMethod]
    public async Task GetByConfederation_ShouldReturnOkWithTeams()
    {
        // Arrange
        var confederation = "CONMEBOL";
        var teams = new List<TeamDto>
        {
            new TeamDto { Id = 1, Name = "Argentina", Confederation = confederation },
            new TeamDto { Id = 2, Name = "Brazil", Confederation = confederation }
        };
        _teamServiceMock.Setup(s => s.GetByConfederationAsync(confederation)).ReturnsAsync(teams);

        // Act
        var result = await _controller.GetByConfederation(confederation);

        // Assert
        result.Result.Should().BeOfType<OkObjectResult>();
        var okResult = result.Result as OkObjectResult;
        okResult.Value.Should().BeEquivalentTo(teams);
    }

    [TestMethod]
    public async Task GetByGroup_ShouldReturnOkWithTeams()
    {
        // Arrange
        var groupId = 1;
        var teams = new List<TeamDto>
        {
            new TeamDto { Id = 1, Name = "Argentina", GroupId = groupId },
            new TeamDto { Id = 2, Name = "Brazil", GroupId = groupId }
        };
        _teamServiceMock.Setup(s => s.GetByGroupIdAsync(groupId)).ReturnsAsync(teams);

        // Act
        var result = await _controller.GetByGroup(groupId);

        // Assert
        result.Result.Should().BeOfType<OkObjectResult>();
        var okResult = result.Result as OkObjectResult;
        okResult.Value.Should().BeEquivalentTo(teams);
    }

    [TestMethod]
    public async Task Search_ShouldReturnOkWithMatchingTeams()
    {
        // Arrange
        var searchTerm = "Arg";
        var teams = new List<TeamDto>
        {
            new TeamDto { Id = 1, Name = "Argentina", Code = "ARG" }
        };
        _teamServiceMock.Setup(s => s.SearchAsync(searchTerm)).ReturnsAsync(teams);

        // Act
        var result = await _controller.Search(searchTerm);

        // Assert
        result.Result.Should().BeOfType<OkObjectResult>();
        var okResult = result.Result as OkObjectResult;
        okResult.Value.Should().BeEquivalentTo(teams);
    }
}

// Made with Bob
