# Ingress Configuration - Production

This directory contains the Ingress configuration for the production environment.

## Overview

The Ingress routes external traffic to:
- **Frontend**: `electronic-paradise.com` → `frontend` service
- **API Gateway**: `api.electronic-paradise.com` → `gateway` service

## Features

- ✅ Path-based routing
- ✅ Rate limiting (200 requests/minute per IP)
- ✅ CORS enabled for frontend
- ✅ SSL redirect enabled (when TLS is configured)
- ✅ Request body size limit (10MB)

## TLS Configuration

TLS is configured but commented out. To enable:

1. Install cert-manager:
   ```bash
   kubectl apply -f https://github.com/cert-manager/cert-manager/releases/download/v1.13.0/cert-manager.yaml
   ```

2. Create a ClusterIssuer for Let's Encrypt:
   ```yaml
   apiVersion: cert-manager.io/v1
   kind: ClusterIssuer
   metadata:
     name: letsencrypt-prod
   spec:
     acme:
       server: https://acme-v02.api.letsencrypt.org/directory
       email: your-email@example.com
       privateKeySecretRef:
         name: letsencrypt-prod
       solvers:
       - http01:
           ingress:
             class: nginx
   ```

3. Uncomment the `tls` section in `ingress.yaml`

4. Add annotation to Ingress:
   ```yaml
   cert-manager.io/cluster-issuer: "letsencrypt-prod"
   ```

## DNS Configuration

Point your DNS records to the Ingress Controller's external IP:

```bash
# Get the external IP
kubectl get service -n ingress-nginx ingress-nginx-controller

# Create DNS records:
# A record: electronic-paradise.com → <EXTERNAL-IP>
# A record: api.electronic-paradise.com → <EXTERNAL-IP>
```

## Rate Limiting

Rate limiting is configured at 200 requests per minute per IP address for production. Adjust based on your needs.

## Monitoring

Monitor Ingress metrics:
- Request rate
- Error rate
- Response times
- Rate limit hits

## Next Steps

- [ ] Set up cert-manager and TLS certificates
- [ ] Configure DNS records
- [ ] Set up monitoring dashboards
- [ ] Configure WAF rules (optional)

---