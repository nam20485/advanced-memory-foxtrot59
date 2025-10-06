using AdvancedMemory.Core.Domain.Entities;
using AdvancedMemory.Core.Domain.ValueObjects;

namespace AdvancedMemory.Core.Interfaces;

/// <summary>
/// Memory service interface for managing user memories.
/// </summary>
public interface IMemoryService
{
    /// <summary>
    /// Adds a new memory for a user.
    /// </summary>
    Task<Memory> AddMemoryAsync(string userId, string content, MemoryType type, Dictionary<string, string>? metadata = null, CancellationToken cancellationToken = default);

    /// <summary>
    /// Searches memories for a user.
    /// </summary>
    Task<QueryResult> SearchMemoriesAsync(QueryRequest request, CancellationToken cancellationToken = default);

    /// <summary>
    /// Retrieves a specific memory by ID.
    /// </summary>
    Task<Memory?> GetMemoryByIdAsync(string memoryId, CancellationToken cancellationToken = default);

    /// <summary>
    /// Updates an existing memory.
    /// </summary>
    Task<Memory> UpdateMemoryAsync(string memoryId, string content, Dictionary<string, string>? metadata = null, CancellationToken cancellationToken = default);

    /// <summary>
    /// Deletes a memory.
    /// </summary>
    Task<bool> DeleteMemoryAsync(string memoryId, CancellationToken cancellationToken = default);

    /// <summary>
    /// Retrieves all memories for a user with optional filtering.
    /// </summary>
    Task<List<Memory>> GetUserMemoriesAsync(string userId, MemoryType? type = null, int? limit = null, CancellationToken cancellationToken = default);

    /// <summary>
    /// Consolidates similar memories to reduce redundancy.
    /// </summary>
    Task<int> ConsolidateMemoriesAsync(string userId, CancellationToken cancellationToken = default);
}
