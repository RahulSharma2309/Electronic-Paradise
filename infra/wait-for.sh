#!/usr/bin/env bash
# Simple wait-for script to wait for TCP service availability and HTTP endpoint readiness.
# Usage:
#   ./wait-for.sh host:port -- command-to-run
# Or to wait for HTTP endpoint:
#   ./wait-for.sh http://host:port/path [timeout]

set -e

if [ "$#" -lt 1 ]; then
  echo "Usage: $0 <host:port or http://url> [timeout_seconds]"
  exit 2
fi

TARGET=$1
TIMEOUT=${2:-60}

echo "Waiting for $TARGET (timeout ${TIMEOUT}s)..."

start=$(date +%s)

if [[ "$TARGET" =~ ^https?:// ]]; then
  # HTTP check
  while true; do
    if curl -sSf "$TARGET" >/dev/null 2>&1; then
      echo "$TARGET is up"
      exit 0
    fi
    now=$(date +%s)
    if [ $((now-start)) -ge $TIMEOUT ]; then
      echo "Timed out waiting for $TARGET"
      exit 1
    fi
    sleep 1
  done
else
  # TCP check (host:port)
  host=$(echo "$TARGET" | cut -d: -f1)
  port=$(echo "$TARGET" | cut -d: -f2)
  while true; do
    if nc -z "$host" "$port" >/dev/null 2>&1; then
      echo "$TARGET is accepting connections"
      exit 0
    fi
    now=$(date +%s)
    if [ $((now-start)) -ge $TIMEOUT ]; then
      echo "Timed out waiting for $TARGET"
      exit 1
    fi
    sleep 1
  done
fi
