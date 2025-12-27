# Start Here — “Fly-through” Guide (PO / PM / Dev / QA / Frontend / DevOps)

If you want to understand this system **like a fly on the wall** (what it does, why it was built this way, and how the code behaves), start here.

This repo is intentionally documented from multiple viewpoints:

- **Product Owner / Product Manager**: what the product is and what users do
- **Developer**: how requests move through the gateway/services and where the code lives
- **Tester (QA)**: what to verify, common failure modes, where to observe behavior (Swagger, logs)
- **Frontend developer**: which APIs exist and which flows depend on which services
- **DevOps / platform**: how to run it, what ports exist, health checks

---

## Quick orientation (5 minutes)

- **What this is**: an MVP e-commerce app designed as a **learning system** for “enterprise-ish” patterns (microservices, gateway, distributed workflows, rollback/compensation).
- **What users can do (MVP)**: signup, login, browse products, manage a cart (frontend-only), add wallet balance, checkout (order + payment + stock), view order history.
- **Where the code lives**:
  - Frontend: `frontend/`
  - Gateway: `gateway/`
  - Services: `services/*-service/`
  - Infra (docker-compose): `infra/`

If some terms feel “too enterprise”, open: [`GLOSSARY.md`](GLOSSARY.md)

---

## Choose your path (by role)

### If you are a **PO / PM** (understand the product)

Read in this order:

1. **Project overview**: [`PROJECT_OVERVIEW.md`](PROJECT_OVERVIEW.md)
2. **User journeys (MVP flows)**: [`Functionality/README.md`](Functionality/README.md)
3. **Product thinking** (vision/strategy/roadmap): [`PRODUCT_STRATEGY.md`](PRODUCT_STRATEGY.md) and `docs/product-documents/`

What you should get at the end:

- What the MVP includes and excludes
- Why this product category and what’s next
- How the user journeys map to services

---

### If you are a **Developer** (understand the system + code flow)

Read in this order:

1. “Novel style” walkthrough (best single document): [`LEARNING_GUIDE.md`](LEARNING_GUIDE.md)
2. Architecture: `docs/diagram/architecture.md` and `docs/diagram/low-level-design.md`
3. Services index: [`Services/README.md`](Services/README.md)
4. Decisions + patterns (how to extend it correctly): [`ENGINEERING_PLAYBOOK.md`](ENGINEERING_PLAYBOOK.md)

What you should get at the end:

- How a request travels: UI → Gateway → Service → DB (and sometimes service → service)
- Where “orchestration” happens (Order Service) and how rollback works
- How to decide future implementations (catalog patterns, async messaging, idempotency)

---

### If you are a **Tester / QA** (what to verify and how)

Start here:

1. User flows: [`Functionality/README.md`](Functionality/README.md)
2. Services index (endpoints + error codes): [`Services/README.md`](Services/README.md)

How to test quickly:

- **Swagger UIs** (after running the stack):
  - Auth: `http://localhost:5001/swagger`
  - User: `http://localhost:5005/swagger`
  - Product: `http://localhost:5002/swagger`
  - Order: `http://localhost:5004/swagger`
  - Payment: `http://localhost:5003/swagger`

High-value test scenarios (MVP):

- **Signup**:
  - Duplicate email → `409`
  - Duplicate phone → `409`
  - User Service down → `503` (registration should not “partially create” a user)
- **Login**:
  - Wrong password → `401`
- **Add balance**:
  - Negative/zero amount → `400`
- **Checkout**:
  - Insufficient wallet → `409`
  - Insufficient stock → `409`
  - Stock reservation failure after payment → refund should happen (compensation)

Where to learn “expected failures”:

- [`ENGINEERING_PLAYBOOK.md`](ENGINEERING_PLAYBOOK.md) (failure modes and why they happen)

---

### If you are a **Frontend developer**

Start here:

1. User journeys: [`Functionality/README.md`](Functionality/README.md)
2. Gateway routing concept: [`Services/API_GATEWAY.md`](Services/API_GATEWAY.md)
3. Endpoint contracts: service docs in [`Services/`](Services/)

Important reality in MVP:

- Cart is **frontend-only** right now (documented in `ADD_TO_CART_FLOW.md`). This is a deliberate MVP simplification.

---

### If you are **DevOps / running the stack**

Start here:

- **Run everything**: `infra/docker-compose.yml` (referenced from [`docs/README.md`](README.md))
- **Service ports**: [`Services/README.md`](Services/README.md#quick-reference)
- **Health endpoints**: each service exposes `/health` (see service docs / compose config)

---

## “I only have 30 minutes”

- Read [`LEARNING_GUIDE.md`](LEARNING_GUIDE.md) sections 1–4 (product + architecture + request lifecycle)
- Then read the two most important service docs:
  - [`Services/ORDER_SERVICE.md`](Services/ORDER_SERVICE.md) (the orchestrator)
  - [`Services/PAYMENT_SERVICE.md`](Services/PAYMENT_SERVICE.md) (wallet debit/refund + audit trail)

---

## How to extend the project (without guessing)

When you add new features (catalog, search, messaging, etc.), use:

- [`ENGINEERING_PLAYBOOK.md`](ENGINEERING_PLAYBOOK.md) — the “why this pattern” guide
- [`GLOSSARY.md`](GLOSSARY.md) — shared vocabulary so docs stay consistent

