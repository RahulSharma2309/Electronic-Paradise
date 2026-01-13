# 📘 Continuous Deployment (CD) Pipeline - Complete Guide

## 📖 Table of Contents
1. [CI vs CD - Understanding the Difference](#ci-vs-cd---understanding-the-difference)
2. [What is a Staging Environment?](#what-is-a-staging-environment)
3. [Smoke Tests vs CI Tests](#smoke-tests-vs-ci-tests)
4. [Benefits of CD Pipeline](#benefits-of-cd-pipeline)
5. [CD Workflow Architecture](#cd-workflow-architecture)
6. [Implementation Components](#implementation-components)
7. [Best Practices](#best-practices)

---

## 🎯 CI vs CD - Understanding the Difference

### Current State: CI (Continuous Integration)
**What CI Does:**
- Builds code on every commit
- Runs automated tests (unit, integration, component)
- Checks code quality with SonarCloud
- Builds Docker images
- Pushes images to GitHub Container Registry (GHCR)

**CI Output:** Docker images sitting in a registry, NOT running anywhere

**Files Involved:**
- `.github/workflows/ci.yml` - Main CI pipeline
- `scripts/get-next-version.sh` - Version calculation
- All test files in `*/tests/` directories

---

### What CD Adds: Continuous Deployment/Delivery
**What CD Does:**
- Pulls Docker images from registry
- Deploys to staging environment
- Runs smoke tests on deployed system
- Sends notifications about deployment status
- Rolls back automatically on failure

**CD Output:** Application running in a staging environment, validated and ready

**Files to Create:**
- `.github/workflows/cd-staging.yml` - CD pipeline workflow
- `k8s/staging/` - Kubernetes manifests for staging
- `tests/smoke/` - Smoke test scripts
- `scripts/deploy-staging.sh` - Deployment helper script

---

## 🏗️ What is a Staging Environment?

### The 3-Environment Strategy

| Environment | Purpose | Infrastructure | Data | Access |
|------------|---------|----------------|------|--------|
| **Development** | Local coding | Your machine | Fake/minimal | Developer only |
| **Staging** | Pre-production testing | K8s cluster (staging namespace) | Test data | Team + QA |
| **Production** | Real users | K8s cluster (prod namespace) | Real data | Public |

### Staging Environment Characteristics

**1. Production-Like (Mirror)**
- Same infrastructure setup (Kubernetes)
- Same number of services (all 7 microservices)
- Same database structure (but separate instance)
- Same configurations (except URLs and secrets)

**2. Safe to Break**
- Separate from production
- No impact on real users
- Can be reset/rebuilt anytime
- Uses test data only

**3. Purpose**
- **Final validation** before production deployment
- **Integration testing** with real infrastructure
- **Demo environment** for stakeholders and QA
- **Performance testing** under realistic conditions
- **Rollback practice** before production

### Your Staging Setup (Local K3s)

**Location:** K3s cluster, namespace `staging`

**Components:**
```
Staging Namespace
├── auth-service (deployment + service)
├── user-service (deployment + service)
├── product-service (deployment + service)
├── order-service (deployment + service)
├── payment-service (deployment + service)
├── gateway (deployment + service)
├── frontend (deployment + service)
└── SQL Server (deployment + PVC)
```

**Access:** `http://staging.local` or `http://localhost:8080` (via port forwarding)

**Configuration Files:**
- `k8s/staging/namespace.yaml` - Namespace definition
- `k8s/staging/*-deployment.yaml` - Service deployments
- `k8s/staging/*-service.yaml` - Service networking
- `k8s/staging/configmap.yaml` - Environment variables
- `k8s/staging/secrets.yaml` - Sensitive data (encoded)

---

## 🔥 Smoke Tests vs CI Tests

### CI Tests (What You Already Have)

**Unit Tests**
- **Test:** Individual functions and methods
- **When:** Every commit, before merge
- **Where:** GitHub Actions runner (in-memory)
- **Example:** "Does `CalculatePrice()` return correct value?"
- **Files:** `*/tests/UnitTests/*.cs`, `frontend/src/**/*.test.tsx`

**Integration Tests**
- **Test:** Service internals with database
- **When:** Every commit, before merge
- **Where:** GitHub Actions runner (Docker SQL Server)
- **Example:** "Does Auth API create user in database correctly?"
- **Files:** `*/tests/IntegrationTests/*.cs`

**Component Tests**
- **Test:** Frontend components in isolation
- **When:** Every commit, before merge
- **Where:** GitHub Actions runner (jsdom)
- **Example:** "Does LoginForm render and handle input?"
- **Files:** `frontend/src/components/**/*.test.tsx`

**CI Tests Say:** "The code works correctly in isolation" ✅

---

### Smoke Tests (What CD Adds)

**Smoke Tests**
- **Test:** Deployed system end-to-end
- **When:** After deployment to staging
- **Where:** Against actual staging environment
- **Example:** "Can I login to the running app and view products?"
- **Files:** `tests/smoke/*.spec.js` (Playwright or similar)

**Smoke Tests Say:** "The deployed system works as a whole" ✅

---

### Key Differences Table

| Aspect | CI Tests | Smoke Tests |
|--------|----------|-------------|
| **Timing** | Before deployment | After deployment |
| **Environment** | Isolated (GitHub Actions) | Real staging environment |
| **Purpose** | Verify code correctness | Verify deployment success |
| **Depth** | Deep (100+ tests) | Shallow (5-10 critical paths) |
| **Duration** | 5-10 minutes | 1-2 minutes |
| **Focus** | Find bugs in code | Find deployment/config issues |
| **Mocking** | Heavy mocking | No mocking (real services) |
| **Failures** | Code bugs | Infrastructure issues |

---

### Smoke Test Examples for Electronic-Paradise

**1. Health Check Test**
- Verify all 5 microservices respond to `/health` endpoint
- Ensure gateway is routing correctly
- File: `tests/smoke/health.spec.js`

**2. Authentication Flow Test**
- Login with test credentials
- Verify JWT token is returned
- Verify dashboard is accessible
- File: `tests/smoke/auth.spec.js`

**3. Product Listing Test**
- Access products page
- Verify at least 1 product is displayed
- Verify product images load
- File: `tests/smoke/products.spec.js`

**4. Order Creation Test**
- Add product to cart
- Proceed to checkout
- Create order
- Verify order ID is returned
- File: `tests/smoke/order.spec.js`

**5. Frontend Load Test**
- Access homepage
- Verify no JavaScript errors
- Verify all static assets load
- File: `tests/smoke/frontend.spec.js`

**What Smoke Tests DON'T Do:**
- ❌ Deep validation of business logic (that's integration tests)
- ❌ Performance/load testing (that's separate)
- ❌ Security penetration testing (that's separate)
- ❌ Test every edge case (that's unit tests)

**What Smoke Tests DO:**
- ✅ Quick sanity check: "Is the app functional?"
- ✅ Critical path validation: "Can users do basic tasks?"
- ✅ Deployment verification: "Did deployment break anything?"

---

## 🎯 Benefits of CD Pipeline

### 1. Faster Feedback Loop

**Without CD:**
```
Developer merges to main
→ Wait for CI (10 min)
→ Manually deploy to staging (30 min)
→ Manually test (1 hour)
→ Find bug (too late!)

Total: ~2 hours to discover deployment issues
```

**With CD:**
```
Developer merges to main
→ CI runs (10 min)
→ CD auto-deploys (2 min)
→ Smoke tests run (2 min)
→ Failure detected immediately
→ Slack alert sent

Total: ~15 minutes to discover deployment issues
```

**Benefit:** 8x faster feedback on deployment issues

---

### 2. Confidence Before Production

**Without CD:**
```
Developer: "It works on my machine! 🤷"
CI: "Tests pass locally ✅"
Production: *Deploys*
Production: *Crashes* ❌
Users: *Angry* 😡
```

**With CD:**
```
Developer: "It works on my machine!"
CI: "Tests pass ✅"
CD: "Works in staging ✅"
CD: "Smoke tests pass ✅"
Production: *Deploys confidently*
Production: *Works* ✅
Users: *Happy* 😊
```

**Benefit:** Staging validates deployment before production

---

### 3. Catch Infrastructure Issues Early

**Issues CI Tests CANNOT Catch:**
- ❌ Kubernetes resource misconfigurations
- ❌ Service mesh networking issues
- ❌ Ingress routing problems
- ❌ Database migration failures in real DB
- ❌ Environment variable typos
- ❌ Docker image pull authentication issues
- ❌ PersistentVolume mount failures
- ❌ Service-to-service communication in real cluster

**Issues CD Smoke Tests WILL Catch:**
- ✅ All of the above!

**Benefit:** Discover infrastructure problems before production

---

### 4. Always-Ready Demo Environment

**Without CD:**
```
Stakeholder: "Can I see the latest features?"
Developer: "Let me deploy to staging first..."
*30 minutes of manual work*
Developer: "Okay, it's ready"
Stakeholder: "Actually, I have a meeting now..."
```

**With CD:**
```
Stakeholder: "Can I see the latest features?"
Developer: "Sure! staging.yourapp.com is always current"
Stakeholder: *Immediately accesses* ✅
```

**Benefit:** Staging is always up-to-date with main branch

---

### 5. Production-Like Testing

**Differences Between CI and Real Infrastructure:**
- Network latency (CI is fast, real clusters have latency)
- Resource constraints (CI has unlimited memory, K8s has limits)
- Service discovery (CI uses localhost, K8s uses DNS)
- Secrets management (CI uses env vars, K8s uses secrets)
- Load balancing behavior
- SSL/TLS termination
- Ingress routing rules

**Benefit:** Staging tests your app in production-like conditions

---

### 6. Practice for Production Deployments

**What You Learn in Staging:**
- Deployment process
- Rollback procedures
- Monitoring and alerting
- Debugging in Kubernetes
- Performance under load

**Benefit:** Deployment to production becomes routine, not risky

---

## 🔄 CD Workflow Architecture

### Complete CI/CD Flow

```
┌─────────────────────────────────────────────────────────────────┐
│                      Developer Workflow                          │
└─────────────────────────────────────────────────────────────────┘
                              ↓
                    1. Push to feature branch
                              ↓
┌─────────────────────────────────────────────────────────────────┐
│                     CI Pipeline (PR Build)                       │
│  - Run unit tests                                                │
│  - Run integration tests                                         │
│  - Run SonarCloud analysis                                       │
│  - Build Docker images (alpha tags)                              │
│  - Push to GHCR                                                  │
└─────────────────────────────────────────────────────────────────┘
                              ↓
                    2. Code review & merge to main
                              ↓
┌─────────────────────────────────────────────────────────────────┐
│                CI Pipeline (Main Branch Build)                   │
│  - Run all tests again                                           │
│  - Build Docker images (production tags: v1.0.0, latest)         │
│  - Push to GHCR                                                  │
└─────────────────────────────────────────────────────────────────┘
                              ↓
                    3. CI completes successfully
                              ↓
┌─────────────────────────────────────────────────────────────────┐
│                   CD Pipeline (Staging Deploy) 🆕                │
│  Step 1: Authenticate to K8s cluster                             │
│  Step 2: Pull images from GHCR                                   │
│  Step 3: Update K8s deployments                                  │
│  Step 4: Wait for pods to be ready (kubectl rollout status)     │
│  Step 5: Run smoke tests                                         │
│  Step 6: If pass → Success notification                          │
│          If fail → Rollback & alert                              │
└─────────────────────────────────────────────────────────────────┘
                              ↓
                    ✅ App running in staging
                              ↓
                    4. Manual validation by QA
                              ↓
                    5. Manual trigger to production (later)
```

---

## 🛠️ Implementation Components

### 1. GitHub Actions Workflow

**File:** `.github/workflows/cd-staging.yml`

**Trigger:** Runs when CI pipeline completes successfully on main branch

**Key Steps:**
1. Setup kubectl (authenticate to K8s cluster)
2. Pull latest images from GHCR
3. Deploy to staging namespace using kubectl
4. Wait for deployments to stabilize
5. Run smoke tests against staging
6. Send notifications (Slack/email)
7. Rollback on failure

**Secrets Required:**
- `KUBECONFIG` - Kubernetes cluster credentials
- `SLACK_WEBHOOK` - Notification webhook URL

---

### 2. Kubernetes Manifests

**Directory:** `k8s/staging/`

**Files Structure:**
```
k8s/staging/
├── namespace.yaml              # Create staging namespace
├── configmap.yaml              # Environment variables
├── secrets.yaml                # Sensitive data (base64 encoded)
├── auth-deployment.yaml        # Auth service deployment
├── auth-service.yaml           # Auth service networking
├── user-deployment.yaml        # User service deployment
├── user-service.yaml           # User service networking
├── product-deployment.yaml     # Product service deployment
├── product-service.yaml        # Product service networking
├── order-deployment.yaml       # Order service deployment
├── order-service.yaml          # Order service networking
├── payment-deployment.yaml     # Payment service deployment
├── payment-service.yaml        # Payment service networking
├── gateway-deployment.yaml     # Gateway deployment
├── gateway-service.yaml        # Gateway service + ingress
├── frontend-deployment.yaml    # Frontend deployment
├── frontend-service.yaml       # Frontend service + ingress
├── sqlserver-deployment.yaml   # SQL Server deployment
├── sqlserver-pvc.yaml          # Persistent volume claim
└── sqlserver-service.yaml      # SQL Server networking
```

**Key Configurations:**
- Image tags: Use production tags from GHCR (e.g., `v1.0.0`)
- Resource limits: CPU and memory limits per pod
- Readiness probes: Health check endpoints
- Liveness probes: Restart unhealthy pods
- Environment variables: From ConfigMap and Secrets

---

### 3. Smoke Tests

**Directory:** `tests/smoke/`

**Files Structure:**
```
tests/smoke/
├── package.json               # Test dependencies (Playwright)
├── playwright.config.js       # Playwright configuration
├── health.spec.js             # Health check tests
├── auth.spec.js               # Authentication tests
├── products.spec.js           # Product listing tests
├── order.spec.js              # Order creation tests
└── frontend.spec.js           # Frontend loading tests
```

**Running Smoke Tests Locally:**
```bash
cd tests/smoke
npm install
npm run test                   # Run all smoke tests
npm run test:health            # Run specific test
```

**Environment Variables:**
- `STAGING_URL` - Base URL for staging environment
- `TEST_USERNAME` - Test user credentials
- `TEST_PASSWORD` - Test user password

---

### 4. Deployment Scripts

**File:** `scripts/deploy-staging.sh`

**Purpose:** Helper script for manual deployments

**Usage:**
```bash
./scripts/deploy-staging.sh                    # Deploy all services
./scripts/deploy-staging.sh auth-service       # Deploy specific service
./scripts/deploy-staging.sh --rollback         # Rollback last deployment
```

**What it does:**
- Validates kubectl connection
- Applies K8s manifests to staging namespace
- Waits for deployments to complete
- Runs smoke tests
- Displays deployment status

---

### 5. Rollback Mechanism

**Automatic Rollback (in CD pipeline):**
- Triggered when smoke tests fail
- Uses `kubectl rollout undo` command
- Reverts to previous working deployment
- Sends alert notification

**Manual Rollback:**
```bash
# Rollback specific service
kubectl rollout undo deployment/auth-service -n staging

# Rollback all services
./scripts/rollback-staging.sh
```

**Rollback Script:** `scripts/rollback-staging.sh`

---

### 6. Notifications

**Success Notification:**
- Sent to Slack channel (e.g., `#deployments`)
- Contains: Deployed version, commit SHA, deployment time
- Includes link to staging environment

**Failure Notification:**
- Sent to Slack channel (e.g., `#alerts`)
- Contains: Failed service, error message, rollback status
- Includes link to failed smoke test results
- Tags relevant team members

**Notification Service:** Slack incoming webhook

**Configuration:** `SLACK_WEBHOOK` secret in GitHub Actions

---

## 📋 Best Practices

### 1. Staging Environment Management

**Keep Staging Stable:**
- Only deploy from main branch (not feature branches)
- Run full CI before CD
- Always run smoke tests after deployment
- Have monitoring and alerting set up

**Staging Data:**
- Use test data, not production data
- Create seed scripts for consistent test data
- Reset staging database periodically
- Don't store sensitive information

---

### 2. Smoke Test Guidelines

**Write Effective Smoke Tests:**
- Test only critical paths (login, view products, create order)
- Keep tests fast (1-2 minutes total)
- Make tests idempotent (can run multiple times)
- Use test user accounts, not admin accounts
- Don't test every edge case (that's integration tests)

**Maintain Smoke Tests:**
- Update when adding critical features
- Remove tests for deprecated features
- Keep test data in sync with schema changes

---

### 3. Deployment Strategy

**Rolling Updates:**
- Deploy one service at a time
- Wait for each service to stabilize
- Reduces risk of complete outage

**Zero-Downtime Deployment:**
- Use readiness probes
- Configure proper graceful shutdown
- Set appropriate terminationGracePeriodSeconds

**Blue-Green Deployment (Advanced):**
- Maintain two staging environments
- Deploy to inactive environment first
- Switch traffic after validation

---

### 4. Monitoring and Alerting

**What to Monitor:**
- Pod health (ready, running)
- Resource usage (CPU, memory)
- Response times
- Error rates
- Deployment success/failure

**Alerting Thresholds:**
- Critical: Deployment failure, smoke test failure
- Warning: High resource usage, slow response times
- Info: Successful deployment

---

### 5. Security Considerations

**Secrets Management:**
- Never commit secrets to Git
- Use Kubernetes Secrets (base64 encoded)
- Rotate secrets regularly
- Limit secret access (RBAC)

**Network Security:**
- Use NetworkPolicies to restrict traffic
- Enable TLS for service-to-service communication
- Configure ingress with SSL certificates

---

## 🎓 Prerequisites

**Before Implementing PBI 2.6 (CD Pipeline):**

1. ✅ Complete Epic 2 (CI Pipeline) - Status: 76% complete
   - Finish PBI 2.5 (Dependency Scanning)
   
2. ❌ Complete Epic 3.1 (K8s Cluster Setup)
   - Install K3s
   - Configure kubectl
   - Set up staging namespace
   
3. ❌ Complete Epic 3.2 (Kubernetes Manifests)
   - Create deployment YAMLs
   - Create service YAMLs
   - Test manual deployment

**Recommended Order:**
```
PBI 2.5 (Dependency Scanning)
    ↓
Epic 3.1 (K8s Setup)
    ↓
Epic 3.2 (K8s Manifests)
    ↓
PBI 2.6 (CD Pipeline) ← You'll be here!
```

---

## 📚 Related Documentation

- [CI Pipeline Architecture](./MODULAR_CI_ARCHITECTURE.md)
- [Image Tagging Strategy](./IMAGE_TAGGING_STRATEGY.md)
- [Semantic Release Guide](./SEMANTIC_RELEASE_GUIDE.md)
- [Project Roadmap](../9-roadmap-and-tracking/PROJECT_ROADMAP.md) - Epic 2 & 3

---

## 🚀 Summary

### What is CD?
Continuous Deployment automates the process of deploying validated code to a staging environment.

### Why CD?
- Faster feedback on deployment issues (15 min vs 2 hours)
- Catch infrastructure problems before production
- Always-ready demo environment
- Confidence in production deployments

### What You'll Build:
- CD workflow (`.github/workflows/cd-staging.yml`)
- K8s staging environment (`k8s/staging/`)
- Smoke tests (`tests/smoke/`)
- Deployment scripts (`scripts/`)
- Notifications (Slack integration)

### When to Implement:
After completing Epic 3.1-3.2 (K8s setup and manifests)

---

**Last Updated:** January 13, 2026  
**Related PBI:** 2.6 - CD Pipeline (Deploy to Staging)  
**Story Points:** 5 points
