# Run all tests with coverage
# Usage: .\run-tests.ps1 [-Configuration Release|Debug] [-CollectCoverage $true|$false]

param(
    [Parameter(Mandatory=$false)]
    [ValidateSet("Release", "Debug")]
    [string]$Configuration = "Release",
    
    [Parameter(Mandatory=$false)]
    [bool]$CollectCoverage = $true
)

$ErrorActionPreference = "Stop"

Write-Host "====================" -ForegroundColor Cyan
Write-Host "Running Advanced Memory Tests" -ForegroundColor Cyan
Write-Host "====================" -ForegroundColor Cyan
Write-Host ""

$scriptDir = Split-Path -Parent $MyInvocation.MyCommand.Path
$rootDir = Resolve-Path "$scriptDir\..\.."
$coverageDir = Join-Path $rootDir "coverage"

Push-Location $rootDir

try {
    Write-Host "Test configuration: $Configuration" -ForegroundColor Yellow
    Write-Host "Collect coverage: $CollectCoverage" -ForegroundColor Yellow
    Write-Host ""

    # Create coverage directory
    if ($CollectCoverage) {
        if (-not (Test-Path $coverageDir)) {
            New-Item -Path $coverageDir -ItemType Directory | Out-Null
        }
        Write-Host "Coverage reports will be saved to: $coverageDir" -ForegroundColor Yellow
        Write-Host ""
    }

    # Run tests
    Write-Host "Running tests..." -ForegroundColor Yellow

    if ($CollectCoverage) {
        dotnet test AdvancedMemory.sln `
            --configuration $Configuration `
            --no-build `
            --verbosity normal `
            --collect:"XPlat Code Coverage" `
            --results-directory $coverageDir `
            --logger "trx;LogFileName=test-results.trx"
    } else {
        dotnet test AdvancedMemory.sln `
            --configuration $Configuration `
            --no-build `
            --verbosity normal `
            --logger "trx;LogFileName=test-results.trx"
    }

    if ($LASTEXITCODE -eq 0) {
        Write-Host ""
        Write-Host "✓ All tests passed" -ForegroundColor Green
        
        if ($CollectCoverage) {
            Write-Host ""
            Write-Host "Coverage report generated at: $coverageDir" -ForegroundColor Green
            
            # Check if reportgenerator is installed
            if (Get-Command reportgenerator -ErrorAction SilentlyContinue) {
                Write-Host "Generating HTML coverage report..." -ForegroundColor Yellow
                $htmlDir = Join-Path $coverageDir "html"
                reportgenerator `
                    "-reports:$coverageDir/**/coverage.cobertura.xml" `
                    "-targetdir:$htmlDir" `
                    "-reporttypes:Html"
                Write-Host "HTML report: $htmlDir\index.html" -ForegroundColor Green
            } else {
                Write-Host "Install reportgenerator for HTML coverage reports: dotnet tool install -g dotnet-reportgenerator-globaltool" -ForegroundColor Yellow
            }
        }
    } else {
        Write-Host ""
        Write-Host "✗ Tests failed" -ForegroundColor Red
        exit 1
    }

    Write-Host ""
    Write-Host "====================" -ForegroundColor Cyan
    Write-Host "Tests Complete" -ForegroundColor Cyan
    Write-Host "====================" -ForegroundColor Cyan
}
finally {
    Pop-Location
}
