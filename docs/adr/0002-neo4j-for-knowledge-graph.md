# ADR-0002: Neo4j for Knowledge Graph

**Status**: Accepted  
**Date**: 2025-10-05  
**Deciders**: Development Team  
**Technical Story**: Data store selection for knowledge graph and entity relationships

## Context

The Advanced-Memory2 system requires a graph database to:

1. Store entities (people, places, concepts) and their relationships
2. Support GraphRAG (Graph-based Retrieval Augmented Generation)
3. Perform graph traversal queries (shortest path, community detection)
4. Scale to millions of entities and relationships
5. Provide ACID transactions for consistency
6. Support graph algorithms (PageRank, community detection, centrality)

## Decision

We will use **Neo4j 5.15** as our graph database for knowledge graph storage and GraphRAG operations.

## Rationale

### Why Neo4j?

1. **Native Graph Database**:
   - Purpose-built for graph data (not a relational database with graph features)
   - Index-free adjacency for O(1) relationship traversal
   - Optimized storage format for nodes and relationships

2. **Cypher Query Language**:
   - Declarative, SQL-like syntax for graph queries
   - Pattern matching with ASCII-art syntax: `(a)-[:KNOWS]->(b)`
   - Powerful aggregation and filtering capabilities
   - Easy to learn and read

3. **Graph Algorithms**:
   - **APOC (Awesome Procedures On Cypher)**: 500+ utility procedures
   - **Graph Data Science (GDS)**: Production-grade graph algorithms
     - Community detection (Louvain, Label Propagation)
     - Centrality (PageRank, Betweenness, Closeness)
     - Pathfinding (Dijkstra, A*, Yen's k-shortest paths)
     - Similarity algorithms (Node Similarity, Jaccard)

4. **Performance**:
   - Handles billions of nodes and relationships
   - Sub-millisecond traversal times
   - Efficient indexing on properties
   - Connection pooling and query caching

5. **Ecosystem**:
   - Official .NET driver (Neo4j.Driver)
   - Active community and extensive documentation
   - Neo4j Browser for visual graph exploration
   - Bloom for business user visualization

6. **GraphRAG Support**:
   - Combines graph structure with vector embeddings
   - Supports hybrid queries (graph + semantic search)
   - Community detection for context clustering
   - Relationship-aware retrieval

### Alternatives Considered

1. **Amazon Neptune**
   - ❌ Cloud-only (no local development)
   - ❌ More expensive
   - ✅ Managed service
   - **Decision**: Neo4j better for local development

2. **ArangoDB**
   - ❌ Multi-model complexity
   - ❌ Weaker graph algorithm support
   - ✅ Document + graph hybrid
   - **Decision**: Neo4j more focused on graphs

3. **JanusGraph**
   - ❌ Complex setup (requires Cassandra/HBase + Elasticsearch)
   - ❌ Smaller community
   - ✅ Distributed by design
   - **Decision**: Neo4j simpler and more mature

4. **Azure Cosmos DB (Gremlin API)**
   - ❌ Gremlin query language less intuitive than Cypher
   - ❌ Higher cost
   - ✅ Managed Azure service
   - **Decision**: Neo4j better query language and local development

## Implementation

### Neo4j Configuration (docker-compose.yml)

```yaml
neo4j:
  image: neo4j:5.15
  ports:
    - "7474:7474"  # HTTP (Browser)
    - "7687:7687"  # Bolt protocol
  environment:
    - NEO4J_AUTH=neo4j/advancedmemory123
    - NEO4J_PLUGINS=["apoc", "graph-data-science"]
    - NEO4J_dbms_security_procedures_unrestricted=apoc.*,gds.*
    - NEO4J_dbms_memory_heap_initial__size=512m
    - NEO4J_dbms_memory_heap_max__size=2g
  volumes:
    - neo4j-data:/data
    - neo4j-logs:/logs
```

### .NET Driver Usage

```csharp
// Connection
var driver = GraphDatabase.Driver(
    "bolt://localhost:7687",
    AuthTokens.Basic("neo4j", "advancedmemory123"));

// Entity creation
await session.RunAsync(@"
    CREATE (e:Entity {
        id: $id,
        name: $name,
        type: $type,
        embedding: $embedding
    })
", new { id, name, type, embedding });

// Relationship creation
await session.RunAsync(@"
    MATCH (a:Entity {id: $sourceId})
    MATCH (b:Entity {id: $targetId})
    CREATE (a)-[r:RELATES_TO {
        type: $relationType,
        confidence: $confidence
    }]->(b)
", new { sourceId, targetId, relationType, confidence });

// GraphRAG query
var result = await session.RunAsync(@"
    MATCH (e:Entity)
    WHERE e.type = $entityType
    CALL gds.similarity.cosine.stream({
        nodeProjection: 'Entity',
        relationshipProjection: '*',
        nodeProperties: ['embedding']
    })
    YIELD node1, node2, similarity
    WHERE similarity > $threshold
    RETURN node1, node2, similarity
    ORDER BY similarity DESC
    LIMIT $limit
", new { entityType, threshold, limit });
```

## Consequences

### Positive

- **Graph-Native Performance**: Fast traversals and pattern matching
- **Intuitive Queries**: Cypher is readable and maintainable
- **Rich Algorithms**: APOC and GDS provide production-grade graph algorithms
- **GraphRAG Ready**: Native support for entity-relationship reasoning
- **Developer Experience**: Neo4j Browser for visual debugging
- **Scalability**: Proven at scale (billions of nodes)

### Negative

- **Single Database Type**: Not a multi-model database
- **Cost**: Enterprise features require paid license (community edition for development)
- **Learning Curve**: Team must learn Cypher
- **Memory Usage**: Graph databases are memory-intensive
- **Backup Complexity**: Requires proper backup strategy

### Mitigation Strategies

1. **Cypher Training**: Team workshops on Cypher query patterns
2. **Community Edition**: Use free community edition for development and testing
3. **Connection Pooling**: Reuse connections to minimize overhead
4. **Monitoring**: Track memory usage and query performance
5. **Indexing Strategy**: Create indexes on frequently queried properties

## Schema Design

### Node Labels
- `Entity`: Core entities (people, places, concepts)
- `Memory`: Episodic memories
- `Community`: Detected communities of related entities
- `Document`: Source documents

### Relationship Types
- `RELATES_TO`: General relationship between entities
- `MENTIONED_IN`: Entity mentioned in document/memory
- `BELONGS_TO`: Entity belongs to community
- `SIMILAR_TO`: Semantic similarity between entities

### Properties
- **Entity**: `id`, `name`, `type`, `embedding`, `createdAt`, `updatedAt`
- **Relationship**: `type`, `confidence`, `source`, `createdAt`

## Validation

- ✅ Neo4j 5.15 runs successfully in Docker
- ✅ APOC and GDS plugins load correctly
- ✅ .NET driver connects and executes queries
- ✅ Graph traversal performance meets requirements (<100ms for typical queries)
- ✅ Neo4j Browser accessible for visual debugging

## Performance Considerations

1. **Indexing**:
   ```cypher
   CREATE INDEX entity_id FOR (e:Entity) ON (e.id)
   CREATE INDEX entity_type FOR (e:Entity) ON (e.type)
   CREATE TEXT INDEX entity_name FOR (e:Entity) ON (e.name)
   ```

2. **Query Optimization**:
   - Use `EXPLAIN` and `PROFILE` to analyze query plans
   - Limit result sets with `LIMIT`
   - Use indexes for property lookups
   - Batch writes in transactions

3. **Memory Tuning**:
   - Initial heap: 512MB
   - Max heap: 2GB (adjustable based on data size)
   - Page cache: 512MB (for frequently accessed data)

## References

- [Neo4j Documentation](https://neo4j.com/docs/)
- [Neo4j .NET Driver](https://neo4j.com/docs/dotnet-manual/current/)
- [Cypher Manual](https://neo4j.com/docs/cypher-manual/current/)
- [APOC Documentation](https://neo4j.com/docs/apoc/current/)
- [Graph Data Science Library](https://neo4j.com/docs/graph-data-science/current/)
- [GraphRAG with Neo4j](https://neo4j.com/use-cases/graph-rag/)

## Notes

- Neo4j 5.15 is the latest stable version (as of October 2025)
- Community Edition is sufficient for development and small deployments
- Enterprise Edition required for clustering and advanced security features
- Neo4j Browser available at `http://localhost:7474`
- Bolt protocol (port 7687) used for driver connections
