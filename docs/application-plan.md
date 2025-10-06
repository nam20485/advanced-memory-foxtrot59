# Advanced-Memory2 – Complete Implementation (Application Plan)

## Overview

**Advanced-Memory2** is a cloud-native .NET 9.0 MCP (Model Context Protocol) server that combines GraphRAG (structured knowledge retrieval) with Mem0-inspired agentic memory patterns to provide AI agents with both deep domain knowledge and personalized, evolving memory capabilities. The system includes a verification/grounding layer to reduce hallucinations and a real-time Blazor WebAssembly dashboard for monitoring system activity.

**Problem Statement**: Current AI agents lack persistent, structured memory and domain-specific knowledge retrieval. They cannot remember user preferences across sessions, build relationships between facts, or verify claims against trusted sources.

**Desired Outcomes**:
- Enable AI agents to maintain personalized, evolving memory across sessions
- Provide structured knowledge retrieval via graph-based reasoning
- Reduce hallucinations through fact verification
- Deliver real-time observability into system operations
- Support multi-user, multi-tenant deployments with horizontal scalability

**Supporting Documentation**:
- [Application Template](./ai-new-app-template.md) - Original requirements
- [Technical Report](./Enhanced%20Technical%20Report%20on%20Architecting%20and%20Implementing%20a%20Unified%20Knowledge%20and%20Memory%20Server.md) - Research and design
- [Architecture Documentation](./architecture.md) - High-level system design
- [Technology Stack](./tech-stack.md) - Complete technology inventory
- [Project Structure](./project-structure.md) - Directory organization and conventions

## Goals

- **G1**: Deliver a production-ready MCP server exposing memory, knowledge, and verification tools
- **G2**: Implement GraphRAG for structured knowledge retrieval from document corpus
- **G3**: Build Mem0-inspired memory layer supporting working, episodic, factual, and semantic memory types
- **G4**: Create verification/grounding service to validate factual claims
- **G5**: Provide real-time monitoring via Blazor WebAssembly dashboard
- **G6**: Enable containerized deployment with Docker and infrastructure-as-code via Terraform
- **G7**: Achieve 80%+ test coverage with comprehensive testing strategy
- **G8**: Establish CI/CD pipeline with automated build, test, security scanning, and deployment

## Technology Stack

### Core Technologies
- **Language**: C# 12
- **Framework**: .NET 9.0 (SDK 9.0.102 via global.json)
- **UI Framework**: Blazor WebAssembly (monitoring dashboard)
- **Orchestration**: .NET Aspire (microservices coordination, service discovery, telemetry)
- **API Framework**: ASP.NET Core 9.0 (Minimal APIs, OpenAPI/Swagger)

### AI & Machine Learning
- **LLM Provider**: OpenAI API (GPT-4o-mini, GPT-4o)
- **Embeddings**: text-embedding-3-small (OpenAI)
- **GraphRAG**: Custom .NET implementation inspired by Microsoft's GraphRAG
- **Memory Layer**: Custom Mem0-inspired .NET implementation

### Data Storage
- **Graph Database**: Neo4j (knowledge graph, memory relationships) - Docker or Neo4j AuraDB
- **Vector Database**: Qdrant (semantic search, embeddings) - Docker or Qdrant Cloud
- **Cache**: Redis (distributed caching for multi-instance deployments)

### Architecture Patterns
- **Primary**: Microservices Architecture with Clean Architecture
- **Communication**: REST APIs with Server-Sent Events (SSE) for MCP streaming
- **Protocol**: Model Context Protocol (MCP) for agent-tool communication

### Testing & Quality
- **Unit Testing**: xUnit, Moq, FluentAssertions (target: 80%+ coverage)
- **Integration Testing**: xUnit with WebApplicationFactory, Testcontainers.NET
- **E2E Testing**: Playwright for .NET (Blazor UI), RestSharp (API)
- **Performance Testing**: NBomber, BenchmarkDotNet

### Observability & Monitoring
- **Logging**: Serilog (Console, File, Seq, Application Insights)
- **Telemetry**: OpenTelemetry (Jaeger for tracing, Prometheus for metrics)
- **Monitoring**: Grafana dashboards, Aspire Dashboard (development)
- **Alerting**: Alertmanager

### Infrastructure & DevOps
- **Containerization**: Docker, Docker Compose
- **IaC**: Terraform (multi-provider: Docker, Azure, AWS)
- **CI/CD**: GitHub Actions
- **Code Quality**: Roslyn analyzers, StyleCop, SonarCloud
- **Security**: Dependabot, Snyk/GitHub Security Scanning, OWASP Dependency Check

## Application Features

### Core Capabilities
- **MCP Server**: Expose unified MCP-compliant API with SSE streaming for AI agents
- **GraphRAG Knowledge Service**: 
  - Document ingestion and indexing pipeline
  - Entity and relationship extraction using LLM
  - Community detection and hierarchical summarization
  - Global search (thematic questions) and local search (entity-specific queries)
  - Multi-hop reasoning via graph traversal
- **Mem0 Memory Service**:
  - Working memory (in-session context, ephemeral)
  - Episodic memory (conversation history, past events)
  - Factual memory (user preferences, settings, facts)
  - Semantic memory (learned patterns, generalizations)
  - Hybrid storage: vector search (Qdrant) + graph relationships (Neo4j)
- **Grounding/Verification Service**:
  - Fact-checking of generated claims against trusted corpus
  - Confidence scoring and source citation
  - Contradiction detection
- **Orchestration Layer**:
  - Composite tool patterns for complex workflows
  - Parallel execution of independent operations
  - End-to-end workflows (e.g., comprehensive answer with memory + knowledge + verification)
- **Blazor Dashboard**:
  - Real-time activity feed (memory/knowledge operations)
  - System metrics (request rates, latency, error rates)
  - Interactive knowledge graph visualization
  - Memory timeline explorer (user memory evolution)
  - Service health monitoring

### User Experience
- **Multi-User Support**: Isolated memory contexts per user
- **Real-Time Updates**: SignalR-based live dashboard
- **Observability**: Comprehensive logging, tracing, and metrics
- **Developer Experience**: OpenAPI/Swagger documentation, health checks, Aspire dashboard

## System Architecture

### Core Services

1. **MCP Server (API Gateway)** — Expose MCP protocol endpoints, route requests, handle authentication/authorization, rate limiting
2. **GraphRAG Service** — Index documents, extract entities/relationships, build knowledge graph, answer queries via global/local search
3. **Memory Service** — Store and retrieve user memories, manage memory types, consolidate similar memories
4. **Grounding Service** — Verify factual claims, provide confidence scores, cite sources
5. **Orchestration Service** — Coordinate multi-step workflows, combine memory + knowledge + verification
6. **Blazor Dashboard** — Real-time monitoring UI, graph visualization, system health

### Key System-Level Features

- **MCP Protocol Support**: Server-Sent Events (SSE) streaming, tool registration, request/response handling
- **Clean Architecture**: Separation of concerns across layers (Core, Infrastructure, Services, API)
- **Microservices**: Independent services with REST APIs, service discovery via .NET Aspire
- **Resilience**: Polly-based retry, circuit breaker, timeout, bulkhead patterns
- **Multi-Tenancy**: User-specific data isolation, row-level security in queries
- **Scalability**: Horizontal scaling, connection pooling, multi-level caching
- **Security**: API key authentication, JWT tokens, TLS 1.3, Azure Key Vault integration
- **Observability**: Distributed tracing, structured logging, Prometheus metrics, Grafana dashboards

## Project Structure

```
advanced-memory-foxtrot59/
├── src/
│   ├── AdvancedMemory.AppHost/               # .NET Aspire Orchestration
│   ├── AdvancedMemory.ServiceDefaults/       # Shared Aspire defaults
│   ├── AdvancedMemory.Core/                  # Domain models, interfaces
│   ├── AdvancedMemory.Infrastructure/        # Data access, external services
│   ├── AdvancedMemory.McpServer/             # MCP API Gateway
│   ├── AdvancedMemory.GraphRAGService/       # Knowledge Service
│   ├── AdvancedMemory.MemoryService/         # Memory Service
│   ├── AdvancedMemory.GroundingService/      # Verification Service
│   ├── AdvancedMemory.OrchestrationService/  # Orchestration Layer
│   ├── AdvancedMemory.Dashboard/             # Blazor WebAssembly UI
│   └── AdvancedMemory.Shared/                # Shared utilities
├── tests/
│   ├── AdvancedMemory.Core.Tests/
│   ├── AdvancedMemory.Infrastructure.Tests/
│   ├── AdvancedMemory.GraphRAGService.Tests/
│   ├── AdvancedMemory.MemoryService.Tests/
│   ├── AdvancedMemory.McpServer.Tests/
│   └── AdvancedMemory.E2E.Tests/
├── docs/
│   ├── architecture.md
│   ├── tech-stack.md
│   ├── project-structure.md
│   ├── adr/                                   # Architecture Decision Records
│   ├── api/                                   # API documentation
│   └── guides/                                # How-to guides
├── scripts/                                   # Build/deployment scripts
├── docker/                                    # Dockerfiles and Compose
├── terraform/                                 # Infrastructure as Code
├── .github/workflows/                         # CI/CD pipelines
├── global.json                                # .NET SDK pinning (9.0.102)
├── Directory.Build.props                      # Shared MSBuild properties
└── Directory.Packages.props                   # Central package management
```

**See [project-structure.md](./project-structure.md) for complete directory hierarchy and naming conventions.**

---

## Implementation Plan

### Phase 1: Foundation & Setup
**Duration**: 2-3 weeks  
**Goals**: Establish development environment, project structure, core infrastructure

#### Tasks
- [ ] **1.1. Repository and Solution Bootstrap**
  - [ ] 1.1.1. Create .NET 9.0 solution (`advanced-memory-foxtrot59.sln`)
  - [ ] 1.1.2. Configure `global.json` (SDK 9.0.102, latestFeature rollforward)
  - [ ] 1.1.3. Set up `Directory.Build.props` (C# 12, nullable enabled, treat warnings as errors)
  - [ ] 1.1.4. Set up `Directory.Packages.props` (central package management)
  - [ ] 1.1.5. Configure `.editorconfig` (code style enforcement)
  - [ ] 1.1.6. Create empty project structure (src/, tests/, docs/, scripts/, docker/, terraform/)
  - [ ] 1.1.7. Set up `.gitignore` and `.dockerignore`

- [ ] **1.2. Core Dependencies and Configuration**
  - [ ] 1.2.1. Create `AdvancedMemory.Core` project (domain models, interfaces)
  - [ ] 1.2.2. Define core models: Memory, Entity, Relationship, MCP request/response DTOs
  - [ ] 1.2.3. Define service interfaces: `IGraphRAGService`, `IMemoryService`, `IGroundingService`, `IOrchestrationService`
  - [ ] 1.2.4. Create `AdvancedMemory.Infrastructure` project
  - [ ] 1.2.5. Add NuGet packages: Neo4j.Driver, Qdrant.Client, Azure.AI.OpenAI, Serilog, Polly
  - [ ] 1.2.6. Configure dependency injection extensions

- [ ] **1.3. Database Foundation**
  - [ ] 1.3.1. Create `docker-compose.yml` with Neo4j, Qdrant, Redis, Seq, Jaeger
  - [ ] 1.3.2. Implement Neo4j connection factory and base repository
  - [ ] 1.3.3. Implement Qdrant connection factory and base repository
  - [ ] 1.3.4. Implement Redis cache service
  - [ ] 1.3.5. Test database connectivity with integration tests

- [ ] **1.4. .NET Aspire Orchestration Setup**
  - [ ] 1.4.1. Create `AdvancedMemory.AppHost` project
  - [ ] 1.4.2. Create `AdvancedMemory.ServiceDefaults` project
  - [ ] 1.4.3. Configure service discovery and health checks
  - [ ] 1.4.4. Set up Aspire dashboard for development
  - [ ] 1.4.5. Configure OpenTelemetry integration (tracing, metrics)

- [ ] **1.5. AI Service Integration**
  - [ ] 1.5.1. Implement OpenAI client factory (chat completions, embeddings)
  - [ ] 1.5.2. Create prompt templates and builders
  - [ ] 1.5.3. Implement embedding service with caching
  - [ ] 1.5.4. Test LLM connectivity and token usage tracking

### Phase 2: Core Services Implementation
**Duration**: 4-5 weeks  
**Goals**: Build GraphRAG, Memory, and Grounding services

#### Tasks
- [ ] **2.1. GraphRAG Service - Indexing Pipeline**
  - [ ] 2.1.1. Create `AdvancedMemory.GraphRAGService` project
  - [ ] 2.1.2. Implement document ingestion (support .txt, .csv, .json)
  - [ ] 2.1.3. Implement text chunking (512-1024 token chunks with overlap)
  - [ ] 2.1.4. Implement entity extraction using LLM (GPT-4o-mini)
  - [ ] 2.1.5. Implement relationship extraction between entities
  - [ ] 2.1.6. Build knowledge graph in Neo4j (nodes: Entity, Chunk; edges: EXTRACTED_FROM, RELATED_TO)
  - [ ] 2.1.7. Generate embeddings for chunks and store in Qdrant
  - [ ] 2.1.8. Implement community detection (graph algorithms)
  - [ ] 2.1.9. Generate hierarchical community summaries
  - [ ] 2.1.10. Add indexing status tracking and progress reporting

- [ ] **2.2. GraphRAG Service - Query Engine**
  - [ ] 2.2.1. Implement global search (community summary-based)
  - [ ] 2.2.2. Implement local search (entity-specific via graph traversal)
  - [ ] 2.2.3. Implement multi-hop reasoning (follow relationships across nodes)
  - [ ] 2.2.4. Build query planner (automatic global vs. local selection)
  - [ ] 2.2.5. Integrate Redis caching for frequent queries
  - [ ] 2.2.6. Add query performance metrics and optimization

- [ ] **2.3. Memory Service - Storage & Operations**
  - [ ] 2.3.1. Create `AdvancedMemory.MemoryService` project
  - [ ] 2.3.2. Implement memory data model (working, episodic, factual, semantic types)
  - [ ] 2.3.3. Build hybrid storage: Qdrant (vectors) + Neo4j (relationships)
  - [ ] 2.3.4. Implement memory addition with entity extraction
  - [ ] 2.3.5. Implement memory search with multi-factor scoring (relevance + recency + importance)
  - [ ] 2.3.6. Build user profile management (Redis-backed)
  - [ ] 2.3.7. Implement memory update and deletion operations
  - [ ] 2.3.8. Create memory consolidation service (merge similar memories)
  - [ ] 2.3.9. Add memory type-specific logic (working: TTL, episodic: timestamp-based, etc.)

- [ ] **2.4. Grounding/Verification Service**
  - [ ] 2.4.1. Create `AdvancedMemory.GroundingService` project
  - [ ] 2.4.2. Build trusted corpus indexing (lightweight RAG)
  - [ ] 2.4.3. Implement claim extraction from text (using LLM)
  - [ ] 2.4.4. Build fact verification via vector search against corpus
  - [ ] 2.4.5. Implement confidence scoring algorithm
  - [ ] 2.4.6. Add source citation and reference tracking
  - [ ] 2.4.7. Detect contradictions between claims and trusted data

- [ ] **2.5. Orchestration Service**
  - [ ] 2.5.1. Create `AdvancedMemory.OrchestrationService` project
  - [ ] 2.5.2. Implement workflow engine (coordinate service calls)
  - [ ] 2.5.3. Build `ComprehensiveAnswerWorkflow` (memory + knowledge + verification)
  - [ ] 2.5.4. Implement parallel execution for independent service calls
  - [ ] 2.5.5. Add workflow observability (trace context propagation)

### Phase 3: MCP Server & API Integration
**Duration**: 2-3 weeks  
**Goals**: Expose MCP protocol endpoints, build Blazor dashboard

#### Tasks
- [ ] **3.1. MCP Server - API Gateway**
  - [ ] 3.1.1. Create `AdvancedMemory.McpServer` project
  - [ ] 3.1.2. Implement MCP protocol SSE endpoint (`POST /mcp/sse`)
  - [ ] 3.1.3. Build tool registration and dispatch system
  - [ ] 3.1.4. Implement request routing to appropriate services
  - [ ] 3.1.5. Add authentication (API key) and authorization
  - [ ] 3.1.6. Implement rate limiting per user/API key
  - [ ] 3.1.7. Configure OpenAPI/Swagger documentation
  - [ ] 3.1.8. Add health check endpoints (`/health`, `/health/ready`)
  - [ ] 3.1.9. Implement Prometheus metrics endpoint (`/metrics`)

- [ ] **3.2. MCP Server - Middleware & Cross-Cutting Concerns**
  - [ ] 3.2.1. Implement exception handling middleware
  - [ ] 3.2.2. Add request/response logging middleware
  - [ ] 3.2.3. Configure CORS policies
  - [ ] 3.2.4. Set up Polly resilience policies (retry, circuit breaker, timeout)
  - [ ] 3.2.5. Implement request validation with FluentValidation

- [ ] **3.3. Blazor WebAssembly Dashboard - Foundation**
  - [ ] 3.3.1. Create `AdvancedMemory.Dashboard` project (Blazor WASM)
  - [ ] 3.3.2. Set up layout and navigation (MainLayout, NavMenu)
  - [ ] 3.3.3. Implement SignalR client for real-time updates
  - [ ] 3.3.4. Create dashboard service (API client for backend)

- [ ] **3.4. Blazor WebAssembly Dashboard - Components**
  - [ ] 3.4.1. Build `ActivityFeed.razor` (real-time operation log)
  - [ ] 3.4.2. Build `GraphViewer.razor` (Neo4j graph visualization)
  - [ ] 3.4.3. Build `MemoryExplorer.razor` (user memory browser)
  - [ ] 3.4.4. Build `SystemHealth.razor` (service health dashboard)
  - [ ] 3.4.5. Add chart visualizations (Chart.js or Plotly.NET)

- [ ] **3.5. Integration & Testing**
  - [ ] 3.5.1. Integrate all services with Aspire orchestration
  - [ ] 3.5.2. Test end-to-end flows (document indexing → query → memory storage)
  - [ ] 3.5.3. Verify MCP protocol compliance
  - [ ] 3.5.4. Validate dashboard real-time updates

### Phase 4: Advanced Capabilities & Security
**Duration**: 2-3 weeks  
**Goals**: Optimize performance, enhance security, add advanced features

#### Tasks
- [ ] **4.1. Performance Optimizations**
  - [ ] 4.1.1. Implement multi-level caching (L1: in-memory, L2: Redis, L3: DB)
  - [ ] 4.1.2. Optimize database queries (Neo4j indexing, Cypher optimization)
  - [ ] 4.1.3. Add connection pooling for all external services
  - [ ] 4.1.4. Implement async/await best practices throughout
  - [ ] 4.1.5. Profile and optimize hot paths (BenchmarkDotNet)
  - [ ] 4.1.6. Add batch processing for bulk operations

- [ ] **4.2. Security Hardening**
  - [ ] 4.2.1. Integrate Azure Key Vault for secrets management
  - [ ] 4.2.2. Implement JWT token-based service-to-service authentication
  - [ ] 4.2.3. Add input sanitization (prevent injection attacks)
  - [ ] 4.2.4. Configure TLS 1.3 for all connections
  - [ ] 4.2.5. Implement audit logging for sensitive operations
  - [ ] 4.2.6. Add GDPR compliance features (right to erasure)

- [ ] **4.3. Observability Enhancements**
  - [ ] 4.3.1. Create Grafana dashboards (request rates, latency, errors)
  - [ ] 4.3.2. Configure Jaeger for distributed tracing
  - [ ] 4.3.3. Add custom business metrics (memory operations per user, etc.)
  - [ ] 4.3.4. Set up Alertmanager rules (high error rates, low uptime, etc.)
  - [ ] 4.3.5. Implement Application Insights integration (production telemetry)

- [ ] **4.4. Advanced Features**
  - [ ] 4.4.1. Add multi-user isolation enforcement (tenant-specific labels)
  - [ ] 4.4.2. Implement memory expiration policies (configurable TTL)
  - [ ] 4.4.3. Build feedback loop for memory importance scoring
  - [ ] 4.4.4. Add support for custom entity types in GraphRAG
  - [ ] 4.4.5. Create admin API for system management

### Phase 5: Testing, Documentation, Packaging & Deployment
**Duration**: 3-4 weeks  
**Goals**: Achieve test coverage targets, complete documentation, deploy to environments

#### Tasks
- [ ] **5.1. Comprehensive Test Suites**
  - [ ] 5.1.1. Write unit tests for all services (target: 80%+ coverage)
  - [ ] 5.1.2. Create integration tests using Testcontainers.NET
  - [ ] 5.1.3. Build E2E tests for critical workflows (Playwright for Blazor)
  - [ ] 5.1.4. Add performance/load tests (NBomber)
  - [ ] 5.1.5. Generate code coverage reports (Coverlet)
  - [ ] 5.1.6. Integrate tests into CI pipeline

- [ ] **5.2. API & Developer Documentation**
  - [ ] 5.2.1. Write comprehensive README.md (overview, quick start)
  - [ ] 5.2.2. Document MCP API with examples
  - [ ] 5.2.3. Create architecture decision records (ADRs)
  - [ ] 5.2.4. Write API reference docs for each service
  - [ ] 5.2.5. Build developer guide (local setup, debugging)
  - [ ] 5.2.6. Create troubleshooting guide and FAQ
  - [ ] 5.2.7. Add XML comments to all public APIs

- [ ] **5.3. Containerization & Packaging**
  - [ ] 5.3.1. Create Dockerfiles for all services
  - [ ] 5.3.2. Build multi-stage Dockerfiles (optimize image size)
  - [ ] 5.3.3. Create docker-compose.yml (full stack)
  - [ ] 5.3.4. Create docker-compose.dev.yml (development overrides)
  - [ ] 5.3.5. Test container builds and orchestration
  - [ ] 5.3.6. Push images to container registry (GitHub Container Registry)

- [ ] **5.4. Infrastructure as Code & CI/CD**
  - [ ] 5.4.1. Create Terraform modules (networking, compute, database, monitoring)
  - [ ] 5.4.2. Build Terraform configurations for Docker (local)
  - [ ] 5.4.3. Build Terraform configurations for Azure
  - [ ] 5.4.4. Build Terraform configurations for AWS
  - [ ] 5.4.5. Create GitHub Actions workflows:
    - [ ] 5.4.5.1. CI: Build & Test (on PR)
    - [ ] 5.4.5.2. CI: Security scanning (Dependabot, Snyk)
    - [ ] 5.4.5.3. CI: Code analysis (SonarCloud)
    - [ ] 5.4.5.4. CD: Deploy to dev (auto on merge to main)
    - [ ] 5.4.5.5. CD: Deploy to staging (manual approval)
    - [ ] 5.4.5.6. CD: Deploy to production (tag-based)
  - [ ] 5.4.6. Test deployment pipelines end-to-end

- [ ] **5.5. Final Hardening & Release Checklist**
  - [ ] 5.5.1. Conduct security review and penetration testing
  - [ ] 5.5.2. Perform load testing (validate performance targets)
  - [ ] 5.5.3. Review and optimize resource usage (CPU, memory, storage)
  - [ ] 5.5.4. Validate all health checks and monitoring
  - [ ] 5.5.5. Create runbooks for operational procedures
  - [ ] 5.5.6. Conduct disaster recovery testing (backup/restore)
  - [ ] 5.5.7. Prepare release notes and changelog
  - [ ] 5.5.8. Tag release (v1.0.0)

---

## Mandatory Requirements Implementation

### Testing & Quality Assurance

- [ ] **Unit Tests**
  - Coverage target: **80%+**
  - All service classes have corresponding test classes
  - Mock external dependencies (Neo4j, Qdrant, OpenAI)
  - Use FluentAssertions for readable assertions

- [ ] **Integration Tests**
  - Use Testcontainers.NET for Neo4j, Qdrant, Redis
  - Test actual database operations
  - Validate API contracts with WebApplicationFactory
  - Test service-to-service communication

- [ ] **End-to-End Tests**
  - Use Playwright for Blazor UI testing
  - Test critical user workflows (indexing → query → memory)
  - Validate MCP protocol end-to-end
  - Test real-time dashboard updates

- [ ] **Performance/Load Tests**
  - Use NBomber for load testing
  - Validate latency targets (< 500ms simple, < 2s complex)
  - Test throughput (100+ requests/second)
  - Test concurrency (50+ concurrent users)

- [ ] **Automated Tests in CI**
  - Run all tests on every PR
  - Fail build if tests fail or coverage drops below 80%
  - Generate coverage reports (publish to CI artifacts)

### Documentation & UX

- [ ] **Comprehensive README**
  - Project overview and vision
  - Quick start guide (Docker Compose setup)
  - Architecture summary
  - Contribution guidelines

- [ ] **User Manual & Feature Docs**
  - MCP server usage guide
  - How to index documents
  - How to query knowledge
  - How to manage memories
  - Dashboard user guide

- [ ] **XML/API Docs (Public APIs)**
  - All public classes and methods have XML comments
  - Generate API docs with DocFX
  - Publish to GitHub Pages

- [ ] **Troubleshooting/FAQ**
  - Common issues and solutions
  - Performance tuning guide
  - Debugging tips

- [ ] **In-App Help**
  - Blazor dashboard includes help tooltips
  - Link to documentation from UI

### Build & Distribution

- [ ] **Build Scripts**
  - Cross-platform scripts (bash + PowerShell)
  - `build-all.sh/ps1` - Build entire solution
  - `clean.sh/ps1` - Clean build artifacts
  - `run-all-tests.sh/ps1` - Execute all tests

- [ ] **Containerization Support**
  - Dockerfiles for all services
  - Multi-stage builds (optimize size)
  - docker-compose.yml (full stack)
  - Base images: mcr.microsoft.com/dotnet/aspnet:9.0, sdk:9.0

- [ ] **Release Pipeline**
  - GitHub Actions workflow for releases
  - Semantic versioning (v1.0.0)
  - Automated changelog generation
  - Container image tagging (latest, version-specific)

### Infrastructure & DevOps

- [ ] **CI/CD Workflows**
  - **Build/Test**: Run on every PR, fail on errors
  - **Security Scanning**: Dependabot, Snyk, OWASP checks
  - **Code Analysis**: SonarCloud integration
  - **Deployment**: Auto-deploy to dev, manual to staging/prod

- [ ] **Static Analysis & Security Scanning**
  - Roslyn analyzers, StyleCop (enforce code style)
  - SonarCloud (code quality, security hotspots)
  - Dependabot (dependency updates)
  - Snyk or GitHub Security Scanning (vulnerability scanning)

- [ ] **Performance Benchmarking/Monitoring**
  - BenchmarkDotNet for micro-benchmarks
  - NBomber for load tests
  - Application Insights (production monitoring)
  - Grafana dashboards (request rates, latency, errors)

---

## Acceptance Criteria

### Functional Requirements
- [ ] **AC-1**: MCP server successfully handles SSE streaming requests and routes to appropriate services
- [ ] **AC-2**: GraphRAG indexing pipeline processes documents and builds knowledge graph in Neo4j
- [ ] **AC-3**: GraphRAG query engine returns relevant results via global and local search
- [ ] **AC-4**: Memory service stores and retrieves memories across all four types (working, episodic, factual, semantic)
- [ ] **AC-5**: Grounding service verifies claims and returns confidence scores with sources
- [ ] **AC-6**: Orchestration layer executes comprehensive answer workflow (memory + knowledge + verification)
- [ ] **AC-7**: Blazor dashboard displays real-time activity and system metrics

### Non-Functional Requirements
- [ ] **AC-8**: System achieves < 500ms latency for simple queries, < 2s for complex queries
- [ ] **AC-9**: Test coverage reaches 80%+ across all projects
- [ ] **AC-10**: All services successfully deploy via Docker Compose and Terraform
- [ ] **AC-11**: CI/CD pipeline builds, tests, scans, and deploys automatically
- [ ] **AC-12**: Observability stack (Serilog, OpenTelemetry, Grafana) captures actionable signals
- [ ] **AC-13**: Security controls validated (TLS, authentication, secrets management, input validation)
- [ ] **AC-14**: Documentation complete, accurate, and published (README, API docs, guides, ADRs)

---

## Risk Mitigation Strategies

| Risk | Impact | Probability | Mitigation |
|------|--------|-------------|------------|
| **Neo4j performance degradation with large graphs** | High | Medium | Implement database indexing, query optimization, caching strategy; monitor query performance; consider sharding for scale |
| **OpenAI API rate limits or cost overruns** | High | Medium | Implement aggressive caching; use cheaper models (GPT-4o-mini) for most tasks; add retry with exponential backoff; monitor token usage |
| **Memory consolidation complexity** | Medium | High | Start with simple similarity-based merging; defer advanced consolidation to Phase 3; use LLM for semantic similarity |
| **MCP protocol compliance issues** | High | Low | Study MCP spec thoroughly; validate with reference implementations; build comprehensive integration tests |
| **Testcontainers performance in CI** | Medium | Medium | Use GitHub Actions caching for Docker images; consider hosted test databases for CI; optimize test parallelization |
| **GraphRAG community detection performance** | Medium | Medium | Use existing graph algorithm libraries (Neo4j Graph Data Science); implement incremental community updates; defer full recomputation |
| **Multi-tenant data isolation bugs** | High | Medium | Implement comprehensive integration tests for tenant isolation; use database-level security; conduct security review |
| **Blazor WASM bundle size** | Low | Medium | Use lazy loading for routes; trim unused code; compress assets; monitor bundle size in CI |
| **Infrastructure drift between environments** | Medium | Low | Use IaC (Terraform) for all environments; validate Terraform plans in PR; implement environment parity testing |
| **Documentation becoming outdated** | Medium | High | Integrate doc updates into Definition of Done; use automated API doc generation; conduct quarterly doc reviews |

---

## Timeline Estimate

### Development Schedule
- **Phase 1 (Foundation & Setup)**: 2-3 weeks
- **Phase 2 (Core Services)**: 4-5 weeks
- **Phase 3 (MCP Server & Dashboard)**: 2-3 weeks
- **Phase 4 (Advanced Capabilities)**: 2-3 weeks
- **Phase 5 (Testing & Deployment)**: 3-4 weeks

**Total Estimated Duration**: **13-18 weeks** (approximately 3-4.5 months)

### Assumptions
- Full-time dedicated development effort (1-2 engineers)
- Familiarity with .NET, C#, Neo4j, and Qdrant
- Access to OpenAI API and cloud infrastructure
- Minimal scope changes during development

### Milestones
1. **M1 (Week 3)**: Foundation complete, databases running, Aspire orchestration functional
2. **M2 (Week 8)**: Core services implemented (GraphRAG, Memory, Grounding)
3. **M3 (Week 11)**: MCP server and Blazor dashboard deployed, end-to-end flows working
4. **M4 (Week 14)**: Performance optimized, security hardened, observability complete
5. **M5 (Week 18)**: All tests passing, documentation complete, production deployment ready

---

## Success Metrics

### Technical Metrics
- **Test Coverage**: ≥ 80% across all projects
- **API Latency**: < 500ms (p95) for simple queries, < 2s (p95) for complex queries
- **Throughput**: 100+ requests/second per instance
- **Uptime**: 99.9% availability (production)
- **Build Success Rate**: ≥ 95% (CI pipeline)

### Quality Metrics
- **Code Quality**: SonarCloud rating A or higher
- **Security**: Zero high/critical vulnerabilities in production
- **Documentation**: 100% of public APIs have XML comments
- **User Satisfaction**: Positive feedback from early adopters (internal testing)

### Business Metrics
- **Time to First Query**: < 5 minutes from deployment
- **Developer Onboarding**: New developer productive in < 1 day
- **Operational Overhead**: < 2 hours/week for maintenance
- **Cost Efficiency**: Stay within $500/month budget for dev/staging environments

---

## Repository Branch

**Target Branch for Implementation**: `main` (via feature branches)

**Development Workflow**:
- Feature branches: `feature/[short-description]` (e.g., `feature/graphrag-indexing`)
- Bug fixes: `fix/[short-description]`
- Releases: `release/v1.0.0`

**PR Requirements**:
- All tests passing
- Code coverage ≥ 80%
- Security scan clean
- Code review approval
- Documentation updated

---

## Implementation Notes

### Key Assumptions
1. **Neo4j**: Using community edition for development; production may require Enterprise for clustering
2. **OpenAI API**: Assuming access to GPT-4o-mini and text-embedding-3-small; fallback to Azure OpenAI if needed
3. **Hosting**: Designed for containerized deployment (Docker, Kubernetes, Azure Container Apps)
4. **.NET 9.0**: Leveraging latest features (minimal APIs, improved performance, Aspire)

### Design Decisions
Refer to Architecture Decision Records (ADRs) in `docs/adr/`:
- **ADR-001**: Choice of .NET over Python (performance, type safety, enterprise tooling)
- **ADR-002**: Neo4j as primary graph database (maturity, Cypher query language)
- **ADR-003**: Qdrant for vector storage (performance, .NET support, Docker deployment)
- **ADR-004**: MCP protocol for agent communication (standardization, streaming, observability)
- **ADR-005**: Clean Architecture pattern (separation of concerns, testability)
- **ADR-006**: .NET Aspire for orchestration (service discovery, telemetry, developer experience)

### References
- [Microsoft GraphRAG Research Paper](https://www.microsoft.com/en-us/research/project/graphrag/)
- [Mem0 Architecture Documentation](https://github.com/mem0ai/mem0)
- [Model Context Protocol Specification](https://modelcontextprotocol.io/)
- [Neo4j .NET Driver Documentation](https://neo4j.com/docs/dotnet-manual/current/)
- [.NET Aspire Documentation](https://learn.microsoft.com/en-us/dotnet/aspire/)
- [Clean Architecture (Robert C. Martin)](https://blog.cleancoder.com/uncle-bob/2012/08/13/the-clean-architecture.html)

### Adaptations from Original Template
The application template specified using Python with Neo4j. This implementation uses **ASP.NET Core** instead of Python, as explicitly requested in the template. All other requirements remain aligned:
- GraphRAG + Mem0 architecture preserved
- MCP server protocol implemented
- Blazor WebAssembly dashboard for monitoring
- .NET Aspire for microservices orchestration
- Multi-provider Terraform deployment (Docker, Azure, AWS)

---

**This plan provides a comprehensive roadmap for delivering Advanced-Memory2 as a production-ready MCP server with GraphRAG knowledge retrieval, Mem0-inspired memory, verification, and real-time monitoring capabilities.**
