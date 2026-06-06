using AutoMapper;
using FluentAssertions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using WorldCup2026.Application.DTOs.Group;
using WorldCup2026.Application.Services;
using WorldCup2026.Domain.Entities;
using WorldCup2026.Domain.Interfaces;
using DomainMatch = WorldCup2026.Domain.Entities.Match;

namespace WorldCup2026.Tests.Services;

[TestClass]
public class GroupServiceTests
{
    private Mock<IUnitOfWork> _unitOfWorkMock;
    private Mock<IMapper> _mapperMock;
    private GroupService _groupService;

    [TestInitialize]
    public void Setup()
    {
        _unitOfWorkMock = new Mock<IUnitOfWork>();
        _mapperMock = new Mock<IMapper>();
        _groupService = new GroupService(_unitOfWorkMock.Object, _mapperMock.Object);
    }

    [TestMethod]
    public async Task GetAllGroupsAsync_ShouldReturnOrderedGroups()
    {
        // Arrange
        var groups = new List<Group>
        {
            new Group { Id = 1, Name = "B" },
            new Group { Id = 2, Name = "A" },
            new Group { Id = 3, Name = "C" }
        };

        var groupDtos = new List<GroupDto>
        {
            new GroupDto { Id = 2, Name = "A" },
            new GroupDto { Id = 1, Name = "B" },
            new GroupDto { Id = 3, Name = "C" }
        };

        _unitOfWorkMock.Setup(u => u.Groups.GetAllAsync(It.IsAny<CancellationToken>())).ReturnsAsync(groups);
        _mapperMock.Setup(m => m.Map<IEnumerable<GroupDto>>(It.IsAny<IEnumerable<Group>>())).Returns(groupDtos);

        // Act
        var result = await _groupService.GetAllGroupsAsync();

        // Assert
        result.Should().NotBeNull();
        result.Should().HaveCount(3);
        _unitOfWorkMock.Verify(u => u.Groups.GetAllAsync(It.IsAny<CancellationToken>()), Times.Once);
    }

    [TestMethod]
    public async Task GetGroupByIdAsync_WithValidId_ShouldReturnGroup()
    {
        // Arrange
        var groupId = 1;
        var group = new Group { Id = groupId, Name = "A" };
        var groupDto = new GroupDto { Id = groupId, Name = "A" };

        _unitOfWorkMock.Setup(u => u.Groups.GetByIdAsync(groupId, It.IsAny<CancellationToken>())).ReturnsAsync(group);
        _mapperMock.Setup(m => m.Map<GroupDto>(group)).Returns(groupDto);

        // Act
        var result = await _groupService.GetGroupByIdAsync(groupId);

        // Assert
        result.Should().NotBeNull();
        result.Should().BeEquivalentTo(groupDto);
        _unitOfWorkMock.Verify(u => u.Groups.GetByIdAsync(groupId, It.IsAny<CancellationToken>()), Times.Once);
    }

    [TestMethod]
    public async Task GetGroupByIdAsync_WithInvalidId_ShouldReturnNull()
    {
        // Arrange
        var groupId = 999;
        _unitOfWorkMock.Setup(u => u.Groups.GetByIdAsync(groupId, It.IsAny<CancellationToken>())).ReturnsAsync((Group)null);

        // Act
        var result = await _groupService.GetGroupByIdAsync(groupId);

        // Assert
        result.Should().BeNull();
        _unitOfWorkMock.Verify(u => u.Groups.GetByIdAsync(groupId, It.IsAny<CancellationToken>()), Times.Once);
    }

    [TestMethod]
    public async Task GetGroupByNameAsync_WithValidName_ShouldReturnGroup()
    {
        // Arrange
        var groupName = "A";
        var group = new Group { Id = 1, Name = groupName };
        var groupDto = new GroupDto { Id = 1, Name = groupName };

        _unitOfWorkMock.Setup(u => u.Groups.GetByNameAsync(groupName, It.IsAny<CancellationToken>())).ReturnsAsync(group);
        _mapperMock.Setup(m => m.Map<GroupDto>(group)).Returns(groupDto);

        // Act
        var result = await _groupService.GetGroupByNameAsync(groupName);

        // Assert
        result.Should().NotBeNull();
        result.Should().BeEquivalentTo(groupDto);
        _unitOfWorkMock.Verify(u => u.Groups.GetByNameAsync(groupName, It.IsAny<CancellationToken>()), Times.Once);
    }

    [TestMethod]
    public async Task CreateGroupAsync_WithValidData_ShouldCreateGroup()
    {
        // Arrange
        var groupName = "A";
        var group = new Group { Id = 1, Name = groupName };
        var groupDto = new GroupDto { Id = 1, Name = groupName };

        _unitOfWorkMock.Setup(u => u.Groups.FindAsync(It.IsAny<System.Linq.Expressions.Expression<Func<Group, bool>>>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(new List<Group>());
        _unitOfWorkMock.Setup(u => u.Groups.AddAsync(It.IsAny<Group>(), It.IsAny<CancellationToken>())).ReturnsAsync(group);
        _unitOfWorkMock.Setup(u => u.SaveChangesAsync(It.IsAny<CancellationToken>())).ReturnsAsync(1);
        _unitOfWorkMock.Setup(u => u.Groups.GetByIdAsync(It.IsAny<int>(), It.IsAny<CancellationToken>())).ReturnsAsync(group);
        _mapperMock.Setup(m => m.Map<GroupDto>(group)).Returns(groupDto);

        // Act
        var result = await _groupService.CreateGroupAsync(groupName);

        // Assert
        result.Should().NotBeNull();
        result.Should().BeEquivalentTo(groupDto);
        _unitOfWorkMock.Verify(u => u.Groups.AddAsync(It.IsAny<Group>(), It.IsAny<CancellationToken>()), Times.Once);
        _unitOfWorkMock.Verify(u => u.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Once);
    }

    [TestMethod]
    public async Task CreateGroupAsync_WithDuplicateName_ShouldThrowException()
    {
        // Arrange
        var groupName = "A";
        var existingGroup = new Group { Id = 1, Name = groupName };

        _unitOfWorkMock.Setup(u => u.Groups.FindAsync(It.IsAny<System.Linq.Expressions.Expression<Func<Group, bool>>>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(new List<Group> { existingGroup });

        // Act & Assert
        var act = async () => await _groupService.CreateGroupAsync(groupName);
        await act.Should().ThrowAsync<InvalidOperationException>()
            .WithMessage($"Group with name '{groupName}' already exists.");

        _unitOfWorkMock.Verify(u => u.Groups.AddAsync(It.IsAny<Group>(), It.IsAny<CancellationToken>()), Times.Never);
    }

    [TestMethod]
    public async Task UpdateGroupAsync_WithValidData_ShouldUpdateGroup()
    {
        // Arrange
        var groupId = 1;
        var newName = "B";
        var existingGroup = new Group { Id = groupId, Name = "A" };
        var updatedGroup = new Group { Id = groupId, Name = newName };
        var groupDto = new GroupDto { Id = groupId, Name = newName };

        _unitOfWorkMock.Setup(u => u.Groups.GetByIdAsync(groupId, It.IsAny<CancellationToken>())).ReturnsAsync(existingGroup);
        _unitOfWorkMock.Setup(u => u.Groups.FindAsync(It.IsAny<System.Linq.Expressions.Expression<Func<Group, bool>>>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(new List<Group>());
        _unitOfWorkMock.Setup(u => u.Groups.Update(It.IsAny<Group>()));
        _unitOfWorkMock.Setup(u => u.SaveChangesAsync(It.IsAny<CancellationToken>())).ReturnsAsync(1);
        _unitOfWorkMock.SetupSequence(u => u.Groups.GetByIdAsync(groupId, It.IsAny<CancellationToken>()))
            .ReturnsAsync(existingGroup)
            .ReturnsAsync(updatedGroup);
        _mapperMock.Setup(m => m.Map<GroupDto>(updatedGroup)).Returns(groupDto);

        // Act
        var result = await _groupService.UpdateGroupAsync(groupId, newName);

        // Assert
        result.Should().NotBeNull();
        result.Name.Should().Be(newName);
        _unitOfWorkMock.Verify(u => u.Groups.Update(It.IsAny<Group>()), Times.Once);
        _unitOfWorkMock.Verify(u => u.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Once);
    }

    [TestMethod]
    public async Task UpdateGroupAsync_WithInvalidId_ShouldThrowException()
    {
        // Arrange
        var groupId = 999;
        var newName = "B";
        _unitOfWorkMock.Setup(u => u.Groups.GetByIdAsync(groupId, It.IsAny<CancellationToken>())).ReturnsAsync((Group)null);

        // Act & Assert
        var act = async () => await _groupService.UpdateGroupAsync(groupId, newName);
        await act.Should().ThrowAsync<InvalidOperationException>()
            .WithMessage($"Group with ID {groupId} not found.");

        _unitOfWorkMock.Verify(u => u.Groups.Update(It.IsAny<Group>()), Times.Never);
    }

    [TestMethod]
    public async Task DeleteGroupAsync_WithValidId_ShouldReturnTrue()
    {
        // Arrange
        var groupId = 1;
        var group = new Group { Id = groupId, Name = "A" };

        _unitOfWorkMock.Setup(u => u.Groups.GetByIdAsync(groupId, It.IsAny<CancellationToken>())).ReturnsAsync(group);
        _unitOfWorkMock.Setup(u => u.Teams.GetByGroupIdAsync(groupId, It.IsAny<CancellationToken>())).ReturnsAsync(new List<Team>());
        _unitOfWorkMock.Setup(u => u.Matches.GetByGroupIdAsync(groupId, It.IsAny<CancellationToken>())).ReturnsAsync(new List<DomainMatch>());
        _unitOfWorkMock.Setup(u => u.Groups.Delete(group));
        _unitOfWorkMock.Setup(u => u.SaveChangesAsync(It.IsAny<CancellationToken>())).ReturnsAsync(1);

        // Act
        var result = await _groupService.DeleteGroupAsync(groupId);

        // Assert
        result.Should().BeTrue();
        _unitOfWorkMock.Verify(u => u.Groups.Delete(group), Times.Once);
        _unitOfWorkMock.Verify(u => u.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Once);
    }

    [TestMethod]
    public async Task DeleteGroupAsync_WithInvalidId_ShouldReturnFalse()
    {
        // Arrange
        var groupId = 999;
        _unitOfWorkMock.Setup(u => u.Groups.GetByIdAsync(groupId, It.IsAny<CancellationToken>())).ReturnsAsync((Group)null);

        // Act
        var result = await _groupService.DeleteGroupAsync(groupId);

        // Assert
        result.Should().BeFalse();
        _unitOfWorkMock.Verify(u => u.Groups.Delete(It.IsAny<Group>()), Times.Never);
        _unitOfWorkMock.Verify(u => u.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Never);
    }

    [TestMethod]
    public async Task DeleteGroupAsync_WithTeamsAssigned_ShouldThrowException()
    {
        // Arrange
        var groupId = 1;
        var group = new Group { Id = groupId, Name = "A" };
        var teams = new List<Team> { new Team { Id = 1, Name = "Team 1", GroupId = groupId } };

        _unitOfWorkMock.Setup(u => u.Groups.GetByIdAsync(groupId, It.IsAny<CancellationToken>())).ReturnsAsync(group);
        _unitOfWorkMock.Setup(u => u.Teams.GetByGroupIdAsync(groupId, It.IsAny<CancellationToken>())).ReturnsAsync(teams);

        // Act & Assert
        var act = async () => await _groupService.DeleteGroupAsync(groupId);
        await act.Should().ThrowAsync<InvalidOperationException>()
            .WithMessage($"Cannot delete group '{group.Name}' because it has teams assigned.");

        _unitOfWorkMock.Verify(u => u.Groups.Delete(It.IsAny<Group>()), Times.Never);
    }

    [TestMethod]
    public async Task GroupExistsAsync_WithExistingId_ShouldReturnTrue()
    {
        // Arrange
        var groupId = 1;
        var group = new Group { Id = groupId, Name = "A" };
        _unitOfWorkMock.Setup(u => u.Groups.GetByIdAsync(groupId, It.IsAny<CancellationToken>())).ReturnsAsync(group);

        // Act
        var result = await _groupService.GroupExistsAsync(groupId);

        // Assert
        result.Should().BeTrue();
    }

    [TestMethod]
    public async Task GroupExistsAsync_WithNonExistingId_ShouldReturnFalse()
    {
        // Arrange
        var groupId = 999;
        _unitOfWorkMock.Setup(u => u.Groups.GetByIdAsync(groupId, It.IsAny<CancellationToken>())).ReturnsAsync((Group)null);

        // Act
        var result = await _groupService.GroupExistsAsync(groupId);

        // Assert
        result.Should().BeFalse();
    }

    [TestMethod]
    public async Task GroupNameExistsAsync_WithExistingName_ShouldReturnTrue()
    {
        // Arrange
        var groupName = "A";
        var groups = new List<Group> { new Group { Id = 1, Name = groupName } };

        _unitOfWorkMock.Setup(u => u.Groups.FindAsync(It.IsAny<System.Linq.Expressions.Expression<Func<Group, bool>>>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(groups);

        // Act
        var result = await _groupService.GroupNameExistsAsync(groupName);

        // Assert
        result.Should().BeTrue();
    }

    [TestMethod]
    public async Task GroupNameExistsAsync_WithNonExistingName_ShouldReturnFalse()
    {
        // Arrange
        var groupName = "Z";
        _unitOfWorkMock.Setup(u => u.Groups.FindAsync(It.IsAny<System.Linq.Expressions.Expression<Func<Group, bool>>>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(new List<Group>());

        // Act
        var result = await _groupService.GroupNameExistsAsync(groupName);

        // Assert
        result.Should().BeFalse();
    }
}

// Made with Bob