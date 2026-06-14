using AutoMapper;
using WorldCup2026.Application.DTOs.Dashboard;
using WorldCup2026.Application.DTOs.Group;
using WorldCup2026.Application.DTOs.Match;
using WorldCup2026.Application.DTOs.Stadium;
using WorldCup2026.Application.DTOs.Standing;
using WorldCup2026.Application.DTOs.Team;
using WorldCup2026.Domain.Entities;
using WorldCup2026.Domain.Enums;

namespace WorldCup2026.Application.Mappings;

/// <summary>
/// AutoMapper profile for entity to DTO mappings
/// </summary>
public class MappingProfile : Profile
{
    public MappingProfile()
    {
        ConfigureTeamMappings();
        ConfigureGroupMappings();
        ConfigureStadiumMappings();
        ConfigureMatchMappings();
        ConfigureStandingMappings();
    }

    private void ConfigureTeamMappings()
    {
        // Team -> TeamDto
        CreateMap<Team, TeamDto>()
            .ForMember(dest => dest.GroupName, opt => opt.MapFrom(src => src.Group != null ? src.Group.Name : null))
            .ForMember(dest => dest.ConfederationName, opt => opt.MapFrom(src => GetConfederationDisplayName(src.Confederation)));

        // CreateTeamDto -> Team
        CreateMap<CreateTeamDto, Team>()
            .ForMember(dest => dest.Id, opt => opt.Ignore())
            .ForMember(dest => dest.CreatedAt, opt => opt.Ignore())
            .ForMember(dest => dest.UpdatedAt, opt => opt.Ignore())
            .ForMember(dest => dest.Group, opt => opt.Ignore())
            .ForMember(dest => dest.HomeMatches, opt => opt.Ignore())
            .ForMember(dest => dest.AwayMatches, opt => opt.Ignore())
            .ForMember(dest => dest.Standing, opt => opt.Ignore())
            .ForMember(dest => dest.WonMatches, opt => opt.Ignore());

        // UpdateTeamDto -> Team
        CreateMap<UpdateTeamDto, Team>()
            .ForMember(dest => dest.Id, opt => opt.Ignore())
            .ForMember(dest => dest.CreatedAt, opt => opt.Ignore())
            .ForMember(dest => dest.UpdatedAt, opt => opt.Ignore())
            .ForMember(dest => dest.Group, opt => opt.Ignore())
            .ForMember(dest => dest.HomeMatches, opt => opt.Ignore())
            .ForMember(dest => dest.AwayMatches, opt => opt.Ignore())
            .ForMember(dest => dest.Standing, opt => opt.Ignore())
            .ForMember(dest => dest.WonMatches, opt => opt.Ignore());
    }

    private void ConfigureGroupMappings()
    {
        // Group -> GroupDto
        CreateMap<Group, GroupDto>()
            .ForMember(dest => dest.TeamCount, opt => opt.MapFrom(src => src.Teams != null ? src.Teams.Count : 0));

        // Group -> GroupWithStandingsDto
        CreateMap<Group, GroupWithStandingsDto>()
            .ForMember(dest => dest.Standings, opt => opt.MapFrom(src => src.Standings.OrderBy(s => s.Position)));

        // CreateGroupDto -> Group
        CreateMap<CreateGroupDto, Group>()
            .ForMember(dest => dest.Id, opt => opt.Ignore())
            .ForMember(dest => dest.CreatedAt, opt => opt.Ignore())
            .ForMember(dest => dest.UpdatedAt, opt => opt.Ignore())
            .ForMember(dest => dest.Teams, opt => opt.Ignore())
            .ForMember(dest => dest.Standings, opt => opt.Ignore())
            .ForMember(dest => dest.Matches, opt => opt.Ignore())
            .ForMember(dest => dest.Description, opt => opt.Ignore());

        // UpdateGroupDto -> Group
        CreateMap<UpdateGroupDto, Group>()
            .ForMember(dest => dest.Id, opt => opt.Ignore())
            .ForMember(dest => dest.CreatedAt, opt => opt.Ignore())
            .ForMember(dest => dest.UpdatedAt, opt => opt.Ignore())
            .ForMember(dest => dest.Teams, opt => opt.Ignore())
            .ForMember(dest => dest.Standings, opt => opt.Ignore())
            .ForMember(dest => dest.Matches, opt => opt.Ignore())
            .ForMember(dest => dest.Description, opt => opt.Ignore());
    }

    private void ConfigureStadiumMappings()
    {
        // Stadium -> StadiumDto
        CreateMap<Stadium, StadiumDto>()
            .ForMember(dest => dest.MatchCount, opt => opt.MapFrom(src => src.Matches != null ? src.Matches.Count : 0));

        // CreateStadiumDto -> Stadium
        CreateMap<CreateStadiumDto, Stadium>()
            .ForMember(dest => dest.Id, opt => opt.Ignore())
            .ForMember(dest => dest.CreatedAt, opt => opt.Ignore())
            .ForMember(dest => dest.UpdatedAt, opt => opt.Ignore())
            .ForMember(dest => dest.Matches, opt => opt.Ignore());

        // UpdateStadiumDto -> Stadium
        CreateMap<UpdateStadiumDto, Stadium>()
            .ForMember(dest => dest.Id, opt => opt.Ignore())
            .ForMember(dest => dest.CreatedAt, opt => opt.Ignore())
            .ForMember(dest => dest.UpdatedAt, opt => opt.Ignore())
            .ForMember(dest => dest.Matches, opt => opt.Ignore());
    }

    private void ConfigureMatchMappings()
    {
        // Match -> MatchDto
        CreateMap<Match, MatchDto>()
            .ForMember(dest => dest.HomeTeamName, opt => opt.MapFrom(src => src.HomeTeam.Name))
            .ForMember(dest => dest.HomeTeamCode, opt => opt.MapFrom(src => src.HomeTeam.Code))
            .ForMember(dest => dest.AwayTeamName, opt => opt.MapFrom(src => src.AwayTeam.Name))
            .ForMember(dest => dest.AwayTeamCode, opt => opt.MapFrom(src => src.AwayTeam.Code))
            .ForMember(dest => dest.StadiumName, opt => opt.MapFrom(src => src.Stadium.Name))
            .ForMember(dest => dest.StadiumCity, opt => opt.MapFrom(src => src.Stadium.City))
            .ForMember(dest => dest.GroupName, opt => opt.MapFrom(src => src.Group != null ? src.Group.Name : null))
            .ForMember(dest => dest.PhaseName, opt => opt.MapFrom(src => GetPhaseDisplayName(src.Phase)))
            .ForMember(dest => dest.StatusName, opt => opt.MapFrom(src => GetStatusDisplayName(src.Status)))
            .ForMember(dest => dest.Result, opt => opt.MapFrom(src => src.Result));

        // MatchResult -> MatchResultDto
        CreateMap<MatchResult, MatchResultDto>()
            .ForMember(dest => dest.WinnerTeamName, opt => opt.MapFrom(src => src.WinnerTeam != null ? src.WinnerTeam.Name : null));

        // CreateMatchDto -> Match
        CreateMap<CreateMatchDto, Match>()
            .ForMember(dest => dest.Id, opt => opt.Ignore())
            .ForMember(dest => dest.CreatedAt, opt => opt.Ignore())
            .ForMember(dest => dest.UpdatedAt, opt => opt.Ignore())
            .ForMember(dest => dest.HomeTeam, opt => opt.Ignore())
            .ForMember(dest => dest.AwayTeam, opt => opt.Ignore())
            .ForMember(dest => dest.Stadium, opt => opt.Ignore())
            .ForMember(dest => dest.Group, opt => opt.Ignore())
            .ForMember(dest => dest.Result, opt => opt.Ignore())
            .ForMember(dest => dest.Status, opt => opt.MapFrom(src => MatchStatus.Scheduled));

        // UpdateMatchResultDto -> MatchResult
        CreateMap<UpdateMatchResultDto, MatchResult>()
            .ForMember(dest => dest.Id, opt => opt.Ignore())
            .ForMember(dest => dest.MatchId, opt => opt.Ignore())
            .ForMember(dest => dest.CreatedAt, opt => opt.Ignore())
            .ForMember(dest => dest.UpdatedAt, opt => opt.Ignore())
            .ForMember(dest => dest.Match, opt => opt.Ignore())
            .ForMember(dest => dest.WinnerTeam, opt => opt.Ignore())
            .ForMember(dest => dest.WinnerTeamId, opt => opt.Ignore())
            .ForMember(dest => dest.HomeTeamPenalties, opt => opt.Ignore())
            .ForMember(dest => dest.AwayTeamPenalties, opt => opt.Ignore());
    }

    private void ConfigureStandingMappings()
    {
        // Standing -> StandingDto
        CreateMap<Standing, StandingDto>()
            .ForMember(dest => dest.TeamName, opt => opt.MapFrom(src => src.Team.Name))
            .ForMember(dest => dest.TeamCode, opt => opt.MapFrom(src => src.Team.Code))
            .ForMember(dest => dest.TeamFlagUrl, opt => opt.MapFrom(src => src.Team.FlagUrl))
            .ForMember(dest => dest.GroupName, opt => opt.MapFrom(src => src.Group.Name));
    }

    // Helper methods for display names
    private static string GetConfederationDisplayName(Confederation confederation)
    {
        return confederation switch
        {
            Confederation.AFC => "AFC",
            Confederation.CAF => "CAF",
            Confederation.CONCACAF => "CONCACAF",
            Confederation.CONMEBOL => "CONMEBOL",
            Confederation.OFC => "OFC",
            Confederation.UEFA => "UEFA",
            _ => confederation.ToString()
        };
    }

    private static string GetPhaseDisplayName(MatchPhase phase)
    {
        return phase switch
        {
            MatchPhase.GroupStage => "Group Stage",
            MatchPhase.RoundOf32 => "Round of 32",
            MatchPhase.RoundOf16 => "Round of 16",
            MatchPhase.QuarterFinals => "Quarter Finals",
            MatchPhase.SemiFinals => "Semi Finals",
            MatchPhase.ThirdPlace => "Third Place",
            MatchPhase.Final => "Final",
            _ => phase.ToString()
        };
    }

    private static string GetStatusDisplayName(MatchStatus status)
    {
        return status switch
        {
            MatchStatus.Scheduled => "Scheduled",
            MatchStatus.InProgress => "In Progress",
            MatchStatus.Finished => "Finished",
            MatchStatus.Postponed => "Postponed",
            MatchStatus.Cancelled => "Cancelled",
            _ => status.ToString()
        };
    }
}

// Made with Bob
