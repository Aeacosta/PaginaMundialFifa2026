using AutoMapper;
using WorldCup2026.Application.DTOs.Group;
using WorldCup2026.Application.Interfaces;
using WorldCup2026.Domain.Entities;
using WorldCup2026.Domain.Interfaces;

namespace WorldCup2026.Application.Services;

/// <summary>
/// Service implementation for group management operations
/// </summary>
public class GroupService : IGroupService
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;

    public GroupService(IUnitOfWork unitOfWork, IMapper mapper)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
    }

    public async Task<IEnumerable<GroupDto>> GetAllGroupsAsync(CancellationToken cancellationToken = default)
    {
        var groups = await _unitOfWork.Groups.GetAllAsync(cancellationToken);
        var orderedGroups = groups.OrderBy(g => g.Name);
        return _mapper.Map<IEnumerable<GroupDto>>(orderedGroups);
    }

    public async Task<GroupDto?> GetGroupByIdAsync(int id, CancellationToken cancellationToken = default)
    {
        var group = await _unitOfWork.Groups.GetByIdAsync(id, cancellationToken);
        return group != null ? _mapper.Map<GroupDto>(group) : null;
    }

    public async Task<GroupDto?> GetGroupByNameAsync(string name, CancellationToken cancellationToken = default)
    {
        var group = await _unitOfWork.Groups.GetByNameAsync(name, cancellationToken);
        return group != null ? _mapper.Map<GroupDto>(group) : null;
    }

    public async Task<GroupWithStandingsDto?> GetGroupWithStandingsAsync(int id, CancellationToken cancellationToken = default)
    {
        var group = await _unitOfWork.Groups.GetWithStandingsAsync(id, cancellationToken);
        return group != null ? _mapper.Map<GroupWithStandingsDto>(group) : null;
    }

    public async Task<IEnumerable<GroupWithStandingsDto>> GetAllGroupsWithStandingsAsync(CancellationToken cancellationToken = default)
    {
        var groups = await _unitOfWork.Groups.GetAllAsync(cancellationToken);
        var groupsWithStandings = new List<GroupWithStandingsDto>();

        foreach (var group in groups.OrderBy(g => g.Name))
        {
            var groupWithStandings = await _unitOfWork.Groups.GetWithStandingsAsync(group.Id, cancellationToken);
            if (groupWithStandings != null)
            {
                groupsWithStandings.Add(_mapper.Map<GroupWithStandingsDto>(groupWithStandings));
            }
        }

        return groupsWithStandings;
    }

    public async Task<GroupDto> CreateGroupAsync(string name, CancellationToken cancellationToken = default)
    {
        // Validate name uniqueness
        if (await GroupNameExistsAsync(name, null, cancellationToken))
        {
            throw new InvalidOperationException($"Group with name '{name}' already exists.");
        }

        // Validate name format (should be single letter A-Z)
        if (string.IsNullOrWhiteSpace(name) || name.Length > 10)
        {
            throw new ArgumentException("Group name must be between 1 and 10 characters.", nameof(name));
        }

        var group = new Group
        {
            Name = name.ToUpper().Trim()
        };

        await _unitOfWork.Groups.AddAsync(group, cancellationToken);
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        var createdGroup = await _unitOfWork.Groups.GetByIdAsync(group.Id, cancellationToken);
        return _mapper.Map<GroupDto>(createdGroup!);
    }

    public async Task<GroupDto> UpdateGroupAsync(int id, string name, CancellationToken cancellationToken = default)
    {
        var group = await _unitOfWork.Groups.GetByIdAsync(id, cancellationToken);
        if (group == null)
        {
            throw new InvalidOperationException($"Group with ID {id} not found.");
        }

        // Validate name uniqueness (excluding current group)
        if (await GroupNameExistsAsync(name, id, cancellationToken))
        {
            throw new InvalidOperationException($"Group with name '{name}' already exists.");
        }

        // Validate name format
        if (string.IsNullOrWhiteSpace(name) || name.Length > 10)
        {
            throw new ArgumentException("Group name must be between 1 and 10 characters.", nameof(name));
        }

        group.Name = name.ToUpper().Trim();
        _unitOfWork.Groups.Update(group);
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        var updatedGroup = await _unitOfWork.Groups.GetByIdAsync(id, cancellationToken);
        return _mapper.Map<GroupDto>(updatedGroup!);
    }

    public async Task<bool> DeleteGroupAsync(int id, CancellationToken cancellationToken = default)
    {
        var group = await _unitOfWork.Groups.GetByIdAsync(id, cancellationToken);
        if (group == null)
        {
            return false;
        }

        // Check if group has teams
        var teams = await _unitOfWork.Teams.GetByGroupIdAsync(id, cancellationToken);
        if (teams.Any())
        {
            throw new InvalidOperationException($"Cannot delete group '{group.Name}' because it has teams assigned.");
        }

        // Check if group has matches
        var matches = await _unitOfWork.Matches.GetByGroupIdAsync(id, cancellationToken);
        if (matches.Any())
        {
            throw new InvalidOperationException($"Cannot delete group '{group.Name}' because it has scheduled matches.");
        }

        _unitOfWork.Groups.Delete(group);
        await _unitOfWork.SaveChangesAsync(cancellationToken);
        return true;
    }

    public async Task<bool> GroupExistsAsync(int id, CancellationToken cancellationToken = default)
    {
        var group = await _unitOfWork.Groups.GetByIdAsync(id, cancellationToken);
        return group != null;
    }

    public async Task<bool> GroupNameExistsAsync(string name, int? excludeGroupId = null, CancellationToken cancellationToken = default)
    {
        var groups = await _unitOfWork.Groups.FindAsync(
            g => g.Name.ToUpper() == name.ToUpper(), 
            cancellationToken);

        if (excludeGroupId.HasValue)
        {
            groups = groups.Where(g => g.Id != excludeGroupId.Value);
        }

        return groups.Any();
    }
}

// Made with Bob
