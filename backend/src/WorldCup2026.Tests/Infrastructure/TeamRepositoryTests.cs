using FluentAssertions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using WorldCup2026.Domain.Entities;
using WorldCup2026.Domain.Enums;
using WorldCup2026.Infrastructure.Repositories;

namespace WorldCup2026.Tests.Infrastructure;

[TestClass]
public class TeamRepositoryTests : RepositoryTestBase
{
    private TeamRepository _repository;

    [TestInitialize]
    public void Setup()
    {
        _repository = new TeamRepository(_context);
    }

    [TestMethod]
    public async Task GetByIdAsync_WithValidId_ShouldReturnTeam()
    {
        // Arrange
        var team = new Team
        {
            Name = "Argentina",
            Code = "ARG",
            Confederation = Confederation.CONMEBOL,
            FifaRanking = 1,
            FlagUrl = "https://example.com/arg.png"
        };
        await _context.Teams.AddAsync(team);
        await _context.SaveChangesAsync();

        // Act
        var result = await _repository.GetByIdAsync(team.Id);

        // Assert
        result.Should().NotBeNull();
        result!.Name.Should().Be("Argentina");
        result.Code.Should().Be("ARG");
    }

    [TestMethod]
    public async Task GetByIdAsync_WithInvalidId_ShouldReturnNull()
    {
        // Act
        var result = await _repository.GetByIdAsync(999);

        // Assert
        result.Should().BeNull();
    }

    [TestMethod]
    public async Task GetAllAsync_ShouldReturnAllTeams()
    {
        // Arrange
        var teams = new List<Team>
        {
            new Team { Name = "Argentina", Code = "ARG", Confederation = Confederation.CONMEBOL, FifaRanking = 1 },
            new Team { Name = "Brazil", Code = "BRA", Confederation = Confederation.CONMEBOL, FifaRanking = 2 },
            new Team { Name = "Germany", Code = "GER", Confederation = Confederation.UEFA, FifaRanking = 3 }
        };
        await _context.Teams.AddRangeAsync(teams);
        await _context.SaveChangesAsync();

        // Act
        var result = await _repository.GetAllAsync();

        // Assert
        result.Should().HaveCount(3);
    }

    [TestMethod]
    public async Task GetByCodeAsync_WithValidCode_ShouldReturnTeam()
    {
        // Arrange
        var team = new Team
        {
            Name = "Argentina",
            Code = "ARG",
            Confederation = Confederation.CONMEBOL,
            FifaRanking = 1
        };
        await _context.Teams.AddAsync(team);
        await _context.SaveChangesAsync();

        // Act
        var result = await _repository.GetByCodeAsync("ARG");

        // Assert
        result.Should().NotBeNull();
        result!.Name.Should().Be("Argentina");
    }

    [TestMethod]
    public async Task GetByCodeAsync_WithInvalidCode_ShouldReturnNull()
    {
        // Act
        var result = await _repository.GetByCodeAsync("XXX");

        // Assert
        result.Should().BeNull();
    }

    [TestMethod]
    public async Task GetByGroupIdAsync_ShouldReturnTeamsInGroup()
    {
        // Arrange
        var group = new Group { Name = "Group A" };
        await _context.Groups.AddAsync(group);
        await _context.SaveChangesAsync();

        var teams = new List<Team>
        {
            new Team { Name = "Argentina", Code = "ARG", Confederation = Confederation.CONMEBOL, FifaRanking = 1, GroupId = group.Id },
            new Team { Name = "Brazil", Code = "BRA", Confederation = Confederation.CONMEBOL, FifaRanking = 2, GroupId = group.Id },
            new Team { Name = "Germany", Code = "GER", Confederation = Confederation.UEFA, FifaRanking = 3 }
        };
        await _context.Teams.AddRangeAsync(teams);
        await _context.SaveChangesAsync();

        // Act
        var result = await _repository.GetByGroupIdAsync(group.Id);

        // Assert
        result.Should().HaveCount(2);
        result.Should().OnlyContain(t => t.GroupId == group.Id);
    }

    [TestMethod]
    public async Task GetByConfederationAsync_ShouldReturnTeamsFromConfederation()
    {
        // Arrange
        var teams = new List<Team>
        {
            new Team { Name = "Argentina", Code = "ARG", Confederation = Confederation.CONMEBOL, FifaRanking = 1 },
            new Team { Name = "Brazil", Code = "BRA", Confederation = Confederation.CONMEBOL, FifaRanking = 2 },
            new Team { Name = "Germany", Code = "GER", Confederation = Confederation.UEFA, FifaRanking = 3 }
        };
        await _context.Teams.AddRangeAsync(teams);
        await _context.SaveChangesAsync();

        // Act
        var result = await _repository.GetByConfederationAsync(Confederation.CONMEBOL);

        // Assert
        result.Should().HaveCount(2);
        result.Should().OnlyContain(t => t.Confederation == Confederation.CONMEBOL);
    }

    [TestMethod]
    public async Task GetUnassignedTeamsAsync_ShouldReturnTeamsWithoutGroup()
    {
        // Arrange
        var group = new Group { Name = "Group A" };
        await _context.Groups.AddAsync(group);
        await _context.SaveChangesAsync();

        var teams = new List<Team>
        {
            new Team { Name = "Argentina", Code = "ARG", Confederation = Confederation.CONMEBOL, FifaRanking = 1, GroupId = group.Id },
            new Team { Name = "Brazil", Code = "BRA", Confederation = Confederation.CONMEBOL, FifaRanking = 2 },
            new Team { Name = "Germany", Code = "GER", Confederation = Confederation.UEFA, FifaRanking = 3 }
        };
        await _context.Teams.AddRangeAsync(teams);
        await _context.SaveChangesAsync();

        // Act
        var result = await _repository.GetUnassignedTeamsAsync();

        // Assert
        result.Should().HaveCount(2);
        result.Should().OnlyContain(t => t.GroupId == null);
    }

    [TestMethod]
    public async Task SearchByNameAsync_ShouldReturnMatchingTeams()
    {
        // Arrange
        var teams = new List<Team>
        {
            new Team { Name = "Argentina", Code = "ARG", Confederation = Confederation.CONMEBOL, FifaRanking = 1 },
            new Team { Name = "Brazil", Code = "BRA", Confederation = Confederation.CONMEBOL, FifaRanking = 2 },
            new Team { Name = "Germany", Code = "GER", Confederation = Confederation.UEFA, FifaRanking = 3 }
        };
        await _context.Teams.AddRangeAsync(teams);
        await _context.SaveChangesAsync();

        // Act
        var result = await _repository.SearchByNameAsync("arg");

        // Assert
        result.Should().HaveCount(1);
        result.First().Name.Should().Be("Argentina");
    }

    [TestMethod]
    public async Task AddAsync_ShouldAddTeam()
    {
        // Arrange
        var team = new Team
        {
            Name = "Argentina",
            Code = "ARG",
            Confederation = Confederation.CONMEBOL,
            FifaRanking = 1
        };

        // Act
        await _repository.AddAsync(team);
        await _context.SaveChangesAsync();

        // Assert
        var result = await _repository.GetByIdAsync(team.Id);
        result.Should().NotBeNull();
        result!.Name.Should().Be("Argentina");
    }

    [TestMethod]
    public async Task Update_ShouldUpdateTeam()
    {
        // Arrange
        var team = new Team
        {
            Name = "Argentina",
            Code = "ARG",
            Confederation = Confederation.CONMEBOL,
            FifaRanking = 1
        };
        await _context.Teams.AddAsync(team);
        await _context.SaveChangesAsync();

        // Act
        team.FifaRanking = 2;
        _repository.Update(team);
        await _context.SaveChangesAsync();

        // Assert
        var result = await _repository.GetByIdAsync(team.Id);
        result!.FifaRanking.Should().Be(2);
    }

    [TestMethod]
    public async Task Delete_ShouldRemoveTeam()
    {
        // Arrange
        var team = new Team
        {
            Name = "Argentina",
            Code = "ARG",
            Confederation = Confederation.CONMEBOL,
            FifaRanking = 1
        };
        await _context.Teams.AddAsync(team);
        await _context.SaveChangesAsync();
        var teamId = team.Id;

        // Act
        _repository.Delete(team);
        await _context.SaveChangesAsync();

        // Assert
        var result = await _repository.GetByIdAsync(teamId);
        result.Should().BeNull();
    }

    [TestMethod]
    public async Task FindAsync_WithPredicate_ShouldReturnMatchingTeams()
    {
        // Arrange
        var teams = new List<Team>
        {
            new Team { Name = "Argentina", Code = "ARG", Confederation = Confederation.CONMEBOL, FifaRanking = 1 },
            new Team { Name = "Brazil", Code = "BRA", Confederation = Confederation.CONMEBOL, FifaRanking = 2 },
            new Team { Name = "Germany", Code = "GER", Confederation = Confederation.UEFA, FifaRanking = 3 }
        };
        await _context.Teams.AddRangeAsync(teams);
        await _context.SaveChangesAsync();

        // Act
        var result = await _repository.FindAsync(t => t.FifaRanking <= 2);

        // Assert
        result.Should().HaveCount(2);
    }

    [TestMethod]
    public async Task AnyAsync_WithMatchingPredicate_ShouldReturnTrue()
    {
        // Arrange
        var team = new Team
        {
            Name = "Argentina",
            Code = "ARG",
            Confederation = Confederation.CONMEBOL,
            FifaRanking = 1
        };
        await _context.Teams.AddAsync(team);
        await _context.SaveChangesAsync();

        // Act
        var result = await _repository.AnyAsync(t => t.Code == "ARG");

        // Assert
        result.Should().BeTrue();
    }

    [TestMethod]
    public async Task AnyAsync_WithNonMatchingPredicate_ShouldReturnFalse()
    {
        // Act
        var result = await _repository.AnyAsync(t => t.Code == "XXX");

        // Assert
        result.Should().BeFalse();
    }

    [TestMethod]
    public async Task CountAsync_WithoutPredicate_ShouldReturnTotalCount()
    {
        // Arrange
        var teams = new List<Team>
        {
            new Team { Name = "Argentina", Code = "ARG", Confederation = Confederation.CONMEBOL, FifaRanking = 1 },
            new Team { Name = "Brazil", Code = "BRA", Confederation = Confederation.CONMEBOL, FifaRanking = 2 }
        };
        await _context.Teams.AddRangeAsync(teams);
        await _context.SaveChangesAsync();

        // Act
        var result = await _repository.CountAsync();

        // Assert
        result.Should().Be(2);
    }

    [TestMethod]
    public async Task CountAsync_WithPredicate_ShouldReturnMatchingCount()
    {
        // Arrange
        var teams = new List<Team>
        {
            new Team { Name = "Argentina", Code = "ARG", Confederation = Confederation.CONMEBOL, FifaRanking = 1 },
            new Team { Name = "Brazil", Code = "BRA", Confederation = Confederation.CONMEBOL, FifaRanking = 2 },
            new Team { Name = "Germany", Code = "GER", Confederation = Confederation.UEFA, FifaRanking = 3 }
        };
        await _context.Teams.AddRangeAsync(teams);
        await _context.SaveChangesAsync();

        // Act
        var result = await _repository.CountAsync(t => t.Confederation == Confederation.CONMEBOL);

        // Assert
        result.Should().Be(2);
    }
}

// Made with Bob