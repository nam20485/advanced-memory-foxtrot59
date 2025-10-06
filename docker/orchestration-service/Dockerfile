# Build stage
FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build
WORKDIR /src

# Copy solution and project files
COPY ["AdvancedMemory.sln", "./"]
COPY ["Directory.Build.props", "./"]
COPY ["Directory.Packages.props", "./"]
COPY ["src/AdvancedMemory.OrchestrationService/AdvancedMemory.OrchestrationService.csproj", "src/AdvancedMemory.OrchestrationService/"]
COPY ["src/AdvancedMemory.ServiceDefaults/AdvancedMemory.ServiceDefaults.csproj", "src/AdvancedMemory.ServiceDefaults/"]
COPY ["src/AdvancedMemory.Core/AdvancedMemory.Core.csproj", "src/AdvancedMemory.Core/"]
COPY ["src/AdvancedMemory.Infrastructure/AdvancedMemory.Infrastructure.csproj", "src/AdvancedMemory.Infrastructure/"]
COPY ["src/AdvancedMemory.Shared/AdvancedMemory.Shared.csproj", "src/AdvancedMemory.Shared/"]

# Restore dependencies
RUN dotnet restore "src/AdvancedMemory.OrchestrationService/AdvancedMemory.OrchestrationService.csproj"

# Copy source code
COPY ["src/", "src/"]

# Build and publish
RUN dotnet publish "src/AdvancedMemory.OrchestrationService/AdvancedMemory.OrchestrationService.csproj" \
    -c Release \
    -o /app/publish \
    --no-restore

# Runtime stage
FROM mcr.microsoft.com/dotnet/aspnet:9.0 AS runtime
WORKDIR /app

# Copy published app
COPY --from=build /app/publish .

# Configure ASP.NET Core
ENV ASPNETCORE_URLS=http://+:8084
ENV ASPNETCORE_ENVIRONMENT=Production

# Health check
HEALTHCHECK --interval=30s --timeout=10s --start-period=60s --retries=3 \
    CMD curl --fail http://localhost:8084/health || exit 1

EXPOSE 8084

ENTRYPOINT ["dotnet", "AdvancedMemory.OrchestrationService.dll"]
