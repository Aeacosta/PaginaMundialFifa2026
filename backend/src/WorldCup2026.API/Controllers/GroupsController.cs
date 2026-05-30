using FluentValidation;
using Microsoft.AspNetCore.Mvc;
using WorldCup2026.Application.DTOs.Group;
using WorldCup2026.Application.Interfaces;

namespace WorldCup2026.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class GroupsController : ControllerBase
{
    private readonly IGroupService _groupService;
    private readonly IValidator<CreateGroupDto> _createValidator;
    private readonly IValidator<UpdateGroupDto> _updateValidator;

    public GroupsController(
        IGroupService groupService,
        IValidator<CreateGroupDto> createValidator,
        IValidator<UpdateGroupDto> updateValidator)
    {
        _groupService = groupService;
        _createValidator = createValidator;
        _updateValidator = updateValidator;
    }

    /// <summary>
    /// Get all groups
    /// </summary>
    [HttpGet]
    public async Task<IActionResult> GetAllGroups(CancellationToken cancellationToken = default)
    {
        var groups = await _groupService.GetAllGroupsAsync(cancellationToken);
        return Ok(groups);
    }

    /// <summary>
    /// Get all groups with standings
    /// </summary>
    [HttpGet("with-standings")]
    public async Task<IActionResult> GetAllGroupsWithStandings(CancellationToken cancellationToken = default)
    {
        var groups = await _groupService.GetAllGroupsWithStandingsAsync(cancellationToken);
        return Ok(groups);
    }

    /// <summary>
    /// Get group by ID
    /// </summary>
    [HttpGet("{id}")]
    public async Task<IActionResult> GetGroupById(int id, CancellationToken cancellationToken = default)
    {
        var group = await _groupService.GetGroupByIdAsync(id, cancellationToken);
        if (group == null)
            return NotFound(new { message = $"Group with ID {id} not found" });

        return Ok(group);
    }

    /// <summary>
    /// Get group with standings by ID
    /// </summary>
    [HttpGet("{id}/standings")]
    public async Task<IActionResult> GetGroupWithStandings(int id, CancellationToken cancellationToken = default)
    {
        var group = await _groupService.GetGroupWithStandingsAsync(id, cancellationToken);
        if (group == null)
            return NotFound(new { message = $"Group with ID {id} not found" });

        return Ok(group);
    }

    /// <summary>
    /// Get group by name
    /// </summary>
    [HttpGet("name/{name}")]
    public async Task<IActionResult> GetGroupByName(string name, CancellationToken cancellationToken = default)
    {
        var group = await _groupService.GetGroupByNameAsync(name, cancellationToken);
        if (group == null)
            return NotFound(new { message = $"Group with name '{name}' not found" });

        return Ok(group);
    }

    /// <summary>
    /// Create a new group
    /// </summary>
    [HttpPost]
    public async Task<IActionResult> CreateGroup(
        [FromBody] CreateGroupDto dto,
        CancellationToken cancellationToken = default)
    {
        var validationResult = await _createValidator.ValidateAsync(dto, cancellationToken);
        if (!validationResult.IsValid)
            return BadRequest(validationResult.Errors);

        var group = await _groupService.CreateGroupAsync(dto.Name, cancellationToken);
        return CreatedAtAction(nameof(GetGroupById), new { id = group.Id }, group);
    }

    /// <summary>
    /// Update an existing group
    /// </summary>
    [HttpPut("{id}")]
    public async Task<IActionResult> UpdateGroup(
        int id,
        [FromBody] UpdateGroupDto dto,
        CancellationToken cancellationToken = default)
    {
        var validationResult = await _updateValidator.ValidateAsync(dto, cancellationToken);
        if (!validationResult.IsValid)
            return BadRequest(validationResult.Errors);

        var group = await _groupService.UpdateGroupAsync(id, dto.Name, cancellationToken);
        if (group == null)
            return NotFound(new { message = $"Group with ID {id} not found" });

        return Ok(group);
    }

    /// <summary>
    /// Delete a group
    /// </summary>
    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteGroup(int id, CancellationToken cancellationToken = default)
    {
        var result = await _groupService.DeleteGroupAsync(id, cancellationToken);
        if (!result)
            return NotFound(new { message = $"Group with ID {id} not found" });

        return NoContent();
    }

    /// <summary>
    /// Check if group exists
    /// </summary>
    [HttpHead("{id}")]
    public async Task<IActionResult> GroupExists(int id, CancellationToken cancellationToken = default)
    {
        var exists = await _groupService.GroupExistsAsync(id, cancellationToken);
        return exists ? Ok() : NotFound();
    }
}

// Made with Bob
