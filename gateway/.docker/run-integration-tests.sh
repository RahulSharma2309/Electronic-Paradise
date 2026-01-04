#!/usr/bin/env bash
set -euo pipefail

CONFIGURATION="${1:-Release}"
KEEP_ENV="${KEEP_ENV:-false}"

HERE="$(cd "$(dirname "${BASH_SOURCE[0]}")" && pwd)"
cd "$HERE"

PROJECT_NAME="${COMPOSE_PROJECT_NAME:-gateway-it}"

echo "Publishing integration tests..."
dotnet publish ../test/integration-test/Gateway.Integration.Test/Gateway.Integration.Test.csproj -c "$CONFIGURATION" -o ../test/integration-test/Gateway.Integration.Test/obj/docker/publish

echo "Running docker compose integration tests..."
export COMPOSE_PROJECT_NAME="$PROJECT_NAME"

echo "Building docker images explicitly..."
GATEWAY_CONTEXT="$HERE/.."
TEST_CONTEXT="$HERE/../test/integration-test/Gateway.Integration.Test"
MOCK_CONTEXT="$TEST_CONTEXT/Mocks"

docker build -t gateway:it -f "$GATEWAY_CONTEXT/src/Dockerfile" "$GATEWAY_CONTEXT"
docker build -t gateway-dependency-mock:it -f "$MOCK_CONTEXT/Dockerfile.mockserver" "$MOCK_CONTEXT"
docker build -t gateway.integration.test:it -f "$TEST_CONTEXT/Dockerfile.test" "$GATEWAY_CONTEXT"

set +e
docker compose -f docker-compose.yml -f docker-compose.test.yml up --no-build --abort-on-container-exit --exit-code-from gateway.integration.test
EXIT_CODE=$?
set -e

if [ "$KEEP_ENV" != "true" ]; then
  echo "Cleaning up docker compose environment..."
  docker compose -f docker-compose.yml -f docker-compose.test.yml down -v
fi

exit "$EXIT_CODE"

