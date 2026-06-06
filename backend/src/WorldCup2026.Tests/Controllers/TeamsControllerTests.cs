using FluentAssertions;
using FluentValidation;
using FluentValidation.Results;
using Microsoft.AspNetCore.Mvc;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using WorldCup2026.API.Controllers;
using WorldCup2026.Application.DTOs.Common;
using WorldCup2026.Application.DTOs.Team;
using WorldCup2026.Application.Interfaces;
using WorldCup2026.Domain.Enums;

namespace WorldCup2026.Tests.Controllers;

[TestClass]
public class TeamsControllerTests
{
    private Mock<ITeamService> _teamServiceMock;
    private Mock<IValidator<CreateTeamDto>> _createValidatorMock;
    private Mock<IValidator<UpdateTeamDto>> _updateValidatorMock;
    private TeamsController _controller;

    [TestInitialize]
    public void Setup()
    {
        _teamServiceMock = new Mock<ITeamService>();
        _createValidatorMock = new Mock<IValidator<CreateTeamDto>>();
        _updateValidatorMock = new Mock<IValidator<UpdateTeamDto>>();
        _controller = new TeamsController(
            _teamServiceMock.Object,
            _createValidatorMock.Object,
            _updateValidatorMock.Object);
    }

    [TestMethod]
    public async Task GetAllTeams_ShouldReturnOkWithPagedTeams()
    {
        // Arrange
        var pagedResult = new PagedResult<TeamDto>
        {
            Items = new List<TeamDto>
            {
                new TeamDto { Id = 1, Name = "Argentina", Code = "ARG" },
                new TeamDto { Id = 2, Name = "Brazil", Code = "BRA" }
            },
            TotalCount = 2,
            PageNumber = 1,
            PageSize = 10
        };
        _teamServiceMock.Setup(s => s.GetAllTeamsAsync(
            It.IsAny<int>(), It.IsAny<int>(), It.IsAny<Confederation?>(),
            It.IsAny<int?>(), It.IsAny<string?>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(pagedResult);

        // Act
        var result = await _controller.GetAllTeams();

        // Assert
        result.Should().BeOfType<OkObjectResult>();
        var okResult = result as OkObjectResult;
        okResult.Value.Should().BeEquivalentTo(pagedResult);
    }

    [TestMethod]
    public async Task GetTeamById_WithValidId_ShouldReturnOkWithTeam()
    {
        // Arrange
        var teamId = 1;
        var team = new TeamDto { Id = teamId, Name = "Argentina", Code = "ARG" };
        _teamServiceMock.Setup(s => s.GetTeamByIdAsync(teamId, It.IsAny<CancellationToken>())).ReturnsAsync(team);

        // Act
        var result = await _controller.GetTeamById(teamId);

        // Assert
        result.Should().BeOfType<OkObjectResult>();
        var okResult = result as OkObjectResult;
        okResult.Value.Should().BeEquivalentTo(team);
    }

    [TestMethod]
    public async Task GetTeamById_WithInvalidId_ShouldReturnNotFound()
    {
        // Arrange
        var teamId = 999;
        _teamServiceMock.Setup(s => s.GetTeamByIdAsync(teamId, It.IsAny<CancellationToken>())).ReturnsAsync((TeamDto)null);

        // Act
        var result = await _controller.GetTeamById(teamId);

        // Assert
        result.Should().BeOfType<NotFoundObjectResult>();
    }

    [TestMethod]
    public async Task CreateTeam_WithValidData_ShouldReturnCreatedAtAction()
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

        var createdTeam = new TeamDto
        {
            Id = 1,
            Name = "Argentina",
            Code = "ARG",
            Confederation = Confederation.CONMEBOL,
            FifaRanking = 1,
            GroupId = 1
        };

        _createValidatorMock.Setup(v => v.ValidateAsync(createDto, It.IsAny<CancellationToken>()))
            .ReturnsAsync(new ValidationResult());
        _teamServiceMock.Setup(s => s.CreateTeamAsync(createDto, It.IsAny<CancellationToken>())).ReturnsAsync(createdTeam);

        // Act
        var result = await _controller.CreateTeam(createDto);

        // Assert
        result.Should().BeOfType<CreatedAtActionResult>();
        var createdResult = result as CreatedAtActionResult;
        createdResult.ActionName.Should().Be(nameof(_controller.GetTeamById));
        createdResult.RouteValues["id"].Should().Be(createdTeam.Id);
        createdResult.Value.Should().BeEquivalentTo(createdTeam);
    }

    [TestMethod]
    public async Task UpdateTeam_WithValidData_ShouldReturnOkWithUpdatedTeam()
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

        var updatedTeam = new TeamDto
        {
            Id = teamId,
            Name = "Argentina Updated",
            Code = "ARG",
            Confederation = Confederation.CONMEBOL,
            FifaRanking = 2,
            GroupId = 1
        };

        _updateValidatorMock.Setup(v => v.ValidateAsync(updateDto, It.IsAny<CancellationToken>()))
            .ReturnsAsync(new ValidationResult());
        _teamServiceMock.Setup(s => s.UpdateTeamAsync(teamId, updateDto, It.IsAny<CancellationToken>())).ReturnsAsync(updatedTeam);

        // Act
        var result = await _controller.UpdateTeam(teamId, updateDto);

        // Assert
        result.Should().BeOfType<OkObjectResult>();
        var okResult = result as OkObjectResult;
        okResult.Value.Should().BeEquivalentTo(updatedTeam);
    }

    [TestMethod]
    public async Task UpdateTeam_WithInvalidId_ShouldReturnNotFound()
    {
        // Arrange
        var teamId = 999;
        var updateDto = new UpdateTeamDto { Name = "Test", Code = "TST", Confederation = Confederation.UEFA };
        
        _updateValidatorMock.Setup(v => v.ValidateAsync(updateDto, It.IsAny<CancellationToken>()))
            .ReturnsAsync(new ValidationResult());
        _teamServiceMock.Setup(s => s.UpdateTeamAsync(teamId, updateDto, It.IsAny<CancellationToken>())).ReturnsAsync((TeamDto)null);

        // Act
        var result = await _controller.UpdateTeam(teamId, updateDto);

        // Assert
        result.Should().BeOfType<NotFoundObjectResult>();
    }

    [TestMethod]
    public async Task DeleteTeam_WithValidId_ShouldReturnNoContent()
    {
        // Arrange
        var teamId = 1;
        _teamServiceMock.Setup(s => s.DeleteTeamAsync(teamId, It.IsAny<CancellationToken>())).ReturnsAsync(true);

        // Act
        var result = await _controller.DeleteTeam(teamId);

        // Assert
        result.Should().BeOfType<NoContentResult>();
    }

    [TestMethod]
    public async Task DeleteTeam_WithInvalidId_ShouldReturnNotFound()
    {
        // Arrange
        var teamId = 999;
        _teamServiceMock.Setup(s => s.DeleteTeamAsync(teamId, It.IsAny<CancellationToken>())).ReturnsAsync(false);

        // Act
        var result = await _controller.DeleteTeam(teamId);

        // Assert
        result.Should().BeOfType<NotFoundObjectResult>();
    }

    [TestMethod]
    public async Task GetTeamsByConfederation_ShouldReturnOkWithTeams()
    {
        // Arrange
        var confederation = Confederation.CONMEBOL;
        var teams = new List<TeamDto>
        {
            new TeamDto { Id = 1, Name = "Argentina", Confederation = Confederation.CONMEBOL },
            new TeamDto { Id = 2, Name = "Brazil", Confederation = Confederation.CONMEBOL }
        };
        _teamServiceMock.Setup(s => s.GetTeamsByConfederationAsync(confederation, It.IsAny<CancellationToken>())).ReturnsAsync(teams);

        // Act
        var result = await _controller.GetTeamsByConfederation(confederation);

        // Assert
        result.Should().BeOfType<OkObjectResult>();
        var okResult = result as OkObjectResult;
        okResult.Value.Should().BeEquivalentTo(teams);
    }

    [TestMethod]
    public async Task GetTeamsByGroup_ShouldReturnOkWithTeams()
    {
        // Arrange
        var groupId = 1;
        var teams = new List<TeamDto>
        {
            new TeamDto { Id = 1, Name = "Argentina", GroupId = groupId },
            new TeamDto { Id = 2, Name = "Brazil", GroupId = groupId }
        };
        _teamServiceMock.Setup(s => s.GetTeamsByGroupAsync(groupId, It.IsAny<CancellationToken>())).ReturnsAsync(teams);

        // Act
        var result = await _controller.GetTeamsByGroup(groupId);

        // Assert
        result.Should().BeOfType<OkObjectResult>();
        var okResult = result as OkObjectResult;
        okResult.Value.Should().BeEquivalentTo(teams);
    }
}

// Made with Bob
