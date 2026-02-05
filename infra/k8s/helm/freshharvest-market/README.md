# FreshHarvest Market Helm Chart

This Helm chart packages the FreshHarvest Market organic food marketplace for Kubernetes deployment.

## Overview

This chart deploys all 7 microservices (auth, user, product, order, payment, gateway, frontend) with their associated resources:
- Deployments
- Services
- ConfigMaps
- Secrets
- RBAC (ServiceAccounts, Roles, RoleBindings)
- Ingress

## Prerequisites

- Kubernetes 1.19+
- kubectl configured
- Helm 3.x (optional, but recommended)
- NGINX Ingress Controller installed

## Installation

### Staging Environment

```bash
# Install to staging namespace
helm install freshharvest-market . \
  --namespace staging \
  --create-namespace \
  -f values-staging.yaml
```

### Production Environment

```bash
# Install to prod namespace
helm install freshharvest-market . \
  --namespace prod \
  --create-namespace \
  -f values-prod.yaml
```

## Configuration

### Values Files

- `values.yaml` - Default values
- `values-staging.yaml` - Staging overrides
- `values-prod.yaml` - Production overrides

### Key Configuration Options

```yaml
global:
  imageRegistry: ghcr.io/rahulsharma2309
  environment: staging

services:
  auth:
    enabled: true
    replicas: 2
    image:
      tag: latest
    resources:
      requests:
        memory: "256Mi"
        cpu: "100m"
```

## Upgrading

```bash
# Upgrade staging
helm upgrade freshharvest-market . \
  --namespace staging \
  -f values-staging.yaml

# Upgrade production
helm upgrade freshharvest-market . \
  --namespace prod \
  -f values-prod.yaml
```

## Uninstalling

```bash
# Uninstall from staging
helm uninstall freshharvest-market --namespace staging

# Uninstall from production
helm uninstall freshharvest-market --namespace prod
```

## Chart Structure

```
freshharvest-market/
├── Chart.yaml              # Chart metadata
├── values.yaml             # Default values
├── values-staging.yaml     # Staging overrides
├── values-prod.yaml        # Production overrides
├── templates/              # Kubernetes templates
│   ├── _helpers.tpl        # Template helpers
│   ├── deployment.yaml     # Deployment templates
│   ├── service.yaml        # Service templates
│   └── ...
└── README.md               # This file
```

## Notes

- All services use the same image registry: `ghcr.io/rahulsharma2309`
- Image pull secrets must be created before installation
- ConfigMaps and Secrets should be created separately or via sub-charts
- Ingress is optional and can be disabled

---