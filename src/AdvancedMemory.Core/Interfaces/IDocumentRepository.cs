using AdvancedMemory.Core.Domain.Entities;

namespace AdvancedMemory.Core.Interfaces;

/// <summary>
/// Repository interface for document storage operations.
/// </summary>
public interface IDocumentRepository
{
    /// <summary>
    /// Adds a new document to storage.
    /// </summary>
    Task<Document> AddAsync(Document document, CancellationToken cancellationToken = default);

    /// <summary>
    /// Retrieves a document by ID.
    /// </summary>
    Task<Document?> GetByIdAsync(string documentId, CancellationToken cancellationToken = default);

    /// <summary>
    /// Updates an existing document.
    /// </summary>
    Task<Document> UpdateAsync(Document document, CancellationToken cancellationToken = default);

    /// <summary>
    /// Deletes a document by ID.
    /// </summary>
    Task<bool> DeleteAsync(string documentId, CancellationToken cancellationToken = default);

    /// <summary>
    /// Retrieves all documents for a user with optional status filtering.
    /// </summary>
    Task<List<Document>> GetByUserIdAsync(string userId, DocumentStatus? status = null, int? limit = null, CancellationToken cancellationToken = default);

    /// <summary>
    /// Retrieves documents by title pattern.
    /// </summary>
    Task<List<Document>> SearchByTitleAsync(string userId, string titlePattern, CancellationToken cancellationToken = default);

    /// <summary>
    /// Retrieves chunks for a specific document.
    /// </summary>
    Task<List<DocumentChunk>> GetChunksAsync(string documentId, CancellationToken cancellationToken = default);

    /// <summary>
    /// Adds a chunk to a document.
    /// </summary>
    Task<DocumentChunk> AddChunkAsync(DocumentChunk chunk, CancellationToken cancellationToken = default);

    /// <summary>
    /// Updates the status of a document.
    /// </summary>
    Task<bool> UpdateStatusAsync(string documentId, DocumentStatus status, CancellationToken cancellationToken = default);

    /// <summary>
    /// Gets the count of documents for a user.
    /// </summary>
    Task<int> CountAsync(string userId, DocumentStatus? status = null, CancellationToken cancellationToken = default);
}
