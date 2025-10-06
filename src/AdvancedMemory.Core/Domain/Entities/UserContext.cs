namespace AdvancedMemory.Core.Domain.Entities;

/// <summary>
/// Represents a user context for multi-tenant memory isolation.
/// </summary>
public class UserContext
{
    public required string UserId { get; init; }
    public required string DisplayName { get; init; }
    public DateTime CreatedAt { get; init; }
    public DateTime? LastActiveAt { get; set; }
    public Dictionary<string, string> Preferences { get; init; } = new();
    public List<string> Tags { get; init; } = new();
}
