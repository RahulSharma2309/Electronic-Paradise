#!/usr/bin/env bash
set -euo pipefail

CONFIGURATION="${1:-Release}"
KEEP_ENV="${KEEP_ENV:-false}"

HERE="$(cd "$(dirname "${BASH_SOURCE[0]}")" && pwd)"
cd "$HERE"

PROJECT_NAME="${COMPOSE_PROJECT_NAME:-payment-it}"

echo "Publishing integration tests..."
dotnet publish ../test/integration-test/PaymentService.Integration.Test/PaymentService.Integration.Test.csproj -c "$CONFIGURATION" -o ../test/integration-test/PaymentService.Integration.Test/obj/docker/publish

echo "Running docker compose integration tests..."
export COMPOSE_PROJECT_NAME="$PROJECT_NAME"

echo "Building docker images explicitly..."
SERVICE_CONTEXT="$HERE/.."
TEST_CONTEXT="$HERE/../test/integration-test/PaymentService.Integration.Test"
MOCK_CONTEXT="$TEST_CONTEXT/UserServiceMock"

if [ -n "${GITHUB_TOKEN:-}" ]; then
  docker build -t payment-service:it --build-arg "GITHUB_TOKEN=$GITHUB_TOKEN" -f "$SERVICE_CONTEXT/src/Dockerfile" "$SERVICE_CONTEXT"
else
  docker build -t payment-service:it -f "$SERVICE_CONTEXT/src/Dockerfile" "$SERVICE_CONTEXT"
fi

docker build -t payment-user-service-mock:it -f "$MOCK_CONTEXT/Dockerfile.mockserver" "$MOCK_CONTEXT"
docker build -t payment-service.integration.test:it -f "$TEST_CONTEXT/Dockerfile.test" "$TEST_CONTEXT"

set +e
docker compose -f docker-compose.yml -f docker-compose.test.yml up --no-build --abort-on-container-exit --exit-code-from payment-service.integration.test
EXIT_CODE=$?
set -e

if [ "$KEEP_ENV" != "true" ]; then
  echo "Cleaning up docker compose environment..."
  docker compose -f docker-compose.yml -f docker-compose.test.yml down -v
fi

exit "$EXIT_CODE"



