# Stop all services
# Usage: .\docker-down.ps1 [-RemoveVolumes]

param(
    [Parameter(Mandatory=$false)]
    [switch]$RemoveVolumes
)

$ErrorActionPreference = "Stop"

Write-Host "====================" -ForegroundColor Cyan
Write-Host "Stopping Advanced Memory Services" -ForegroundColor Cyan
Write-Host "====================" -ForegroundColor Cyan
Write-Host ""

$scriptDir = Split-Path -Parent $MyInvocation.MyCommand.Path
$rootDir = Resolve-Path "$scriptDir\..\.."

Push-Location $rootDir

try {
    Write-Host "Stopping services..." -ForegroundColor Yellow
    docker-compose down

    if ($RemoveVolumes) {
        Write-Host "Removing volumes..." -ForegroundColor Yellow
        docker-compose down -v
    }

    Write-Host ""
    Write-Host "✓ Services stopped" -ForegroundColor Green
    Write-Host ""
    Write-Host "====================" -ForegroundColor Cyan
    Write-Host "Shutdown Complete" -ForegroundColor Cyan
    Write-Host "====================" -ForegroundColor Cyan
}
finally {
    Pop-Location
}
