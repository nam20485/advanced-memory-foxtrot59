# Architecture - Advanced-Memory2

## Executive Summary

Advanced-Memory2 is a cloud-native .NET 9.0 application that implements a next-generation AI agent architecture combining GraphRAG (structured knowledge retrieval), Mem0 (agentic memory), and MCP (Model Context Protocol) orchestration. The system provides AI agents with both deep domain knowledge and personalized, evolving memory capabilities, enabling intelligent, context-aware interactions.

## System Overview

### Vision Statement
Create a "digital expert" system that possesses:
1. **Deep Domain Knowledge**: Structured, queryable knowledge base via GraphRAG
2. **Personal Memory**: Evolving, user-specific memory via Mem0 patterns
3. **Verification**: Fact-checking layer to reduce hallucinations
4. **Orchestration**: Intelligent workflow management via MCP

### Core Capabilities
- Advanced memory management (working, episodic, factual, semantic)
- Graph-based knowledge retrieval with multi-hop reasoning
- Real-time monitoring and observability via Blazor UI
- Scalable microservices architecture with .NET Aspire
- Multi-user support with isolated memory contexts

## High-Level Architecture

### Architectural Style
- **Primary**: Microservices Architecture
- **Pattern**: Clean Architecture with Domain-Driven Design
- **Communication**: RESTful APIs with Server-Sent Events (SSE) for streaming
- **Protocol**: Model Context Protocol (MCP) for agent-tool communication

### System Context

```
┌─────────────┐
│   AI Agent  │ (External LLM-based agents: Claude, GPT, etc.)
└──────┬──────┘
       │ MCP Protocol (SSE)
       ▼
┌─────────────────────────────────────────────────────┐
│         Advanced-Memory2 MCP Server                 │
│  ┌──────────────────────────────────────────────┐  │
│  │   API Gateway / MCP Endpoint Layer          │  │
│  └────────────┬─────────────────────────────────┘  │
│               │                                      │
│       ┌───────┼────────┬──────────┐                │
│       ▼       ▼        ▼          ▼                │
│  ┌────────┐ ┌────┐ ┌──────┐ ┌─────────┐           │
│  │GraphRAG│ │Mem0│ │Ground│ │Orchestr.│           │
│  │Service │ │Svc │ │Check │ │ Layer   │           │
│  └───┬────┘ └──┬─┘ └───┬──┘ └────┬────┘           │
└──────┼─────────┼───────┼─────────┼────────────────┘
       │         │       │         │
       ▼         ▼       ▼         ▼
┌────────┐ ┌─────────┐ ┌────────┐ ┌──────────┐
│ Neo4j  │ │ Qdrant  │ │ Cache  │ │ Logging  │
│(Graph) │ │(Vector) │ │(Redis) │ │(Seq/OTel)│
└────────┘ └─────────┘ └────────┘ └──────────┘

       ┌──────────────────────┐
       │   Blazor WebAssembly │ (Monitoring Dashboard)
       │   Real-Time UI       │
       └──────────────────────┘
```

## Core Components

### 1. MCP Server (API Gateway)

**Responsibility**: Expose unified MCP-compliant API for AI agents

**Key Features**:
- Server-Sent Events (SSE) endpoint for MCP protocol
- Request routing to appropriate service
- Authentication & authorization
- Rate limiting & throttling
- Request/response logging

**Technology**:
- ASP.NET Core 9.0 Minimal APIs
- FastAPI-inspired endpoint design
- Swagger/OpenAPI documentation

**Endpoints**:
- `POST /mcp/sse` - Main MCP endpoint (streaming)
- `GET /health` - Health check
- `GET /metrics` - Prometheus metrics

### 2. GraphRAG Knowledge Service

**Responsibility**: Provide structured knowledge retrieval from document corpus

**Architecture Pattern**: "Graph-of-Graphs"

**Sub-Components**:

#### a) Indexing Pipeline
- **Document Ingestion**: Load raw documents (.txt, .csv, .json)
- **Text Chunking**: Split documents into processable chunks (512-1024 tokens)
- **Entity Extraction**: Use LLM to extract entities and relationships
- **Graph Construction**: Build knowledge graph in Neo4j
- **Community Detection**: Identify thematic clusters using graph algorithms
- **Community Summarization**: Generate hierarchical summaries

#### b) Query Engine
- **Global Search**: Answer broad thematic questions using community summaries
- **Local Search**: Answer specific entity questions via graph traversal
- **Multi-hop Reasoning**: Follow relationships across multiple nodes

**Data Storage**:
- **Graph Database**: Neo4j (entities as nodes, relationships as edges)
- **Vector Storage**: Qdrant (for semantic search of text chunks)
- **Cache**: Redis (for frequently accessed queries)

**API**:
```csharp
// Example API contract
public interface IGraphRAGService
{
    Task<KnowledgeResponse> QueryAsync(
        string query, 
        SearchType searchType, // Global or Local
        string? userContext = null
    );
    
    Task<IndexingStatus> IndexDocumentsAsync(
        IEnumerable<Document> documents
    );
}
```

### 3. Mem0 Memory Service

**Responsibility**: Manage user-specific, evolving memory

**Memory Types** (inspired by human cognition):

1. **Working Memory**: In-session context (ephemeral)
2. **Episodic Memory**: Past conversations and events
3. **Factual Memory**: User preferences, settings, facts
4. **Semantic Memory**: Learned patterns and generalizations

**Architecture**:

#### a) Memory Storage (Hybrid)
- **Vector DB** (Qdrant): Semantic search of memories
- **Graph DB** (Neo4j): Relationship tracking between memories
- **Key-Value** (Redis): Fast access to user profiles

#### b) Memory Operations
- **Add**: Store new interaction, extract entities/relationships
- **Search**: Retrieve relevant memories via semantic + recency + importance scoring
- **Update**: Modify existing memory
- **Consolidate**: Merge similar memories, form semantic patterns

**Data Model**:
```csharp
public class Memory
{
    public string Id { get; set; }
    public string UserId { get; set; }
    public MemoryType Type { get; set; } // Working, Episodic, Factual, Semantic
    public string Content { get; set; }
    public DateTime Timestamp { get; set; }
    public float ImportanceScore { get; set; } // 1-10
    public Dictionary<string, string> Metadata { get; set; }
    public float[] Embedding { get; set; }
}
```

**API**:
```csharp
public interface IMemoryService
{
    Task<MemoryAddResult> AddInteractionAsync(
        string userId, 
        List<Message> conversationTurn, 
        Dictionary<string, string>? metadata = null
    );
    
    Task<List<Memory>> SearchMemoriesAsync(
        string userId, 
        string query, 
        int limit = 5
    );
    
    Task<UserProfile> GetUserProfileAsync(string userId);
}
```

### 4. Grounding/Verification Service

**Responsibility**: Fact-check generated claims against trusted sources

**Purpose**: Reduce hallucinations and increase trust

**Architecture**:
- Lightweight RAG pointed at verified, up-to-date corpus
- Fast vector search (optimized for speed over depth)
- Confidence scoring for verification results

**Process**:
1. Agent generates answer from GraphRAG + Mem0
2. Before returning to user, extract factual claims
3. Verify each claim against grounding corpus
4. Return answer with confidence scores and sources

**API**:
```csharp
public interface IGroundingService
{
    Task<VerificationResult> VerifyClaimAsync(
        string claim, 
        string? domain = null
    );
}

public class VerificationResult
{
    public bool IsVerified { get; set; }
    public float ConfidenceScore { get; set; }
    public List<SourceReference> Sources { get; set; }
    public string? Contradiction { get; set; }
}
```

### 5. Orchestration Layer

**Responsibility**: Coordinate multi-step workflows across services

**Pattern**: Composite Tool Pattern

**Benefits**:
- Simplify agent logic (high-level tool calls)
- Enable parallel execution of independent calls
- Centralize complex workflows
- Improve observability

**Example Workflow**: `GetComprehensiveAnswer`
```csharp
public async Task<ComprehensiveAnswer> GetComprehensiveAnswerAsync(
    string userId, 
    string query
)
{
    // Step 1: Parallel retrieval
    var memoryTask = _memoryService.SearchMemoriesAsync(userId, query);
    var knowledgeTask = _graphRAGService.QueryAsync(
        query, 
        SearchType.Auto, 
        userContext: null // Will be filled after memory retrieval
    );
    
    await Task.WhenAll(memoryTask, knowledgeTask);
    
    var memories = await memoryTask;
    var knowledge = await knowledgeTask;
    
    // Step 2: Synthesize preliminary answer
    var preliminaryAnswer = await SynthesizeAnswerAsync(
        query, 
        memories, 
        knowledge
    );
    
    // Step 3: Verify factual claims
    var verification = await _groundingService.VerifyClaimAsync(
        preliminaryAnswer.Text
    );
    
    // Step 4: Refine answer with verification
    var finalAnswer = RefineWithVerification(
        preliminaryAnswer, 
        verification
    );
    
    // Step 5: Store interaction in memory
    await _memoryService.AddInteractionAsync(
        userId, 
        new[] { 
            new Message("user", query), 
            new Message("assistant", finalAnswer.Text) 
        }
    );
    
    return finalAnswer;
}
```

### 6. Blazor WebAssembly Dashboard

**Responsibility**: Real-time monitoring and visualization

**Features**:
- **Live Activity Feed**: Stream of memory/knowledge operations
- **System Metrics**: Request rates, latency, error rates
- **Knowledge Graph Visualization**: Interactive graph explorer
- **Memory Timeline**: User memory evolution over time
- **Service Health**: Status of all microservices

**Architecture**:
- Client-side Blazor WebAssembly (runs in browser)
- SignalR for real-time updates from backend
- Chart.js or Plotly.NET for visualizations

**Components**:
- `ActivityFeed.razor` - Real-time operation log
- `GraphViewer.razor` - Neo4j graph visualization
- `MemoryExplorer.razor` - User memory browser
- `SystemHealth.razor` - Service dashboard

### 7. .NET Aspire Orchestration

**Responsibility**: Service discovery, health checks, and telemetry

**Capabilities**:
- **Service Discovery**: Dynamic service registration
- **Health Checks**: Liveness and readiness probes
- **Telemetry**: OpenTelemetry integration
- **Configuration**: Centralized config management
- **Dashboard**: Built-in Aspire dashboard for development

**Configuration**:
```csharp
// Program.cs (Aspire App Host)
var builder = DistributedApplication.CreateBuilder(args);

var neo4j = builder.AddNeo4j("neo4j-db");
var qdrant = builder.AddQdrant("qdrant-db");
var redis = builder.AddRedis("redis-cache");

builder.AddProject<Projects.GraphRAGService>("graphrag-svc")
    .WithReference(neo4j)
    .WithReference(qdrant)
    .WithReference(redis);

builder.AddProject<Projects.MemoryService>("memory-svc")
    .WithReference(neo4j)
    .WithReference(qdrant)
    .WithReference(redis);

builder.AddProject<Projects.McpServer>("mcp-server")
    .WithReference("graphrag-svc")
    .WithReference("memory-svc");

builder.AddProject<Projects.BlazorDashboard>("dashboard");

builder.Build().Run();
```

## Data Architecture

### Data Flow

1. **Inbound Request**:
   ```
   AI Agent → MCP Server → Orchestration Layer → Service(s)
   ```

2. **Knowledge Retrieval**:
   ```
   GraphRAG Service → Neo4j (graph traversal) → Qdrant (vector search) → LLM (synthesis)
   ```

3. **Memory Operations**:
   ```
   Memory Service → LLM (entity extraction) → Qdrant (semantic storage) → Neo4j (relationship storage)
   ```

4. **Verification**:
   ```
   Grounding Service → Qdrant (trusted corpus) → Confidence scoring
   ```

### Data Models

#### Knowledge Graph Schema (Neo4j)
```
Nodes:
- Entity (properties: id, name, type, description, embedding)
- Chunk (properties: id, text, source, embedding)
- Community (properties: id, level, summary)

Relationships:
- EXTRACTED_FROM (Entity → Chunk)
- RELATED_TO (Entity → Entity, weight, description)
- BELONGS_TO (Entity → Community)
- PARENT (Community → Community) // Hierarchical structure
```

#### Memory Graph Schema (Neo4j)
```
Nodes:
- User (properties: id, profile)
- Memory (properties: id, type, content, timestamp, importance, embedding)
- MemoryEntity (properties: id, name, type)

Relationships:
- HAS_MEMORY (User → Memory)
- MENTIONS (Memory → MemoryEntity)
- RELATES_TO (Memory → Memory)
- REFERENCES (Memory → KnowledgeEntity) // Link to knowledge graph
```

### Data Security & Privacy

**Multi-Tenancy**:
- Memory data isolated by userId
- Graph database uses tenant-specific labels/namespaces
- Row-level security in queries

**Encryption**:
- Data at rest: Database-level encryption (Neo4j EE, Qdrant)
- Data in transit: TLS 1.3 for all connections
- Secrets: Azure Key Vault integration

**Data Retention**:
- Configurable memory expiration policies
- GDPR compliance: Right to erasure (delete user data)
- Audit logs for all data access

## Cross-Cutting Concerns

### 1. Observability

**Logging** (Serilog):
- Structured logging with context (userId, requestId, service)
- Centralized aggregation (Seq)
- Log levels: Debug (dev), Information (prod), Error (always)

**Tracing** (OpenTelemetry):
- Distributed tracing across services
- Trace context propagation (W3C Trace Context)
- Export to Jaeger or Azure Application Insights

**Metrics** (OpenTelemetry + Prometheus):
- Request rates, latency (p50, p95, p99)
- Error rates by service
- Cache hit rates
- Database query performance
- Custom business metrics (e.g., memory operations per user)

**Dashboard**:
- Grafana for metrics visualization
- Aspire Dashboard for service health

### 2. Resilience

**Fault Tolerance** (Polly):
- **Retry**: Exponential backoff for transient failures
- **Circuit Breaker**: Prevent cascading failures
- **Timeout**: Request timeout policies
- **Bulkhead**: Isolate critical resources

**Health Checks**:
- Liveness: Service is running
- Readiness: Service can handle requests
- Dependency health: Neo4j, Qdrant, Redis connectivity

### 3. Performance Optimization

**Caching Strategy** (Multi-Level):
- **L1 (In-Memory)**: Frequently accessed data (user profiles)
- **L2 (Redis)**: Shared cache across instances (query results)
- **L3 (Database)**: Persistent storage

**Connection Pooling**:
- Neo4j driver: Connection pool size based on load
- Qdrant gRPC: Keep-alive connections
- HttpClient: HttpClientFactory with pooling

**Async/Await**:
- Non-blocking I/O for all database and API calls
- Parallel execution where dependencies allow

### 4. Security

**Authentication**:
- API Key authentication for MCP server
- JWT tokens for service-to-service communication
- OAuth 2.0 / OpenID Connect (optional, for user auth)

**Authorization**:
- Role-based access control (RBAC)
- Policy-based authorization
- Tenant isolation enforcement

**Input Validation**:
- FluentValidation for request DTOs
- Sanitization of user inputs (prevent injection)
- Rate limiting per user/API key

### 5. Configuration Management

**Environment-Specific Config**:
- `appsettings.json` (base configuration)
- `appsettings.Development.json` (local dev)
- `appsettings.Production.json` (production overrides)
- Environment variables (secrets, connection strings)

**Secrets Management**:
- .NET User Secrets (development)
- Azure Key Vault (production)
- Never commit secrets to version control

## Deployment Architecture

### Local Development
```
Docker Compose:
  - Neo4j (port 7687, 7474)
  - Qdrant (port 6333)
  - Redis (port 6379)
  - Seq (port 5341)
  
.NET Aspire:
  - Orchestrates all .NET services
  - Provides service discovery
  - Aspire Dashboard (port 18888)
```

### Cloud Deployment (Azure Example)

```
Azure Resources:
  - Azure Container Apps (MCP Server, Services)
  - Azure Cosmos DB for Neo4j API (or VM with Neo4j)
  - Qdrant Cloud (or self-hosted in AKS)
  - Azure Cache for Redis
  - Azure Key Vault (secrets)
  - Azure Application Insights (telemetry)
  - Azure Static Web Apps (Blazor Dashboard)
  
Networking:
  - Virtual Network (VNet)
  - Private Endpoints for databases
  - Application Gateway (ingress)
  - Azure Front Door (CDN for Blazor)
```

### Kubernetes Deployment (Optional)

```
Kubernetes Cluster:
  - Namespace: advanced-memory
  - Deployments: MCP Server, GraphRAG, Memory, Grounding
  - StatefulSets: Neo4j, Qdrant (if self-hosted)
  - Services: ClusterIP for internal, LoadBalancer for MCP endpoint
  - ConfigMaps: Configuration
  - Secrets: Sensitive data
  - Ingress: NGINX or Traefik
  - Helm Charts: Package management
```

## Scalability Strategy

### Horizontal Scaling
- **Stateless Services**: Scale MCP Server, GraphRAG, Memory services independently
- **Load Balancing**: Round-robin or least-connections across instances
- **Auto-Scaling**: CPU/memory-based scaling rules

### Database Scaling
- **Neo4j**: Cluster deployment (Causal Cluster for HA)
- **Qdrant**: Sharding for large-scale vector storage
- **Redis**: Cluster mode or Redis Sentinel

### Performance Targets
- **Latency**: < 500ms for simple queries, < 2s for complex
- **Throughput**: 100+ requests/second per instance
- **Concurrency**: 50+ concurrent users per instance
- **Uptime**: 99.9% availability (3 nines)

## Technology Decisions & Trade-offs

### Why .NET Instead of Python?
- **Performance**: AOT compilation, native speed
- **Type Safety**: Strong typing reduces runtime errors
- **Ecosystem**: Rich .NET libraries, Aspire orchestration
- **Enterprise**: Better tooling for large-scale systems
- **Team Expertise**: Leverage existing .NET skills

### Why Neo4j for Graph Storage?
- **Maturity**: Production-grade graph database
- **Query Language**: Cypher is expressive for graph traversals
- **Performance**: Optimized for relationship queries
- **Drivers**: Official .NET driver with excellent support

### Why Qdrant for Vector Storage?
- **Performance**: Rust-based, highly optimized
- **Features**: Filtering, metadata search, payload storage
- **Deployment**: Easy Docker deployment, cloud option
- **.NET Support**: Good .NET client library

### MCP Protocol vs REST
- **Streaming**: SSE enables real-time updates
- **Standardization**: MCP is emerging standard for agent-tool communication
- **Flexibility**: Supports complex, multi-turn interactions
- **Observability**: Built-in trace propagation

## Migration & Evolution Path

### Phase 1: MVP (Proof of Concept)
- Single-instance deployment
- Simplified GraphRAG (document chunking + embedding)
- Basic memory (episodic only)
- No verification layer
- Console logging

### Phase 2: Production-Ready
- Multi-instance deployment with load balancing
- Full GraphRAG with community detection
- All memory types (working, episodic, factual, semantic)
- Grounding/verification layer
- Structured logging + metrics

### Phase 3: Advanced Features
- Multi-modal support (images, PDFs)
- Advanced reasoning (chain-of-thought)
- User feedback loops (reinforcement learning)
- A/B testing framework
- Cost optimization (model selection, caching)

## Architecture Decision Records (ADRs)

Key architectural decisions will be documented in `docs/adr/`:
- ADR-001: Choice of .NET over Python
- ADR-002: Neo4j as primary graph database
- ADR-003: Qdrant for vector storage
- ADR-004: MCP protocol for agent communication
- ADR-005: Clean Architecture pattern
- ADR-006: .NET Aspire for orchestration

## References

### Architecture Patterns
- Clean Architecture (Robert C. Martin)
- Microservices Patterns (Chris Richardson)
- Domain-Driven Design (Eric Evans)

### Technical References
- Microsoft GraphRAG Research Paper
- Mem0 Architecture Documentation
- Model Context Protocol Specification
- Neo4j Best Practices
- .NET Aspire Documentation

## Appendix: Component Dependencies

```
MCP Server
  ├── GraphRAG Service
  │   ├── Neo4j.Driver
  │   ├── Qdrant.Client
  │   └── OpenAI SDK
  ├── Memory Service
  │   ├── Neo4j.Driver
  │   ├── Qdrant.Client
  │   └── OpenAI SDK
  ├── Grounding Service
  │   └── Qdrant.Client
  └── Orchestration Layer
      └── (Coordinates above services)

Blazor Dashboard
  ├── SignalR Client
  └── Chart.js (via JS Interop)

Infrastructure
  ├── Neo4j (Docker or AuraDB)
  ├── Qdrant (Docker or Cloud)
  ├── Redis (Docker or Azure Cache)
  ├── Seq (Docker)
  └── Jaeger (Docker)
```

---

This architecture provides a solid foundation for building a production-grade AI agent system with advanced memory and knowledge capabilities. The design prioritizes modularity, scalability, observability, and maintainability while leveraging modern .NET 9.0 capabilities and proven architectural patterns.
