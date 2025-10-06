#!/bin/bash
# Run all tests with coverage

set -e

echo "===================="
echo "Running Advanced Memory Tests"
echo "===================="
echo ""

GREEN='\033[0;32m'
RED='\033[0;31m'
YELLOW='\033[1;33m'
NC='\033[0m'

SCRIPT_DIR="$( cd "$( dirname "${BASH_SOURCE[0]}" )" && pwd )"
ROOT_DIR="$SCRIPT_DIR/../.."
COVERAGE_DIR="$ROOT_DIR/coverage"

cd "$ROOT_DIR"

# Configuration
CONFIGURATION="${1:-Release}"
COLLECT_COVERAGE="${2:-true}"

echo "Test configuration: $CONFIGURATION"
echo "Collect coverage: $COLLECT_COVERAGE"
echo ""

# Create coverage directory
if [ "$COLLECT_COVERAGE" == "true" ]; then
    mkdir -p "$COVERAGE_DIR"
    echo "Coverage reports will be saved to: $COVERAGE_DIR"
    echo ""
fi

# Run tests
echo "Running tests..."

if [ "$COLLECT_COVERAGE" == "true" ]; then
    dotnet test AdvancedMemory.sln \
        --configuration "$CONFIGURATION" \
        --no-build \
        --verbosity normal \
        --collect:"XPlat Code Coverage" \
        --results-directory "$COVERAGE_DIR" \
        --logger "trx;LogFileName=test-results.trx"
else
    dotnet test AdvancedMemory.sln \
        --configuration "$CONFIGURATION" \
        --no-build \
        --verbosity normal \
        --logger "trx;LogFileName=test-results.trx"
fi

if [ $? -eq 0 ]; then
    echo ""
    echo -e "${GREEN}✓ All tests passed${NC}"
    
    if [ "$COLLECT_COVERAGE" == "true" ]; then
        echo ""
        echo "Coverage report generated at: $COVERAGE_DIR"
        
        # Check if reportgenerator is installed
        if command -v reportgenerator &> /dev/null; then
            echo "Generating HTML coverage report..."
            reportgenerator \
                "-reports:$COVERAGE_DIR/**/coverage.cobertura.xml" \
                "-targetdir:$COVERAGE_DIR/html" \
                "-reporttypes:Html"
            echo "HTML report: $COVERAGE_DIR/html/index.html"
        else
            echo -e "${YELLOW}Install reportgenerator for HTML coverage reports: dotnet tool install -g dotnet-reportgenerator-globaltool${NC}"
        fi
    fi
else
    echo ""
    echo -e "${RED}✗ Tests failed${NC}"
    exit 1
fi

echo ""
echo "===================="
echo "Tests Complete"
echo "===================="
