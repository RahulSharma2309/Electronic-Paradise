# Ingress Configuration - Staging

This directory contains the Ingress configuration for the staging environment.

## Overview

The Ingress routes external traffic to:
- **Frontend**: `staging.freshharvest-market.local` → `frontend` service
- **API Gateway**: `api.staging.freshharvest-market.local` → `gateway` service

## Features

- ✅ Path-based routing
- ✅ Rate limiting (100 requests/minute per IP)
- ✅ CORS enabled for frontend
- ✅ SSL redirect disabled (for local development)

## Accessing Services

### Option 1: Using /etc/hosts (Recommended)

Add these entries to your `/etc/hosts` file (Windows: `C:\Windows\System32\drivers\etc\hosts`):

```
127.0.0.1 staging.freshharvest-market.local
127.0.0.1 api.staging.freshharvest-market.local
```

Then access:
- Frontend: http://staging.freshharvest-market.local
- API: http://api.staging.freshharvest-market.local/api

### Option 2: Using Port Forward

```bash
# Get the Ingress Controller service port
kubectl get service -n ingress-nginx ingress-nginx-controller

# Port forward (if using NodePort)
kubectl port-forward -n ingress-nginx service/ingress-nginx-controller 8080:80
```

Then access via `localhost:8080` with proper Host headers.

### Option 3: Direct Port Access (Docker Desktop)

Docker Desktop exposes the Ingress Controller on `localhost`:
- Frontend: http://localhost (with Host header: `staging.freshharvest-market.local`)
- API: http://localhost (with Host header: `api.staging.freshharvest-market.local`)

## Testing

```bash
# Test frontend
curl -H "Host: staging.freshharvest-market.local" http://localhost

# Test API gateway
curl -H "Host: api.staging.freshharvest-market.local" http://localhost/api/health
```

## Rate Limiting

Rate limiting is configured at 100 requests per minute per IP address. This can be adjusted in the Ingress annotations.

## Next Steps

- [ ] Set up cert-manager for TLS certificates
- [ ] Configure DNS for staging environment
- [ ] Add monitoring and alerting for Ingress

---