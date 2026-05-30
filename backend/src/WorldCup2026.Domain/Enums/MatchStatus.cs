namespace WorldCup2026.Domain.Enums;

/// <summary>
/// Represents the current status of a match
/// </summary>
public enum MatchStatus
{
    /// <summary>
    /// Match is scheduled but not yet started
    /// </summary>
    Scheduled = 1,

    /// <summary>
    /// Match is currently in progress
    /// </summary>
    InProgress = 2,

    /// <summary>
    /// Match has finished
    /// </summary>
    Finished = 3,

    /// <summary>
    /// Match has been postponed
    /// </summary>
    Postponed = 4,

    /// <summary>
    /// Match has been cancelled
    /// </summary>
    Cancelled = 5
}

// Made with Bob
