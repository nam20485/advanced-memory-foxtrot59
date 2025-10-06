# ADR-0003: Qdrant for Vector Search

**Status**: Accepted  
**Date**: 2025-10-05  
**Deciders**: Development Team  
**Technical Story**: Data store selection for semantic memory and vector embeddings

## Context

The Advanced-Memory2 system requires a vector database to:

1. Store memory embeddings (3072-dimensional vectors from `text-embedding-3-large`)
2. Perform fast semantic similarity search (approximate nearest neighbors)
3. Support filtering by metadata (user, timestamp, type)
4. Handle millions of vectors with sub-second query times
5. Provide hybrid search (vector + keyword + filters)
6. Scale horizontally as data grows
7. Integrate with .NET via client library

## Decision

We will use **Qdrant v1.7** as our vector database for semantic memory storage and similarity search.

## Rationale

### Why Qdrant?

1. **Performance**:
   - HNSW (Hierarchical Navigable Small World) algorithm for fast ANN search
   - Sub-millisecond query times for millions of vectors
   - Efficient memory usage with quantization support
   - Batch operations for bulk inserts

2. **Filtering Capabilities**:
   - Rich filtering on metadata fields
   - Combined vector + filter queries in single request
   - Supports complex boolean logic (AND, OR, NOT)
   - Range queries on numeric fields

3. **API Design**:
   - REST API (port 6333) and gRPC API (port 6334)
   - Official clients for multiple languages (including .NET via `Qdrant.Client`)
   - OpenAPI/Swagger documentation
   - Simple, intuitive data model

4. **Features**:
   - **Collections**: Logical grouping of vectors with shared schema
   - **Payloads**: Arbitrary JSON metadata attached to vectors
   - **Snapshots**: Point-in-time backups
   - **Indexing**: Multiple index types (HNSW, IVF)
   - **Quantization**: Scalar and product quantization for memory efficiency

5. **Deployment**:
   - Single binary, easy to deploy
   - Docker image available
   - Persistent storage with configurable backends
   - Cluster support for high availability

6. **Developer Experience**:
   - Web UI at `http://localhost:6333/dashboard`
   - Excellent documentation
   - Active community and GitHub repository
   - Regular updates and improvements

### Alternatives Considered

1. **Pinecone**
   - ❌ Cloud-only (no local development)
   - ❌ Vendor lock-in
   - ❌ Higher cost
   - ✅ Managed service
   - **Decision**: Qdrant better for local development and cost control

2. **Weaviate**
   - ❌ More complex setup
   - ❌ GraphQL API (less familiar)
   - ✅ Built-in ML models
   - **Decision**: Qdrant simpler and REST API more familiar

3. **Milvus**
   - ❌ Requires multiple components (Etcd, MinIO, Pulsar)
   - ❌ Complex deployment
   - ✅ Very high scalability
   - **Decision**: Qdrant simpler for our scale

4. **ChromaDB**
   - ❌ Less mature
   - ❌ Limited production deployments
   - ✅ Python-native
   - **Decision**: Qdrant more production-ready

5. **pgvector (PostgreSQL extension)**
   - ❌ Slower for large datasets
   - ❌ Limited filtering capabilities
   - ✅ Familiar PostgreSQL
   - **Decision**: Qdrant built for vector search

## Implementation

### Qdrant Configuration (docker-compose.yml)

```yaml
qdrant:
  image: qdrant/qdrant:v1.7.0
  ports:
    - "6333:6333"  # REST API
    - "6334:6334"  # gRPC API
  environment:
    - QDRANT__SERVICE__HTTP_PORT=6333
    - QDRANT__SERVICE__GRPC_PORT=6334
  volumes:
    - qdrant-storage:/qdrant/storage
```

### Collection Schema

```csharp
// Create collection
await client.CreateCollectionAsync("memories", new VectorParams
{
    Size = 3072,  // text-embedding-3-large dimensions
    Distance = Distance.Cosine
});

// Configure payload schema
await client.CreatePayloadIndexAsync("memories", "userId", PayloadSchemaType.Keyword);
await client.CreatePayloadIndexAsync("memories", "timestamp", PayloadSchemaType.Integer);
await client.CreatePayloadIndexAsync("memories", "type", PayloadSchemaType.Keyword);
```

### .NET Client Usage

```csharp
// Install: dotnet add package Qdrant.Client

// Initialize client
var client = new QdrantClient("localhost", 6333);

// Store memory with embedding
var point = new PointStruct
{
    Id = Guid.NewGuid(),
    Vectors = embedding,  // float[] of 3072 dimensions
    Payload =
    {
        ["userId"] = userId,
        ["content"] = memoryText,
        ["timestamp"] = DateTimeOffset.UtcNow.ToUnixTimeSeconds(),
        ["type"] = "episodic"
    }
};
await client.UpsertAsync("memories", new[] { point });

// Semantic search with filters
var searchResult = await client.SearchAsync("memories", new SearchRequest
{
    Vector = queryEmbedding,
    Limit = 10,
    Filter = new Filter
    {
        Must = new List<Condition>
        {
            new Condition
            {
                Key = "userId",
                Match = new Match { Value = userId }
            },
            new Condition
            {
                Key = "timestamp",
                Range = new Range
                {
                    Gte = oneWeekAgo.ToUnixTimeSeconds()
                }
            }
        }
    },
    WithPayload = true,
    ScoreThreshold = 0.7f  // Only return similarities > 0.7
});
```

## Consequences

### Positive

- **Fast Similarity Search**: Sub-100ms queries for millions of vectors
- **Flexible Filtering**: Combine vector search with metadata filters
- **Easy Deployment**: Single Docker container for local development
- **Good .NET Support**: Official client library (`Qdrant.Client`)
- **Cost-Effective**: Open-source with no licensing costs
- **Horizontal Scaling**: Cluster support for production

### Negative

- **Learning Curve**: Team must learn vector database concepts
- **Memory Usage**: HNSW index requires significant RAM
- **Operational Complexity**: Need monitoring and backup strategies
- **Consistency**: Eventually consistent in clustered mode
- **Query Language**: Custom filter syntax (not SQL)

### Mitigation Strategies

1. **Documentation**: Comprehensive guides for Qdrant operations
2. **Monitoring**: Track query performance and memory usage
3. **Backup Strategy**: Regular snapshots to persistent storage
4. **Connection Pooling**: Reuse client connections
5. **Batch Operations**: Use bulk upsert for efficiency

## Data Model

### Collections

1. **memories**: Episodic memories and facts
   - Vector: 3072-dimensional embedding
   - Payload: `userId`, `content`, `timestamp`, `type`, `source`

2. **entities**: Entity embeddings for semantic entity search
   - Vector: 3072-dimensional embedding
   - Payload: `entityId`, `name`, `type`, `attributes`

3. **documents**: Document embeddings for retrieval
   - Vector: 3072-dimensional embedding
   - Payload: `docId`, `title`, `url`, `timestamp`, `chunkIndex`

### Payload Schema

```json
{
  "userId": "string (keyword index)",
  "content": "string (full-text index)",
  "timestamp": "integer (range index)",
  "type": "string (keyword index)",
  "source": "string",
  "metadata": {
    "confidence": "float",
    "tags": "array<string>"
  }
}
```

## Validation

- ✅ Qdrant v1.7 runs successfully in Docker
- ✅ REST API (6333) and gRPC API (6334) accessible
- ✅ .NET client connects and performs operations
- ✅ Similarity search performance meets requirements (<100ms)
- ✅ Filtering works correctly with metadata
- ✅ Persistent storage maintains data across restarts

## Performance Considerations

1. **Indexing Parameters**:
   ```csharp
   var hnswConfig = new HnswConfigDiff
   {
       M = 16,              // Number of edges per node (higher = better accuracy, more memory)
       EfConstruct = 100,   // Construction time quality (higher = better quality)
       FullScanThreshold = 10000  // Switch to brute-force for small collections
   };
   ```

2. **Quantization** (for memory efficiency):
   ```csharp
   var quantizationConfig = new ScalarQuantization
   {
       Type = QuantizationType.Int8,
       Quantile = 0.99f,
       AlwaysRam = true
   };
   ```

3. **Search Parameters**:
   ```csharp
   var searchParams = new SearchParams
   {
       Hnsw_ef = 128,  // Higher = better accuracy, slower search
       Exact = false   // Use approximate search (faster)
   };
   ```

4. **Batch Operations**:
   - Use `UpsertAsync` with batches of 100-1000 points
   - Use `ScrollAsync` for paginated retrieval
   - Use `ParallelAsync` for concurrent operations

## Monitoring and Observability

### Health Checks
```csharp
var health = await client.HealthAsync();
// Returns: { status: "ok" }
```

### Metrics
- Collection size (number of vectors)
- Query latency (p50, p95, p99)
- Memory usage per collection
- Index build time

### Dashboard
- Web UI: `http://localhost:6333/dashboard`
- View collections, points, and cluster status
- Run test queries

## References

- [Qdrant Documentation](https://qdrant.tech/documentation/)
- [Qdrant .NET Client](https://github.com/qdrant/qdrant-dotnet)
- [HNSW Algorithm](https://arxiv.org/abs/1603.09320)
- [Vector Search Best Practices](https://qdrant.tech/articles/vector-search/)

## Notes

- Qdrant v1.7 is the latest stable version (as of October 2025)
- REST API on port 6333, gRPC on port 6334
- Default distance metric: Cosine similarity (values 0-1, higher = more similar)
- Supports Euclidean and Dot Product distance metrics
- Payload indexing automatically created for filtered fields
- Snapshots can be created via API for backups
