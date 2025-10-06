# Build all projects in the solution
# Usage: .\build-all.ps1 [-Configuration Release|Debug]

param(
    [Parameter(Mandatory=$false)]
    [ValidateSet("Release", "Debug")]
    [string]$Configuration = "Release"
)

$ErrorActionPreference = "Stop"

Write-Host "====================" -ForegroundColor Cyan
Write-Host "Building Advanced Memory Solution" -ForegroundColor Cyan
Write-Host "====================" -ForegroundColor Cyan
Write-Host ""

# Get script directory
$scriptDir = Split-Path -Parent $MyInvocation.MyCommand.Path
$rootDir = Resolve-Path "$scriptDir\..\.."

Push-Location $rootDir

try {
    # Check for .NET SDK
    Write-Host "Checking .NET SDK..." -ForegroundColor Yellow
    
    if (-not (Get-Command dotnet -ErrorAction SilentlyContinue)) {
        Write-Host "ERROR: .NET SDK not found. Please install .NET 9.0 SDK." -ForegroundColor Red
        exit 1
    }

    $dotnetVersion = dotnet --version
    Write-Host "Found .NET SDK version: $dotnetVersion" -ForegroundColor Green
    Write-Host ""

    Write-Host "Build configuration: $Configuration" -ForegroundColor Yellow
    Write-Host ""

    # Restore dependencies
    Write-Host "Restoring dependencies..." -ForegroundColor Yellow
    dotnet restore AdvancedMemory.sln
    
    if ($LASTEXITCODE -eq 0) {
        Write-Host "✓ Restore successful" -ForegroundColor Green
    } else {
        Write-Host "✗ Restore failed" -ForegroundColor Red
        exit 1
    }
    Write-Host ""

    # Build solution
    Write-Host "Building solution..." -ForegroundColor Yellow
    dotnet build AdvancedMemory.sln `
        --configuration $Configuration `
        --no-restore `
        /p:TreatWarningsAsErrors=true

    if ($LASTEXITCODE -eq 0) {
        Write-Host ""
        Write-Host "✓ Build successful" -ForegroundColor Green
    } else {
        Write-Host ""
        Write-Host "✗ Build failed" -ForegroundColor Red
        exit 1
    }

    Write-Host ""
    Write-Host "====================" -ForegroundColor Cyan
    Write-Host "Build Complete" -ForegroundColor Cyan
    Write-Host "====================" -ForegroundColor Cyan
}
finally {
    Pop-Location
}
