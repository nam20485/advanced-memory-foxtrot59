# Build stage
FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build
WORKDIR /src

# Copy solution and project files
COPY ["AdvancedMemory.sln", "./"]
COPY ["Directory.Build.props", "./"]
COPY ["Directory.Packages.props", "./"]
COPY ["src/AdvancedMemory.Dashboard/AdvancedMemory.Dashboard.csproj", "src/AdvancedMemory.Dashboard/"]
COPY ["src/AdvancedMemory.Shared/AdvancedMemory.Shared.csproj", "src/AdvancedMemory.Shared/"]

# Restore dependencies
RUN dotnet restore "src/AdvancedMemory.Dashboard/AdvancedMemory.Dashboard.csproj"

# Copy source code
COPY ["src/", "src/"]

# Build and publish
RUN dotnet publish "src/AdvancedMemory.Dashboard/AdvancedMemory.Dashboard.csproj" \
    -c Release \
    -o /app/publish \
    --no-restore

# Runtime stage
FROM nginx:alpine AS runtime
WORKDIR /usr/share/nginx/html

# Copy published Blazor WASM app
COPY --from=build /app/publish/wwwroot .

# Copy nginx configuration
COPY docker/dashboard/nginx.conf /etc/nginx/nginx.conf

EXPOSE 80

CMD ["nginx", "-g", "daemon off;"]
