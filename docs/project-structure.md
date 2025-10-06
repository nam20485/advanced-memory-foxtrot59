# Project Structure - Advanced-Memory2

## Overview
This document defines the complete directory structure, file organization, and naming conventions for the Advanced-Memory2 project.

## Root Directory Structure

```
advanced-memory-foxtrot59/
├── src/                          # Source code
├── tests/                        # All test projects
├── docs/                         # Documentation
├── scripts/                      # Build and deployment scripts
├── docker/                       # Docker-related files
├── terraform/                    # Infrastructure as Code
├── .github/                      # GitHub workflows and templates
├── assets/                       # Static assets (images, etc.)
├── global.json                   # .NET SDK version pinning
├── Directory.Build.props         # Shared MSBuild properties
├── Directory.Packages.props      # Centralized package versioning
├── .editorconfig                 # Code style configuration
├── .gitignore                    # Git ignore rules
├── .dockerignore                 # Docker ignore rules
├── README.md                     # Project overview
├── LICENSE.md                    # License information
└── advanced-memory-foxtrot59.sln # Solution file
```

## src/ - Source Code Structure

```
src/
├── AdvancedMemory.AppHost/               # .NET Aspire Orchestration
│   ├── Program.cs
│   ├── appsettings.json
│   └── AdvancedMemory.AppHost.csproj
│
├── AdvancedMemory.ServiceDefaults/       # Shared Aspire defaults
│   ├── Extensions.cs
│   └── AdvancedMemory.ServiceDefaults.csproj
│
├── AdvancedMemory.Core/                  # Domain models and interfaces
│   ├── Models/
│   │   ├── Memory/
│   │   │   ├── Memory.cs
│   │   │   ├── MemoryType.cs
│   │   │   ├── UserProfile.cs
│   │   │   └── MemoryAddResult.cs
│   │   ├── Knowledge/
│   │   │   ├── Entity.cs
│   │   │   ├── Relationship.cs
│   │   │   ├── KnowledgeResponse.cs
│   │   │   └── SearchType.cs
│   │   ├── MCP/
│   │   │   ├── McpRequest.cs
│   │   │   ├── McpResponse.cs
│   │   │   └── ToolCall.cs
│   │   └── Common/
│   │       ├── Result.cs
│   │       └── PagedResult.cs
│   ├── Interfaces/
│   │   ├── IGraphRAGService.cs
│   │   ├── IMemoryService.cs
│   │   ├── IGroundingService.cs
│   │   └── IOrchestrationService.cs
│   ├── Exceptions/
│   │   ├── MemoryException.cs
│   │   ├── KnowledgeException.cs
│   │   └── ValidationException.cs
│   └── AdvancedMemory.Core.csproj
│
├── AdvancedMemory.Infrastructure/        # Data access and external services
│   ├── Persistence/
│   │   ├── Neo4j/
│   │   │   ├── Neo4jConnectionFactory.cs
│   │   │   ├── Neo4jKnowledgeRepository.cs
│   │   │   └── Neo4jMemoryRepository.cs
│   │   ├── Qdrant/
│   │   │   ├── QdrantConnectionFactory.cs
│   │   │   ├── QdrantVectorRepository.cs
│   │   │   └── QdrantCollectionManager.cs
│   │   └── Redis/
│   │       ├── RedisCacheService.cs
│   │       └── RedisConnectionFactory.cs
│   ├── AI/
│   │   ├── OpenAI/
│   │   │   ├── OpenAIClientFactory.cs
│   │   │   ├── EmbeddingService.cs
│   │   │   └── ChatCompletionService.cs
│   │   └── Prompts/
│   │       ├── PromptTemplates.cs
│   │       └── PromptBuilder.cs
│   ├── Configuration/
│   │   ├── Neo4jOptions.cs
│   │   ├── QdrantOptions.cs
│   │   ├── OpenAIOptions.cs
│   │   └── CacheOptions.cs
│   └── AdvancedMemory.Infrastructure.csproj
│
├── AdvancedMemory.McpServer/             # MCP API Gateway
│   ├── Controllers/
│   │   └── McpController.cs
│   ├── Middleware/
│   │   ├── ExceptionHandlingMiddleware.cs
│   │   ├── RequestLoggingMiddleware.cs
│   │   └── RateLimitingMiddleware.cs
│   ├── Endpoints/
│   │   ├── McpEndpoints.cs              # Minimal API endpoints
│   │   └── HealthEndpoints.cs
│   ├── Services/
│   │   ├── ToolDispatcher.cs
│   │   └── SseStreamService.cs
│   ├── Configuration/
│   │   └── McpServerOptions.cs
│   ├── Program.cs
│   ├── appsettings.json
│   ├── appsettings.Development.json
│   └── AdvancedMemory.McpServer.csproj
│
├── AdvancedMemory.GraphRAGService/       # Knowledge Service
│   ├── Services/
│   │   ├── GraphRAGService.cs
│   │   ├── IndexingService.cs
│   │   ├── QueryEngine/
│   │   │   ├── GlobalSearchEngine.cs
│   │   │   ├── LocalSearchEngine.cs
│   │   │   └── QueryPlanner.cs
│   │   └── GraphProcessing/
│   │       ├── EntityExtractor.cs
│   │       ├── RelationshipExtractor.cs
│   │       ├── CommunityDetector.cs
│   │       └── CommunitySummarizer.cs
│   ├── Repositories/
│   │   ├── IKnowledgeGraphRepository.cs
│   │   └── KnowledgeGraphRepository.cs
│   ├── Models/
│   │   ├── IndexingRequest.cs
│   │   └── QueryRequest.cs
│   ├── Program.cs
│   ├── appsettings.json
│   └── AdvancedMemory.GraphRAGService.csproj
│
├── AdvancedMemory.MemoryService/         # Memory Service
│   ├── Services/
│   │   ├── MemoryService.cs
│   │   ├── MemoryOperations/
│   │   │   ├── MemoryAddOperation.cs
│   │   │   ├── MemorySearchOperation.cs
│   │   │   ├── MemoryUpdateOperation.cs
│   │   │   └── MemoryConsolidation.cs
│   │   └── Scoring/
│   │       ├── RelevanceScorer.cs
│   │       ├── RecencyScorer.cs
│   │       └── ImportanceScorer.cs
│   ├── Repositories/
│   │   ├── IMemoryRepository.cs
│   │   └── MemoryRepository.cs
│   ├── Models/
│   │   ├── MemoryQuery.cs
│   │   └── UserContext.cs
│   ├── Program.cs
│   ├── appsettings.json
│   └── AdvancedMemory.MemoryService.csproj
│
├── AdvancedMemory.GroundingService/      # Verification Service
│   ├── Services/
│   │   ├── GroundingService.cs
│   │   ├── ClaimExtractor.cs
│   │   └── ConfidenceCalculator.cs
│   ├── Repositories/
│   │   ├── ITrustedCorpusRepository.cs
│   │   └── TrustedCorpusRepository.cs
│   ├── Models/
│   │   ├── Claim.cs
│   │   └── VerificationRequest.cs
│   ├── Program.cs
│   ├── appsettings.json
│   └── AdvancedMemory.GroundingService.csproj
│
├── AdvancedMemory.OrchestrationService/  # Orchestration Layer
│   ├── Services/
│   │   ├── OrchestrationService.cs
│   │   ├── Workflows/
│   │   │   ├── ComprehensiveAnswerWorkflow.cs
│   │   │   ├── KnowledgeWithMemoryWorkflow.cs
│   │   │   └── VerifiedAnswerWorkflow.cs
│   │   └── WorkflowEngine.cs
│   ├── Program.cs
│   ├── appsettings.json
│   └── AdvancedMemory.OrchestrationService.csproj
│
├── AdvancedMemory.Dashboard/             # Blazor WebAssembly UI
│   ├── wwwroot/
│   │   ├── index.html
│   │   ├── css/
│   │   │   └── app.css
│   │   └── js/
│   │       └── interop.js
│   ├── Pages/
│   │   ├── Index.razor
│   │   ├── ActivityFeed.razor
│   │   ├── GraphViewer.razor
│   │   ├── MemoryExplorer.razor
│   │   └── SystemHealth.razor
│   ├── Shared/
│   │   ├── MainLayout.razor
│   │   ├── NavMenu.razor
│   │   └── ErrorBoundary.razor
│   ├── Services/
│   │   ├── DashboardService.cs
│   │   └── SignalRClient.cs
│   ├── Models/
│   │   └── DashboardModels.cs
│   ├── Program.cs
│   └── AdvancedMemory.Dashboard.csproj
│
└── AdvancedMemory.Shared/                # Shared utilities
    ├── Extensions/
    │   ├── StringExtensions.cs
    │   ├── EnumerableExtensions.cs
    │   └── ServiceCollectionExtensions.cs
    ├── Utilities/
    │   ├── Retry/
    │   │   └── RetryPolicy.cs
    │   ├── Validation/
    │   │   └── Validators.cs
    │   └── Helpers/
    │       └── JsonHelper.cs
    └── AdvancedMemory.Shared.csproj
```

## tests/ - Test Projects Structure

```
tests/
├── AdvancedMemory.Core.Tests/
│   ├── Models/
│   │   └── MemoryTests.cs
│   ├── Validators/
│   │   └── MemoryValidatorTests.cs
│   └── AdvancedMemory.Core.Tests.csproj
│
├── AdvancedMemory.Infrastructure.Tests/
│   ├── Persistence/
│   │   ├── Neo4j/
│   │   │   └── Neo4jRepositoryTests.cs
│   │   └── Qdrant/
│   │       └── QdrantRepositoryTests.cs
│   ├── AI/
│   │   └── EmbeddingServiceTests.cs
│   └── AdvancedMemory.Infrastructure.Tests.csproj
│
├── AdvancedMemory.GraphRAGService.Tests/
│   ├── Unit/
│   │   ├── EntityExtractorTests.cs
│   │   ├── RelationshipExtractorTests.cs
│   │   └── QueryEngineTests.cs
│   ├── Integration/
│   │   ├── IndexingIntegrationTests.cs
│   │   └── QueryIntegrationTests.cs
│   └── AdvancedMemory.GraphRAGService.Tests.csproj
│
├── AdvancedMemory.MemoryService.Tests/
│   ├── Unit/
│   │   ├── MemoryServiceTests.cs
│   │   └── ScoringTests.cs
│   ├── Integration/
│   │   └── MemoryOperationsIntegrationTests.cs
│   └── AdvancedMemory.MemoryService.Tests.csproj
│
├── AdvancedMemory.McpServer.Tests/
│   ├── Unit/
│   │   ├── ToolDispatcherTests.cs
│   │   └── SseStreamServiceTests.cs
│   ├── Integration/
│   │   └── McpEndpointsTests.cs
│   └── AdvancedMemory.McpServer.Tests.csproj
│
└── AdvancedMemory.E2E.Tests/             # End-to-End Tests
    ├── Scenarios/
    │   ├── ComprehensiveAnswerE2ETests.cs
    │   ├── MemoryPersistenceE2ETests.cs
    │   └── DashboardE2ETests.cs
    ├── Fixtures/
    │   └── TestEnvironmentFixture.cs
    └── AdvancedMemory.E2E.Tests.csproj
```

## docs/ - Documentation Structure

```
docs/
├── architecture.md                  # High-level architecture (already created)
├── tech-stack.md                    # Technology stack (already created)
├── ai-new-app-template.md          # Original template (already exists)
├── Enhanced Technical Report...md   # Research document (already exists)
├── index.html                       # Interactive report (already exists)
├── adr/                             # Architecture Decision Records
│   ├── README.md
│   ├── 001-dotnet-over-python.md
│   ├── 002-neo4j-graph-database.md
│   ├── 003-qdrant-vector-storage.md
│   ├── 004-mcp-protocol.md
│   ├── 005-clean-architecture.md
│   └── 006-aspire-orchestration.md
├── api/                             # API Documentation
│   ├── mcp-api.md
│   ├── graphrag-api.md
│   ├── memory-api.md
│   └── grounding-api.md
├── guides/                          # How-to guides
│   ├── getting-started.md
│   ├── local-development.md
│   ├── deployment-azure.md
│   ├── deployment-aws.md
│   └── troubleshooting.md
├── runbooks/                        # Operational runbooks
│   ├── incident-response.md
│   ├── scaling.md
│   └── backup-restore.md
└── diagrams/                        # Architecture diagrams
    ├── system-context.png
    ├── container-diagram.png
    └── component-diagram.png
```

## scripts/ - Build and Deployment Scripts

```
scripts/
├── build/
│   ├── build-all.sh
│   ├── build-all.ps1
│   ├── clean.sh
│   └── clean.ps1
├── test/
│   ├── run-all-tests.sh
│   ├── run-all-tests.ps1
│   └── test-coverage.sh
├── docker/
│   ├── build-images.sh
│   ├── build-images.ps1
│   ├── push-images.sh
│   └── compose-up.sh
├── database/
│   ├── neo4j-setup.sh
│   ├── qdrant-setup.sh
│   └── seed-data.sh
├── deployment/
│   ├── deploy-azure.sh
│   ├── deploy-aws.sh
│   └── deploy-local.sh
└── ci/
    ├── lint.sh
    ├── security-scan.sh
    └── generate-sbom.sh
```

## docker/ - Docker Configuration

```
docker/
├── mcp-server/
│   └── Dockerfile
├── graphrag-service/
│   └── Dockerfile
├── memory-service/
│   └── Dockerfile
├── grounding-service/
│   └── Dockerfile
├── orchestration-service/
│   └── Dockerfile
├── dashboard/
│   └── Dockerfile
├── docker-compose.yml              # Full stack
├── docker-compose.dev.yml          # Development overrides
├── docker-compose.test.yml         # Testing environment
└── .dockerignore
```

## terraform/ - Infrastructure as Code

```
terraform/
├── modules/
│   ├── networking/
│   │   ├── main.tf
│   │   ├── variables.tf
│   │   └── outputs.tf
│   ├── compute/
│   │   ├── main.tf
│   │   ├── variables.tf
│   │   └── outputs.tf
│   ├── database/
│   │   ├── main.tf
│   │   ├── variables.tf
│   │   └── outputs.tf
│   └── monitoring/
│       ├── main.tf
│       ├── variables.tf
│       └── outputs.tf
├── environments/
│   ├── dev/
│   │   ├── main.tf
│   │   ├── terraform.tfvars
│   │   └── backend.tf
│   ├── staging/
│   │   ├── main.tf
│   │   ├── terraform.tfvars
│   │   └── backend.tf
│   └── production/
│       ├── main.tf
│       ├── terraform.tfvars
│       └── backend.tf
└── docker/                         # Docker provider (local)
    └── main.tf
```

## .github/ - GitHub Configuration

```
.github/
├── workflows/
│   ├── ci-build.yml               # Build and test on PR
│   ├── ci-security.yml            # Security scanning
│   ├── cd-dev.yml                 # Deploy to dev
│   ├── cd-staging.yml             # Deploy to staging
│   ├── cd-production.yml          # Deploy to production
│   └── release.yml                # Create release
├── ISSUE_TEMPLATE/
│   ├── application-plan.md        # (already exists)
│   ├── bug_report.md
│   ├── feature_request.md
│   └── epic.md                    # (already exists)
├── PULL_REQUEST_TEMPLATE.md
└── CODEOWNERS
```

## Naming Conventions

### C# Code Conventions

#### Files
- **Classes**: `PascalCase.cs` (e.g., `MemoryService.cs`)
- **Interfaces**: `IPascalCase.cs` (e.g., `IMemoryService.cs`)
- **Tests**: `[ClassName]Tests.cs` (e.g., `MemoryServiceTests.cs`)
- **Configuration**: `appsettings.[Environment].json`

#### Namespaces
- **Pattern**: `AdvancedMemory.[Layer].[Feature]`
- **Examples**:
  - `AdvancedMemory.Core.Models.Memory`
  - `AdvancedMemory.Infrastructure.Persistence.Neo4j`
  - `AdvancedMemory.GraphRAGService.Services.QueryEngine`

#### Classes and Types
- **Classes**: `PascalCase` (e.g., `MemoryService`)
- **Interfaces**: `IPascalCase` (e.g., `IMemoryRepository`)
- **Enums**: `PascalCase` (e.g., `MemoryType`)
- **Records**: `PascalCase` (e.g., `MemoryRecord`)

#### Members
- **Methods**: `PascalCase` (e.g., `AddMemoryAsync`)
- **Properties**: `PascalCase` (e.g., `UserId`)
- **Fields (private)**: `_camelCase` (e.g., `_memoryRepository`)
- **Constants**: `PascalCase` (e.g., `MaxRetryAttempts`)
- **Parameters**: `camelCase` (e.g., `userId`)
- **Local Variables**: `camelCase` (e.g., `memoryResult`)

### Project Naming
- **Pattern**: `AdvancedMemory.[Component]`
- **Examples**:
  - `AdvancedMemory.Core`
  - `AdvancedMemory.McpServer`
  - `AdvancedMemory.GraphRAGService`
  - `AdvancedMemory.Core.Tests`

### Docker Images
- **Pattern**: `advanced-memory-[component]:[tag]`
- **Examples**:
  - `advanced-memory-mcp-server:latest`
  - `advanced-memory-graphrag-service:1.0.0`
  - `advanced-memory-dashboard:dev`

### Configuration Keys
- **Pattern**: `Component:SubComponent:Setting`
- **Examples**:
  - `Neo4j:Uri`
  - `OpenAI:ApiKey`
  - `Logging:LogLevel:Default`

### Environment Variables
- **Pattern**: `COMPONENT_SUBCOMPONENT_SETTING`
- **Examples**:
  - `NEO4J_URI`
  - `OPENAI_API_KEY`
  - `ASPNETCORE_ENVIRONMENT`

### Database Naming

#### Neo4j
- **Node Labels**: `PascalCase` (e.g., `Entity`, `Memory`, `User`)
- **Relationship Types**: `SCREAMING_SNAKE_CASE` (e.g., `HAS_MEMORY`, `RELATED_TO`)
- **Properties**: `camelCase` (e.g., `userId`, `createdAt`)

#### Qdrant
- **Collection Names**: `kebab-case` (e.g., `knowledge-chunks`, `user-memories`)
- **Payload Keys**: `camelCase` (e.g., `userId`, `timestamp`)

#### Redis
- **Key Pattern**: `component:entity:id` (e.g., `memory:user:alice123`, `cache:query:hash123`)

### API Endpoints
- **Pattern**: `/api/v{version}/[resource]/[action]`
- **Examples**:
  - `POST /mcp/sse`
  - `GET /api/v1/health`
  - `POST /api/v1/knowledge/query`
  - `POST /api/v1/memory/search`

### Git Branch Naming
- **Features**: `feature/short-description` (e.g., `feature/memory-consolidation`)
- **Bug Fixes**: `fix/short-description` (e.g., `fix/memory-leak-neo4j`)
- **Releases**: `release/v1.0.0`
- **Hotfixes**: `hotfix/critical-bug`

### Tags/Releases
- **Pattern**: `v{MAJOR}.{MINOR}.{PATCH}[-{PRERELEASE}]`
- **Examples**:
  - `v1.0.0`
  - `v1.2.3-beta.1`
  - `v2.0.0-rc.2`

## Configuration File Standards

### appsettings.json Structure
```json
{
  "Logging": {
    "LogLevel": {
      "Default": "Information"
    }
  },
  "AllowedHosts": "*",
  "Neo4j": {
    "Uri": "bolt://localhost:7687",
    "Username": "neo4j",
    "Password": "password"
  },
  "Qdrant": {
    "Host": "localhost",
    "Port": 6333,
    "ApiKey": ""
  },
  "OpenAI": {
    "ApiKey": "",
    "Model": "gpt-4o-mini",
    "EmbeddingModel": "text-embedding-3-small"
  },
  "Redis": {
    "ConnectionString": "localhost:6379"
  }
}
```

### Directory.Build.props
```xml
<Project>
  <PropertyGroup>
    <TargetFramework>net9.0</TargetFramework>
    <LangVersion>12</LangVersion>
    <Nullable>enable</Nullable>
    <ImplicitUsings>enable</ImplicitUsings>
    <TreatWarningsAsErrors>true</TreatWarningsAsErrors>
  </PropertyGroup>
  
  <ItemGroup>
    <PackageReference Include="SonarAnalyzer.CSharp" Version="9.0.0">
      <PrivateAssets>all</PrivateAssets>
      <IncludeAssets>runtime; build; native; contentfiles; analyzers</IncludeAssets>
    </PackageReference>
  </ItemGroup>
</Project>
```

### Directory.Packages.props (Central Package Management)
```xml
<Project>
  <PropertyGroup>
    <ManagePackageVersionsCentrally>true</ManagePackageVersionsCentrally>
  </PropertyGroup>
  
  <ItemGroup>
    <PackageVersion Include="Neo4j.Driver" Version="5.15.0" />
    <PackageVersion Include="Qdrant.Client" Version="1.7.0" />
    <PackageVersion Include="Azure.AI.OpenAI" Version="1.0.0-beta.14" />
    <PackageVersion Include="Serilog.AspNetCore" Version="8.0.0" />
    <PackageVersion Include="xunit" Version="2.6.2" />
    <PackageVersion Include="Moq" Version="4.20.69" />
    <PackageVersion Include="FluentAssertions" Version="6.12.0" />
  </ItemGroup>
</Project>
```

## Build Artifacts Organization

```
artifacts/
├── bin/                           # Compiled binaries
│   ├── Debug/
│   └── Release/
├── publish/                       # Published applications
│   ├── mcp-server/
│   ├── graphrag-service/
│   └── dashboard/
├── packages/                      # NuGet packages (if creating)
├── test-results/                  # Test output
│   ├── coverage/
│   └── reports/
└── docker-images/                 # Built Docker images (manifests)
```

## Development Workflow

1. **Feature Development**:
   - Create feature branch: `git checkout -b feature/my-feature`
   - Work in appropriate src/ project
   - Write tests in corresponding tests/ project
   - Update docs/ if needed

2. **Testing**:
   - Run unit tests: `dotnet test`
   - Run specific tests: `dotnet test --filter "FullyQualifiedName~MemoryServiceTests"`
   - Check coverage: `dotnet test /p:CollectCoverage=true`

3. **Code Quality**:
   - Format: `dotnet format`
   - Analyze: `dotnet build /p:RunAnalyzers=true`
   - Security scan: Run GitHub Actions workflow

4. **Docker Build**:
   - Build all: `./scripts/docker/build-images.sh`
   - Run locally: `docker-compose up -f docker/docker-compose.dev.yml`

5. **Deployment**:
   - Development: Auto-deploy on merge to `main`
   - Staging: Manual approval required
   - Production: Tag-based deployment

## Summary

This project structure follows .NET best practices and Clean Architecture principles:
- **Separation of Concerns**: Clear boundaries between layers
- **Testability**: Each component has corresponding test project
- **Modularity**: Services can be developed and deployed independently
- **Consistency**: Naming conventions enforced across all artifacts
- **Scalability**: Structure supports growth and team collaboration
