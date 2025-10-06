namespace AdvancedMemory.Core.Domain.Entities;

/// <summary>
/// Represents an entity extracted from knowledge sources or memory.
/// </summary>
public class Entity
{
    public required string Id { get; init; }
    public required string Name { get; init; }
    public required EntityType Type { get; init; }
    public string? Description { get; set; }
    public required DateTime CreatedAt { get; init; }
    public DateTime? UpdatedAt { get; set; }
    public Dictionary<string, object> Properties { get; init; } = new();
    public required float[] Embedding { get; init; }
    public List<string> Aliases { get; init; } = new();
    public string? SourceDocumentId { get; init; }
    public int MentionCount { get; set; }
}

/// <summary>
/// Entity type classification for knowledge graph.
/// </summary>
public enum EntityType
{
    Person,
    Organization,
    Location,
    Event,
    Concept,
    Product,
    Technology,
    Other
}
