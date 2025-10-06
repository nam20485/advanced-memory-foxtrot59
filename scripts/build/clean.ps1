# Clean all build artifacts

$ErrorActionPreference = "Stop"

Write-Host "====================" -ForegroundColor Cyan
Write-Host "Cleaning Advanced Memory Solution" -ForegroundColor Cyan
Write-Host "====================" -ForegroundColor Cyan
Write-Host ""

$scriptDir = Split-Path -Parent $MyInvocation.MyCommand.Path
$rootDir = Resolve-Path "$scriptDir\..\.."

Push-Location $rootDir

try {
    # Clean solution
    Write-Host "Cleaning solution..." -ForegroundColor Yellow
    dotnet clean AdvancedMemory.sln

    # Remove bin and obj directories
    Write-Host "Removing bin and obj directories..." -ForegroundColor Yellow
    Get-ChildItem -Path . -Include bin,obj -Recurse -Directory | ForEach-Object {
        Write-Host "  Removing $($_.FullName)" -ForegroundColor Gray
        Remove-Item $_.FullName -Recurse -Force
    }

    Write-Host ""
    Write-Host "✓ Clean complete" -ForegroundColor Green
    Write-Host ""
    Write-Host "====================" -ForegroundColor Cyan
    Write-Host "Clean Complete" -ForegroundColor Cyan
    Write-Host "====================" -ForegroundColor Cyan
}
finally {
    Pop-Location
}
