using AdvancedMemory.Core.Domain.Entities;

namespace AdvancedMemory.Core.Interfaces;

/// <summary>
/// Repository interface for memory storage operations.
/// </summary>
public interface IMemoryRepository
{
    /// <summary>
    /// Adds a new memory to storage.
    /// </summary>
    Task<Memory> AddAsync(Memory memory, CancellationToken cancellationToken = default);

    /// <summary>
    /// Retrieves a memory by ID.
    /// </summary>
    Task<Memory?> GetByIdAsync(string memoryId, CancellationToken cancellationToken = default);

    /// <summary>
    /// Updates an existing memory.
    /// </summary>
    Task<Memory> UpdateAsync(Memory memory, CancellationToken cancellationToken = default);

    /// <summary>
    /// Deletes a memory by ID.
    /// </summary>
    Task<bool> DeleteAsync(string memoryId, CancellationToken cancellationToken = default);

    /// <summary>
    /// Retrieves all memories for a user with optional filtering.
    /// </summary>
    Task<List<Memory>> GetByUserIdAsync(string userId, MemoryType? type = null, int? limit = null, CancellationToken cancellationToken = default);

    /// <summary>
    /// Retrieves memories within a specific time range.
    /// </summary>
    Task<List<Memory>> GetByTimeRangeAsync(string userId, DateTime startTime, DateTime endTime, CancellationToken cancellationToken = default);

    /// <summary>
    /// Retrieves memories by importance threshold.
    /// </summary>
    Task<List<Memory>> GetByImportanceAsync(string userId, double minImportance, int? limit = null, CancellationToken cancellationToken = default);

    /// <summary>
    /// Finds similar memories based on embedding similarity.
    /// </summary>
    Task<List<Memory>> FindSimilarAsync(float[] embedding, string userId, int topK = 10, double minSimilarity = 0.7, CancellationToken cancellationToken = default);

    /// <summary>
    /// Retrieves the count of memories for a user.
    /// </summary>
    Task<int> CountAsync(string userId, MemoryType? type = null, CancellationToken cancellationToken = default);
}
