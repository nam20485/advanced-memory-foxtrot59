#!/bin/bash
# Clean all build artifacts

set -e

echo "===================="
echo "Cleaning Advanced Memory Solution"
echo "===================="
echo ""

SCRIPT_DIR="$( cd "$( dirname "${BASH_SOURCE[0]}" )" && pwd )"
ROOT_DIR="$SCRIPT_DIR/../.."

cd "$ROOT_DIR"

GREEN='\033[0;32m'
NC='\033[0m'

# Clean solution
echo "Cleaning solution..."
dotnet clean AdvancedMemory.sln

# Remove bin and obj directories
echo "Removing bin and obj directories..."
find . -type d -name "bin" -o -name "obj" | while read dir; do
    echo "  Removing $dir"
    rm -rf "$dir"
done

echo ""
echo -e "${GREEN}✓ Clean complete${NC}"
echo ""
echo "===================="
echo "Clean Complete"
echo "===================="
