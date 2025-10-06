#!/bin/bash
# Stop all services

set -e

echo "===================="
echo "Stopping Advanced Memory Services"
echo "===================="
echo ""

SCRIPT_DIR="$( cd "$( dirname "${BASH_SOURCE[0]}" )" && pwd )"
ROOT_DIR="$SCRIPT_DIR/../.."

cd "$ROOT_DIR"

# Stop services
echo "Stopping services..."
docker-compose down

if [ "$1" == "--volumes" ]; then
    echo "Removing volumes..."
    docker-compose down -v
fi

echo ""
echo "✓ Services stopped"
echo ""
echo "===================="
echo "Shutdown Complete"
echo "===================="
