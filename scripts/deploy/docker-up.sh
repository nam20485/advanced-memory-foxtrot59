#!/bin/bash
# Start all services with Docker Compose

set -e

echo "===================="
echo "Starting Advanced Memory Services"
echo "===================="
echo ""

GREEN='\033[0;32m'
YELLOW='\033[1;33m'
NC='\033[0m'

SCRIPT_DIR="$( cd "$( dirname "${BASH_SOURCE[0]}" )" && pwd )"
ROOT_DIR="$SCRIPT_DIR/../.."

cd "$ROOT_DIR"

# Check if .env file exists
if [ ! -f ".env" ]; then
    echo -e "${YELLOW}Warning: .env file not found. Creating from .env.example...${NC}"
    if [ -f ".env.example" ]; then
        cp .env.example .env
        echo "Please edit .env file with your configuration (especially OPENAI_API_KEY)"
        echo ""
    else
        echo "ERROR: .env.example not found"
        exit 1
    fi
fi

# Build mode (default: no build, use --build to build)
BUILD_FLAG=""
if [ "$1" == "--build" ]; then
    BUILD_FLAG="--build"
    echo "Building images before starting..."
fi

# Start services
echo "Starting services with Docker Compose..."
docker-compose up -d $BUILD_FLAG

if [ $? -eq 0 ]; then
    echo ""
    echo -e "${GREEN}✓ Services started successfully${NC}"
    echo ""
    echo "Service URLs:"
    echo "  MCP Server:            http://localhost:8080"
    echo "  GraphRAG Service:      http://localhost:8081"
    echo "  Memory Service:        http://localhost:8082"
    echo "  Grounding Service:     http://localhost:8083"
    echo "  Orchestration Service: http://localhost:8084"
    echo "  Dashboard:             http://localhost:8090"
    echo ""
    echo "Infrastructure URLs:"
    echo "  Neo4j Browser:         http://localhost:7474"
    echo "  Seq Logs:              http://localhost:5342"
    echo "  Jaeger Tracing:        http://localhost:16686"
    echo ""
    echo "To view logs: docker-compose logs -f [service-name]"
    echo "To stop: docker-compose down"
else
    echo "ERROR: Failed to start services"
    exit 1
fi

echo ""
echo "===================="
echo "Startup Complete"
echo "===================="
