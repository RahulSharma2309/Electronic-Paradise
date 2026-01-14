# Kubernetes Deployments - Electronic Paradise

## What is a Deployment?

Think of a **Deployment** as a **job description** for Kubernetes:

```
Deployment = "Hey Kubernetes, please run my application!"
```

### Real-World Analogy

**Without Deployment:**
- You: "I want to run auth-service"
- Kubernetes: "How? Where? What image? How many copies?"
- You: "Umm..."

**With Deployment:**
- You: "Here's a Deployment file"
- Kubernetes: "Got it! Running 2 copies of auth-service using image X, with these settings..."
- You: "Perfect!"

## What a Deployment Contains

A Deployment tells Kubernetes:

1. **What to run** (Docker image)
2. **How many copies** (replicas)
3. **Which ServiceAccount** to use (identity)
4. **Environment variables** (configuration)
5. **Resource limits** (CPU, memory)
6. **Health checks** (how to know if it's healthy)
7. **Restart policy** (what to do if it crashes)

## Our Deployment Structure

```
deployments/
├── README.md                    # This file
├── auth-service/
│   ├── deployment.yaml         # Runs the auth-service pods
│   └── service.yaml             # Makes auth-service accessible
├── user-service/
│   ├── deployment.yaml
│   └── service.yaml
├── product-service/
│   ├── deployment.yaml
│   └── service.yaml
├── order-service/
│   ├── deployment.yaml
│   └── service.yaml
├── payment-service/
│   ├── deployment.yaml
│   └── service.yaml
└── gateway/
    ├── deployment.yaml
    └── service.yaml
```

## Key Concepts

### 1. Deployment vs Pod

- **Pod** = One running container (like one employee)
- **Deployment** = Manager that creates and manages pods (like a manager hiring employees)

**Example:**
```yaml
replicas: 2  # Run 2 copies
```
This creates 2 pods. If one crashes, Deployment automatically creates a new one!

### 2. Kubernetes Service

A **Service** is like a **phone book** or **name tag**:

- Pods can have random IPs (they change when restarted)
- Service gives them a **stable name** and **IP**
- Other services can find them by name: `http://auth-service:80`

**Example:**
```yaml
# Service makes auth-service accessible at:
# http://auth-service:80 (inside cluster)
```

### 3. ConfigMap vs Secret

- **ConfigMap** = Non-sensitive configuration (like app settings)
- **Secret** = Sensitive data (like passwords, API keys)

**Example:**
```yaml
# ConfigMap
environment: staging
log-level: info

# Secret
database-password: Your_password123
api-key: secret-key-here
```

## How to Use

### Apply All Deployments

```bash
# Apply all services to staging
kubectl apply -f deployments/ -n staging

# Apply all services to production
kubectl apply -f deployments/ -n prod
```

### Check Status

```bash
# See all deployments
kubectl get deployments -n staging

# See all pods
kubectl get pods -n staging

# See all services
kubectl get services -n staging

# Detailed status
kubectl describe deployment auth-service -n staging
```

### Update a Deployment

```bash
# Update the image
kubectl set image deployment/auth-service \
  auth-service=my-image:v2.0.0 -n staging

# Or edit the deployment
kubectl edit deployment auth-service -n staging
```

### Scale a Deployment

```bash
# Scale to 3 replicas
kubectl scale deployment auth-service --replicas=3 -n staging
```

## Deployment Lifecycle

```
1. You create Deployment
   ↓
2. Kubernetes creates Pods
   ↓
3. Pods start containers
   ↓
4. Health checks verify they're healthy
   ↓
5. Service makes them accessible
   ↓
6. If pod crashes → Deployment creates new one
```

## Best Practices

1. **Always specify resource limits**
   - Prevents one service from using all resources

2. **Use health checks**
   - Kubernetes knows when service is ready
   - Automatically restarts unhealthy pods

3. **Use ServiceAccounts**
   - Each service has its own identity
   - Better security and auditing

4. **Use ConfigMaps for configuration**
   - Easy to update without rebuilding images
   - Separate config from code

5. **Use Secrets for sensitive data**
   - Never hardcode passwords
   - Kubernetes encrypts secrets at rest

## Troubleshooting

### Pod not starting?

```bash
# Check pod status
kubectl get pods -n staging

# Check pod logs
kubectl logs <pod-name> -n staging

# Describe pod (see events)
kubectl describe pod <pod-name> -n staging
```

### Service not accessible?

```bash
# Check service exists
kubectl get services -n staging

# Check service endpoints
kubectl get endpoints -n staging

# Test from inside cluster
kubectl run test-pod --image=curlimages/curl -it --rm -- \
  curl http://auth-service:80/api/health
```

### Deployment not updating?

```bash
# Check deployment status
kubectl rollout status deployment/auth-service -n staging

# View rollout history
kubectl rollout history deployment/auth-service -n staging

# Rollback if needed
kubectl rollout undo deployment/auth-service -n staging
```

## Next Steps

After creating deployments:
1. ✅ Deployments created
2. ✅ Services created
3. ⏳ ConfigMaps created
4. ⏳ Secrets created
5. ⏳ Test in staging
6. ⏳ Deploy to production
