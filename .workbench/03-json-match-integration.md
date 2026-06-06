# JSON Match Data Integration Guide

## Overview

The World Cup 2026 application now supports importing match data from JSON files for integration purposes. This feature allows external systems to provide match schedules, results, and other match-related data through a standardized JSON format.

## Features

- ✅ Import matches from JSON files
- ✅ Support for all match phases (Group Stage, Knockout Stages, Final)
- ✅ Include match results with scores and penalties
- ✅ Configurable seeding method (JSON vs. Code-based)
- ✅ Automatic team, stadium, and group resolution
- ✅ Validation and error handling
- ✅ Development and production configurations

## Configuration

### Enable JSON Seeding

Edit `appsettings.json` or `appsettings.Development.json`:

```json
{
  "Seeding": {
    "UseJsonForMatches": true
  }
}
```

- **Production** (`appsettings.json`): Set to `false` (uses code-based seeding)
- **Development** (`appsettings.Development.json`): Set to `true` (uses JSON seeding)

## JSON File Format

### Location

Place your JSON file at:
```
backend/src/WorldCup2026.Infrastructure/Data/Seeding/SeedData/matches.json
```

### Schema

```json
{
  "matches": [
    {
      "homeTeamCode": "USA",
      "awayTeamCode": "MEX",
      "stadiumName": "MetLife Stadium",
      "groupName": "Group A",
      "phase": "GroupStage",
      "round": 1,
      "matchDate": "2026-06-11T18:00:00Z",
      "status": "Scheduled",
      "result": {
        "homeTeamScore": 2,
        "awayTeamScore": 1,
        "homeTeamPenalties": null,
        "awayTeamPenalties": null,
        "homeTeamExtraTimeScore": null,
        "awayTeamExtraTimeScore": null
      }
    }
  ]
}
```

### Field Descriptions

| Field | Type | Required | Description |
|-------|------|----------|-------------|
| `homeTeamCode` | string | Yes | 3-letter FIFA team code (e.g., "USA", "BRA") |
| `awayTeamCode` | string | Yes | 3-letter FIFA team code |
| `stadiumName` | string | Yes | Exact stadium name as stored in database |
| `groupName` | string | No | Group name (e.g., "Group A"). Null for knockout stages |
| `phase` | string | Yes | Match phase (see Phase Values below) |
| `round` | integer | Yes | Round number within the phase |
| `matchDate` | datetime | Yes | ISO 8601 UTC format (e.g., "2026-06-11T18:00:00Z") |
| `status` | string | Yes | Match status (see Status Values below) |
| `result` | object | No | Match result (only for completed matches) |

### Phase Values

- `GroupStage` - Group stage matches
- `RoundOf32` - Round of 32 (first knockout round)
- `RoundOf16` - Round of 16
- `QuarterFinals` - Quarter finals
- `SemiFinals` - Semi finals
- `ThirdPlace` - Third place match
- `Final` - Final match

### Status Values

- `Scheduled` - Match is scheduled but not started
- `InProgress` - Match is currently being played
- `Completed` - Match has finished
- `Postponed` - Match has been postponed
- `Cancelled` - Match has been cancelled

### Result Object (Optional)

Only include for completed matches:

```json
"result": {
  "homeTeamScore": 2,
  "awayTeamScore": 1,
  "homeTeamPenalties": 4,
  "awayTeamPenalties": 3,
  "homeTeamExtraTimeScore": 2,
  "awayTeamExtraTimeScore": 2
}
```

- `homeTeamScore` / `awayTeamScore`: Final scores (required if result is provided)
- `homeTeamPenalties` / `awayTeamPenalties`: Penalty shootout scores (optional)
- `homeTeamExtraTimeScore` / `awayTeamExtraTimeScore`: Extra time scores (optional)

## Usage Examples

### Example 1: Group Stage Match (Scheduled)

```json
{
  "homeTeamCode": "USA",
  "awayTeamCode": "MEX",
  "stadiumName": "MetLife Stadium",
  "groupName": "Group A",
  "phase": "GroupStage",
  "round": 1,
  "matchDate": "2026-06-11T18:00:00Z",
  "status": "Scheduled"
}
```

### Example 2: Group Stage Match (Completed)

```json
{
  "homeTeamCode": "BRA",
  "awayTeamCode": "ARG",
  "stadiumName": "MetLife Stadium",
  "groupName": "Group B",
  "phase": "GroupStage",
  "round": 1,
  "matchDate": "2026-06-12T18:00:00Z",
  "status": "Completed",
  "result": {
    "homeTeamScore": 2,
    "awayTeamScore": 1
  }
}
```

### Example 3: Knockout Stage Match (TBD Teams)

```json
{
  "homeTeamCode": "TBD1",
  "awayTeamCode": "TBD2",
  "stadiumName": "MetLife Stadium",
  "groupName": null,
  "phase": "RoundOf16",
  "round": 1,
  "matchDate": "2026-07-01T18:00:00Z",
  "status": "Scheduled"
}
```

### Example 4: Final with Penalties

```json
{
  "homeTeamCode": "BRA",
  "awayTeamCode": "ARG",
  "stadiumName": "MetLife Stadium",
  "groupName": null,
  "phase": "Final",
  "round": 1,
  "matchDate": "2026-07-19T18:00:00Z",
  "status": "Completed",
  "result": {
    "homeTeamScore": 1,
    "awayTeamScore": 1,
    "homeTeamExtraTimeScore": 1,
    "awayTeamExtraTimeScore": 1,
    "homeTeamPenalties": 4,
    "awayTeamPenalties": 5
  }
}
```

## Seeding Process

### Automatic Seeding

The database is automatically seeded when the application starts if no matches exist.

### Manual Seeding (Development Only)

Use the seeding endpoint:

```bash
POST http://localhost:5000/api/seed
```

Or using curl:

```bash
curl -X POST http://localhost:5000/api/seed
```

### Programmatic Seeding

```csharp
// Inject DataSeeder
public class MyService
{
    private readonly DataSeeder _seeder;
    
    public MyService(DataSeeder seeder)
    {
        _seeder = seeder;
    }
    
    public async Task SeedData()
    {
        // Use JSON seeding
        await _seeder.SeedAllAsync(useJsonForMatches: true);
        
        // Use code-based seeding
        await _seeder.SeedAllAsync(useJsonForMatches: false);
        
        // Use configuration setting
        await _seeder.SeedAllAsync();
    }
}
```

## Data Resolution

### Team Resolution

Teams are matched by their 3-letter FIFA code (case-insensitive):
- `"USA"` → United States
- `"BRA"` → Brazil
- `"TBD1"` → Placeholder (uses first available team)

### Stadium Resolution

Stadiums are matched by exact name (case-insensitive):
- `"MetLife Stadium"` → MetLife Stadium in New Jersey
- If not found, uses first available stadium as fallback

### Group Resolution

Groups are matched by name (case-insensitive):
- `"Group A"` → Group A
- `null` or empty → No group (for knockout stages)

## Error Handling

The seeder includes comprehensive error handling:

- **Missing JSON file**: Logs warning and skips JSON seeding
- **Invalid JSON format**: Throws exception with details
- **Team not found**: Logs warning and skips match
- **Stadium not found**: Uses fallback stadium
- **Invalid enum values**: Uses default values

## Logging

The seeder provides detailed logging:

```
[Information] Loading matches from JSON file: /path/to/matches.json
[Information] Successfully seeded 12 matches from JSON file.
[Warning] Could not find teams: TBD1 vs TBD2
[Error] Error processing match: USA vs MEX
```

## Best Practices

1. **Validate JSON**: Use a JSON validator before importing
2. **Use UTC timestamps**: All dates should be in UTC format
3. **Match team codes**: Ensure team codes match database entries
4. **Test in development**: Test JSON files in development environment first
5. **Backup data**: Always backup before importing large datasets
6. **Incremental imports**: Import matches in batches for large tournaments
7. **Version control**: Keep JSON files in version control

## Integration Scenarios

### Scenario 1: External Match Schedule Provider

```json
{
  "matches": [
    // Import official FIFA match schedule
  ]
}
```

### Scenario 2: Live Score Updates

Update `matches.json` with results and re-seed:

```json
{
  "homeTeamCode": "USA",
  "awayTeamCode": "MEX",
  "status": "Completed",
  "result": {
    "homeTeamScore": 2,
    "awayTeamScore": 1
  }
}
```

### Scenario 3: Tournament Simulation

Generate matches programmatically and export to JSON for testing.

## Troubleshooting

### Issue: Matches not importing

**Solution**: Check that:
- JSON file exists at correct path
- `UseJsonForMatches` is set to `true`
- JSON format is valid
- Teams and stadiums exist in database

### Issue: Teams not found

**Solution**: 
- Verify team codes match database entries
- Check case sensitivity
- Ensure teams are seeded before matches

### Issue: Duplicate matches

**Solution**:
- Clear database before re-seeding
- Check for existing matches in database

## API Reference

### JsonMatchSeeder Class

```csharp
public class JsonMatchSeeder : IDataSeeder
{
    public JsonMatchSeeder(
        WorldCupDbContext context, 
        ILogger<JsonMatchSeeder> logger,
        string? jsonFilePath = null)
    
    public Task SeedAsync(CancellationToken cancellationToken = default)
}
```

### MatchSeedData Model

```csharp
public class MatchSeedData
{
    public List<MatchData> Matches { get; set; }
}

public class MatchData
{
    public string? HomeTeamCode { get; set; }
    public string? AwayTeamCode { get; set; }
    public string? StadiumName { get; set; }
    public string? GroupName { get; set; }
    public string Phase { get; set; }
    public int Round { get; set; }
    public DateTime MatchDate { get; set; }
    public string Status { get; set; }
    public MatchResultData? Result { get; set; }
}
```

## Support

For issues or questions:
1. Check logs for detailed error messages
2. Validate JSON format
3. Review this documentation
4. Contact development team

---

**Made with Bob** 🤖