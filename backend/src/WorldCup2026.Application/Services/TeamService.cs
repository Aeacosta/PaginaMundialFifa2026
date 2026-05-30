using AutoMapper;
using WorldCup2026.Application.DTOs.Common;
using WorldCup2026.Application.DTOs.Team;
using WorldCup2026.Application.Interfaces;
using WorldCup2026.Domain.Entities;
using WorldCup2026.Domain.Enums;
using WorldCup2026.Domain.Interfaces;

namespace WorldCup2026.Application.Services;

/// <summary>
/// Service implementation for team management operations
/// </summary>
public class TeamService : ITeamService
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;

    public TeamService(IUnitOfWork unitOfWork, IMapper mapper)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
    }

    public async Task<PagedResult<TeamDto>> GetAllTeamsAsync(
        int pageNumber = 1,
        int pageSize = 10,
        Confederation? confederation = null,
        int? groupId = null,
        string? searchTerm = null,
        CancellationToken cancellationToken = default)
    {
        // Get all teams
        var teams = await _unitOfWork.Teams.GetAllAsync(cancellationToken);

        // Apply filters
        if (confederation.HasValue)
        {
            teams = teams.Where(t => t.Confederation == confederation.Value);
        }

        if (groupId.HasValue)
        {
            teams = teams.Where(t => t.GroupId == groupId.Value);
        }

        if (!string.IsNullOrWhiteSpace(searchTerm))
        {
            var search = searchTerm.ToLower();
            teams = teams.Where(t => 
                t.Name.ToLower().Contains(search) || 
                t.Code.ToLower().Contains(search));
        }

        // Get total count
        var totalCount = teams.Count();

        // Apply pagination
        var pagedTeams = teams
            .OrderBy(t => t.Name)
            .Skip((pageNumber - 1) * pageSize)
            .Take(pageSize)
            .ToList();

        // Map to DTOs
        var teamDtos = _mapper.Map<List<TeamDto>>(pagedTeams);

        return new PagedResult<TeamDto>
        {
            Items = teamDtos,
            TotalCount = totalCount,
            PageNumber = pageNumber,
            PageSize = pageSize
        };
    }

    public async Task<TeamDto?> GetTeamByIdAsync(int id, CancellationToken cancellationToken = default)
    {
        var team = await _unitOfWork.Teams.GetByIdAsync(id, cancellationToken);
        return team != null ? _mapper.Map<TeamDto>(team) : null;
    }

    public async Task<TeamDto?> GetTeamByCodeAsync(string code, CancellationToken cancellationToken = default)
    {
        var team = await _unitOfWork.Teams.GetByCodeAsync(code, cancellationToken);
        return team != null ? _mapper.Map<TeamDto>(team) : null;
    }

    public async Task<IEnumerable<TeamDto>> GetTeamsByGroupAsync(int groupId, CancellationToken cancellationToken = default)
    {
        var teams = await _unitOfWork.Teams.GetByGroupIdAsync(groupId, cancellationToken);
        return _mapper.Map<IEnumerable<TeamDto>>(teams);
    }

    public async Task<IEnumerable<TeamDto>> GetTeamsByConfederationAsync(Confederation confederation, CancellationToken cancellationToken = default)
    {
        var teams = await _unitOfWork.Teams.GetByConfederationAsync(confederation, cancellationToken);
        return _mapper.Map<IEnumerable<TeamDto>>(teams);
    }

    public async Task<TeamDto> CreateTeamAsync(CreateTeamDto createTeamDto, CancellationToken cancellationToken = default)
    {
        // Validate FIFA code uniqueness
        if (await CodeExistsAsync(createTeamDto.Code, null, cancellationToken))
        {
            throw new InvalidOperationException($"Team with code '{createTeamDto.Code}' already exists.");
        }

        // Validate group exists if provided
        if (createTeamDto.GroupId.HasValue)
        {
            var groupExists = await _unitOfWork.Groups.GetByIdAsync(createTeamDto.GroupId.Value, cancellationToken);
            if (groupExists == null)
            {
                throw new InvalidOperationException($"Group with ID {createTeamDto.GroupId.Value} does not exist.");
            }
        }

        // Map and create team
        var team = _mapper.Map<Team>(createTeamDto);
        await _unitOfWork.Teams.AddAsync(team, cancellationToken);
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        // Return created team
        var createdTeam = await _unitOfWork.Teams.GetByIdAsync(team.Id, cancellationToken);
        return _mapper.Map<TeamDto>(createdTeam!);
    }

    public async Task<TeamDto> UpdateTeamAsync(int id, UpdateTeamDto updateTeamDto, CancellationToken cancellationToken = default)
    {
        // Get existing team
        var team = await _unitOfWork.Teams.GetByIdAsync(id, cancellationToken);
        if (team == null)
        {
            throw new InvalidOperationException($"Team with ID {id} not found.");
        }

        // Validate FIFA code uniqueness (excluding current team)
        if (await CodeExistsAsync(updateTeamDto.Code, id, cancellationToken))
        {
            throw new InvalidOperationException($"Team with code '{updateTeamDto.Code}' already exists.");
        }

        // Validate group exists if provided
        if (updateTeamDto.GroupId.HasValue)
        {
            var groupExists = await _unitOfWork.Groups.GetByIdAsync(updateTeamDto.GroupId.Value, cancellationToken);
            if (groupExists == null)
            {
                throw new InvalidOperationException($"Group with ID {updateTeamDto.GroupId.Value} does not exist.");
            }
        }

        // Update team properties
        _mapper.Map(updateTeamDto, team);
        _unitOfWork.Teams.Update(team);
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        // Return updated team
        var updatedTeam = await _unitOfWork.Teams.GetByIdAsync(id, cancellationToken);
        return _mapper.Map<TeamDto>(updatedTeam!);
    }

    public async Task<bool> DeleteTeamAsync(int id, CancellationToken cancellationToken = default)
    {
        var team = await _unitOfWork.Teams.GetByIdAsync(id, cancellationToken);
        if (team == null)
        {
            return false;
        }

        // Check if team has matches
        var hasMatches = await _unitOfWork.Matches.FindAsync(
            m => m.HomeTeamId == id || m.AwayTeamId == id, 
            cancellationToken);
        
        if (hasMatches.Any())
        {
            throw new InvalidOperationException($"Cannot delete team '{team.Name}' because it has scheduled matches.");
        }

        _unitOfWork.Teams.Delete(team);
        await _unitOfWork.SaveChangesAsync(cancellationToken);
        return true;
    }

    public async Task<bool> TeamExistsAsync(int id, CancellationToken cancellationToken = default)
    {
        var team = await _unitOfWork.Teams.GetByIdAsync(id, cancellationToken);
        return team != null;
    }

    public async Task<bool> CodeExistsAsync(string code, int? excludeTeamId = null, CancellationToken cancellationToken = default)
    {
        var teams = await _unitOfWork.Teams.FindAsync(
            t => t.Code.ToUpper() == code.ToUpper(), 
            cancellationToken);

        if (excludeTeamId.HasValue)
        {
            teams = teams.Where(t => t.Id != excludeTeamId.Value);
        }

        return teams.Any();
    }
}

// Made with Bob
