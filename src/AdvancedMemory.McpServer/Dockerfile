FROM mcr.microsoft.com/dotnet/aspnet:9.0 AS base
WORKDIR /app
EXPOSE 8080
EXPOSE 8081

FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build
WORKDIR /src
COPY ["src/AdvancedMemory.McpServer/AdvancedMemory.McpServer.csproj", "src/AdvancedMemory.McpServer/"]
COPY ["src/AdvancedMemory.Core/AdvancedMemory.Core.csproj", "src/AdvancedMemory.Core/"]
COPY ["src/AdvancedMemory.Infrastructure/AdvancedMemory.Infrastructure.csproj", "src/AdvancedMemory.Infrastructure/"]
COPY ["src/AdvancedMemory.ServiceDefaults/AdvancedMemory.ServiceDefaults.csproj", "src/AdvancedMemory.ServiceDefaults/"]
COPY ["src/AdvancedMemory.Shared/AdvancedMemory.Shared.csproj", "src/AdvancedMemory.Shared/"]
COPY ["Directory.Build.props", "./"]
COPY ["Directory.Packages.props", "./"]
RUN dotnet restore "src/AdvancedMemory.McpServer/AdvancedMemory.McpServer.csproj"
COPY . .
WORKDIR "/src/src/AdvancedMemory.McpServer"
RUN dotnet build "AdvancedMemory.McpServer.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "AdvancedMemory.McpServer.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "AdvancedMemory.McpServer.dll"]
