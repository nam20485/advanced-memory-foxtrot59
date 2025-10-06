using AdvancedMemory.Core.Domain.Entities;

namespace AdvancedMemory.Core.Interfaces;

/// <summary>
/// Repository interface for entity and relationship storage (Neo4j graph database).
/// </summary>
public interface IEntityRepository
{
    /// <summary>
    /// Adds a new entity to the graph.
    /// </summary>
    Task<Entity> AddEntityAsync(Entity entity, CancellationToken cancellationToken = default);

    /// <summary>
    /// Retrieves an entity by ID.
    /// </summary>
    Task<Entity?> GetEntityByIdAsync(string entityId, CancellationToken cancellationToken = default);

    /// <summary>
    /// Finds entities by name and type.
    /// </summary>
    Task<List<Entity>> FindEntitiesByNameAsync(string name, EntityType? type = null, CancellationToken cancellationToken = default);

    /// <summary>
    /// Updates an existing entity.
    /// </summary>
    Task<Entity> UpdateEntityAsync(Entity entity, CancellationToken cancellationToken = default);

    /// <summary>
    /// Deletes an entity by ID.
    /// </summary>
    Task<bool> DeleteEntityAsync(string entityId, CancellationToken cancellationToken = default);

    /// <summary>
    /// Retrieves all entities for a user.
    /// </summary>
    Task<List<Entity>> GetEntitiesByUserIdAsync(string userId, EntityType? type = null, int? limit = null, CancellationToken cancellationToken = default);

    /// <summary>
    /// Adds a relationship between two entities.
    /// </summary>
    Task<Relationship> AddRelationshipAsync(Relationship relationship, CancellationToken cancellationToken = default);

    /// <summary>
    /// Retrieves a relationship by ID.
    /// </summary>
    Task<Relationship?> GetRelationshipByIdAsync(string relationshipId, CancellationToken cancellationToken = default);

    /// <summary>
    /// Retrieves all relationships for a specific entity.
    /// </summary>
    Task<List<Relationship>> GetRelationshipsForEntityAsync(string entityId, CancellationToken cancellationToken = default);

    /// <summary>
    /// Deletes a relationship by ID.
    /// </summary>
    Task<bool> DeleteRelationshipAsync(string relationshipId, CancellationToken cancellationToken = default);

    /// <summary>
    /// Finds entities connected to a source entity within a specified depth.
    /// </summary>
    Task<List<Entity>> GetConnectedEntitiesAsync(string entityId, int maxDepth = 2, CancellationToken cancellationToken = default);

    /// <summary>
    /// Retrieves a subgraph around a specific entity.
    /// </summary>
    Task<(List<Entity> Entities, List<Relationship> Relationships)> GetSubgraphAsync(string entityId, int depth = 2, CancellationToken cancellationToken = default);

    /// <summary>
    /// Finds the shortest path between two entities.
    /// </summary>
    Task<List<Entity>?> FindShortestPathAsync(string sourceEntityId, string targetEntityId, int maxDepth = 5, CancellationToken cancellationToken = default);

    /// <summary>
    /// Merges duplicate entities.
    /// </summary>
    Task<Entity> MergeEntitiesAsync(string primaryEntityId, List<string> duplicateEntityIds, CancellationToken cancellationToken = default);
}
