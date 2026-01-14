# Kubernetes Manifests - Electronic Paradise

## Overview
This directory contains all Kubernetes configuration files for deploying Electronic Paradise to a Kubernetes cluster (Docker Desktop).

## Structure
```
k8s/
├── README.md              # This file
├── namespaces/            # Environment namespaces
│   └── namespaces.yaml    # staging, prod namespaces
├── rbac/                  # Role-Based Access Control
│   ├── README.md          # RBAC documentation
│   ├── service-accounts.yaml
│   ├── roles.yaml
│   └── role-bindings.yaml
├── configmaps/            # Application configuration
│   └── configmaps.yaml
├── secrets/               # Sensitive data (passwords, keys)
│   └── secrets.yaml
├── deployments/           # Service deployments
│   ├── README.md          # Deployment documentation
│   ├── auth-service/
│   │   ├── deployment.yaml
│   │   └── service.yaml
│   ├── user-service/
│   │   ├── deployment.yaml
│   │   └── service.yaml
│   ├── product-service/
│   │   ├── deployment.yaml
│   │   └── service.yaml
│   ├── order-service/
│   │   ├── deployment.yaml
│   │   └── service.yaml
│   ├── payment-service/
│   │   ├── deployment.yaml
│   │   └── service.yaml
│   ├── gateway/
│   │   ├── deployment.yaml
│   │   └── service.yaml
│   └── frontend/
│       ├── deployment.yaml
│       └── service.yaml
└── storage/               # Storage Classes (coming soon)
```

## Namespaces
We use two namespaces to separate environments:

| Namespace | Purpose | Access Level |
|-----------|---------|--------------|
| `staging` | Pre-production testing | Limited access |
| `prod` | Live production | Restricted access |

## Quick Start

### Apply Namespaces
```bash
kubectl apply -f namespaces/namespaces.yaml
```

### Verify Namespaces
```bash
kubectl get namespaces
kubectl get ns staging -o yaml
kubectl get ns prod -o yaml
```

### Switch to a Namespace
```bash
kubectl config set-context --current --namespace=staging
kubectl config set-context --current --namespace=prod
```

### List Resources in a Namespace
```bash
kubectl get all -n staging
kubectl get all -n prod
```

## Useful Commands

### View all resources across namespaces
```bash
kubectl get all --all-namespaces
```

### Describe a namespace
```bash
kubectl describe namespace staging
kubectl describe namespace prod
```

### Delete a namespace (⚠️ DANGER: Deletes everything in it!)
```bash
kubectl delete namespace staging
kubectl delete namespace prod
```

## Best Practices

1. **Always specify namespace when deploying:**
   ```bash
   kubectl apply -f deployment.yaml -n staging
   kubectl apply -f deployment.yaml -n prod
   ```

2. **Never deploy to prod accidentally:**
   - Always double-check which namespace you're in
   - Use different terminals for different namespaces
   - Set terminal prompts to show current namespace

3. **Label everything for easy filtering:**
   ```bash
   kubectl get all -l environment=staging
   kubectl get all -l environment=production
   ```

## RBAC (Role-Based Access Control)

See [rbac/README.md](rbac/README.md) for detailed documentation.

### Quick Apply
```bash
kubectl apply -f rbac/
```

## Next Steps
- [x] Set up RBAC for access control
- [ ] Configure storage classes
- [ ] Create service deployments
- [ ] Set up ingress
