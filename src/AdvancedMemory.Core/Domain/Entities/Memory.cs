namespace AdvancedMemory.Core.Domain.Entities;

/// <summary>
/// Represents a memory item in the system.
/// </summary>
public class Memory
{
    public required string Id { get; init; }
    public required string UserId { get; init; }
    public required string Content { get; init; }
    public required MemoryType Type { get; init; }
    public required DateTime CreatedAt { get; init; }
    public DateTime? UpdatedAt { get; set; }
    public DateTime? ExpiresAt { get; set; }
    public required float[] Embedding { get; init; }
    public Dictionary<string, string> Metadata { get; init; } = new();
    public float Importance { get; set; } = 0.5f;
    public int AccessCount { get; set; }
    public DateTime? LastAccessedAt { get; set; }
    public List<string> Tags { get; init; } = new();
    public List<string> RelatedEntityIds { get; init; } = new();
}

/// <summary>
/// Memory type classification following Mem0 patterns.
/// </summary>
public enum MemoryType
{
    /// <summary>
    /// Working memory - temporary, in-session context (ephemeral, TTL-based).
    /// </summary>
    Working,

    /// <summary>
    /// Episodic memory - conversation history, past events with timestamps.
    /// </summary>
    Episodic,

    /// <summary>
    /// Factual memory - user preferences, settings, persistent facts.
    /// </summary>
    Factual,

    /// <summary>
    /// Semantic memory - learned patterns, generalizations, abstractions.
    /// </summary>
    Semantic
}
