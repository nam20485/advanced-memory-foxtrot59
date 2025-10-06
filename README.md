# Advanced-Memory2 MCP Server

> A unified knowledge graph and semantic memory system with Model Context Protocol (MCP) integration

[![.NET 9.0](https://img.shields.io/badge/.NET-9.0-512BD4)](https://dotnet.microsoft.com/)
[![License: MIT](https://img.shields.io/badge/License-MIT-yellow.svg)](LICENSE.md)

## Overview

Advanced-Memory2 is an enterprise-grade semantic memory and knowledge graph system built with .NET 9.0 and .NET Aspire. It provides a unified MCP server that integrates GraphRAG, vector memory, and grounding capabilities to enable AI agents to maintain long-term memory and contextual understanding across conversations.

### Key Features

- **🧠 Semantic Memory**: Vector-based memory storage with embeddings using Qdrant
- **🕸️ Knowledge Graphs**: Neo4j-powered graph database for entity relationships and GraphRAG
- **✅ Fact Grounding**: LLM-based verification to ensure factual accuracy
- **🔗 MCP Integration**: Fully compliant Model Context Protocol server for AI agent integration
- **📊 Observability**: Built-in structured logging (Seq), distributed tracing (Jaeger), and OpenTelemetry
- **⚡ High Performance**: Redis caching, connection pooling, and async/await patterns throughout
- **🎯 .NET Aspire**: Cloud-native orchestration and service defaults for microservices

## Architecture

The system is composed of five microservices orchestrated via .NET Aspire:

```
┌─────────────────┐
│   MCP Server    │ ← AI Agents connect here
│   (Gateway)     │
└────────┬────────┘
         │
    ┌────┴─────────────────┐
    │                      │
┌───▼────────┐    ┌───────▼────────┐
│ GraphRAG   │    │  Memory        │
│ Service    │    │  Service       │
│ (Neo4j)    │    │  (Qdrant)      │
└────┬───────┘    └────────┬───────┘
     │                     │
     └──────┬──────────────┘
            │
    ┌───────▼────────┐
    │  Grounding     │
    │  Service       │
    │  (Verification)│
    └────────────────┘
            │
    ┌───────▼────────┐
    │ Orchestration  │
    │   Service      │
    └────────────────┘
```

### Services

- **MCP Server**: API Gateway implementing MCP protocol, handles all client requests
- **GraphRAG Service**: Manages knowledge graph operations with Neo4j (entities, relationships, communities)
- **Memory Service**: Vector memory operations with Qdrant (store, search, consolidate memories)
- **Grounding Service**: Fact-checking and verification using LLMs
- **Orchestration Service**: Coordinates multi-service workflows and complex operations

## Technology Stack

### Core Technologies
- **.NET 9.0** with C# 12
- **.NET Aspire** for orchestration
- **ASP.NET Core** for web APIs

### Data Stores
- **Neo4j 5.15** - Graph database for entities and relationships
- **Qdrant v1.7** - Vector database for semantic embeddings
- **Redis 7.2** - Distributed caching and session state

### AI/ML
- **OpenAI GPT-4** - Language model for reasoning
- **text-embedding-3-large** - Embeddings (3072 dimensions)

### Observability
- **Serilog** - Structured logging
- **Seq** - Log aggregation and search
- **OpenTelemetry** - Distributed tracing
- **Jaeger** - Trace visualization

### Testing
- **xUnit** - Test framework
- **Moq** - Mocking library
- **FluentAssertions** - Assertion library
- **Testcontainers** - Integration testing with containers

## Getting Started

### Prerequisites

- [.NET 9.0 SDK](https://dotnet.microsoft.com/download/dotnet/9.0) (version 9.0.102 or later)
- [Docker Desktop](https://www.docker.com/products/docker-desktop) (for running infrastructure)
- [Git](https://git-scm.com/)
- An [OpenAI API Key](https://platform.openai.com/api-keys)

### Quick Start

1. **Clone the repository**

```bash
git clone https://github.com/your-org/advanced-memory-foxtrot59.git
cd advanced-memory-foxtrot59
```

2. **Configure environment variables**

```bash
cp .env.example .env
# Edit .env and add your OPENAI_API_KEY
```

3. **Start infrastructure services**

```bash
# Linux/macOS
./scripts/deploy/docker-up.sh

# Windows PowerShell
.\scripts\deploy\docker-up.ps1
```

This starts Neo4j, Qdrant, Redis, Seq, and Jaeger in Docker containers.

4. **Build the solution**

```bash
# Linux/macOS
./scripts/build/build-all.sh

# Windows PowerShell
.\scripts\build\build-all.ps1
```

5. **Run tests**

```bash
# Linux/macOS
./scripts/test/run-tests.sh

# Windows PowerShell
.\scripts\test\run-tests.ps1
```

6. **Start the services**

```bash
cd src/AdvancedMemory.AppHost
dotnet run
```

The Aspire dashboard will be available at `http://localhost:18888`

### Service Endpoints

Once running, services are available at:

- **MCP Server**: `http://localhost:8080`
- **GraphRAG Service**: `http://localhost:8081`
- **Memory Service**: `http://localhost:8082`
- **Grounding Service**: `http://localhost:8083`
- **Orchestration Service**: `http://localhost:8084`
- **Dashboard (UI)**: `http://localhost:8090`

### Infrastructure UIs

- **Neo4j Browser**: `http://localhost:7474` (user: `neo4j`, pass: `advancedmemory123`)
- **Seq Logs**: `http://localhost:5342`
- **Jaeger Tracing**: `http://localhost:16686`

## Development

### Project Structure

```
.
├── src/                          # Source code
│   ├── AdvancedMemory.Core/      # Domain models and interfaces
│   ├── AdvancedMemory.Infrastructure/  # Data access implementations
│   ├── AdvancedMemory.Shared/    # Shared utilities
│   ├── AdvancedMemory.ServiceDefaults/  # Aspire service defaults
│   ├── AdvancedMemory.McpServer/ # MCP API Gateway
│   ├── AdvancedMemory.GraphRAGService/  # GraphRAG service
│   ├── AdvancedMemory.MemoryService/    # Memory service
│   ├── AdvancedMemory.GroundingService/ # Grounding service
│   ├── AdvancedMemory.OrchestrationService/  # Orchestration
│   ├── AdvancedMemory.AppHost/   # Aspire orchestration
│   └── AdvancedMemory.Dashboard/ # Blazor WASM UI
├── tests/                        # Test projects
│   ├── AdvancedMemory.Core.Tests/
│   ├── AdvancedMemory.Infrastructure.Tests/
│   ├── AdvancedMemory.GraphRAGService.Tests/
│   ├── AdvancedMemory.MemoryService.Tests/
│   ├── AdvancedMemory.McpServer.Tests/
│   └── AdvancedMemory.E2E.Tests/
├── docker/                       # Dockerfiles for each service
├── scripts/                      # Build, test, and deployment scripts
│   ├── build/                    # Build scripts
│   ├── test/                     # Test scripts
│   └── deploy/                   # Deployment scripts
├── docs/                         # Documentation
├── terraform/                    # Infrastructure as Code
├── docker-compose.yml            # Local development compose file
└── AdvancedMemory.sln            # Solution file
```

### Building

Build the entire solution:

```bash
dotnet build AdvancedMemory.sln
```

Build a specific project:

```bash
dotnet build src/AdvancedMemory.McpServer/AdvancedMemory.McpServer.csproj
```

### Testing

Run all tests:

```bash
dotnet test AdvancedMemory.sln
```

Run tests with coverage:

```bash
./scripts/test/run-tests.sh
# or
.\scripts\test\run-tests.ps1
```

Run a specific test:

```bash
dotnet test --filter "FullyQualifiedName~YourTestName"
```

### Configuration

Services are configured via `appsettings.json` files in each service project. Key configuration sections:

- **ConnectionStrings**: Database connection strings
- **OpenAI**: API key and model settings
- **OpenTelemetry**: Tracing configuration
- **Serilog**: Logging configuration
- **ServiceUrls**: Internal service-to-service URLs

Environment-specific overrides go in `appsettings.Development.json` or `appsettings.Production.json`.

### Docker Compose

For local development without Aspire:

```bash
# Start all services
docker-compose up -d

# View logs
docker-compose logs -f mcp-server

# Stop all services
docker-compose down

# Stop and remove volumes
docker-compose down -v
```

## MCP Protocol Usage

The MCP Server implements the Model Context Protocol for integration with AI agents.

### Connecting to the MCP Server

Configure your AI agent to connect to `http://localhost:8080` with MCP protocol support.

### Available Tools

- `store_memory`: Store a new memory or fact
- `search_memories`: Search memories by semantic similarity
- `create_entity`: Create a knowledge graph entity
- `create_relationship`: Link entities with relationships
- `search_graph`: Query the knowledge graph
- `verify_fact`: Ground-truth check a statement
- `consolidate_memories`: Merge and deduplicate memories

See the [MCP API documentation](docs/api/mcp-api.md) for detailed usage.

## API Documentation

API documentation is available via Swagger/OpenAPI:

- MCP Server: `http://localhost:8080/swagger`
- GraphRAG Service: `http://localhost:8081/swagger`
- Memory Service: `http://localhost:8082/swagger`

## Deployment

### Docker

Build and run with Docker:

```bash
docker-compose up --build -d
```

### Kubernetes

Kubernetes manifests are available in `k8s/`:

```bash
kubectl apply -f k8s/
```

### Terraform

Infrastructure provisioning with Terraform:

```bash
cd terraform/environments/dev
terraform init
terraform plan
terraform apply
```

## Monitoring & Observability

### Structured Logging

All services emit structured logs to **Seq** at `http://localhost:5342`.

Query example:
```
@Level = 'Error' and ServiceName = 'McpServer'
```

### Distributed Tracing

View traces in **Jaeger** at `http://localhost:16686`.

All requests include:
- Trace ID
- Span ID
- Service name
- Operation name

### Metrics

Metrics are exposed via OpenTelemetry and can be collected by Prometheus.

## Contributing

We welcome contributions! Please see [CONTRIBUTING.md](CONTRIBUTING.md) for guidelines.

1. Fork the repository
2. Create a feature branch (`git checkout -b feature/amazing-feature`)
3. Commit your changes (`git commit -m 'Add amazing feature'`)
4. Push to the branch (`git push origin feature/amazing-feature`)
5. Open a Pull Request

## Testing

- **Unit Tests**: Test individual components in isolation
- **Integration Tests**: Test service integrations with Testcontainers
- **E2E Tests**: Test complete workflows across all services

Aim for 80%+ code coverage.

## License

This project is licensed under the MIT License - see the [LICENSE.md](LICENSE.md) file for details.

## Documentation

- [Application Plan](docs/application-plan.md) - Complete implementation plan
- [Architecture](docs/architecture.md) - System architecture and design
- [Tech Stack](docs/tech-stack.md) - Technology decisions
- [Project Structure](docs/project-structure.md) - Directory structure
- [API Documentation](docs/api/) - API reference

## Support

For questions and support:

- **Issues**: [GitHub Issues](https://github.com/your-org/advanced-memory-foxtrot59/issues)
- **Discussions**: [GitHub Discussions](https://github.com/your-org/advanced-memory-foxtrot59/discussions)

## Acknowledgments

Built with:
- [.NET Aspire](https://learn.microsoft.com/en-us/dotnet/aspire/)
- [Neo4j](https://neo4j.com/)
- [Qdrant](https://qdrant.tech/)
- [OpenAI](https://openai.com/)

---

**Status**: 🚧 In Development - Project structure established, implementation in progress
