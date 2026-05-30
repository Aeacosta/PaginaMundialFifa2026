namespace WorldCup2026.Application.DTOs.Group;

/// <summary>
/// DTO for Group entity
/// </summary>
public class GroupDto
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string? Description { get; set; }
    public int TeamCount { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime UpdatedAt { get; set; }
}

// Made with Bob