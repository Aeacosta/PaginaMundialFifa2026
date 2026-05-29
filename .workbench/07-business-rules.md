# Business Rules - FIFA World Cup 2026 Application

## Overview

This document defines the business rules and validation logic that must be enforced throughout the application. These rules ensure data integrity, tournament validity, and realistic simulation of World Cup operations.

---

## 1. Team Rules

### TR-001: Unique Team Code
**Rule:** Each team must have a unique 3-letter ISO code.

**Validation:**
- Code must be exactly 3 characters
- Code must be uppercase
- Code must be unique across all teams

**Error Message:** "Team code '{code}' already exists."

---

### TR-002: Unique Team Name
**Rule:** Each team must have a unique name.

**Validation:**
- Name must be unique (case-insensitive)
- Name must be between 2 and 100 characters

**Error Message:** "Team name '{name}' already exists."

---

### TR-003: Valid Confederation
**Rule:** Team must belong to a valid confederation.

**Valid Values:**
- UEFA (Europe)
- CONMEBOL (South America)
- CONCACAF (North/Central America & Caribbean)
- CAF (Africa)
- AFC (Asia)
- OFC (Oceania)

**Error Message:** "Invalid confederation. Must be one of: UEFA, CONMEBOL, CONCACAF, CAF, AFC, OFC."

---

### TR-004: FIFA Ranking Validation
**Rule:** If provided, FIFA ranking must be a positive integer.

**Validation:**
- Ranking ≥ 1
- Ranking is optional (can be null)

**Error Message:** "FIFA ranking must be a positive number."

---

### TR-005: Group Assignment
**Rule:** A team can only be assigned to one group at a time.

**Validation:**
- GroupId must reference an existing group
- GroupId can be null (for teams not yet assigned)

**Error Message:** "Invalid group assignment."

---

### TR-006: Team Deletion Restriction
**Rule:** Cannot delete a team that has scheduled or completed matches.

**Validation:**
- Check if team has any matches (as home or away team)
- Only allow deletion if no matches exist

**Error Message:** "Cannot delete team with existing matches. Delete matches first."

---

## 2. Group Rules

### GR-001: Unique Group Name
**Rule:** Each group must have a unique name.

**Validation:**
- Name must be unique
- Name typically follows pattern "Group A", "Group B", etc.

**Error Message:** "Group name '{name}' already exists."

---

### GR-002: Group Size Limit
**Rule:** Each group should have exactly 4 teams.

**Validation:**
- Warn if group has fewer than 4 teams
- Prevent adding more than 4 teams to a group

**Error Message:** "Group already has 4 teams. Cannot add more."

---

### GR-003: Total Groups
**Rule:** Tournament should have exactly 12 groups (for 48 teams).

**Validation:**
- This is a soft rule for data seeding
- Can be flexible for testing

**Note:** 48 teams ÷ 4 teams per group = 12 groups

---

## 3. Match Rules

### MR-001: Team Self-Match Prevention
**Rule:** A team cannot play against itself.

**Validation:**
- HomeTeamId ≠ AwayTeamId

**Error Message:** "A team cannot play against itself."

---

### MR-002: Match Date Validation
**Rule:** Match date must be within tournament period.

**Validation:**
- Match date ≥ Tournament start date (June 11, 2026)
- Match date ≤ Tournament end date (July 19, 2026)
- For creation: Match date should be in the future

**Error Message:** "Match date must be between June 11, 2026 and July 19, 2026."

---

### MR-003: No Overlapping Matches
**Rule:** A team cannot have two matches at the same time.

**Validation:**
- Check if team (home or away) has another match within ±3 hours
- Allow some buffer time between matches

**Error Message:** "Team '{team}' already has a match scheduled at this time."

---

### MR-004: Group Stage Match Requirements
**Rule:** Group stage matches must have a group assignment.

**Validation:**
- If Phase = GroupStage, then GroupId must not be null
- GroupId must reference an existing group
- Both teams must belong to the same group

**Error Message:** "Group stage matches must have a valid group assignment."

---

### MR-005: Knockout Match Requirements
**Rule:** Knockout matches must not have a group assignment.

**Validation:**
- If Phase ≠ GroupStage, then GroupId must be null

**Error Message:** "Knockout matches cannot have a group assignment."

---

### MR-006: Valid Match Phase
**Rule:** Match phase must be valid.

**Valid Values:**
- GroupStage
- RoundOf32
- RoundOf16
- QuarterFinals
- SemiFinals
- ThirdPlace
- Final

**Error Message:** "Invalid match phase."

---

### MR-007: Valid Match Status
**Rule:** Match status must be valid.

**Valid Values:**
- Scheduled
- Live
- Finished
- Postponed
- Cancelled

**Error Message:** "Invalid match status."

---

### MR-008: Stadium Assignment
**Rule:** Every match must be assigned to a valid stadium.

**Validation:**
- StadiumId must reference an existing stadium
- Stadium must be available at the match time

**Error Message:** "Invalid stadium assignment."

---

### MR-009: Round Number for Group Stage
**Rule:** Group stage matches must have a round number (1, 2, or 3).

**Validation:**
- If Phase = GroupStage, Round must be 1, 2, or 3
- Each team plays 3 matches in group stage

**Error Message:** "Group stage round must be 1, 2, or 3."

---

### MR-010: Match Deletion Restriction
**Rule:** Cannot delete a match that has a result.

**Validation:**
- Check if match has an associated result
- Only allow deletion if no result exists

**Error Message:** "Cannot delete match with existing result. Delete result first."

---

## 4. Match Result Rules

### RR-001: Result for Finished Matches Only
**Rule:** Results can only be added to finished matches.

**Validation:**
- Match status must be "Finished" or will be set to "Finished" when result is added

**Error Message:** "Can only add results to finished matches."

---

### RR-002: Non-Negative Scores
**Rule:** Scores must be non-negative integers.

**Validation:**
- HomeTeamScore ≥ 0
- AwayTeamScore ≥ 0

**Error Message:** "Scores must be non-negative numbers."

---

### RR-003: Winner Determination
**Rule:** Winner must be correctly determined based on scores.

**Validation:**
- If HomeTeamScore > AwayTeamScore, WinnerTeamId = HomeTeamId
- If AwayTeamScore > HomeTeamScore, WinnerTeamId = AwayTeamId
- If scores are equal (group stage), WinnerTeamId = null

**Error Message:** "Winner must match the team with higher score."

---

### RR-004: Knockout Must Have Winner
**Rule:** Knockout matches cannot end in a draw.

**Validation:**
- If Phase ≠ GroupStage, there must be a winner
- If regular time is a draw, penalties must be provided

**Error Message:** "Knockout matches must have a winner. Provide penalty scores if needed."

---

### RR-005: Penalty Validation
**Rule:** Penalties are only valid for knockout matches.

**Validation:**
- If Phase = GroupStage, penalties must be null
- If Phase ≠ GroupStage and regular time is a draw, penalties required
- Penalty scores must be non-negative

**Error Message:** "Penalties are only allowed in knockout matches."

---

### RR-006: Penalty Winner Determination
**Rule:** If match goes to penalties, winner is determined by penalty score.

**Validation:**
- If penalties are used, one team must have more penalty goals
- Winner is team with higher penalty score

**Error Message:** "Penalty shootout must have a winner."

---

### RR-007: One Result Per Match
**Rule:** Each match can have only one result.

**Validation:**
- MatchId must be unique in MatchResult table
- Cannot add multiple results to same match

**Error Message:** "Match already has a result. Update existing result instead."

---

### RR-008: Result Update Triggers Standing Update
**Rule:** When a result is added or updated, standings must be recalculated.

**Business Logic:**
- Automatically update standings for both teams
- Recalculate points, goal difference, etc.
- Update group standings table

**Implementation:** Service layer handles this automatically.

---

## 5. Standing Rules

### SR-001: One Standing Per Team Per Group
**Rule:** Each team can have only one standing record per group.

**Validation:**
- (TeamId, GroupId) must be unique

**Error Message:** "Standing already exists for this team in this group."

---

### SR-002: Matches Played Calculation
**Rule:** Matches played must equal wins + draws + losses.

**Validation:**
- MatchesPlayed = Wins + Draws + Losses

**Error Message:** "Invalid standing data: matches played doesn't match sum of results."

---

### SR-003: Points Calculation
**Rule:** Points are calculated as (Wins × 3) + Draws.

**Validation:**
- Points = (Wins × 3) + Draws
- Automatically calculated, not user input

**Implementation:** Calculated field or computed in service layer.

---

### SR-004: Goal Difference Calculation
**Rule:** Goal difference is GoalsFor - GoalsAgainst.

**Validation:**
- GoalDifference = GoalsFor - GoalsAgainst
- Automatically calculated

**Implementation:** Calculated field or computed in service layer.

---

### SR-005: Non-Negative Counts
**Rule:** All count fields must be non-negative.

**Validation:**
- MatchesPlayed ≥ 0
- Wins ≥ 0
- Draws ≥ 0
- Losses ≥ 0
- GoalsFor ≥ 0
- GoalsAgainst ≥ 0

**Error Message:** "Standing counts must be non-negative."

---

### SR-006: Standing Sort Order
**Rule:** Standings are sorted by specific criteria.

**Sort Order:**
1. Points (descending)
2. Goal Difference (descending)
3. Goals For (descending)
4. Head-to-head record (if applicable)
5. Fair play points (future)
6. Drawing of lots (manual)

**Implementation:** Handled in query/service layer.

---

### SR-007: Automatic Standing Updates
**Rule:** Standings are automatically updated when match results are recorded.

**Business Logic:**
- When result is added: Update both teams' standings
- When result is updated: Recalculate both teams' standings
- When result is deleted: Recalculate both teams' standings

**Implementation:** Service layer handles this automatically.

---

## 6. Stadium Rules

### STR-001: Positive Capacity
**Rule:** Stadium capacity must be a positive integer.

**Validation:**
- Capacity > 0
- Typically between 40,000 and 100,000 for World Cup

**Error Message:** "Stadium capacity must be a positive number."

---

### STR-002: Valid Country
**Rule:** Stadium must be in one of the host countries.

**Valid Values:**
- United States
- Mexico
- Canada

**Error Message:** "Stadium must be in United States, Mexico, or Canada."

---

### STR-003: Valid Coordinates
**Rule:** If provided, coordinates must be valid.

**Validation:**
- Latitude: -90 to 90
- Longitude: -180 to 180
- Both or neither must be provided

**Error Message:** "Invalid geographic coordinates."

---

## 7. Tournament Structure Rules

### TSR-001: Group Stage Structure
**Rule:** Group stage consists of 48 matches.

**Structure:**
- 12 groups × 4 teams = 48 teams
- Each team plays 3 matches (round-robin)
- 6 matches per group
- Total: 12 groups × 6 matches = 72 matches

---

### TSR-002: Qualification Rules
**Rule:** 32 teams advance to knockout stage.

**Qualification:**
- Top 2 teams from each group (24 teams)
- 8 best third-place teams (8 teams)
- Total: 32 teams

**Ranking Third-Place Teams:**
1. Points
2. Goal difference
3. Goals scored
4. Fair play points
5. Drawing of lots

---

### TSR-003: Knockout Stage Structure
**Rule:** Knockout stage consists of 32 matches.

**Structure:**
- Round of 32: 16 matches
- Round of 16: 8 matches
- Quarter-finals: 4 matches
- Semi-finals: 2 matches
- Third place: 1 match
- Final: 1 match
- Total: 32 matches

**Total Tournament Matches:** 72 (group) + 32 (knockout) = 104 matches

---

### TSR-004: Knockout Bracket Progression
**Rule:** Winners advance to next round.

**Business Logic:**
- Winner of Round of 32 Match 1 vs Winner of Match 2 → Round of 16 Match 1
- Continue pattern through to Final
- Losers of Semi-finals play Third Place match

**Implementation:** Service layer handles bracket progression.

---

## 8. Data Integrity Rules

### DIR-001: Referential Integrity
**Rule:** All foreign keys must reference existing records.

**Validation:**
- Enforced by database constraints
- Validated in application layer

**Error Message:** "Referenced {entity} does not exist."

---

### DIR-002: Cascade Deletes
**Rule:** Deleting parent records affects child records.

**Cascade Rules:**
- Delete Group → Cascade delete Standings
- Delete Team → Cascade delete Standings
- Delete Match → Cascade delete MatchResult
- Delete Team → Restrict if has Matches
- Delete Stadium → Restrict if has Matches

---

### DIR-003: Audit Trail
**Rule:** All records maintain creation and update timestamps.

**Fields:**
- CreatedAt: Set on creation, never changed
- UpdatedAt: Set on creation, updated on every change

**Implementation:** Handled automatically by DbContext.

---

## 9. Security Rules

### SEC-001: Input Validation
**Rule:** All user input must be validated.

**Validation:**
- Required fields must be present
- String lengths must be within limits
- Numeric values must be within valid ranges
- Dates must be valid
- Enums must be valid values

---

### SEC-002: SQL Injection Prevention
**Rule:** All database queries must use parameterized queries.

**Implementation:**
- Entity Framework Core handles this automatically
- Never concatenate user input into SQL

---

### SEC-003: XSS Prevention
**Rule:** All output must be properly encoded.

**Implementation:**
- React handles this automatically
- Be careful with dangerouslySetInnerHTML

---

## 10. Performance Rules

### PERF-001: Pagination
**Rule:** Large result sets must be paginated.

**Implementation:**
- Default page size: 20
- Maximum page size: 100
- Include pagination metadata in response

---

### PERF-002: Eager Loading
**Rule:** Related entities should be loaded efficiently.

**Implementation:**
- Use .Include() for related entities
- Avoid N+1 query problems
- Use projection when only specific fields needed

---

### PERF-003: Caching
**Rule:** Static data should be cached.

**Cacheable Data:**
- Groups (rarely change)
- Stadiums (rarely change)
- Teams (change infrequently)

**Cache Duration:**
- Groups: 1 hour
- Stadiums: 1 hour
- Teams: 15 minutes
- Matches: 5 minutes
- Standings: 1 minute

---

## 11. Business Logic Workflows

### Workflow 1: Recording Match Result

**Steps:**
1. Validate match exists and is finished
2. Validate scores are non-negative
3. Determine winner based on scores
4. If knockout and draw, validate penalties
5. Create or update MatchResult
6. Update match status to "Finished"
7. If group stage match:
   - Update home team standing
   - Update away team standing
   - Recalculate group standings
8. If knockout match:
   - Advance winner to next round
   - Update bracket

**Rollback:** If any step fails, rollback entire transaction.

---

### Workflow 2: Updating Standing

**Steps:**
1. Get all group stage matches for team
2. Calculate:
   - Matches played
   - Wins, draws, losses
   - Goals for, goals against
   - Goal difference
   - Points
3. Update standing record
4. Recalculate group rankings

**Trigger:** Automatically after match result is recorded.

---

### Workflow 3: Determining Group Winners

**Steps:**
1. Get all standings for group
2. Sort by:
   - Points (desc)
   - Goal difference (desc)
   - Goals for (desc)
3. Top 2 teams qualify automatically
4. Third-place team added to third-place pool
5. After all groups complete:
   - Rank all third-place teams
   - Top 8 qualify

---

## 12. Error Handling Rules

### ERR-001: Validation Errors
**HTTP Status:** 422 Unprocessable Entity

**Response Format:**
```json
{
  "error": {
    "code": "VALIDATION_ERROR",
    "message": "Validation failed",
    "details": [
      "Team code must be exactly 3 characters",
      "Team name is required"
    ]
  }
}
```

---

### ERR-002: Not Found Errors
**HTTP Status:** 404 Not Found

**Response Format:**
```json
{
  "error": {
    "code": "NOT_FOUND",
    "message": "Team with ID 123 not found"
  }
}
```

---

### ERR-003: Business Rule Violations
**HTTP Status:** 400 Bad Request

**Response Format:**
```json
{
  "error": {
    "code": "BUSINESS_RULE_VIOLATION",
    "message": "A team cannot play against itself"
  }
}
```

---

### ERR-004: Conflict Errors
**HTTP Status:** 409 Conflict

**Response Format:**
```json
{
  "error": {
    "code": "CONFLICT",
    "message": "Team code 'ARG' already exists"
  }
}
```

---

## Implementation Checklist

- [ ] Implement all validation rules in FluentValidation validators
- [ ] Implement business logic in service layer
- [ ] Add database constraints for data integrity
- [ ] Create custom exceptions for business rule violations
- [ ] Add unit tests for all business rules
- [ ] Document all rules in API documentation
- [ ] Add error messages to localization files (future)
- [ ] Create integration tests for workflows
- [ ] Add logging for business rule violations
- [ ] Monitor and track rule violations in production

---

## Future Enhancements

1. **Fair Play Points:** Track yellow/red cards for tiebreaking
2. **Head-to-Head Records:** Implement for tiebreaking
3. **Match Events:** Track goals, cards, substitutions
4. **Player Statistics:** Individual player performance
5. **Advanced Analytics:** Possession, shots, passes, etc.
6. **Prediction System:** User predictions with scoring
7. **Real-time Updates:** Live match updates
8. **Video Highlights:** Link to match highlights
9. **News Integration:** Tournament news and updates
10. **Social Features:** Comments, sharing, discussions