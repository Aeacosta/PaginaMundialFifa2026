using FluentAssertions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using WorldCup2026.Domain.Entities;
using WorldCup2026.Domain.Enums;
using WorldCup2026.Infrastructure.Repositories;

namespace WorldCup2026.Tests.Infrastructure;

[TestClass]
public class UnitOfWorkTests : RepositoryTestBase
{
    private UnitOfWork _unitOfWork;

    [TestInitialize]
    public void Setup()
    {
        _unitOfWork = new UnitOfWork(_context);
    }

    [TestMethod]
    public void Teams_ShouldReturnTeamRepository()
    {
        // Act
        var repository = _unitOfWork.Teams;

        // Assert
        repository.Should().NotBeNull();
        repository.Should().BeAssignableTo<TeamRepository>();
    }

    [TestMethod]
    public void Groups_ShouldReturnGroupRepository()
    {
        // Act
        var repository = _unitOfWork.Groups;

        // Assert
        repository.Should().NotBeNull();
        repository.Should().BeAssignableTo<GroupRepository>();
    }

    [TestMethod]
    public void Matches_ShouldReturnMatchRepository()
    {
        // Act
        var repository = _unitOfWork.Matches;

        // Assert
        repository.Should().NotBeNull();
        repository.Should().BeAssignableTo<MatchRepository>();
    }

    [TestMethod]
    public void Standings_ShouldReturnStandingRepository()
    {
        // Act
        var repository = _unitOfWork.Standings;

        // Assert
        repository.Should().NotBeNull();
        repository.Should().BeAssignableTo<StandingRepository>();
    }

    [TestMethod]
    public void Stadiums_ShouldReturnStadiumRepository()
    {
        // Act
        var repository = _unitOfWork.Stadiums;

        // Assert
        repository.Should().NotBeNull();
        repository.Should().BeAssignableTo<StadiumRepository>();
    }

    [TestMethod]
    public async Task SaveChangesAsync_ShouldPersistChanges()
    {
        // Arrange
        var team = new Team
        {
            Name = "Argentina",
            Code = "ARG",
            Confederation = Confederation.CONMEBOL,
            FifaRanking = 1
        };
        await _unitOfWork.Teams.AddAsync(team);

        // Act
        var result = await _unitOfWork.SaveChangesAsync();

        // Assert
        result.Should().BeGreaterThan(0);
        var savedTeam = await _unitOfWork.Teams.GetByIdAsync(team.Id);
        savedTeam.Should().NotBeNull();
    }

    // Note: Transaction tests are skipped because in-memory database doesn't support transactions
    // These would need SQLite in-memory database for proper transaction testing

    [TestMethod]
    public async Task MultipleRepositories_ShouldWorkTogether()
    {
        // Arrange
        var group = new Group { Name = "Group A" };
        await _unitOfWork.Groups.AddAsync(group);
        await _unitOfWork.SaveChangesAsync();

        var team = new Team
        {
            Name = "Argentina",
            Code = "ARG",
            Confederation = Confederation.CONMEBOL,
            FifaRanking = 1,
            GroupId = group.Id
        };
        await _unitOfWork.Teams.AddAsync(team);
        await _unitOfWork.SaveChangesAsync();

        // Act
        var savedGroup = await _unitOfWork.Groups.GetByIdAsync(group.Id);
        var savedTeam = await _unitOfWork.Teams.GetByIdAsync(team.Id);

        // Assert
        savedGroup.Should().NotBeNull();
        savedTeam.Should().NotBeNull();
        savedTeam!.GroupId.Should().Be(group.Id);
    }

}

// Made with Bob