FROM mcr.microsoft.com/dotnet/aspnet:9.0 AS base
WORKDIR /app
EXPOSE 8080
EXPOSE 8081

FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build
WORKDIR /src
COPY ["src/AdvancedMemory.Dashboard/AdvancedMemory.Dashboard.csproj", "src/AdvancedMemory.Dashboard/"]
COPY ["src/AdvancedMemory.ServiceDefaults/AdvancedMemory.ServiceDefaults.csproj", "src/AdvancedMemory.ServiceDefaults/"]
COPY ["Directory.Build.props", "./"]
COPY ["Directory.Packages.props", "./"]
RUN dotnet restore "src/AdvancedMemory.Dashboard/AdvancedMemory.Dashboard.csproj"
COPY . .
WORKDIR "/src/src/AdvancedMemory.Dashboard"
RUN dotnet build "AdvancedMemory.Dashboard.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "AdvancedMemory.Dashboard.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "AdvancedMemory.Dashboard.dll"]
