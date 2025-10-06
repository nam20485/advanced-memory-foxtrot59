# Technology Stack - Advanced-Memory2

## Overview
This document outlines the complete technology stack for the Advanced-Memory2 project, which implements an MCP server-based advanced memory, recall, verification, and reasoning system using .NET.

## Core Technologies

### Language & Runtime
- **Language**: C# 12
- **Framework**: .NET 9.0
- **SDK Version**: 9.0.102 (specified in global.json)
- **Roll Forward Policy**: latestFeature

### Primary Frameworks

#### .NET Aspire
- **Purpose**: Cloud-native orchestration and service management
- **Version**: Latest stable for .NET 9
- **Role**: Orchestrates microservices, manages service discovery, and provides telemetry integration
- **Community Toolkit**: .NET Aspire Community Toolkit for extended capabilities

#### ASP.NET Core Web API
- **Purpose**: RESTful API services for MCP server endpoints
- **Version**: .NET 9.0
- **Features**:
  - Minimal APIs for lightweight endpoints
  - OpenAPI/Swagger documentation
  - MCP protocol implementation via Server-Sent Events (SSE)
  - Service-oriented architecture with separate ApiServices

#### Blazor WebAssembly
- **Purpose**: Interactive web UI for monitoring system activity
- **Version**: .NET 9.0
- **Features**:
  - Real-time dashboard showing memory/recall/reasoning activity
  - WebAssembly client-side execution
  - SignalR integration for real-time updates

## Data Storage & Persistence

### Graph Database
- **Primary**: Neo4j
- **Purpose**: 
  - Knowledge graph storage (GraphRAG)
  - Memory relationship tracking (Mem0)
  - Entity-relationship modeling
- **Deployment**: Docker container or Neo4j AuraDB (cloud)
- **.NET Driver**: Neo4j.Driver (official .NET driver)

### Vector Database
- **Primary**: Qdrant
- **Purpose**: 
  - Semantic search for memories
  - Embedding storage for RAG
  - Fast similarity search
- **Deployment**: Docker container or Qdrant Cloud
- **.NET Client**: Qdrant.Client

### Alternative Vector Storage
- **Secondary Options**: 
  - Azure AI Search (for cloud deployments)
  - FAISS.NET (for embedded scenarios)

## AI & Machine Learning

### Language Models
- **Primary Provider**: OpenAI API
- **Models**:
  - GPT-4o-mini (cost-effective reasoning)
  - GPT-4o (complex reasoning tasks)
  - text-embedding-3-small (embeddings)
- **.NET Client**: Azure.AI.OpenAI or OpenAI SDK for .NET

### Alternative Providers
- **Azure OpenAI Service**: For enterprise deployments
- **Local Models**: ONNX Runtime for on-premises scenarios

### GraphRAG Implementation
- **Approach**: Custom .NET implementation inspired by Microsoft's GraphRAG
- **Components**:
  - Entity extraction using LLM
  - Relationship extraction
  - Community detection (port of Leiden algorithm or use existing .NET graph libraries)
  - Hierarchical summarization

### Memory Layer (Mem0)
- **Approach**: Custom .NET implementation of Mem0 patterns
- **Features**:
  - Working memory (in-session context)
  - Episodic memory (conversation history)
  - Factual memory (user preferences, facts)
  - Semantic memory (learned patterns)

## Testing Frameworks

### Unit Testing
- **Framework**: xUnit
- **Mocking**: Moq
- **Assertions**: FluentAssertions
- **Coverage Target**: 80%+

### Integration Testing
- **Framework**: xUnit with WebApplicationFactory
- **Tools**: Testcontainers.NET (for Neo4j, Qdrant containers)

### End-to-End Testing
- **Framework**: Playwright for .NET (Blazor UI testing)
- **API Testing**: RestSharp or built-in HttpClient

### Performance Testing
- **Framework**: NBomber (load testing)
- **Profiling**: dotTrace, BenchmarkDotNet

## Logging & Observability

### Logging
- **Primary**: Serilog
- **Sinks**:
  - Console (development)
  - File (structured JSON logs)
  - Seq (centralized log aggregation)
  - Application Insights (production)

### Telemetry
- **Framework**: OpenTelemetry
- **Exporters**:
  - Jaeger (distributed tracing)
  - Prometheus (metrics)
  - Application Insights

### Monitoring
- **Dashboard**: Grafana
- **Metrics**: Prometheus
- **Alerts**: Alertmanager

## Containerization & Orchestration

### Containerization
- **Runtime**: Docker
- **Multi-Container**: Docker Compose
- **Base Images**:
  - mcr.microsoft.com/dotnet/aspnet:9.0 (runtime)
  - mcr.microsoft.com/dotnet/sdk:9.0 (build)

### Container Orchestration
- **Local/Dev**: Docker Compose
- **Production Options**:
  - Kubernetes (via Helm charts)
  - Azure Container Apps
  - AWS ECS/Fargate

## Infrastructure as Code

### Terraform
- **Purpose**: Multi-provider infrastructure deployment
- **Providers**:
  1. Docker (local development)
  2. Azure (cloud deployment)
  3. AWS (alternative cloud)
- **Modules**:
  - Networking
  - Container registry
  - Compute resources
  - Database provisioning
  - Monitoring stack

## CI/CD & DevOps

### Version Control
- **Platform**: GitHub
- **Repository**: nam20485/advanced-memory-foxtrot59

### CI/CD Pipeline (GitHub Actions)
- **Workflows**:
  - Build & Test (on PR)
  - Code Analysis & Security Scanning
  - Container Build & Push
  - Deployment (staging/production)

### Code Quality Tools
- **Linting**: Roslyn analyzers, StyleCop
- **Code Analysis**: SonarCloud
- **Security Scanning**: 
  - Dependabot (dependency updates)
  - Snyk or GitHub Security Scanning
  - OWASP Dependency Check

## Development Tools

### Package Management
- **Primary**: NuGet
- **Package Sources**: nuget.org, private feeds (if needed)

### API Documentation
- **OpenAPI/Swagger**: Swashbuckle.AspNetCore
- **Documentation Generation**: DocFX (for developer docs)

### Configuration Management
- **Local**: appsettings.json, User Secrets
- **Production**: Azure Key Vault, environment variables
- **Secrets**: .NET Secret Manager (development)

## Key NuGet Packages

### Core Dependencies
- `Microsoft.AspNetCore.App` (metapackage)
- `Microsoft.NET.Sdk.Web`
- `Aspire.Hosting` (orchestration)

### Database & Storage
- `Neo4j.Driver` (graph database)
- `Qdrant.Client` (vector database)

### AI & ML
- `Azure.AI.OpenAI` or `OpenAI`
- Custom GraphRAG implementation

### Testing
- `xunit`
- `xunit.runner.visualstudio`
- `Moq`
- `FluentAssertions`
- `Testcontainers`
- `Microsoft.AspNetCore.Mvc.Testing`

### Logging & Observability
- `Serilog.AspNetCore`
- `Serilog.Sinks.Console`
- `Serilog.Sinks.File`
- `Serilog.Sinks.Seq`
- `OpenTelemetry.Exporter.Jaeger`
- `OpenTelemetry.Exporter.Prometheus`

### Utilities
- `Polly` (resilience and transient fault handling)
- `FluentValidation` (input validation)
- `AutoMapper` (object mapping)
- `MediatR` (CQRS pattern, if needed)

## Architecture Patterns

### Design Patterns
- **Clean Architecture**: Separation of concerns across layers
- **CQRS**: For complex query/command scenarios (optional)
- **Repository Pattern**: Data access abstraction
- **Service Layer**: Business logic encapsulation
- **MCP Protocol**: Tool/service communication standard

### API Design
- **REST**: Primary API paradigm
- **Server-Sent Events (SSE)**: For MCP streaming
- **GraphQL**: (Optional, for flexible querying)

## Security

### Authentication & Authorization
- **Framework**: ASP.NET Core Identity (if user management needed)
- **JWT Tokens**: For API authentication
- **API Keys**: For MCP server access

### Secrets Management
- **Development**: .NET User Secrets
- **Production**: Azure Key Vault or AWS Secrets Manager

### Network Security
- **HTTPS**: Enforced via Kestrel configuration
- **CORS**: Configured per deployment environment

## Performance Optimization

### Caching
- **In-Memory**: IMemoryCache (ASP.NET Core)
- **Distributed**: Redis (for multi-instance deployments)
- **Strategy**: Cache frequently accessed knowledge/memory queries

### Connection Pooling
- **Database**: Connection pooling for Neo4j and Qdrant
- **HTTP**: HttpClientFactory for external API calls

## Development Environment

### Supported IDEs
- Visual Studio 2022 (v17.8+)
- Visual Studio Code with C# Dev Kit
- JetBrains Rider 2024.1+

### Required Tools
- .NET 9.0 SDK
- Docker Desktop
- Git
- Node.js & npm (for Blazor tooling)

## Documentation Standards

### Code Documentation
- **XML Comments**: For public APIs
- **Markdown**: README files, architecture docs
- **OpenAPI**: Auto-generated API documentation

### Architecture Documentation
- **ADRs**: Architecture Decision Records (docs/adr/)
- **Diagrams**: C4 model diagrams (using Structurizr or similar)

## Versioning Strategy

### Semantic Versioning
- **Format**: MAJOR.MINOR.PATCH
- **Pre-release**: -alpha, -beta, -rc suffixes

### Assembly Versioning
- **AssemblyVersion**: Increment on breaking changes
- **FileVersion**: Build-specific version
- **InformationalVersion**: Full semantic version with metadata

## Compliance & Standards

### Code Style
- **Standard**: Microsoft C# Coding Conventions
- **Enforcement**: .editorconfig, Roslyn analyzers

### Accessibility
- **WCAG 2.1**: Level AA compliance for Blazor UI

### Licensing
- **Project License**: MIT (or as specified)
- **Dependency Review**: Regular review of third-party licenses
