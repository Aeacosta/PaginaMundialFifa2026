using AutoMapper;
using WorldCup2026.Application.DTOs.Standing;
using WorldCup2026.Application.Interfaces;
using WorldCup2026.Domain.Interfaces;

namespace WorldCup2026.Application.Services;

/// <summary>
/// Service implementation for standings management operations
/// </summary>
public class StandingService : IStandingService
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;

    public StandingService(IUnitOfWork unitOfWork, IMapper mapper)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
    }

    public async Task<IEnumerable<StandingDto>> GetStandingsByGroupAsync(int groupId, CancellationToken cancellationToken = default)
    {
        // Verify group exists
        var group = await _unitOfWork.Groups.GetByIdAsync(groupId, cancellationToken);
        if (group == null)
        {
            throw new InvalidOperationException($"Group with ID {groupId} not found.");
        }

        var standings = await _unitOfWork.Standings.GetByGroupIdAsync(groupId, cancellationToken);
        return _mapper.Map<IEnumerable<StandingDto>>(standings);
    }

    public async Task<StandingDto?> GetStandingByTeamAsync(int teamId, CancellationToken cancellationToken = default)
    {
        // Verify team exists
        var team = await _unitOfWork.Teams.GetByIdAsync(teamId, cancellationToken);
        if (team == null)
        {
            throw new InvalidOperationException($"Team with ID {teamId} not found.");
        }

        var standing = await _unitOfWork.Standings.GetByTeamIdAsync(teamId, cancellationToken);
        return standing != null ? _mapper.Map<StandingDto>(standing) : null;
    }

    public async Task RecalculateGroupStandingsAsync(int groupId, CancellationToken cancellationToken = default)
    {
        // Verify group exists
        var group = await _unitOfWork.Groups.GetByIdAsync(groupId, cancellationToken);
        if (group == null)
        {
            throw new InvalidOperationException($"Group with ID {groupId} not found.");
        }

        // Use repository method to recalculate standings
        await _unitOfWork.Standings.UpdateGroupStandingsAsync(groupId, cancellationToken);
        await _unitOfWork.SaveChangesAsync(cancellationToken);
    }

    public async Task RecalculateAllStandingsAsync(CancellationToken cancellationToken = default)
    {
        // Get all groups
        var groups = await _unitOfWork.Groups.GetAllAsync(cancellationToken);

        // Recalculate standings for each group
        foreach (var group in groups)
        {
            await _unitOfWork.Standings.UpdateGroupStandingsAsync(group.Id, cancellationToken);
        }

        await _unitOfWork.SaveChangesAsync(cancellationToken);
    }

    public async Task<IEnumerable<StandingDto>> GetTopTeamsAsync(int count = 16, CancellationToken cancellationToken = default)
    {
        // Get all standings
        var allStandings = await _unitOfWork.Standings.GetAllAsync(cancellationToken);

        // Order by points, goal difference, goals for
        var topStandings = allStandings
            .OrderByDescending(s => s.Points)
            .ThenByDescending(s => s.GoalDifference)
            .ThenByDescending(s => s.GoalsFor)
            .Take(count)
            .ToList();

        return _mapper.Map<IEnumerable<StandingDto>>(topStandings);
    }

    public async Task<IEnumerable<StandingDto>> GetQualifiedTeamsFromGroupAsync(int groupId, int count = 2, CancellationToken cancellationToken = default)
    {
        // Verify group exists
        var group = await _unitOfWork.Groups.GetByIdAsync(groupId, cancellationToken);
        if (group == null)
        {
            throw new InvalidOperationException($"Group with ID {groupId} not found.");
        }

        // Get standings for the group (already ordered by position)
        var standings = await _unitOfWork.Standings.GetByGroupIdAsync(groupId, cancellationToken);

        // Take top N teams
        var qualifiedStandings = standings.Take(count).ToList();

        return _mapper.Map<IEnumerable<StandingDto>>(qualifiedStandings);
    }
}

// Made with Bob
