namespace AdvancedMemory.Core.Domain.Entities;

/// <summary>
/// Represents a relationship between two entities in the knowledge graph.
/// </summary>
public class Relationship
{
    public required string Id { get; init; }
    public required string SourceEntityId { get; init; }
    public required string TargetEntityId { get; init; }
    public required string RelationType { get; init; }
    public required DateTime CreatedAt { get; init; }
    public DateTime? UpdatedAt { get; set; }
    public float Confidence { get; set; } = 1.0f;
    public Dictionary<string, object> Properties { get; init; } = new();
    public string? SourceDocumentId { get; init; }
    public string? ExtractedContext { get; set; }
}
