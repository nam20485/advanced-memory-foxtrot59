using AdvancedMemory.Core.Domain.Entities;
using AdvancedMemory.Core.Domain.ValueObjects;

namespace AdvancedMemory.Core.Interfaces;

/// <summary>
/// Graph-based Retrieval-Augmented Generation service interface.
/// Handles document indexing, entity extraction, and graph queries.
/// </summary>
#pragma warning disable S101 // Types should be named in PascalCase - GraphRAG is an industry-standard acronym
public interface IGraphRAGService
#pragma warning restore S101
{
    /// <summary>
    /// Indexes a document by extracting entities and relationships.
    /// </summary>
    Task<Document> IndexDocumentAsync(string userId, string content, string title, Dictionary<string, string>? metadata = null, CancellationToken cancellationToken = default);

    /// <summary>
    /// Extracts entities from text content.
    /// </summary>
    Task<List<Entity>> ExtractEntitiesAsync(string content, CancellationToken cancellationToken = default);

    /// <summary>
    /// Extracts relationships between entities from text content.
    /// </summary>
    Task<List<Relationship>> ExtractRelationshipsAsync(string content, List<Entity> entities, CancellationToken cancellationToken = default);

    /// <summary>
    /// Performs a graph-based query to find related information.
    /// </summary>
    Task<QueryResult> QueryGraphAsync(QueryRequest request, CancellationToken cancellationToken = default);

    /// <summary>
    /// Retrieves all entities connected to a specific entity.
    /// </summary>
    Task<List<Entity>> GetRelatedEntitiesAsync(string entityId, int maxDepth = 2, CancellationToken cancellationToken = default);

    /// <summary>
    /// Gets the subgraph around a specific entity.
    /// </summary>
    Task<(List<Entity> Entities, List<Relationship> Relationships)> GetSubgraphAsync(string entityId, int depth = 2, CancellationToken cancellationToken = default);

    /// <summary>
    /// Retrieves a document by ID.
    /// </summary>
    Task<Document?> GetDocumentByIdAsync(string documentId, CancellationToken cancellationToken = default);

    /// <summary>
    /// Retrieves all documents for a user.
    /// </summary>
    Task<List<Document>> GetUserDocumentsAsync(string userId, DocumentStatus? status = null, int? limit = null, CancellationToken cancellationToken = default);

    /// <summary>
    /// Deletes a document and its associated entities/relationships.
    /// </summary>
    Task<bool> DeleteDocumentAsync(string documentId, CancellationToken cancellationToken = default);
}
