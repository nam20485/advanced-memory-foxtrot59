namespace AdvancedMemory.Core.Domain.ValueObjects;

/// <summary>
/// Query request for memory or knowledge retrieval.
/// </summary>
public record QueryRequest
{
    public required string UserId { get; init; }
    public required string Query { get; init; }
    public QueryType Type { get; init; } = QueryType.Hybrid;
    public int TopK { get; init; } = 10;
    public List<string>? MemoryTypes { get; init; }
    public Dictionary<string, string>? Filters { get; init; }
    public bool IncludeRelationships { get; init; } = true;
}

/// <summary>
/// Query result containing retrieved items and metadata.
/// </summary>
public record QueryResult
{
    public required List<QueryResultItem> Items { get; init; }
    public required int TotalCount { get; init; }
    public required TimeSpan ExecutionTime { get; init; }
    public Dictionary<string, object>? Metadata { get; init; }
}

/// <summary>
/// Individual query result item.
/// </summary>
public record QueryResultItem
{
    public required string Id { get; init; }
    public required string Content { get; init; }
    public required float Score { get; init; }
    public required string Type { get; init; }
    public Dictionary<string, object>? Metadata { get; init; }
    public List<RelatedItem>? RelatedItems { get; init; }
}

/// <summary>
/// Related item reference.
/// </summary>
public record RelatedItem
{
    public required string Id { get; init; }
    public required string Type { get; init; }
    public required string RelationType { get; init; }
    public float Confidence { get; init; }
}

/// <summary>
/// Query type classification.
/// </summary>
public enum QueryType
{
    /// <summary>
    /// Semantic vector search only.
    /// </summary>
    Semantic,

    /// <summary>
    /// Graph traversal and reasoning.
    /// </summary>
    Graph,

    /// <summary>
    /// Combined semantic + graph search.
    /// </summary>
    Hybrid,

    /// <summary>
    /// GraphRAG global search (community-based).
    /// </summary>
    Global,

    /// <summary>
    /// GraphRAG local search (entity-specific).
    /// </summary>
    Local
}
