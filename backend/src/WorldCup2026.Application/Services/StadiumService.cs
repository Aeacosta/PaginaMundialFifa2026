using AutoMapper;
using WorldCup2026.Application.DTOs.Common;
using WorldCup2026.Application.DTOs.Stadium;
using WorldCup2026.Application.Interfaces;
using WorldCup2026.Domain.Entities;
using WorldCup2026.Domain.Interfaces;

namespace WorldCup2026.Application.Services;

/// <summary>
/// Service implementation for stadium management operations
/// </summary>
public class StadiumService : IStadiumService
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;

    public StadiumService(IUnitOfWork unitOfWork, IMapper mapper)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
    }

    public async Task<PagedResult<StadiumDto>> GetAllStadiumsAsync(
        int pageNumber = 1,
        int pageSize = 10,
        string? city = null,
        string? country = null,
        string? searchTerm = null,
        CancellationToken cancellationToken = default)
    {
        // Get all stadiums
        var stadiums = await _unitOfWork.Stadiums.GetAllAsync(cancellationToken);

        // Apply filters
        if (!string.IsNullOrWhiteSpace(city))
        {
            stadiums = stadiums.Where(s => s.City.Equals(city, StringComparison.OrdinalIgnoreCase));
        }

        if (!string.IsNullOrWhiteSpace(country))
        {
            stadiums = stadiums.Where(s => s.Country.Equals(country, StringComparison.OrdinalIgnoreCase));
        }

        if (!string.IsNullOrWhiteSpace(searchTerm))
        {
            var search = searchTerm.ToLower();
            stadiums = stadiums.Where(s =>
                s.Name.ToLower().Contains(search) ||
                s.City.ToLower().Contains(search) ||
                s.Country.ToLower().Contains(search));
        }

        // Get total count
        var totalCount = stadiums.Count();

        // Apply pagination
        var pagedStadiums = stadiums
            .OrderBy(s => s.Country)
            .ThenBy(s => s.City)
            .ThenBy(s => s.Name)
            .Skip((pageNumber - 1) * pageSize)
            .Take(pageSize)
            .ToList();

        // Map to DTOs
        var stadiumDtos = _mapper.Map<List<StadiumDto>>(pagedStadiums);

        return new PagedResult<StadiumDto>
        {
            Items = stadiumDtos,
            TotalCount = totalCount,
            PageNumber = pageNumber,
            PageSize = pageSize
        };
    }

    public async Task<StadiumDto?> GetStadiumByIdAsync(int id, CancellationToken cancellationToken = default)
    {
        var stadium = await _unitOfWork.Stadiums.GetByIdAsync(id, cancellationToken);
        return stadium != null ? _mapper.Map<StadiumDto>(stadium) : null;
    }

    public async Task<IEnumerable<StadiumDto>> GetStadiumsByCityAsync(string city, CancellationToken cancellationToken = default)
    {
        var stadiums = await _unitOfWork.Stadiums.GetByCityAsync(city, cancellationToken);
        return _mapper.Map<IEnumerable<StadiumDto>>(stadiums);
    }

    public async Task<IEnumerable<StadiumDto>> GetStadiumsByCountryAsync(string country, CancellationToken cancellationToken = default)
    {
        var stadiums = await _unitOfWork.Stadiums.GetByCountryAsync(country, cancellationToken);
        return _mapper.Map<IEnumerable<StadiumDto>>(stadiums);
    }

    public async Task<StadiumDto> CreateStadiumAsync(CreateStadiumDto createStadiumDto, CancellationToken cancellationToken = default)
    {
        // Validate required fields
        if (string.IsNullOrWhiteSpace(createStadiumDto.Name))
        {
            throw new ArgumentException("Stadium name is required.", nameof(createStadiumDto.Name));
        }

        if (string.IsNullOrWhiteSpace(createStadiumDto.City))
        {
            throw new ArgumentException("City is required.", nameof(createStadiumDto.City));
        }

        if (string.IsNullOrWhiteSpace(createStadiumDto.Country))
        {
            throw new ArgumentException("Country is required.", nameof(createStadiumDto.Country));
        }

        if (createStadiumDto.Capacity <= 0)
        {
            throw new ArgumentException("Capacity must be greater than zero.", nameof(createStadiumDto.Capacity));
        }

        // Map and create stadium
        var stadium = _mapper.Map<Stadium>(createStadiumDto);
        await _unitOfWork.Stadiums.AddAsync(stadium, cancellationToken);
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        // Return created stadium
        var createdStadium = await _unitOfWork.Stadiums.GetByIdAsync(stadium.Id, cancellationToken);
        return _mapper.Map<StadiumDto>(createdStadium!);
    }

    public async Task<StadiumDto> UpdateStadiumAsync(int id, CreateStadiumDto updateStadiumDto, CancellationToken cancellationToken = default)
    {
        // Get existing stadium
        var stadium = await _unitOfWork.Stadiums.GetByIdAsync(id, cancellationToken);
        if (stadium == null)
        {
            throw new InvalidOperationException($"Stadium with ID {id} not found.");
        }

        // Validate required fields
        if (string.IsNullOrWhiteSpace(updateStadiumDto.Name))
        {
            throw new ArgumentException("Stadium name is required.", nameof(updateStadiumDto.Name));
        }

        if (string.IsNullOrWhiteSpace(updateStadiumDto.City))
        {
            throw new ArgumentException("City is required.", nameof(updateStadiumDto.City));
        }

        if (string.IsNullOrWhiteSpace(updateStadiumDto.Country))
        {
            throw new ArgumentException("Country is required.", nameof(updateStadiumDto.Country));
        }

        if (updateStadiumDto.Capacity <= 0)
        {
            throw new ArgumentException("Capacity must be greater than zero.", nameof(updateStadiumDto.Capacity));
        }

        // Update stadium properties
        _mapper.Map(updateStadiumDto, stadium);
        _unitOfWork.Stadiums.Update(stadium);
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        // Return updated stadium
        var updatedStadium = await _unitOfWork.Stadiums.GetByIdAsync(id, cancellationToken);
        return _mapper.Map<StadiumDto>(updatedStadium!);
    }

    public async Task<bool> DeleteStadiumAsync(int id, CancellationToken cancellationToken = default)
    {
        var stadium = await _unitOfWork.Stadiums.GetByIdAsync(id, cancellationToken);
        if (stadium == null)
        {
            return false;
        }

        // Check if stadium has scheduled matches
        if (await HasScheduledMatchesAsync(id, cancellationToken))
        {
            throw new InvalidOperationException($"Cannot delete stadium '{stadium.Name}' because it has scheduled matches.");
        }

        _unitOfWork.Stadiums.Delete(stadium);
        await _unitOfWork.SaveChangesAsync(cancellationToken);
        return true;
    }

    public async Task<bool> StadiumExistsAsync(int id, CancellationToken cancellationToken = default)
    {
        var stadium = await _unitOfWork.Stadiums.GetByIdAsync(id, cancellationToken);
        return stadium != null;
    }

    public async Task<bool> HasScheduledMatchesAsync(int id, CancellationToken cancellationToken = default)
    {
        var matches = await _unitOfWork.Matches.GetByStadiumIdAsync(id, cancellationToken);
        return matches.Any();
    }
}

// Made with Bob
