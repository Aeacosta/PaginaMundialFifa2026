using FluentAssertions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using WorldCup2026.Domain.Entities;
using WorldCup2026.Domain.Enums;

namespace WorldCup2026.Tests.Domain;

[TestClass]
public class TeamTests
{
    [TestMethod]
    public void Team_ShouldInitializeWithDefaultValues()
    {
        // Act
        var team = new Team();

        // Assert
        team.Id.Should().Be(0);
        team.Name.Should().Be(string.Empty);
        team.Code.Should().Be(string.Empty);
        team.FlagUrl.Should().BeNull();
        team.GroupId.Should().BeNull();
        team.HomeMatches.Should().BeEmpty();
        team.AwayMatches.Should().BeEmpty();
    }

    [TestMethod]
    public void Team_ShouldSetProperties()
    {
        // Arrange
        var team = new Team();

        // Act
        team.Name = "Argentina";
        team.Code = "ARG";
        team.Confederation = Confederation.CONMEBOL;
        team.FifaRanking = 1;
        team.FlagUrl = "https://example.com/arg.png";
        team.GroupId = 1;

        // Assert
        team.Name.Should().Be("Argentina");
        team.Code.Should().Be("ARG");
        team.Confederation.Should().Be(Confederation.CONMEBOL);
        team.FifaRanking.Should().Be(1);
        team.FlagUrl.Should().Be("https://example.com/arg.png");
        team.GroupId.Should().Be(1);
    }

    [TestMethod]
    public void Team_ShouldHaveNavigationProperties()
    {
        // Arrange
        var team = new Team { Name = "Argentina", Code = "ARG", Confederation = Confederation.CONMEBOL };
        var group = new Group { Name = "Group A" };
        var standing = new Standing { TeamId = team.Id };

        // Act
        team.Group = group;
        team.Standing = standing;

        // Assert
        team.Group.Should().NotBeNull();
        team.Group.Name.Should().Be("Group A");
        team.Standing.Should().NotBeNull();
        team.Standing.TeamId.Should().Be(team.Id);
    }

    [TestMethod]
    public void Team_ShouldHaveMatchCollections()
    {
        // Arrange
        var team = new Team { Name = "Argentina", Code = "ARG", Confederation = Confederation.CONMEBOL };
        var homeMatch = new Match { HomeTeamId = team.Id, MatchDate = DateTime.UtcNow };
        var awayMatch = new Match { AwayTeamId = team.Id, MatchDate = DateTime.UtcNow };

        // Act
        team.HomeMatches.Add(homeMatch);
        team.AwayMatches.Add(awayMatch);

        // Assert
        team.HomeMatches.Should().HaveCount(1);
        team.AwayMatches.Should().HaveCount(1);
        team.HomeMatches.First().HomeTeamId.Should().Be(team.Id);
        team.AwayMatches.First().AwayTeamId.Should().Be(team.Id);
    }

    [TestMethod]
    public void Team_WithDifferentConfederations_ShouldBeDistinct()
    {
        // Arrange
        var team1 = new Team { Name = "Argentina", Code = "ARG", Confederation = Confederation.CONMEBOL };
        var team2 = new Team { Name = "Germany", Code = "GER", Confederation = Confederation.UEFA };

        // Assert
        team1.Confederation.Should().NotBe(team2.Confederation);
    }

    [TestMethod]
    public void Team_ShouldInheritFromBaseEntity()
    {
        // Arrange
        var team = new Team();

        // Assert
        team.Should().BeAssignableTo<BaseEntity>();
    }

    [TestMethod]
    public void Team_CreatedAt_ShouldBeSet()
    {
        // Arrange
        var team = new Team
        {
            Name = "Argentina",
            Code = "ARG",
            Confederation = Confederation.CONMEBOL
        };

        // Act
        team.CreatedAt = DateTime.UtcNow;

        // Assert
        team.CreatedAt.Should().BeCloseTo(DateTime.UtcNow, TimeSpan.FromSeconds(1));
    }

    [TestMethod]
    public void Team_UpdatedAt_ShouldBeSet()
    {
        // Arrange
        var team = new Team
        {
            Name = "Argentina",
            Code = "ARG",
            Confederation = Confederation.CONMEBOL
        };

        // Act
        team.UpdatedAt = DateTime.UtcNow;

        // Assert
        team.UpdatedAt.Should().BeCloseTo(DateTime.UtcNow, TimeSpan.FromSeconds(1));
    }
}

// Made with Bob