#!/usr/bin/env bash
set -euo pipefail

CONFIGURATION="${1:-Release}"
ROOT="$(cd "$(dirname "${BASH_SOURCE[0]}")" && pwd)"
exec "$ROOT/../services/payment-service/.docker/run-integration-tests.sh" "$CONFIGURATION"



