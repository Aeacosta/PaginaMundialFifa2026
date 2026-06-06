using AutoMapper;
using FluentAssertions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using WorldCup2026.Application.DTOs.Stadium;
using WorldCup2026.Application.Services;
using WorldCup2026.Domain.Entities;
using WorldCup2026.Domain.Interfaces;
using DomainMatch = WorldCup2026.Domain.Entities.Match;

namespace WorldCup2026.Tests.Services;

[TestClass]
public class StadiumServiceTests
{
    private Mock<IUnitOfWork> _unitOfWorkMock;
    private Mock<IMapper> _mapperMock;
    private StadiumService _stadiumService;

    [TestInitialize]
    public void Setup()
    {
        _unitOfWorkMock = new Mock<IUnitOfWork>();
        _mapperMock = new Mock<IMapper>();
        _stadiumService = new StadiumService(_unitOfWorkMock.Object, _mapperMock.Object);
    }

    [TestMethod]
    public async Task GetAllStadiumsAsync_ShouldReturnPagedStadiums()
    {
        // Arrange
        var stadiums = new List<Stadium>
        {
            new Stadium { Id = 1, Name = "Stadium A", City = "City A", Country = "USA", Capacity = 50000 },
            new Stadium { Id = 2, Name = "Stadium B", City = "City B", Country = "Mexico", Capacity = 60000 }
        };

        var stadiumDtos = new List<StadiumDto>
        {
            new StadiumDto { Id = 1, Name = "Stadium A", City = "City A", Country = "USA", Capacity = 50000 },
            new StadiumDto { Id = 2, Name = "Stadium B", City = "City B", Country = "Mexico", Capacity = 60000 }
        };

        _unitOfWorkMock.Setup(u => u.Stadiums.GetAllAsync(It.IsAny<CancellationToken>())).ReturnsAsync(stadiums);
        _mapperMock.Setup(m => m.Map<List<StadiumDto>>(It.IsAny<List<Stadium>>())).Returns(stadiumDtos);

        // Act
        var result = await _stadiumService.GetAllStadiumsAsync();

        // Assert
        result.Should().NotBeNull();
        result.Items.Should().HaveCount(2);
        result.TotalCount.Should().Be(2);
        result.PageNumber.Should().Be(1);
        result.PageSize.Should().Be(10);
        _unitOfWorkMock.Verify(u => u.Stadiums.GetAllAsync(It.IsAny<CancellationToken>()), Times.Once);
    }

    [TestMethod]
    public async Task GetAllStadiumsAsync_WithCountryFilter_ShouldReturnFilteredStadiums()
    {
        // Arrange
        var stadiums = new List<Stadium>
        {
            new Stadium { Id = 1, Name = "Stadium A", City = "City A", Country = "USA", Capacity = 50000 },
            new Stadium { Id = 2, Name = "Stadium B", City = "City B", Country = "Mexico", Capacity = 60000 },
            new Stadium { Id = 3, Name = "Stadium C", City = "City C", Country = "USA", Capacity = 55000 }
        };

        var filteredStadiumDtos = new List<StadiumDto>
        {
            new StadiumDto { Id = 1, Name = "Stadium A", City = "City A", Country = "USA", Capacity = 50000 },
            new StadiumDto { Id = 3, Name = "Stadium C", City = "City C", Country = "USA", Capacity = 55000 }
        };

        _unitOfWorkMock.Setup(u => u.Stadiums.GetAllAsync(It.IsAny<CancellationToken>())).ReturnsAsync(stadiums);
        _mapperMock.Setup(m => m.Map<List<StadiumDto>>(It.IsAny<List<Stadium>>())).Returns(filteredStadiumDtos);

        // Act
        var result = await _stadiumService.GetAllStadiumsAsync(country: "USA");

        // Assert
        result.Should().NotBeNull();
        result.TotalCount.Should().Be(2);
    }

    [TestMethod]
    public async Task GetStadiumByIdAsync_WithValidId_ShouldReturnStadium()
    {
        // Arrange
        var stadiumId = 1;
        var stadium = new Stadium { Id = stadiumId, Name = "Stadium A", City = "City A", Country = "USA", Capacity = 50000 };
        var stadiumDto = new StadiumDto { Id = stadiumId, Name = "Stadium A", City = "City A", Country = "USA", Capacity = 50000 };

        _unitOfWorkMock.Setup(u => u.Stadiums.GetByIdAsync(stadiumId, It.IsAny<CancellationToken>())).ReturnsAsync(stadium);
        _mapperMock.Setup(m => m.Map<StadiumDto>(stadium)).Returns(stadiumDto);

        // Act
        var result = await _stadiumService.GetStadiumByIdAsync(stadiumId);

        // Assert
        result.Should().NotBeNull();
        result.Should().BeEquivalentTo(stadiumDto);
        _unitOfWorkMock.Verify(u => u.Stadiums.GetByIdAsync(stadiumId, It.IsAny<CancellationToken>()), Times.Once);
    }

    [TestMethod]
    public async Task GetStadiumByIdAsync_WithInvalidId_ShouldReturnNull()
    {
        // Arrange
        var stadiumId = 999;
        _unitOfWorkMock.Setup(u => u.Stadiums.GetByIdAsync(stadiumId, It.IsAny<CancellationToken>())).ReturnsAsync((Stadium)null);

        // Act
        var result = await _stadiumService.GetStadiumByIdAsync(stadiumId);

        // Assert
        result.Should().BeNull();
        _unitOfWorkMock.Verify(u => u.Stadiums.GetByIdAsync(stadiumId, It.IsAny<CancellationToken>()), Times.Once);
    }

    [TestMethod]
    public async Task GetStadiumsByCityAsync_ShouldReturnStadiumsInCity()
    {
        // Arrange
        var city = "Los Angeles";
        var stadiums = new List<Stadium>
        {
            new Stadium { Id = 1, Name = "Stadium A", City = city, Country = "USA", Capacity = 50000 },
            new Stadium { Id = 2, Name = "Stadium B", City = city, Country = "USA", Capacity = 60000 }
        };

        var stadiumDtos = new List<StadiumDto>
        {
            new StadiumDto { Id = 1, Name = "Stadium A", City = city, Country = "USA", Capacity = 50000 },
            new StadiumDto { Id = 2, Name = "Stadium B", City = city, Country = "USA", Capacity = 60000 }
        };

        _unitOfWorkMock.Setup(u => u.Stadiums.GetByCityAsync(city, It.IsAny<CancellationToken>())).ReturnsAsync(stadiums);
        _mapperMock.Setup(m => m.Map<IEnumerable<StadiumDto>>(stadiums)).Returns(stadiumDtos);

        // Act
        var result = await _stadiumService.GetStadiumsByCityAsync(city);

        // Assert
        result.Should().NotBeNull();
        result.Should().HaveCount(2);
        result.Should().BeEquivalentTo(stadiumDtos);
    }

    [TestMethod]
    public async Task GetStadiumsByCountryAsync_ShouldReturnStadiumsInCountry()
    {
        // Arrange
        var country = "USA";
        var stadiums = new List<Stadium>
        {
            new Stadium { Id = 1, Name = "Stadium A", City = "City A", Country = country, Capacity = 50000 },
            new Stadium { Id = 2, Name = "Stadium B", City = "City B", Country = country, Capacity = 60000 }
        };

        var stadiumDtos = new List<StadiumDto>
        {
            new StadiumDto { Id = 1, Name = "Stadium A", City = "City A", Country = country, Capacity = 50000 },
            new StadiumDto { Id = 2, Name = "Stadium B", City = "City B", Country = country, Capacity = 60000 }
        };

        _unitOfWorkMock.Setup(u => u.Stadiums.GetByCountryAsync(country, It.IsAny<CancellationToken>())).ReturnsAsync(stadiums);
        _mapperMock.Setup(m => m.Map<IEnumerable<StadiumDto>>(stadiums)).Returns(stadiumDtos);

        // Act
        var result = await _stadiumService.GetStadiumsByCountryAsync(country);

        // Assert
        result.Should().NotBeNull();
        result.Should().HaveCount(2);
        result.Should().BeEquivalentTo(stadiumDtos);
    }

    [TestMethod]
    public async Task CreateStadiumAsync_WithValidData_ShouldCreateStadium()
    {
        // Arrange
        var createDto = new CreateStadiumDto
        {
            Name = "New Stadium",
            City = "New City",
            Country = "USA",
            Capacity = 70000,
            Latitude = 34.0522m,
            Longitude = -118.2437m
        };

        var stadium = new Stadium
        {
            Id = 1,
            Name = "New Stadium",
            City = "New City",
            Country = "USA",
            Capacity = 70000,
            Latitude = 34.0522m,
            Longitude = -118.2437m
        };

        var stadiumDto = new StadiumDto
        {
            Id = 1,
            Name = "New Stadium",
            City = "New City",
            Country = "USA",
            Capacity = 70000,
            Latitude = 34.0522m,
            Longitude = -118.2437m
        };

        _mapperMock.Setup(m => m.Map<Stadium>(createDto)).Returns(stadium);
        _unitOfWorkMock.Setup(u => u.Stadiums.AddAsync(It.IsAny<Stadium>(), It.IsAny<CancellationToken>())).ReturnsAsync(stadium);
        _unitOfWorkMock.Setup(u => u.SaveChangesAsync(It.IsAny<CancellationToken>())).ReturnsAsync(1);
        _unitOfWorkMock.Setup(u => u.Stadiums.GetByIdAsync(It.IsAny<int>(), It.IsAny<CancellationToken>())).ReturnsAsync(stadium);
        _mapperMock.Setup(m => m.Map<StadiumDto>(stadium)).Returns(stadiumDto);

        // Act
        var result = await _stadiumService.CreateStadiumAsync(createDto);

        // Assert
        result.Should().NotBeNull();
        result.Should().BeEquivalentTo(stadiumDto);
        _unitOfWorkMock.Verify(u => u.Stadiums.AddAsync(It.IsAny<Stadium>(), It.IsAny<CancellationToken>()), Times.Once);
        _unitOfWorkMock.Verify(u => u.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Once);
    }

    [TestMethod]
    public async Task CreateStadiumAsync_WithInvalidCapacity_ShouldThrowException()
    {
        // Arrange
        var createDto = new CreateStadiumDto
        {
            Name = "New Stadium",
            City = "New City",
            Country = "USA",
            Capacity = 0 // Invalid capacity
        };

        // Act & Assert
        var act = async () => await _stadiumService.CreateStadiumAsync(createDto);
        await act.Should().ThrowAsync<ArgumentException>()
            .WithMessage("Capacity must be greater than zero.*");

        _unitOfWorkMock.Verify(u => u.Stadiums.AddAsync(It.IsAny<Stadium>(), It.IsAny<CancellationToken>()), Times.Never);
    }

    [TestMethod]
    public async Task UpdateStadiumAsync_WithValidData_ShouldUpdateStadium()
    {
        // Arrange
        var stadiumId = 1;
        var updateDto = new CreateStadiumDto
        {
            Name = "Updated Stadium",
            City = "Updated City",
            Country = "USA",
            Capacity = 75000
        };

        var existingStadium = new Stadium
        {
            Id = stadiumId,
            Name = "Old Stadium",
            City = "Old City",
            Country = "USA",
            Capacity = 50000
        };

        var updatedStadium = new Stadium
        {
            Id = stadiumId,
            Name = "Updated Stadium",
            City = "Updated City",
            Country = "USA",
            Capacity = 75000
        };

        var stadiumDto = new StadiumDto
        {
            Id = stadiumId,
            Name = "Updated Stadium",
            City = "Updated City",
            Country = "USA",
            Capacity = 75000
        };

        _unitOfWorkMock.Setup(u => u.Stadiums.GetByIdAsync(stadiumId, It.IsAny<CancellationToken>())).ReturnsAsync(existingStadium);
        _mapperMock.Setup(m => m.Map(updateDto, existingStadium)).Returns(updatedStadium);
        _unitOfWorkMock.Setup(u => u.Stadiums.Update(It.IsAny<Stadium>()));
        _unitOfWorkMock.Setup(u => u.SaveChangesAsync(It.IsAny<CancellationToken>())).ReturnsAsync(1);
        _unitOfWorkMock.SetupSequence(u => u.Stadiums.GetByIdAsync(stadiumId, It.IsAny<CancellationToken>()))
            .ReturnsAsync(existingStadium)
            .ReturnsAsync(updatedStadium);
        _mapperMock.Setup(m => m.Map<StadiumDto>(updatedStadium)).Returns(stadiumDto);

        // Act
        var result = await _stadiumService.UpdateStadiumAsync(stadiumId, updateDto);

        // Assert
        result.Should().NotBeNull();
        result.Name.Should().Be("Updated Stadium");
        _unitOfWorkMock.Verify(u => u.Stadiums.Update(It.IsAny<Stadium>()), Times.Once);
        _unitOfWorkMock.Verify(u => u.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Once);
    }

    [TestMethod]
    public async Task UpdateStadiumAsync_WithInvalidId_ShouldThrowException()
    {
        // Arrange
        var stadiumId = 999;
        var updateDto = new CreateStadiumDto
        {
            Name = "Updated Stadium",
            City = "Updated City",
            Country = "USA",
            Capacity = 75000
        };

        _unitOfWorkMock.Setup(u => u.Stadiums.GetByIdAsync(stadiumId, It.IsAny<CancellationToken>())).ReturnsAsync((Stadium)null);

        // Act & Assert
        var act = async () => await _stadiumService.UpdateStadiumAsync(stadiumId, updateDto);
        await act.Should().ThrowAsync<InvalidOperationException>()
            .WithMessage($"Stadium with ID {stadiumId} not found.");

        _unitOfWorkMock.Verify(u => u.Stadiums.Update(It.IsAny<Stadium>()), Times.Never);
    }

    [TestMethod]
    public async Task DeleteStadiumAsync_WithValidId_ShouldReturnTrue()
    {
        // Arrange
        var stadiumId = 1;
        var stadium = new Stadium { Id = stadiumId, Name = "Stadium A", City = "City A", Country = "USA", Capacity = 50000 };

        _unitOfWorkMock.Setup(u => u.Stadiums.GetByIdAsync(stadiumId, It.IsAny<CancellationToken>())).ReturnsAsync(stadium);
        _unitOfWorkMock.Setup(u => u.Matches.GetByStadiumIdAsync(stadiumId, It.IsAny<CancellationToken>())).ReturnsAsync(new List<DomainMatch>());
        _unitOfWorkMock.Setup(u => u.Stadiums.Delete(stadium));
        _unitOfWorkMock.Setup(u => u.SaveChangesAsync(It.IsAny<CancellationToken>())).ReturnsAsync(1);

        // Act
        var result = await _stadiumService.DeleteStadiumAsync(stadiumId);

        // Assert
        result.Should().BeTrue();
        _unitOfWorkMock.Verify(u => u.Stadiums.Delete(stadium), Times.Once);
        _unitOfWorkMock.Verify(u => u.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Once);
    }

    [TestMethod]
    public async Task DeleteStadiumAsync_WithInvalidId_ShouldReturnFalse()
    {
        // Arrange
        var stadiumId = 999;
        _unitOfWorkMock.Setup(u => u.Stadiums.GetByIdAsync(stadiumId, It.IsAny<CancellationToken>())).ReturnsAsync((Stadium)null);

        // Act
        var result = await _stadiumService.DeleteStadiumAsync(stadiumId);

        // Assert
        result.Should().BeFalse();
        _unitOfWorkMock.Verify(u => u.Stadiums.Delete(It.IsAny<Stadium>()), Times.Never);
        _unitOfWorkMock.Verify(u => u.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Never);
    }

    [TestMethod]
    public async Task DeleteStadiumAsync_WithScheduledMatches_ShouldThrowException()
    {
        // Arrange
        var stadiumId = 1;
        var stadium = new Stadium { Id = stadiumId, Name = "Stadium A", City = "City A", Country = "USA", Capacity = 50000 };
        var matches = new List<DomainMatch> { new DomainMatch { Id = 1, StadiumId = stadiumId } };

        _unitOfWorkMock.Setup(u => u.Stadiums.GetByIdAsync(stadiumId, It.IsAny<CancellationToken>())).ReturnsAsync(stadium);
        _unitOfWorkMock.Setup(u => u.Matches.GetByStadiumIdAsync(stadiumId, It.IsAny<CancellationToken>())).ReturnsAsync(matches);

        // Act & Assert
        var act = async () => await _stadiumService.DeleteStadiumAsync(stadiumId);
        await act.Should().ThrowAsync<InvalidOperationException>()
            .WithMessage($"Cannot delete stadium '{stadium.Name}' because it has scheduled matches.");

        _unitOfWorkMock.Verify(u => u.Stadiums.Delete(It.IsAny<Stadium>()), Times.Never);
    }

    [TestMethod]
    public async Task StadiumExistsAsync_WithExistingId_ShouldReturnTrue()
    {
        // Arrange
        var stadiumId = 1;
        var stadium = new Stadium { Id = stadiumId, Name = "Stadium A", City = "City A", Country = "USA", Capacity = 50000 };
        _unitOfWorkMock.Setup(u => u.Stadiums.GetByIdAsync(stadiumId, It.IsAny<CancellationToken>())).ReturnsAsync(stadium);

        // Act
        var result = await _stadiumService.StadiumExistsAsync(stadiumId);

        // Assert
        result.Should().BeTrue();
    }

    [TestMethod]
    public async Task StadiumExistsAsync_WithNonExistingId_ShouldReturnFalse()
    {
        // Arrange
        var stadiumId = 999;
        _unitOfWorkMock.Setup(u => u.Stadiums.GetByIdAsync(stadiumId, It.IsAny<CancellationToken>())).ReturnsAsync((Stadium)null);

        // Act
        var result = await _stadiumService.StadiumExistsAsync(stadiumId);

        // Assert
        result.Should().BeFalse();
    }

    [TestMethod]
    public async Task HasScheduledMatchesAsync_WithMatches_ShouldReturnTrue()
    {
        // Arrange
        var stadiumId = 1;
        var matches = new List<DomainMatch> { new DomainMatch { Id = 1, StadiumId = stadiumId } };
        _unitOfWorkMock.Setup(u => u.Matches.GetByStadiumIdAsync(stadiumId, It.IsAny<CancellationToken>())).ReturnsAsync(matches);

        // Act
        var result = await _stadiumService.HasScheduledMatchesAsync(stadiumId);

        // Assert
        result.Should().BeTrue();
    }

    [TestMethod]
    public async Task HasScheduledMatchesAsync_WithoutMatches_ShouldReturnFalse()
    {
        // Arrange
        var stadiumId = 1;
        _unitOfWorkMock.Setup(u => u.Matches.GetByStadiumIdAsync(stadiumId, It.IsAny<CancellationToken>())).ReturnsAsync(new List<DomainMatch>());

        // Act
        var result = await _stadiumService.HasScheduledMatchesAsync(stadiumId);

        // Assert
        result.Should().BeFalse();
    }
}

// Made with Bob