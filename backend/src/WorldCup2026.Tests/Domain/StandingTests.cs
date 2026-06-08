using FluentAssertions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using WorldCup2026.Domain.Entities;
using WorldCup2026.Domain.Enums;

namespace WorldCup2026.Tests.Domain;

[TestClass]
public class StandingTests
{
    [TestMethod]
    public void Standing_ShouldInitializeWithDefaultValues()
    {
        // Act
        var standing = new Standing();

        // Assert
        standing.Id.Should().Be(0);
        standing.Position.Should().Be(0);
        standing.Played.Should().Be(0);
        standing.Won.Should().Be(0);
        standing.Drawn.Should().Be(0);
        standing.Lost.Should().Be(0);
        standing.GoalsFor.Should().Be(0);
        standing.GoalsAgainst.Should().Be(0);
        standing.GoalDifference.Should().Be(0);
        standing.Points.Should().Be(0);
    }

    [TestMethod]
    public void Standing_ShouldCalculateGoalDifference()
    {
        // Arrange
        var standing = new Standing
        {
            GoalsFor = 10,
            GoalsAgainst = 5
        };

        // Act
        standing.GoalDifference = standing.GoalsFor - standing.GoalsAgainst;

        // Assert
        standing.GoalDifference.Should().Be(5);
    }

    [TestMethod]
    public void Standing_ShouldCalculatePoints()
    {
        // Arrange
        var standing = new Standing
        {
            Won = 3,
            Drawn = 2,
            Lost = 1
        };

        // Act
        standing.Points = (standing.Won * 3) + standing.Drawn;

        // Assert
        standing.Points.Should().Be(11);
    }

    [TestMethod]
    public void Standing_ShouldHaveTeamNavigation()
    {
        // Arrange
        var team = new Team { Name = "Argentina", Code = "ARG", Confederation = Confederation.CONMEBOL };
        var standing = new Standing { TeamId = team.Id };

        // Act
        standing.Team = team;

        // Assert
        standing.Team.Should().NotBeNull();
        standing.Team.Name.Should().Be("Argentina");
    }

    [TestMethod]
    public void Standing_ShouldHaveGroupNavigation()
    {
        // Arrange
        var group = new Group { Name = "Group A" };
        var standing = new Standing { GroupId = group.Id };

        // Act
        standing.Group = group;

        // Assert
        standing.Group.Should().NotBeNull();
        standing.Group.Name.Should().Be("Group A");
    }

    [TestMethod]
    public void Standing_WithNoWins_ShouldHaveZeroPoints()
    {
        // Arrange
        var standing = new Standing
        {
            Won = 0,
            Drawn = 0,
            Lost = 3
        };

        // Act
        standing.Points = (standing.Won * 3) + standing.Drawn;

        // Assert
        standing.Points.Should().Be(0);
    }

    [TestMethod]
    public void Standing_WithAllWins_ShouldCalculateCorrectPoints()
    {
        // Arrange
        var standing = new Standing
        {
            Won = 3,
            Drawn = 0,
            Lost = 0,
            Played = 3
        };

        // Act
        standing.Points = (standing.Won * 3) + standing.Drawn;

        // Assert
        standing.Points.Should().Be(9);
        standing.Played.Should().Be(standing.Won + standing.Drawn + standing.Lost);
    }

    [TestMethod]
    public void Standing_ShouldInheritFromBaseEntity()
    {
        // Arrange
        var standing = new Standing();

        // Assert
        standing.Should().BeAssignableTo<BaseEntity>();
    }

    [TestMethod]
    public void Standing_WithNegativeGoalDifference_ShouldBeValid()
    {
        // Arrange
        var standing = new Standing
        {
            GoalsFor = 2,
            GoalsAgainst = 5
        };

        // Act
        standing.GoalDifference = standing.GoalsFor - standing.GoalsAgainst;

        // Assert
        standing.GoalDifference.Should().Be(-3);
    }

    [TestMethod]
    public void Standing_PlayedMatches_ShouldEqualWonDrawnLost()
    {
        // Arrange
        var standing = new Standing
        {
            Won = 2,
            Drawn = 1,
            Lost = 1
        };

        // Act
        standing.Played = standing.Won + standing.Drawn + standing.Lost;

        // Assert
        standing.Played.Should().Be(4);
    }

    [TestMethod]
    public void Standing_Position_ShouldBeSettable()
    {
        // Arrange
        var standing = new Standing();

        // Act
        standing.Position = 1;

        // Assert
        standing.Position.Should().Be(1);
    }
}

// Made with Bob