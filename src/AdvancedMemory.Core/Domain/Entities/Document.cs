namespace AdvancedMemory.Core.Domain.Entities;

/// <summary>
/// Represents a document chunk in the knowledge base.
/// </summary>
public class DocumentChunk
{
    public required string Id { get; init; }
    public required string DocumentId { get; init; }
    public required string Content { get; init; }
    public required int ChunkIndex { get; init; }
    public required float[] Embedding { get; init; }
    public required DateTime CreatedAt { get; init; }
    public Dictionary<string, string> Metadata { get; init; } = new();
    public List<string> ExtractedEntityIds { get; init; } = new();
}

/// <summary>
/// Represents a source document in the knowledge base.
/// </summary>
public class Document
{
    public required string Id { get; init; }
    public required string Title { get; init; }
    public required string Content { get; init; }
    public required DateTime CreatedAt { get; init; }
    public DateTime? UpdatedAt { get; set; }
    public Dictionary<string, string> Metadata { get; init; } = new();
    public required string SourceUri { get; init; }
    public DocumentStatus Status { get; set; } = DocumentStatus.Pending;
    public int ChunkCount { get; set; }
}

/// <summary>
/// Document processing status.
/// </summary>
public enum DocumentStatus
{
    Pending,
    Processing,
    Indexed,
    Failed
}
