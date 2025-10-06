# Start all services with Docker Compose
# Usage: .\docker-up.ps1 [-Build]

param(
    [Parameter(Mandatory=$false)]
    [switch]$Build
)

$ErrorActionPreference = "Stop"

Write-Host "====================" -ForegroundColor Cyan
Write-Host "Starting Advanced Memory Services" -ForegroundColor Cyan
Write-Host "====================" -ForegroundColor Cyan
Write-Host ""

$scriptDir = Split-Path -Parent $MyInvocation.MyCommand.Path
$rootDir = Resolve-Path "$scriptDir\..\.."

Push-Location $rootDir

try {
    # Check if .env file exists
    if (-not (Test-Path ".env")) {
        Write-Host "Warning: .env file not found. Creating from .env.example..." -ForegroundColor Yellow
        if (Test-Path ".env.example") {
            Copy-Item ".env.example" ".env"
            Write-Host "Please edit .env file with your configuration (especially OPENAI_API_KEY)" -ForegroundColor Yellow
            Write-Host ""
        } else {
            Write-Host "ERROR: .env.example not found" -ForegroundColor Red
            exit 1
        }
    }

    # Start services
    Write-Host "Starting services with Docker Compose..." -ForegroundColor Yellow
    
    if ($Build) {
        Write-Host "Building images before starting..." -ForegroundColor Yellow
        docker-compose up -d --build
    } else {
        docker-compose up -d
    }

    if ($LASTEXITCODE -eq 0) {
        Write-Host ""
        Write-Host "✓ Services started successfully" -ForegroundColor Green
        Write-Host ""
        Write-Host "Service URLs:" -ForegroundColor Cyan
        Write-Host "  MCP Server:            http://localhost:8080" -ForegroundColor White
        Write-Host "  GraphRAG Service:      http://localhost:8081" -ForegroundColor White
        Write-Host "  Memory Service:        http://localhost:8082" -ForegroundColor White
        Write-Host "  Grounding Service:     http://localhost:8083" -ForegroundColor White
        Write-Host "  Orchestration Service: http://localhost:8084" -ForegroundColor White
        Write-Host "  Dashboard:             http://localhost:8090" -ForegroundColor White
        Write-Host ""
        Write-Host "Infrastructure URLs:" -ForegroundColor Cyan
        Write-Host "  Neo4j Browser:         http://localhost:7474" -ForegroundColor White
        Write-Host "  Seq Logs:              http://localhost:5342" -ForegroundColor White
        Write-Host "  Jaeger Tracing:        http://localhost:16686" -ForegroundColor White
        Write-Host ""
        Write-Host "To view logs: docker-compose logs -f [service-name]" -ForegroundColor Yellow
        Write-Host "To stop: docker-compose down" -ForegroundColor Yellow
    } else {
        Write-Host "ERROR: Failed to start services" -ForegroundColor Red
        exit 1
    }

    Write-Host ""
    Write-Host "====================" -ForegroundColor Cyan
    Write-Host "Startup Complete" -ForegroundColor Cyan
    Write-Host "====================" -ForegroundColor Cyan
}
finally {
    Pop-Location
}
