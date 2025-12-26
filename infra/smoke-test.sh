#!/usr/bin/env bash
set -euo pipefail

ROOT_URL=${ROOT_URL:-http://localhost:5000}

function parse_json_field() {
  local field=$1
  if command -v jq >/dev/null 2>&1; then
    jq -r ".. | .${field}? // empty" | head -n1
  else
    # crude fallback: look for "field":"value"
    sed -n 's/.*"'"${field}"'"\s*:\s*"\([^"]*\)".*/\1/p' | head -n1
  fi
}

echo "Starting smoke test against $ROOT_URL"

# Wait for gateway health
./wait-for.sh "$ROOT_URL/api/health" 120

# 1) Register user
EMAIL="smoketest+$(date +%s)@example.com"
PASSWORD="Password123!"
FULLNAME="Smoke Test"

echo "Registering user $EMAIL"
REG_RES=$(curl -s -X POST "$ROOT_URL/api/auth/register" -H "Content-Type: application/json" -d "{\"Email\":\"$EMAIL\",\"Password\":\"$PASSWORD\",\"FullName\":\"$FULLNAME\"}")
if echo "$REG_RES" | grep -q "error"; then
  echo "Registration response: $REG_RES"
fi

# 2) Login
echo "Logging in"
LOGIN_RES=$(curl -s -X POST "$ROOT_URL/api/auth/login" -H "Content-Type: application/json" -d "{\"Email\":\"$EMAIL\",\"Password\":\"$PASSWORD\"}")
TOKEN=$(echo "$LOGIN_RES" | (parse_json_field token) )
if [ -z "$TOKEN" ] || [ "$TOKEN" = "null" ]; then
  echo "Login failed: $LOGIN_RES"
  exit 1
fi
echo "Got token"

# extract userId
USERID=$(echo "$LOGIN_RES" | (parse_json_field userId) )
if [ -z "$USERID" ] || [ "$USERID" = "null" ]; then
  echo "Warning: userId not returned by login response"
fi

# 3) Create wallet
echo "Creating wallet for user $USERID"
WALLET_RES=$(curl -s -X POST "$ROOT_URL/api/payments/wallet" -H "Content-Type: application/json" -d "{\"UserId\":\"${USERID}\",\"InitialBalance\":5000}")

# 4) List products
echo "Fetching products"
PROD_RES=$(curl -s "$ROOT_URL/api/products")
# pick first product with stock>0
PRODUCT_ID=$(echo "$PROD_RES" | (if command -v jq >/dev/null 2>&1; then jq -r '.[] | select(.stock>0) | .id' | head -n1; else sed -n 's/.*"id"\s*:\s*"\([^"]*\)".*/\1/p' | head -n1; fi))
if [ -z "$PRODUCT_ID" ]; then
  echo "No available product found: $PROD_RES"
  exit 1
fi
echo "Selected product $PRODUCT_ID"

# 5) Create order
echo "Creating order"
ORDER_BODY=$(cat <<EOF
{
  "UserId": "$USERID",
  "Items": [ { "ProductId": "$PRODUCT_ID", "Quantity": 1 } ]
}
EOF
)
ORDER_RES=$(curl -s -X POST "$ROOT_URL/api/orders/create" -H "Content-Type: application/json" -H "Authorization: Bearer $TOKEN" -d "$ORDER_BODY")

if echo "$ORDER_RES" | grep -q "error"; then
  echo "Order failed: $ORDER_RES"
  exit 1
fi

echo "Order response: $ORDER_RES"

echo "Smoke test succeeded"
