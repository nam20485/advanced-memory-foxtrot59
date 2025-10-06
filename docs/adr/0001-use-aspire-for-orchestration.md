# ADR-0001: Use .NET Aspire for Orchestration

**Status**: Accepted  
**Date**: 2025-10-05  
**Deciders**: Development Team  
**Technical Story**: Project scaffolding and architecture design

## Context

The Advanced-Memory2 system is built as a distributed microservices architecture with multiple independent services (MCP Server, GraphRAG Service, Memory Service, Grounding Service, Orchestration Service). We need an orchestration solution that:

1. Simplifies local development with multiple services
2. Provides built-in observability (logging, tracing, metrics)
3. Supports cloud-native deployment patterns
4. Integrates seamlessly with .NET ecosystem
5. Reduces boilerplate configuration code

## Decision

We will use **.NET Aspire** as our orchestration framework for both local development and cloud deployment.

## Rationale

### Why .NET Aspire?

1. **Built-in Service Defaults**: Aspire provides standardized configuration for:
   - Structured logging with Serilog
   - Distributed tracing with OpenTelemetry
   - Health checks and readiness probes
   - Service discovery
   - Resilience patterns (retry, circuit breaker)

2. **Developer Experience**: 
   - Aspire AppHost provides a single entry point to start all services
   - Aspire Dashboard (`http://localhost:18888`) shows all services, logs, traces, and metrics in one place
   - Automatic service-to-service communication configuration
   - Hot reload support during development

3. **Cloud-Native Ready**:
   - Generates container images automatically
   - Supports Kubernetes deployment via manifests
   - Azure Container Apps integration
   - Infrastructure as code support

4. **Observability**:
   - Pre-configured OpenTelemetry exporters
   - Structured logging to Seq
   - Distributed tracing to Jaeger
   - Metrics exposed for Prometheus

5. **.NET Ecosystem Integration**:
   - First-class support for ASP.NET Core
   - Works with Entity Framework Core
   - Integrates with Azure services
   - NuGet package distribution

### Alternatives Considered

1. **Docker Compose Only**
   - ❌ Requires manual configuration for logging, tracing, metrics
   - ❌ No built-in service discovery
   - ❌ Limited cloud deployment support
   - ✅ Simple and widely understood
   - **Decision**: Keep Docker Compose for infrastructure services (Neo4j, Qdrant, Redis), use Aspire for application services

2. **Kubernetes**
   - ❌ Too complex for local development
   - ❌ Steep learning curve
   - ✅ Production-grade orchestration
   - **Decision**: Use Aspire for development, generate Kubernetes manifests for production

3. **Dapr (Distributed Application Runtime)**
   - ❌ Additional abstraction layer
   - ❌ More complex setup
   - ✅ Language-agnostic
   - **Decision**: Aspire is more .NET-native and simpler for our use case

## Implementation

### AppHost Configuration (`src/AdvancedMemory.AppHost/AppHost.cs`)

```csharp
var builder = DistributedApplication.CreateBuilder(args);

// Infrastructure references (running in Docker)
var neo4j = builder.AddResource("neo4j")
    .WithEndpoint(port: 7687, scheme: "bolt");
var qdrant = builder.AddResource("qdrant")
    .WithEndpoint(port: 6333, scheme: "http");
var redis = builder.AddResource("redis")
    .WithEndpoint(port: 6379);

// Application services
var graphrag = builder.AddProject<Projects.AdvancedMemory_GraphRAGService>("graphrag-service")
    .WithReference(neo4j)
    .WithReference(redis);

var memory = builder.AddProject<Projects.AdvancedMemory_MemoryService>("memory-service")
    .WithReference(qdrant)
    .WithReference(redis);

var grounding = builder.AddProject<Projects.AdvancedMemory_GroundingService>("grounding-service")
    .WithReference(redis);

var orchestration = builder.AddProject<Projects.AdvancedMemory_OrchestrationService>("orchestration-service")
    .WithReference(graphrag)
    .WithReference(memory)
    .WithReference(grounding);

var mcp = builder.AddProject<Projects.AdvancedMemory_McpServer>("mcp-server")
    .WithReference(orchestration)
    .WithReference(graphrag)
    .WithReference(memory)
    .WithReference(grounding);

builder.Build().Run();
```

### Service Defaults (`src/AdvancedMemory.ServiceDefaults/Extensions.cs`)

All services reference `AdvancedMemory.ServiceDefaults` which provides:
- Serilog configuration with Seq sink
- OpenTelemetry with Jaeger exporter
- Health check endpoints
- CORS policies
- Exception handling middleware

## Consequences

### Positive

- **Simplified Development**: Single `dotnet run` command starts entire system
- **Consistent Observability**: All services emit logs and traces in standardized format
- **Reduced Boilerplate**: ServiceDefaults eliminates repetitive configuration
- **Better DX**: Aspire Dashboard provides instant visibility into all services
- **Cloud Readiness**: Easy transition from local to cloud deployment

### Negative

- **Learning Curve**: Team must learn Aspire-specific patterns
- **Framework Lock-in**: Tight coupling to .NET Aspire (mitigated by container support)
- **Early Adoption Risk**: Aspire is relatively new (GA in .NET 9)
- **Debugging Complexity**: Additional abstraction layer may complicate troubleshooting

### Mitigation Strategies

1. **Documentation**: Comprehensive README and ADRs for Aspire usage
2. **Container Support**: All services have Dockerfiles for non-Aspire deployment
3. **Training**: Team knowledge sharing sessions on Aspire patterns
4. **Fallback**: Docker Compose as alternative for local development

## Validation

- ✅ All services start successfully via Aspire AppHost
- ✅ Aspire Dashboard shows logs, traces, and metrics
- ✅ Service-to-service communication works via service discovery
- ✅ Health checks report service status correctly
- ✅ Docker containers can be generated from Aspire projects

## References

- [.NET Aspire Documentation](https://learn.microsoft.com/en-us/dotnet/aspire/)
- [Aspire Service Defaults](https://learn.microsoft.com/en-us/dotnet/aspire/fundamentals/service-defaults)
- [Aspire Orchestration](https://learn.microsoft.com/en-us/dotnet/aspire/fundamentals/app-host-overview)

## Notes

- Aspire is GA as of .NET 9.0 (November 2024)
- Aspire Dashboard provides excellent local development experience
- Can generate Kubernetes manifests via `dotnet aspire generate-manifests`
- Azure Container Apps has first-class Aspire support
