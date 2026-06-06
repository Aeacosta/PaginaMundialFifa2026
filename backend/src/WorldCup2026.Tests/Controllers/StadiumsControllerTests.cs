using FluentValidation;
using FluentValidation.Results;
using Microsoft.AspNetCore.Mvc;
using Moq;
using WorldCup2026.API.Controllers;
using WorldCup2026.Application.DTOs.Common;
using WorldCup2026.Application.DTOs.Stadium;
using WorldCup2026.Application.Interfaces;

namespace WorldCup2026.Tests.Controllers;

[TestClass]
public class StadiumsControllerTests
{
    private Mock<IStadiumService> _stadiumServiceMock;
    private Mock<IValidator<CreateStadiumDto>> _createValidatorMock;
    private Mock<IValidator<UpdateStadiumDto>> _updateValidatorMock;
    private StadiumsController _controller;

    [TestInitialize]
    public void Setup()
    {
        _stadiumServiceMock = new Mock<IStadiumService>();
        _createValidatorMock = new Mock<IValidator<CreateStadiumDto>>();
        _updateValidatorMock = new Mock<IValidator<UpdateStadiumDto>>();
        _controller = new StadiumsController(
            _stadiumServiceMock.Object,
            _createValidatorMock.Object,
            _updateValidatorMock.Object);
    }

    [TestMethod]
    public async Task GetAllStadiums_WithFilters_ReturnsOkWithPagedResult()
    {
        // Arrange
        var pagedResult = new PagedResult<StadiumDto>
        {
            Items = new List<StadiumDto>
            {
                new() { Id = 1, Name = "Stadium A", City = "City A", Country = "Country A" },
                new() { Id = 2, Name = "Stadium B", City = "City A", Country = "Country A" }
            },
            TotalCount = 2,
            PageNumber = 1,
            PageSize = 10
        };

        _stadiumServiceMock.Setup(s => s.GetAllStadiumsAsync(
            1, 10, "City A", null, null, It.IsAny<CancellationToken>()))
            .ReturnsAsync(pagedResult);

        // Act
        var result = await _controller.GetAllStadiums(1, 10, "City A", null, null);

        // Assert
        var okResult = result as OkObjectResult;
        Assert.IsNotNull(okResult);
        Assert.AreEqual(200, okResult.StatusCode);
        
        var returnedResult = okResult.Value as PagedResult<StadiumDto>;
        Assert.IsNotNull(returnedResult);
        Assert.AreEqual(2, returnedResult.Items.Count());
        Assert.AreEqual(2, returnedResult.TotalCount);
    }

    [TestMethod]
    public async Task GetStadiumById_ExistingStadium_ReturnsOkWithStadium()
    {
        // Arrange
        var stadium = new StadiumDto
        {
            Id = 1,
            Name = "Stadium A",
            City = "City A",
            Country = "Country A",
            Capacity = 50000
        };

        _stadiumServiceMock.Setup(s => s.GetStadiumByIdAsync(1, It.IsAny<CancellationToken>()))
            .ReturnsAsync(stadium);

        // Act
        var result = await _controller.GetStadiumById(1);

        // Assert
        var okResult = result as OkObjectResult;
        Assert.IsNotNull(okResult);
        Assert.AreEqual(200, okResult.StatusCode);
        
        var returnedStadium = okResult.Value as StadiumDto;
        Assert.IsNotNull(returnedStadium);
        Assert.AreEqual(1, returnedStadium.Id);
        Assert.AreEqual("Stadium A", returnedStadium.Name);
    }

    [TestMethod]
    public async Task GetStadiumById_NonExistingStadium_ReturnsNotFound()
    {
        // Arrange
        _stadiumServiceMock.Setup(s => s.GetStadiumByIdAsync(999, It.IsAny<CancellationToken>()))
            .ReturnsAsync((StadiumDto?)null);

        // Act
        var result = await _controller.GetStadiumById(999);

        // Assert
        var notFoundResult = result as NotFoundObjectResult;
        Assert.IsNotNull(notFoundResult);
        Assert.AreEqual(404, notFoundResult.StatusCode);
    }

    [TestMethod]
    public async Task GetStadiumsByCity_ReturnsOkWithStadiums()
    {
        // Arrange
        var stadiums = new List<StadiumDto>
        {
            new() { Id = 1, Name = "Stadium A", City = "New York", Country = "USA" },
            new() { Id = 2, Name = "Stadium B", City = "New York", Country = "USA" }
        };

        _stadiumServiceMock.Setup(s => s.GetStadiumsByCityAsync("New York", It.IsAny<CancellationToken>()))
            .ReturnsAsync(stadiums);

        // Act
        var result = await _controller.GetStadiumsByCity("New York");

        // Assert
        var okResult = result as OkObjectResult;
        Assert.IsNotNull(okResult);
        Assert.AreEqual(200, okResult.StatusCode);
        
        var returnedStadiums = okResult.Value as List<StadiumDto>;
        Assert.IsNotNull(returnedStadiums);
        Assert.AreEqual(2, returnedStadiums.Count);
        Assert.IsTrue(returnedStadiums.All(s => s.City == "New York"));
    }

    [TestMethod]
    public async Task GetStadiumsByCountry_ReturnsOkWithStadiums()
    {
        // Arrange
        var stadiums = new List<StadiumDto>
        {
            new() { Id = 1, Name = "Stadium A", City = "City A", Country = "USA" },
            new() { Id = 2, Name = "Stadium B", City = "City B", Country = "USA" },
            new() { Id = 3, Name = "Stadium C", City = "City C", Country = "USA" }
        };

        _stadiumServiceMock.Setup(s => s.GetStadiumsByCountryAsync("USA", It.IsAny<CancellationToken>()))
            .ReturnsAsync(stadiums);

        // Act
        var result = await _controller.GetStadiumsByCountry("USA");

        // Assert
        var okResult = result as OkObjectResult;
        Assert.IsNotNull(okResult);
        Assert.AreEqual(200, okResult.StatusCode);
        
        var returnedStadiums = okResult.Value as List<StadiumDto>;
        Assert.IsNotNull(returnedStadiums);
        Assert.AreEqual(3, returnedStadiums.Count);
        Assert.IsTrue(returnedStadiums.All(s => s.Country == "USA"));
    }

    [TestMethod]
    public async Task CreateStadium_ValidData_ReturnsCreatedAtAction()
    {
        // Arrange
        var createDto = new CreateStadiumDto
        {
            Name = "New Stadium",
            City = "New York",
            Country = "USA",
            Capacity = 60000,
            Latitude = 40.7128m,
            Longitude = -74.0060m
        };

        var createdStadium = new StadiumDto
        {
            Id = 1,
            Name = "New Stadium",
            City = "New York",
            Country = "USA",
            Capacity = 60000
        };

        _createValidatorMock.Setup(v => v.ValidateAsync(createDto, It.IsAny<CancellationToken>()))
            .ReturnsAsync(new ValidationResult());

        _stadiumServiceMock.Setup(s => s.CreateStadiumAsync(createDto, It.IsAny<CancellationToken>()))
            .ReturnsAsync(createdStadium);

        // Act
        var result = await _controller.CreateStadium(createDto);

        // Assert
        var createdResult = result as CreatedAtActionResult;
        Assert.IsNotNull(createdResult);
        Assert.AreEqual(201, createdResult.StatusCode);
        Assert.AreEqual(nameof(StadiumsController.GetStadiumById), createdResult.ActionName);
        
        var returnedStadium = createdResult.Value as StadiumDto;
        Assert.IsNotNull(returnedStadium);
        Assert.AreEqual(1, returnedStadium.Id);
        Assert.AreEqual("New Stadium", returnedStadium.Name);
    }

    [TestMethod]
    public async Task CreateStadium_InvalidData_ReturnsBadRequest()
    {
        // Arrange
        var createDto = new CreateStadiumDto
        {
            Name = "",
            City = "New York",
            Country = "USA",
            Capacity = 60000
        };

        var validationResult = new ValidationResult(new[]
        {
            new ValidationFailure("Name", "Name is required")
        });

        _createValidatorMock.Setup(v => v.ValidateAsync(createDto, It.IsAny<CancellationToken>()))
            .ReturnsAsync(validationResult);

        // Act
        var result = await _controller.CreateStadium(createDto);

        // Assert
        var badRequestResult = result as BadRequestObjectResult;
        Assert.IsNotNull(badRequestResult);
        Assert.AreEqual(400, badRequestResult.StatusCode);
    }

    [TestMethod]
    public async Task UpdateStadium_ValidData_ReturnsOkWithUpdatedStadium()
    {
        // Arrange
        var updateDto = new UpdateStadiumDto
        {
            Name = "Updated Stadium",
            City = "New York",
            Country = "USA",
            Capacity = 65000,
            Latitude = 40.7128m,
            Longitude = -74.0060m
        };

        var updatedStadium = new StadiumDto
        {
            Id = 1,
            Name = "Updated Stadium",
            City = "New York",
            Country = "USA",
            Capacity = 65000
        };

        _updateValidatorMock.Setup(v => v.ValidateAsync(updateDto, It.IsAny<CancellationToken>()))
            .ReturnsAsync(new ValidationResult());

        _stadiumServiceMock.Setup(s => s.UpdateStadiumAsync(1, It.IsAny<CreateStadiumDto>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(updatedStadium);

        // Act
        var result = await _controller.UpdateStadium(1, updateDto);

        // Assert
        var okResult = result as OkObjectResult;
        Assert.IsNotNull(okResult);
        Assert.AreEqual(200, okResult.StatusCode);
        
        var returnedStadium = okResult.Value as StadiumDto;
        Assert.IsNotNull(returnedStadium);
        Assert.AreEqual("Updated Stadium", returnedStadium.Name);
        Assert.AreEqual(65000, returnedStadium.Capacity);
    }

    [TestMethod]
    public async Task UpdateStadium_InvalidData_ReturnsBadRequest()
    {
        // Arrange
        var updateDto = new UpdateStadiumDto
        {
            Name = "",
            City = "New York",
            Country = "USA",
            Capacity = 65000
        };

        var validationResult = new ValidationResult(new[]
        {
            new ValidationFailure("Name", "Name is required")
        });

        _updateValidatorMock.Setup(v => v.ValidateAsync(updateDto, It.IsAny<CancellationToken>()))
            .ReturnsAsync(validationResult);

        // Act
        var result = await _controller.UpdateStadium(1, updateDto);

        // Assert
        var badRequestResult = result as BadRequestObjectResult;
        Assert.IsNotNull(badRequestResult);
        Assert.AreEqual(400, badRequestResult.StatusCode);
    }

    [TestMethod]
    public async Task UpdateStadium_NonExistingStadium_ReturnsNotFound()
    {
        // Arrange
        var updateDto = new UpdateStadiumDto
        {
            Name = "Updated Stadium",
            City = "New York",
            Country = "USA",
            Capacity = 65000,
            Latitude = 40.7128m,
            Longitude = -74.0060m
        };

        _updateValidatorMock.Setup(v => v.ValidateAsync(updateDto, It.IsAny<CancellationToken>()))
            .ReturnsAsync(new ValidationResult());

        _stadiumServiceMock.Setup(s => s.UpdateStadiumAsync(999, It.IsAny<CreateStadiumDto>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync((StadiumDto?)null);

        // Act
        var result = await _controller.UpdateStadium(999, updateDto);

        // Assert
        var notFoundResult = result as NotFoundObjectResult;
        Assert.IsNotNull(notFoundResult);
        Assert.AreEqual(404, notFoundResult.StatusCode);
    }

    [TestMethod]
    public async Task DeleteStadium_ExistingStadium_ReturnsNoContent()
    {
        // Arrange
        _stadiumServiceMock.Setup(s => s.DeleteStadiumAsync(1, It.IsAny<CancellationToken>()))
            .ReturnsAsync(true);

        // Act
        var result = await _controller.DeleteStadium(1);

        // Assert
        var noContentResult = result as NoContentResult;
        Assert.IsNotNull(noContentResult);
        Assert.AreEqual(204, noContentResult.StatusCode);
    }

    [TestMethod]
    public async Task DeleteStadium_NonExistingStadium_ReturnsNotFound()
    {
        // Arrange
        _stadiumServiceMock.Setup(s => s.DeleteStadiumAsync(999, It.IsAny<CancellationToken>()))
            .ReturnsAsync(false);

        // Act
        var result = await _controller.DeleteStadium(999);

        // Assert
        var notFoundResult = result as NotFoundObjectResult;
        Assert.IsNotNull(notFoundResult);
        Assert.AreEqual(404, notFoundResult.StatusCode);
    }

    [TestMethod]
    public async Task StadiumExists_ExistingStadium_ReturnsOk()
    {
        // Arrange
        _stadiumServiceMock.Setup(s => s.StadiumExistsAsync(1, It.IsAny<CancellationToken>()))
            .ReturnsAsync(true);

        // Act
        var result = await _controller.StadiumExists(1);

        // Assert
        var okResult = result as OkResult;
        Assert.IsNotNull(okResult);
        Assert.AreEqual(200, okResult.StatusCode);
    }

    [TestMethod]
    public async Task StadiumExists_NonExistingStadium_ReturnsNotFound()
    {
        // Arrange
        _stadiumServiceMock.Setup(s => s.StadiumExistsAsync(999, It.IsAny<CancellationToken>()))
            .ReturnsAsync(false);

        // Act
        var result = await _controller.StadiumExists(999);

        // Assert
        var notFoundResult = result as NotFoundResult;
        Assert.IsNotNull(notFoundResult);
        Assert.AreEqual(404, notFoundResult.StatusCode);
    }

    [TestMethod]
    public async Task HasScheduledMatches_StadiumWithMatches_ReturnsOkWithTrue()
    {
        // Arrange
        _stadiumServiceMock.Setup(s => s.HasScheduledMatchesAsync(1, It.IsAny<CancellationToken>()))
            .ReturnsAsync(true);

        // Act
        var result = await _controller.HasScheduledMatches(1);

        // Assert
        var okResult = result as OkObjectResult;
        Assert.IsNotNull(okResult);
        Assert.AreEqual(200, okResult.StatusCode);
        
        var response = okResult.Value;
        Assert.IsNotNull(response);
        
        // Check the anonymous object properties
        var stadiumId = response.GetType().GetProperty("stadiumId")?.GetValue(response);
        var hasScheduledMatches = response.GetType().GetProperty("hasScheduledMatches")?.GetValue(response);
        
        Assert.AreEqual(1, stadiumId);
        Assert.AreEqual(true, hasScheduledMatches);
    }

    [TestMethod]
    public async Task HasScheduledMatches_StadiumWithoutMatches_ReturnsOkWithFalse()
    {
        // Arrange
        _stadiumServiceMock.Setup(s => s.HasScheduledMatchesAsync(1, It.IsAny<CancellationToken>()))
            .ReturnsAsync(false);

        // Act
        var result = await _controller.HasScheduledMatches(1);

        // Assert
        var okResult = result as OkObjectResult;
        Assert.IsNotNull(okResult);
        Assert.AreEqual(200, okResult.StatusCode);
        
        var response = okResult.Value;
        Assert.IsNotNull(response);
        
        var stadiumId = response.GetType().GetProperty("stadiumId")?.GetValue(response);
        var hasScheduledMatches = response.GetType().GetProperty("hasScheduledMatches")?.GetValue(response);
        
        Assert.AreEqual(1, stadiumId);
        Assert.AreEqual(false, hasScheduledMatches);
    }
}

// Made with Bob