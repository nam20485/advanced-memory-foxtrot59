FROM mcr.microsoft.com/dotnet/aspnet:9.0 AS base
WORKDIR /app
EXPOSE 8080
EXPOSE 8081

FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build
WORKDIR /src
COPY ["src/AdvancedMemory.AppHost/AdvancedMemory.AppHost.csproj", "src/AdvancedMemory.AppHost/"]
COPY ["src/AdvancedMemory.ServiceDefaults/AdvancedMemory.ServiceDefaults.csproj", "src/AdvancedMemory.ServiceDefaults/"]
COPY ["Directory.Build.props", "./"]
COPY ["Directory.Packages.props", "./"]
RUN dotnet restore "src/AdvancedMemory.AppHost/AdvancedMemory.AppHost.csproj"
COPY . .
WORKDIR "/src/src/AdvancedMemory.AppHost"
RUN dotnet build "AdvancedMemory.AppHost.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "AdvancedMemory.AppHost.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "AdvancedMemory.AppHost.dll"]
