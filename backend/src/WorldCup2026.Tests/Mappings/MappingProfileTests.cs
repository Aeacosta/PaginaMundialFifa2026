using AutoMapper;
using FluentAssertions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using WorldCup2026.Application.DTOs.Group;
using WorldCup2026.Application.DTOs.Match;
using WorldCup2026.Application.DTOs.Stadium;
using WorldCup2026.Application.DTOs.Standing;
using WorldCup2026.Application.DTOs.Team;
using WorldCup2026.Application.Mappings;
using WorldCup2026.Domain.Entities;
using WorldCup2026.Domain.Enums;

namespace WorldCup2026.Tests.Mappings;

[TestClass]
public class MappingProfileTests
{
    private IMapper _mapper;

    [TestInitialize]
    public void Setup()
    {
        var configuration = new MapperConfiguration(cfg =>
        {
            cfg.AddProfile<MappingProfile>();
        });

        _mapper = configuration.CreateMapper();
    }

    [TestMethod]
    public void MappingProfile_ShouldBeValid()
    {
        // Arrange
        var configuration = new MapperConfiguration(cfg =>
        {
            cfg.AddProfile<MappingProfile>();
        });

        // Act & Assert
        configuration.AssertConfigurationIsValid();
    }

    [TestMethod]
    public void Map_Team_To_TeamDto_ShouldMapCorrectly()
    {
        // Arrange
        var team = new Team
        {
            Id = 1,
            Name = "Argentina",
            Code = "ARG",
            Confederation = Confederation.CONMEBOL,
            FifaRanking = 1,
            FlagUrl = "https://example.com/arg.png",
            GroupId = 1
        };

        // Act
        var dto = _mapper.Map<TeamDto>(team);

        // Assert
        dto.Should().NotBeNull();
        dto.Id.Should().Be(team.Id);
        dto.Name.Should().Be(team.Name);
        dto.Code.Should().Be(team.Code);
        dto.Confederation.Should().Be(team.Confederation);
        dto.FifaRanking.Should().Be(team.FifaRanking);
        dto.FlagUrl.Should().Be(team.FlagUrl);
        dto.GroupId.Should().Be(team.GroupId);
    }

    [TestMethod]
    public void Map_CreateTeamDto_To_Team_ShouldMapCorrectly()
    {
        // Arrange
        var dto = new CreateTeamDto
        {
            Name = "Argentina",
            Code = "ARG",
            Confederation = Confederation.CONMEBOL,
            FifaRanking = 1,
            FlagUrl = "https://example.com/arg.png",
            GroupId = 1
        };

        // Act
        var team = _mapper.Map<Team>(dto);

        // Assert
        team.Should().NotBeNull();
        team.Name.Should().Be(dto.Name);
        team.Code.Should().Be(dto.Code);
        team.Confederation.Should().Be(dto.Confederation);
        team.FifaRanking.Should().Be(dto.FifaRanking);
        team.FlagUrl.Should().Be(dto.FlagUrl);
        team.GroupId.Should().Be(dto.GroupId);
    }

    [TestMethod]
    public void Map_Group_To_GroupDto_ShouldMapCorrectly()
    {
        // Arrange
        var group = new Group
        {
            Id = 1,
            Name = "Group A"
        };

        // Act
        var dto = _mapper.Map<GroupDto>(group);

        // Assert
        dto.Should().NotBeNull();
        dto.Id.Should().Be(group.Id);
        dto.Name.Should().Be(group.Name);
    }

    [TestMethod]
    public void Map_CreateGroupDto_To_Group_ShouldMapCorrectly()
    {
        // Arrange
        var dto = new CreateGroupDto
        {
            Name = "Group A"
        };

        // Act
        var group = _mapper.Map<Group>(dto);

        // Assert
        group.Should().NotBeNull();
        group.Name.Should().Be(dto.Name);
    }

    [TestMethod]
    public void Map_Match_To_MatchDto_ShouldMapCorrectly()
    {
        // Arrange
        var match = new Match
        {
            Id = 1,
            HomeTeamId = 1,
            AwayTeamId = 2,
            StadiumId = 1,
            MatchDate = DateTime.UtcNow,
            Phase = MatchPhase.GroupStage,
            Status = MatchStatus.Scheduled,
            Round = 1,
            GroupId = 1
        };

        // Act
        var dto = _mapper.Map<MatchDto>(match);

        // Assert
        dto.Should().NotBeNull();
        dto.Id.Should().Be(match.Id);
        dto.HomeTeamId.Should().Be(match.HomeTeamId);
        dto.AwayTeamId.Should().Be(match.AwayTeamId);
        dto.StadiumId.Should().Be(match.StadiumId);
        dto.Phase.Should().Be(match.Phase);
        dto.Status.Should().Be(match.Status);
        dto.Round.Should().Be(match.Round);
    }

    [TestMethod]
    public void Map_CreateMatchDto_To_Match_ShouldMapCorrectly()
    {
        // Arrange
        var dto = new CreateMatchDto
        {
            HomeTeamId = 1,
            AwayTeamId = 2,
            StadiumId = 1,
            MatchDate = DateTime.UtcNow,
            Phase = MatchPhase.GroupStage,
            Round = 1,
            GroupId = 1
        };

        // Act
        var match = _mapper.Map<Match>(dto);

        // Assert
        match.Should().NotBeNull();
        match.HomeTeamId.Should().Be(dto.HomeTeamId);
        match.AwayTeamId.Should().Be(dto.AwayTeamId);
        match.StadiumId.Should().Be(dto.StadiumId);
        match.Phase.Should().Be(dto.Phase);
        match.Round.Should().Be(dto.Round);
    }

    [TestMethod]
    public void Map_Stadium_To_StadiumDto_ShouldMapCorrectly()
    {
        // Arrange
        var stadium = new Stadium
        {
            Id = 1,
            Name = "Estadio Monumental",
            City = "Buenos Aires",
            Country = "Argentina",
            Capacity = 83000,
            ImageUrl = "https://example.com/stadium.jpg"
        };

        // Act
        var dto = _mapper.Map<StadiumDto>(stadium);

        // Assert
        dto.Should().NotBeNull();
        dto.Id.Should().Be(stadium.Id);
        dto.Name.Should().Be(stadium.Name);
        dto.City.Should().Be(stadium.City);
        dto.Country.Should().Be(stadium.Country);
        dto.Capacity.Should().Be(stadium.Capacity);
        dto.ImageUrl.Should().Be(stadium.ImageUrl);
    }

    [TestMethod]
    public void Map_CreateStadiumDto_To_Stadium_ShouldMapCorrectly()
    {
        // Arrange
        var dto = new CreateStadiumDto
        {
            Name = "Estadio Monumental",
            City = "Buenos Aires",
            Country = "Argentina",
            Capacity = 83000,
            ImageUrl = "https://example.com/stadium.jpg"
        };

        // Act
        var stadium = _mapper.Map<Stadium>(dto);

        // Assert
        stadium.Should().NotBeNull();
        stadium.Name.Should().Be(dto.Name);
        stadium.City.Should().Be(dto.City);
        stadium.Country.Should().Be(dto.Country);
        stadium.Capacity.Should().Be(dto.Capacity);
        stadium.ImageUrl.Should().Be(dto.ImageUrl);
    }

    [TestMethod]
    public void Map_Standing_To_StandingDto_ShouldMapCorrectly()
    {
        // Arrange
        var standing = new Standing
        {
            Id = 1,
            TeamId = 1,
            GroupId = 1,
            Position = 1,
            Played = 3,
            Won = 2,
            Drawn = 1,
            Lost = 0,
            GoalsFor = 5,
            GoalsAgainst = 2,
            GoalDifference = 3,
            Points = 7
        };

        // Act
        var dto = _mapper.Map<StandingDto>(standing);

        // Assert
        dto.Should().NotBeNull();
        dto.Id.Should().Be(standing.Id);
        dto.TeamId.Should().Be(standing.TeamId);
        dto.GroupId.Should().Be(standing.GroupId);
        dto.Position.Should().Be(standing.Position);
        dto.Played.Should().Be(standing.Played);
        dto.Won.Should().Be(standing.Won);
        dto.Drawn.Should().Be(standing.Drawn);
        dto.Lost.Should().Be(standing.Lost);
        dto.GoalsFor.Should().Be(standing.GoalsFor);
        dto.GoalsAgainst.Should().Be(standing.GoalsAgainst);
        dto.GoalDifference.Should().Be(standing.GoalDifference);
        dto.Points.Should().Be(standing.Points);
    }

    [TestMethod]
    public void Map_MatchResult_To_MatchResultDto_ShouldMapCorrectly()
    {
        // Arrange
        var matchResult = new MatchResult
        {
            Id = 1,
            MatchId = 1,
            HomeTeamScore = 2,
            AwayTeamScore = 1
        };

        // Act
        var dto = _mapper.Map<MatchResultDto>(matchResult);

        // Assert
        dto.Should().NotBeNull();
        dto.Id.Should().Be(matchResult.Id);
        dto.MatchId.Should().Be(matchResult.MatchId);
        dto.HomeTeamScore.Should().Be(matchResult.HomeTeamScore);
        dto.AwayTeamScore.Should().Be(matchResult.AwayTeamScore);
    }

    [TestMethod]
    public void Map_UpdateTeamDto_To_Team_ShouldMapCorrectly()
    {
        // Arrange
        var existingTeam = new Team
        {
            Id = 1,
            Name = "Argentina",
            Code = "ARG",
            Confederation = Confederation.CONMEBOL,
            FifaRanking = 1
        };

        var updateDto = new UpdateTeamDto
        {
            Name = "Argentina Updated",
            Code = "ARG",
            Confederation = Confederation.CONMEBOL,
            FifaRanking = 2,
            GroupId = 1
        };

        // Act
        _mapper.Map(updateDto, existingTeam);

        // Assert
        existingTeam.Name.Should().Be(updateDto.Name);
        existingTeam.FifaRanking.Should().Be(updateDto.FifaRanking);
        existingTeam.GroupId.Should().Be(updateDto.GroupId);
    }

    [TestMethod]
    public void Map_ListOfTeams_To_ListOfTeamDtos_ShouldMapCorrectly()
    {
        // Arrange
        var teams = new List<Team>
        {
            new Team { Id = 1, Name = "Argentina", Code = "ARG", Confederation = Confederation.CONMEBOL },
            new Team { Id = 2, Name = "Brazil", Code = "BRA", Confederation = Confederation.CONMEBOL }
        };

        // Act
        var dtos = _mapper.Map<List<TeamDto>>(teams);

        // Assert
        dtos.Should().HaveCount(2);
        dtos[0].Name.Should().Be("Argentina");
        dtos[1].Name.Should().Be("Brazil");
    }
}

// Made with Bob