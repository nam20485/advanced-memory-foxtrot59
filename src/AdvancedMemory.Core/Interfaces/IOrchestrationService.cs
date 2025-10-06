using AdvancedMemory.Core.Domain.ValueObjects;

namespace AdvancedMemory.Core.Interfaces;

/// <summary>
/// Orchestration service interface for coordinating workflows across services.
/// </summary>
public interface IOrchestrationService
{
    /// <summary>
    /// Orchestrates a complete query workflow across memory, graph, and grounding services.
    /// </summary>
    Task<QueryResult> ExecuteQueryWorkflowAsync(QueryRequest request, CancellationToken cancellationToken = default);

    /// <summary>
    /// Orchestrates the process of adding a memory with automatic entity extraction and grounding.
    /// </summary>
    Task<MemoryWorkflowResult> ExecuteAddMemoryWorkflowAsync(string userId, string content, Dictionary<string, string>? metadata = null, CancellationToken cancellationToken = default);

    /// <summary>
    /// Orchestrates document indexing workflow with chunking, entity extraction, and vector indexing.
    /// </summary>
    Task<DocumentWorkflowResult> ExecuteDocumentIndexingWorkflowAsync(string userId, string content, string title, Dictionary<string, string>? metadata = null, CancellationToken cancellationToken = default);

    /// <summary>
    /// Orchestrates memory consolidation across multiple services.
    /// </summary>
    Task<ConsolidationWorkflowResult> ExecuteConsolidationWorkflowAsync(string userId, CancellationToken cancellationToken = default);

    /// <summary>
    /// Checks the health status of all dependent services.
    /// </summary>
    Task<ServiceHealthStatus> GetHealthStatusAsync(CancellationToken cancellationToken = default);
}

/// <summary>
/// Result of memory workflow execution.
/// </summary>
public record MemoryWorkflowResult(
    string MemoryId,
    int EntitiesExtracted,
    int RelationshipsCreated,
    bool IsGrounded,
    double ConfidenceScore,
    TimeSpan Duration
);

/// <summary>
/// Result of document indexing workflow.
/// </summary>
public record DocumentWorkflowResult(
    string DocumentId,
    int ChunksCreated,
    int EntitiesExtracted,
    int RelationshipsCreated,
    int VectorsIndexed,
    TimeSpan Duration
);

/// <summary>
/// Result of consolidation workflow.
/// </summary>
public record ConsolidationWorkflowResult(
    int MemoriesConsolidated,
    int DuplicatesRemoved,
    int EntitiesMerged,
    TimeSpan Duration
);

/// <summary>
/// Health status of orchestrated services.
/// </summary>
public record ServiceHealthStatus(
    bool IsHealthy,
    Dictionary<string, bool> ServiceStatus,
    string? Message = null
);
