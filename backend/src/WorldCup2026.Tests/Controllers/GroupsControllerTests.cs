using FluentValidation;
using FluentValidation.Results;
using Microsoft.AspNetCore.Mvc;
using Moq;
using WorldCup2026.API.Controllers;
using WorldCup2026.Application.DTOs.Group;
using WorldCup2026.Application.Interfaces;

namespace WorldCup2026.Tests.Controllers;

[TestClass]
public class GroupsControllerTests
{
    private Mock<IGroupService> _groupServiceMock;
    private Mock<IValidator<CreateGroupDto>> _createValidatorMock;
    private Mock<IValidator<UpdateGroupDto>> _updateValidatorMock;
    private GroupsController _controller;

    [TestInitialize]
    public void Setup()
    {
        _groupServiceMock = new Mock<IGroupService>();
        _createValidatorMock = new Mock<IValidator<CreateGroupDto>>();
        _updateValidatorMock = new Mock<IValidator<UpdateGroupDto>>();
        _controller = new GroupsController(
            _groupServiceMock.Object,
            _createValidatorMock.Object,
            _updateValidatorMock.Object);
    }

    [TestMethod]
    public async Task GetAllGroups_ReturnsOkWithGroups()
    {
        // Arrange
        var groups = new List<GroupDto>
        {
            new() { Id = 1, Name = "Group A" },
            new() { Id = 2, Name = "Group B" }
        };

        _groupServiceMock.Setup(s => s.GetAllGroupsAsync(It.IsAny<CancellationToken>()))
            .ReturnsAsync(groups);

        // Act
        var result = await _controller.GetAllGroups();

        // Assert
        var okResult = result as OkObjectResult;
        Assert.IsNotNull(okResult);
        Assert.AreEqual(200, okResult.StatusCode);
        
        var returnedGroups = okResult.Value as List<GroupDto>;
        Assert.IsNotNull(returnedGroups);
        Assert.AreEqual(2, returnedGroups.Count);
    }

    [TestMethod]
    public async Task GetAllGroupsWithStandings_ReturnsOkWithGroupsAndStandings()
    {
        // Arrange
        var groupsWithStandings = new List<GroupWithStandingsDto>
        {
            new() { Id = 1, Name = "Group A", Standings = new List<Application.DTOs.Standing.StandingDto>() },
            new() { Id = 2, Name = "Group B", Standings = new List<Application.DTOs.Standing.StandingDto>() }
        };

        _groupServiceMock.Setup(s => s.GetAllGroupsWithStandingsAsync(It.IsAny<CancellationToken>()))
            .ReturnsAsync(groupsWithStandings);

        // Act
        var result = await _controller.GetAllGroupsWithStandings();

        // Assert
        var okResult = result as OkObjectResult;
        Assert.IsNotNull(okResult);
        Assert.AreEqual(200, okResult.StatusCode);
        
        var returnedGroups = okResult.Value as List<GroupWithStandingsDto>;
        Assert.IsNotNull(returnedGroups);
        Assert.AreEqual(2, returnedGroups.Count);
    }

    [TestMethod]
    public async Task GetGroupById_ExistingGroup_ReturnsOkWithGroup()
    {
        // Arrange
        var group = new GroupDto { Id = 1, Name = "Group A" };

        _groupServiceMock.Setup(s => s.GetGroupByIdAsync(1, It.IsAny<CancellationToken>()))
            .ReturnsAsync(group);

        // Act
        var result = await _controller.GetGroupById(1);

        // Assert
        var okResult = result as OkObjectResult;
        Assert.IsNotNull(okResult);
        Assert.AreEqual(200, okResult.StatusCode);
        
        var returnedGroup = okResult.Value as GroupDto;
        Assert.IsNotNull(returnedGroup);
        Assert.AreEqual(1, returnedGroup.Id);
        Assert.AreEqual("Group A", returnedGroup.Name);
    }

    [TestMethod]
    public async Task GetGroupById_NonExistingGroup_ReturnsNotFound()
    {
        // Arrange
        _groupServiceMock.Setup(s => s.GetGroupByIdAsync(999, It.IsAny<CancellationToken>()))
            .ReturnsAsync((GroupDto?)null);

        // Act
        var result = await _controller.GetGroupById(999);

        // Assert
        var notFoundResult = result as NotFoundObjectResult;
        Assert.IsNotNull(notFoundResult);
        Assert.AreEqual(404, notFoundResult.StatusCode);
    }

    [TestMethod]
    public async Task GetGroupWithStandings_ExistingGroup_ReturnsOkWithStandings()
    {
        // Arrange
        var groupWithStandings = new GroupWithStandingsDto
        {
            Id = 1,
            Name = "Group A",
            Standings = new List<Application.DTOs.Standing.StandingDto>
            {
                new() { Id = 1, TeamId = 1, Position = 1, Points = 9 }
            }
        };

        _groupServiceMock.Setup(s => s.GetGroupWithStandingsAsync(1, It.IsAny<CancellationToken>()))
            .ReturnsAsync(groupWithStandings);

        // Act
        var result = await _controller.GetGroupWithStandings(1);

        // Assert
        var okResult = result as OkObjectResult;
        Assert.IsNotNull(okResult);
        Assert.AreEqual(200, okResult.StatusCode);
        
        var returnedGroup = okResult.Value as GroupWithStandingsDto;
        Assert.IsNotNull(returnedGroup);
        Assert.AreEqual(1, returnedGroup.Id);
        Assert.AreEqual(1, returnedGroup.Standings.Count);
    }

    [TestMethod]
    public async Task GetGroupWithStandings_NonExistingGroup_ReturnsNotFound()
    {
        // Arrange
        _groupServiceMock.Setup(s => s.GetGroupWithStandingsAsync(999, It.IsAny<CancellationToken>()))
            .ReturnsAsync((GroupWithStandingsDto?)null);

        // Act
        var result = await _controller.GetGroupWithStandings(999);

        // Assert
        var notFoundResult = result as NotFoundObjectResult;
        Assert.IsNotNull(notFoundResult);
        Assert.AreEqual(404, notFoundResult.StatusCode);
    }

    [TestMethod]
    public async Task GetGroupByName_ExistingGroup_ReturnsOkWithGroup()
    {
        // Arrange
        var group = new GroupDto { Id = 1, Name = "Group A" };

        _groupServiceMock.Setup(s => s.GetGroupByNameAsync("Group A", It.IsAny<CancellationToken>()))
            .ReturnsAsync(group);

        // Act
        var result = await _controller.GetGroupByName("Group A");

        // Assert
        var okResult = result as OkObjectResult;
        Assert.IsNotNull(okResult);
        Assert.AreEqual(200, okResult.StatusCode);
        
        var returnedGroup = okResult.Value as GroupDto;
        Assert.IsNotNull(returnedGroup);
        Assert.AreEqual("Group A", returnedGroup.Name);
    }

    [TestMethod]
    public async Task GetGroupByName_NonExistingGroup_ReturnsNotFound()
    {
        // Arrange
        _groupServiceMock.Setup(s => s.GetGroupByNameAsync("Group Z", It.IsAny<CancellationToken>()))
            .ReturnsAsync((GroupDto?)null);

        // Act
        var result = await _controller.GetGroupByName("Group Z");

        // Assert
        var notFoundResult = result as NotFoundObjectResult;
        Assert.IsNotNull(notFoundResult);
        Assert.AreEqual(404, notFoundResult.StatusCode);
    }

    [TestMethod]
    public async Task CreateGroup_ValidData_ReturnsCreatedAtAction()
    {
        // Arrange
        var createDto = new CreateGroupDto { Name = "Group A" };
        var createdGroup = new GroupDto { Id = 1, Name = "Group A" };

        _createValidatorMock.Setup(v => v.ValidateAsync(createDto, It.IsAny<CancellationToken>()))
            .ReturnsAsync(new ValidationResult());

        _groupServiceMock.Setup(s => s.CreateGroupAsync("Group A", It.IsAny<CancellationToken>()))
            .ReturnsAsync(createdGroup);

        // Act
        var result = await _controller.CreateGroup(createDto);

        // Assert
        var createdResult = result as CreatedAtActionResult;
        Assert.IsNotNull(createdResult);
        Assert.AreEqual(201, createdResult.StatusCode);
        Assert.AreEqual(nameof(GroupsController.GetGroupById), createdResult.ActionName);
        
        var returnedGroup = createdResult.Value as GroupDto;
        Assert.IsNotNull(returnedGroup);
        Assert.AreEqual(1, returnedGroup.Id);
        Assert.AreEqual("Group A", returnedGroup.Name);
    }

    [TestMethod]
    public async Task CreateGroup_InvalidData_ReturnsBadRequest()
    {
        // Arrange
        var createDto = new CreateGroupDto { Name = "" };
        var validationResult = new ValidationResult(new[]
        {
            new ValidationFailure("Name", "Name is required")
        });

        _createValidatorMock.Setup(v => v.ValidateAsync(createDto, It.IsAny<CancellationToken>()))
            .ReturnsAsync(validationResult);

        // Act
        var result = await _controller.CreateGroup(createDto);

        // Assert
        var badRequestResult = result as BadRequestObjectResult;
        Assert.IsNotNull(badRequestResult);
        Assert.AreEqual(400, badRequestResult.StatusCode);
    }

    [TestMethod]
    public async Task UpdateGroup_ValidData_ReturnsOkWithUpdatedGroup()
    {
        // Arrange
        var updateDto = new UpdateGroupDto { Name = "Group A Updated" };
        var updatedGroup = new GroupDto { Id = 1, Name = "Group A Updated" };

        _updateValidatorMock.Setup(v => v.ValidateAsync(updateDto, It.IsAny<CancellationToken>()))
            .ReturnsAsync(new ValidationResult());

        _groupServiceMock.Setup(s => s.UpdateGroupAsync(1, "Group A Updated", It.IsAny<CancellationToken>()))
            .ReturnsAsync(updatedGroup);

        // Act
        var result = await _controller.UpdateGroup(1, updateDto);

        // Assert
        var okResult = result as OkObjectResult;
        Assert.IsNotNull(okResult);
        Assert.AreEqual(200, okResult.StatusCode);
        
        var returnedGroup = okResult.Value as GroupDto;
        Assert.IsNotNull(returnedGroup);
        Assert.AreEqual("Group A Updated", returnedGroup.Name);
    }

    [TestMethod]
    public async Task UpdateGroup_InvalidData_ReturnsBadRequest()
    {
        // Arrange
        var updateDto = new UpdateGroupDto { Name = "" };
        var validationResult = new ValidationResult(new[]
        {
            new ValidationFailure("Name", "Name is required")
        });

        _updateValidatorMock.Setup(v => v.ValidateAsync(updateDto, It.IsAny<CancellationToken>()))
            .ReturnsAsync(validationResult);

        // Act
        var result = await _controller.UpdateGroup(1, updateDto);

        // Assert
        var badRequestResult = result as BadRequestObjectResult;
        Assert.IsNotNull(badRequestResult);
        Assert.AreEqual(400, badRequestResult.StatusCode);
    }

    [TestMethod]
    public async Task UpdateGroup_NonExistingGroup_ReturnsNotFound()
    {
        // Arrange
        var updateDto = new UpdateGroupDto { Name = "Group A Updated" };

        _updateValidatorMock.Setup(v => v.ValidateAsync(updateDto, It.IsAny<CancellationToken>()))
            .ReturnsAsync(new ValidationResult());

        _groupServiceMock.Setup(s => s.UpdateGroupAsync(999, "Group A Updated", It.IsAny<CancellationToken>()))
            .ReturnsAsync((GroupDto?)null);

        // Act
        var result = await _controller.UpdateGroup(999, updateDto);

        // Assert
        var notFoundResult = result as NotFoundObjectResult;
        Assert.IsNotNull(notFoundResult);
        Assert.AreEqual(404, notFoundResult.StatusCode);
    }

    [TestMethod]
    public async Task DeleteGroup_ExistingGroup_ReturnsNoContent()
    {
        // Arrange
        _groupServiceMock.Setup(s => s.DeleteGroupAsync(1, It.IsAny<CancellationToken>()))
            .ReturnsAsync(true);

        // Act
        var result = await _controller.DeleteGroup(1);

        // Assert
        var noContentResult = result as NoContentResult;
        Assert.IsNotNull(noContentResult);
        Assert.AreEqual(204, noContentResult.StatusCode);
    }

    [TestMethod]
    public async Task DeleteGroup_NonExistingGroup_ReturnsNotFound()
    {
        // Arrange
        _groupServiceMock.Setup(s => s.DeleteGroupAsync(999, It.IsAny<CancellationToken>()))
            .ReturnsAsync(false);

        // Act
        var result = await _controller.DeleteGroup(999);

        // Assert
        var notFoundResult = result as NotFoundObjectResult;
        Assert.IsNotNull(notFoundResult);
        Assert.AreEqual(404, notFoundResult.StatusCode);
    }

    [TestMethod]
    public async Task GroupExists_ExistingGroup_ReturnsOk()
    {
        // Arrange
        _groupServiceMock.Setup(s => s.GroupExistsAsync(1, It.IsAny<CancellationToken>()))
            .ReturnsAsync(true);

        // Act
        var result = await _controller.GroupExists(1);

        // Assert
        var okResult = result as OkResult;
        Assert.IsNotNull(okResult);
        Assert.AreEqual(200, okResult.StatusCode);
    }

    [TestMethod]
    public async Task GroupExists_NonExistingGroup_ReturnsNotFound()
    {
        // Arrange
        _groupServiceMock.Setup(s => s.GroupExistsAsync(999, It.IsAny<CancellationToken>()))
            .ReturnsAsync(false);

        // Act
        var result = await _controller.GroupExists(999);

        // Assert
        var notFoundResult = result as NotFoundResult;
        Assert.IsNotNull(notFoundResult);
        Assert.AreEqual(404, notFoundResult.StatusCode);
    }
}

// Made with Bob