# RBAC (Role-Based Access Control) - Electronic Paradise

## Overview
This directory contains RBAC configurations that control who can do what in our Kubernetes namespaces.

## What is RBAC?

RBAC = **Role-Based Access Control**

Think of it like this:
- **ServiceAccount** = Employee ID card (who you are)
- **Role** = Job description (what you're allowed to do)
- **RoleBinding** = Assignment (connecting the ID card to the job)

## Structure

```
rbac/
├── README.md                    # This file
├── service-accounts.yaml        # ServiceAccounts for all services
├── roles.yaml                   # Roles (permissions)
└── role-bindings.yaml           # RoleBindings (assignments)
```

## Components Explained

### 1. ServiceAccount
An identity that pods use to authenticate to the Kubernetes API.

**Example:**
```yaml
apiVersion: v1
kind: ServiceAccount
metadata:
  name: auth-service-sa
  namespace: staging
```

**Why?** Each service needs its own identity so Kubernetes knows:
- Which service is making API calls
- What permissions that service has
- How to audit actions

### 2. Role
Defines **what actions** are allowed on **which resources** in a **specific namespace**.

**Example:**
```yaml
apiVersion: rbac.authorization.k8s.io/v1
kind: Role
metadata:
  name: service-role
  namespace: staging
rules:
- apiGroups: [""]
  resources: ["configmaps", "secrets"]
  verbs: ["get", "list"]
```

**Key Points:**
- `apiGroups: [""]` = Core Kubernetes API (pods, services, configmaps, etc.)
- `resources` = What you can access (pods, services, configmaps, secrets)
- `verbs` = What actions (get, list, create, update, delete, watch)

**Common Verbs:**
- `get` = Read one resource
- `list` = Read all resources of this type
- `create` = Create new resources
- `update` = Modify existing resources
- `delete` = Remove resources
- `watch` = Monitor changes in real-time

### 3. RoleBinding
Connects a **ServiceAccount** (or user) to a **Role**.

**Example:**
```yaml
apiVersion: rbac.authorization.k8s.io/v1
kind: RoleBinding
metadata:
  name: auth-service-binding
  namespace: staging
subjects:
- kind: ServiceAccount
  name: auth-service-sa
  namespace: staging
roleRef:
  kind: Role
  name: service-role
  apiGroup: rbac.authorization.k8s.io/v1
```

**This means:** The `auth-service-sa` ServiceAccount gets the permissions defined in `service-role`.

## Our RBAC Setup

### ServiceAccounts
We create one ServiceAccount per service:
- `auth-service-sa`
- `user-service-sa`
- `product-service-sa`
- `order-service-sa`
- `payment-service-sa`
- `gateway-sa`

### Roles

#### 1. **service-role** (Standard Service Permissions)
Services need to:
- Read ConfigMaps (for configuration)
- Read Secrets (for passwords, API keys)
- Read Services (for service discovery)
- Read Endpoints (for service discovery)

#### 2. **deployment-role** (For CI/CD or Management Tools)
Allows:
- Create/Update/Delete Deployments
- Create/Update/Delete Services
- Create/Update/Delete ConfigMaps
- Create/Update/Delete Secrets

### RoleBindings
- Each service gets `service-role` (read-only access)
- CI/CD or management tools get `deployment-role` (full access)

## Applying RBAC

### Apply All RBAC Resources
```bash
# Apply ServiceAccounts
kubectl apply -f service-accounts.yaml

# Apply Roles
kubectl apply -f roles.yaml

# Apply RoleBindings
kubectl apply -f role-bindings.yaml

# Or apply all at once
kubectl apply -f .
```

### Verify RBAC
```bash
# List ServiceAccounts
kubectl get serviceaccounts -n staging
kubectl get serviceaccounts -n prod

# List Roles
kubectl get roles -n staging
kubectl get roles -n prod

# List RoleBindings
kubectl get rolebindings -n staging
kubectl get rolebindings -n prod

# Check what permissions a ServiceAccount has
kubectl describe rolebinding auth-service-binding -n staging
```

## Using ServiceAccounts in Deployments

When you create a Deployment, specify the ServiceAccount:

```yaml
apiVersion: apps/v1
kind: Deployment
metadata:
  name: auth-service
  namespace: staging
spec:
  template:
    spec:
      serviceAccountName: auth-service-sa  # ← Use the ServiceAccount
      containers:
      - name: auth-service
        image: auth-service:latest
```

## Security Best Practices

1. **Principle of Least Privilege**
   - Only give services the minimum permissions they need
   - Start with read-only, add more only if needed

2. **Separate ServiceAccounts**
   - Each service gets its own ServiceAccount
   - Makes auditing easier ("Who did what?")

3. **Namespace Isolation**
   - Roles are namespace-scoped
   - Services in `staging` can't access `prod` resources

4. **Regular Audits**
   - Review permissions periodically
   - Remove unused ServiceAccounts

## Troubleshooting

### "Forbidden" Errors
If a service gets "Forbidden" errors:
1. Check if ServiceAccount exists: `kubectl get sa -n <namespace>`
2. Check if RoleBinding exists: `kubectl get rolebindings -n <namespace>`
3. Check Role permissions: `kubectl describe role <role-name> -n <namespace>`
4. Verify ServiceAccount is used in Deployment: `kubectl describe deployment <name> -n <namespace>`

### Test Permissions
```bash
# Test if a ServiceAccount can read ConfigMaps
kubectl auth can-i get configmaps --as=system:serviceaccount:staging:auth-service-sa -n staging

# Test if a ServiceAccount can create pods
kubectl auth can-i create pods --as=system:serviceaccount:staging:auth-service-sa -n staging
```

## Next Steps
- [x] ServiceAccounts created
- [x] Roles defined
- [x] RoleBindings configured
- [ ] Use ServiceAccounts in Deployments
- [ ] Set up ClusterRoles for cross-namespace access (if needed)
