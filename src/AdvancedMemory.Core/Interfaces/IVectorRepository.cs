using AdvancedMemory.Core.Domain.Entities;

namespace AdvancedMemory.Core.Interfaces;

/// <summary>
/// Repository interface for vector search operations (Qdrant).
/// </summary>
public interface IVectorRepository
{
    /// <summary>
    /// Indexes a memory with its embedding vector.
    /// </summary>
    Task<bool> IndexMemoryAsync(Memory memory, CancellationToken cancellationToken = default);

    /// <summary>
    /// Indexes a document chunk with its embedding vector.
    /// </summary>
    Task<bool> IndexDocumentChunkAsync(DocumentChunk chunk, CancellationToken cancellationToken = default);

    /// <summary>
    /// Performs a vector similarity search.
    /// </summary>
    Task<List<VectorSearchResult>> SearchAsync(float[] queryEmbedding, string? userId = null, int topK = 10, double minScore = 0.7, Dictionary<string, string>? filters = null, CancellationToken cancellationToken = default);

    /// <summary>
    /// Updates the embedding vector for an existing memory.
    /// </summary>
    Task<bool> UpdateMemoryEmbeddingAsync(string memoryId, float[] embedding, CancellationToken cancellationToken = default);

    /// <summary>
    /// Deletes a memory from the vector index.
    /// </summary>
    Task<bool> DeleteMemoryAsync(string memoryId, CancellationToken cancellationToken = default);

    /// <summary>
    /// Deletes a document chunk from the vector index.
    /// </summary>
    Task<bool> DeleteDocumentChunkAsync(string chunkId, CancellationToken cancellationToken = default);

    /// <summary>
    /// Deletes all vectors for a specific user.
    /// </summary>
    Task<bool> DeleteUserVectorsAsync(string userId, CancellationToken cancellationToken = default);

    /// <summary>
    /// Gets the count of indexed vectors for a user.
    /// </summary>
    Task<int> GetVectorCountAsync(string? userId = null, CancellationToken cancellationToken = default);

    /// <summary>
    /// Creates or updates a vector collection with specific configuration.
    /// </summary>
    Task<bool> EnsureCollectionExistsAsync(string collectionName, int vectorSize, CancellationToken cancellationToken = default);
}

/// <summary>
/// Result of a vector similarity search.
/// </summary>
public record VectorSearchResult(
    string Id,
    double Score,
    string Content,
    string Type,
    Dictionary<string, string> Metadata
);
