#!/usr/bin/env bash
set -euo pipefail

APP_DLL=${APP_DLL:-Gateway.dll}
WAIT_TIMEOUT=${WAIT_TIMEOUT:-120}

echo "[gateway-entrypoint] waiting for dependent services to report healthy"
SERVICES=("http://auth-service/api/health" "http://product-service/api/health" "http://payment-service/api/health" "http://order-service/api/health" "http://user-service/api/health")
for svc in "${SERVICES[@]}"; do
  echo "[gateway-entrypoint] waiting for $svc"
  for i in $(seq 1 $WAIT_TIMEOUT); do
    if curl -sfS "$svc" >/dev/null 2>&1; then
      echo "[gateway-entrypoint] $svc is ready"
      break
    fi
    sleep 1
    if [ "$i" -eq "$WAIT_TIMEOUT" ]; then
      echo "[gateway-entrypoint] timed out waiting for $svc"
      exit 1
    fi
  done
done

echo "[gateway-entrypoint] starting gateway"
exec dotnet "$APP_DLL"
