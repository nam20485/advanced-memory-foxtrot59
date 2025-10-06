#!/bin/bash
# Build all projects in the solution

set -e

echo "===================="
echo "Building Advanced Memory Solution"
echo "===================="
echo ""

# Colors for output
RED='\033[0;31m'
GREEN='\033[0;32m'
YELLOW='\033[1;33m'
NC='\033[0m' # No Color

# Get script directory
SCRIPT_DIR="$( cd "$( dirname "${BASH_SOURCE[0]}" )" && pwd )"
ROOT_DIR="$SCRIPT_DIR/../.."

cd "$ROOT_DIR"

# Check for .NET SDK
echo "Checking .NET SDK..."
if ! command -v dotnet &> /dev/null; then
    echo -e "${RED}ERROR: .NET SDK not found. Please install .NET 9.0 SDK.${NC}"
    exit 1
fi

DOTNET_VERSION=$(dotnet --version)
echo -e "${GREEN}Found .NET SDK version: $DOTNET_VERSION${NC}"
echo ""

# Configuration (default to Release)
CONFIGURATION="${1:-Release}"
echo "Build configuration: $CONFIGURATION"
echo ""

# Restore dependencies
echo "Restoring dependencies..."
dotnet restore AdvancedMemory.sln
if [ $? -eq 0 ]; then
    echo -e "${GREEN}✓ Restore successful${NC}"
else
    echo -e "${RED}✗ Restore failed${NC}"
    exit 1
fi
echo ""

# Build solution
echo "Building solution..."
dotnet build AdvancedMemory.sln \
    --configuration "$CONFIGURATION" \
    --no-restore \
    /p:TreatWarningsAsErrors=true

if [ $? -eq 0 ]; then
    echo ""
    echo -e "${GREEN}✓ Build successful${NC}"
else
    echo ""
    echo -e "${RED}✗ Build failed${NC}"
    exit 1
fi

echo ""
echo "===================="
echo "Build Complete"
echo "===================="
