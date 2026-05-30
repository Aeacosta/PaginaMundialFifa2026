using AutoMapper;
using WorldCup2026.Application.DTOs.Common;
using WorldCup2026.Application.DTOs.Match;
using WorldCup2026.Application.Interfaces;
using WorldCup2026.Domain.Entities;
using WorldCup2026.Domain.Enums;
using WorldCup2026.Domain.Interfaces;

namespace WorldCup2026.Application.Services;

/// <summary>
/// Service implementation for match management operations
/// </summary>
public class MatchService : IMatchService
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;
    private readonly IStandingService _standingService;

    public MatchService(IUnitOfWork unitOfWork, IMapper mapper, IStandingService standingService)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
        _standingService = standingService;
    }

    public async Task<PagedResult<MatchDto>> GetAllMatchesAsync(
        int pageNumber = 1,
        int pageSize = 10,
        MatchPhase? phase = null,
        MatchStatus? status = null,
        int? groupId = null,
        int? stadiumId = null,
        DateTime? fromDate = null,
        DateTime? toDate = null,
        CancellationToken cancellationToken = default)
    {
        // Get all matches
        var matches = await _unitOfWork.Matches.GetAllAsync(cancellationToken);

        // Apply filters
        if (phase.HasValue)
        {
            matches = matches.Where(m => m.Phase == phase.Value);
        }

        if (status.HasValue)
        {
            matches = matches.Where(m => m.Status == status.Value);
        }

        if (groupId.HasValue)
        {
            matches = matches.Where(m => m.GroupId == groupId.Value);
        }

        if (stadiumId.HasValue)
        {
            matches = matches.Where(m => m.StadiumId == stadiumId.Value);
        }

        if (fromDate.HasValue)
        {
            matches = matches.Where(m => m.MatchDate >= fromDate.Value);
        }

        if (toDate.HasValue)
        {
            matches = matches.Where(m => m.MatchDate <= toDate.Value);
        }

        // Get total count
        var totalCount = matches.Count();

        // Apply pagination
        var pagedMatches = matches
            .OrderBy(m => m.MatchDate)
            .Skip((pageNumber - 1) * pageSize)
            .Take(pageSize)
            .ToList();

        // Map to DTOs
        var matchDtos = _mapper.Map<List<MatchDto>>(pagedMatches);

        return new PagedResult<MatchDto>
        {
            Items = matchDtos,
            TotalCount = totalCount,
            PageNumber = pageNumber,
            PageSize = pageSize
        };
    }

    public async Task<MatchDto?> GetMatchByIdAsync(int id, CancellationToken cancellationToken = default)
    {
        var match = await _unitOfWork.Matches.GetByIdAsync(id, cancellationToken);
        return match != null ? _mapper.Map<MatchDto>(match) : null;
    }

    public async Task<IEnumerable<MatchDto>> GetMatchesByTeamAsync(int teamId, CancellationToken cancellationToken = default)
    {
        var matches = await _unitOfWork.Matches.GetByTeamIdAsync(teamId, cancellationToken);
        return _mapper.Map<IEnumerable<MatchDto>>(matches);
    }

    public async Task<IEnumerable<MatchDto>> GetMatchesByGroupAsync(int groupId, CancellationToken cancellationToken = default)
    {
        var matches = await _unitOfWork.Matches.GetByGroupIdAsync(groupId, cancellationToken);
        return _mapper.Map<IEnumerable<MatchDto>>(matches);
    }

    public async Task<IEnumerable<MatchDto>> GetMatchesByStadiumAsync(int stadiumId, CancellationToken cancellationToken = default)
    {
        var matches = await _unitOfWork.Matches.GetByStadiumIdAsync(stadiumId, cancellationToken);
        return _mapper.Map<IEnumerable<MatchDto>>(matches);
    }

    public async Task<IEnumerable<MatchDto>> GetMatchesByPhaseAsync(MatchPhase phase, CancellationToken cancellationToken = default)
    {
        var matches = await _unitOfWork.Matches.GetByPhaseAsync(phase, cancellationToken);
        return _mapper.Map<IEnumerable<MatchDto>>(matches);
    }

    public async Task<IEnumerable<MatchDto>> GetMatchesByStatusAsync(MatchStatus status, CancellationToken cancellationToken = default)
    {
        var matches = await _unitOfWork.Matches.GetByStatusAsync(status, cancellationToken);
        return _mapper.Map<IEnumerable<MatchDto>>(matches);
    }

    public async Task<IEnumerable<MatchDto>> GetUpcomingMatchesAsync(int count = 10, CancellationToken cancellationToken = default)
    {
        var matches = await _unitOfWork.Matches.GetUpcomingMatchesAsync(count, cancellationToken);
        return _mapper.Map<IEnumerable<MatchDto>>(matches);
    }

    public async Task<IEnumerable<MatchDto>> GetRecentMatchesAsync(int count = 10, CancellationToken cancellationToken = default)
    {
        var matches = await _unitOfWork.Matches.GetRecentResultsAsync(count, cancellationToken);
        return _mapper.Map<IEnumerable<MatchDto>>(matches);
    }

    public async Task<IEnumerable<MatchDto>> GetLiveMatchesAsync(CancellationToken cancellationToken = default)
    {
        var matches = await _unitOfWork.Matches.GetByStatusAsync(MatchStatus.InProgress, cancellationToken);
        return _mapper.Map<IEnumerable<MatchDto>>(matches);
    }

    public async Task<MatchDto> CreateMatchAsync(CreateMatchDto createMatchDto, CancellationToken cancellationToken = default)
    {
        // Validate teams exist
        var homeTeam = await _unitOfWork.Teams.GetByIdAsync(createMatchDto.HomeTeamId, cancellationToken);
        if (homeTeam == null)
        {
            throw new InvalidOperationException($"Home team with ID {createMatchDto.HomeTeamId} not found.");
        }

        var awayTeam = await _unitOfWork.Teams.GetByIdAsync(createMatchDto.AwayTeamId, cancellationToken);
        if (awayTeam == null)
        {
            throw new InvalidOperationException($"Away team with ID {createMatchDto.AwayTeamId} not found.");
        }

        // Validate teams are different
        if (createMatchDto.HomeTeamId == createMatchDto.AwayTeamId)
        {
            throw new InvalidOperationException("Home team and away team must be different.");
        }

        // Validate stadium exists
        var stadium = await _unitOfWork.Stadiums.GetByIdAsync(createMatchDto.StadiumId, cancellationToken);
        if (stadium == null)
        {
            throw new InvalidOperationException($"Stadium with ID {createMatchDto.StadiumId} not found.");
        }

        // Validate group exists if provided
        if (createMatchDto.GroupId.HasValue)
        {
            var group = await _unitOfWork.Groups.GetByIdAsync(createMatchDto.GroupId.Value, cancellationToken);
            if (group == null)
            {
                throw new InvalidOperationException($"Group with ID {createMatchDto.GroupId.Value} not found.");
            }
        }

        // Validate team availability
        if (!await ValidateTeamAvailabilityAsync(
            createMatchDto.HomeTeamId,
            createMatchDto.AwayTeamId,
            createMatchDto.MatchDate,
            null,
            cancellationToken))
        {
            throw new InvalidOperationException("One or both teams are already scheduled to play at this time.");
        }

        // Map and create match
        var match = _mapper.Map<Match>(createMatchDto);
        await _unitOfWork.Matches.AddAsync(match, cancellationToken);
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        // Return created match
        var createdMatch = await _unitOfWork.Matches.GetByIdAsync(match.Id, cancellationToken);
        return _mapper.Map<MatchDto>(createdMatch!);
    }

    public async Task<MatchDto> UpdateMatchResultAsync(int id, UpdateMatchResultDto updateResultDto, CancellationToken cancellationToken = default)
    {
        // Get existing match
        var match = await _unitOfWork.Matches.GetByIdAsync(id, cancellationToken);
        if (match == null)
        {
            throw new InvalidOperationException($"Match with ID {id} not found.");
        }

        // Validate match is not scheduled (must be in progress or finished)
        if (match.Status == MatchStatus.Scheduled)
        {
            throw new InvalidOperationException("Cannot update result for a scheduled match. Start the match first.");
        }

        // Determine winner
        int? winnerId = null;
        if (updateResultDto.HomeTeamScore > updateResultDto.AwayTeamScore)
        {
            winnerId = match.HomeTeamId;
        }
        else if (updateResultDto.AwayTeamScore > updateResultDto.HomeTeamScore)
        {
            winnerId = match.AwayTeamId;
        }
        // If scores are equal, check penalties (for knockout matches)
        else if (updateResultDto.HomeTeamPenalties.HasValue && updateResultDto.AwayTeamPenalties.HasValue)
        {
            if (updateResultDto.HomeTeamPenalties > updateResultDto.AwayTeamPenalties)
            {
                winnerId = match.HomeTeamId;
            }
            else if (updateResultDto.AwayTeamPenalties > updateResultDto.HomeTeamPenalties)
            {
                winnerId = match.AwayTeamId;
            }
        }

        // Create or update match result
        if (match.Result == null)
        {
            var matchResult = _mapper.Map<MatchResult>(updateResultDto);
            matchResult.MatchId = id;
            matchResult.WinnerTeamId = winnerId;
            match.Result = matchResult;
        }
        else
        {
            _mapper.Map(updateResultDto, match.Result);
            match.Result.WinnerTeamId = winnerId;
        }

        // Update match status to finished if not already
        if (match.Status != MatchStatus.Finished)
        {
            match.Status = MatchStatus.Finished;
        }

        _unitOfWork.Matches.Update(match);
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        // Recalculate standings if it's a group stage match
        if (match.GroupId.HasValue)
        {
            await _standingService.RecalculateGroupStandingsAsync(match.GroupId.Value, cancellationToken);
        }

        // Return updated match
        var updatedMatch = await _unitOfWork.Matches.GetByIdAsync(id, cancellationToken);
        return _mapper.Map<MatchDto>(updatedMatch!);
    }

    public async Task<MatchDto> UpdateMatchStatusAsync(int id, MatchStatus status, CancellationToken cancellationToken = default)
    {
        var match = await _unitOfWork.Matches.GetByIdAsync(id, cancellationToken);
        if (match == null)
        {
            throw new InvalidOperationException($"Match with ID {id} not found.");
        }

        // Validate status transition
        if (match.Status == MatchStatus.Finished && status != MatchStatus.Finished)
        {
            throw new InvalidOperationException("Cannot change status of a finished match.");
        }

        match.Status = status;
        _unitOfWork.Matches.Update(match);
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        var updatedMatch = await _unitOfWork.Matches.GetByIdAsync(id, cancellationToken);
        return _mapper.Map<MatchDto>(updatedMatch!);
    }

    public async Task<bool> DeleteMatchAsync(int id, CancellationToken cancellationToken = default)
    {
        var match = await _unitOfWork.Matches.GetByIdAsync(id, cancellationToken);
        if (match == null)
        {
            return false;
        }

        // Don't allow deletion of finished matches
        if (match.Status == MatchStatus.Finished)
        {
            throw new InvalidOperationException("Cannot delete a finished match.");
        }

        _unitOfWork.Matches.Delete(match);
        await _unitOfWork.SaveChangesAsync(cancellationToken);
        return true;
    }

    public async Task<bool> MatchExistsAsync(int id, CancellationToken cancellationToken = default)
    {
        var match = await _unitOfWork.Matches.GetByIdAsync(id, cancellationToken);
        return match != null;
    }

    public async Task<bool> ValidateTeamAvailabilityAsync(
        int homeTeamId,
        int awayTeamId,
        DateTime matchDate,
        int? excludeMatchId = null,
        CancellationToken cancellationToken = default)
    {
        // Get all matches on the same date (within 4 hours window)
        var dateFrom = matchDate.AddHours(-4);
        var dateTo = matchDate.AddHours(4);

        var matches = await _unitOfWork.Matches.GetByDateRangeAsync(dateFrom, dateTo, cancellationToken);

        // Exclude current match if updating
        if (excludeMatchId.HasValue)
        {
            matches = matches.Where(m => m.Id != excludeMatchId.Value);
        }

        // Check if either team is playing in any of these matches
        var conflictingMatches = matches.Where(m =>
            m.HomeTeamId == homeTeamId ||
            m.AwayTeamId == homeTeamId ||
            m.HomeTeamId == awayTeamId ||
            m.AwayTeamId == awayTeamId);

        return !conflictingMatches.Any();
    }
}

// Made with Bob
