# Glossary (Plain Language)

This glossary keeps words consistent across docs. It’s intentionally non-academic.

---

## Core architecture terms

- **Service (microservice)**: A small backend app that does *one business area* (Auth, Users, Products, Orders, Payments). It has its own code and its own database.
- **Gateway (API Gateway / reverse proxy)**: The “front door” for the backend. The frontend calls one base URL and the gateway routes requests to the correct service.
- **Endpoint**: A URL + HTTP method like `POST /api/auth/login`.
- **DTO (Data Transfer Object)**: The JSON shape we send/receive. It’s the “contract” between frontend and backend (or service-to-service).
- **Database-per-service**: Each service owns its own DB. Other services do not directly read/write it.

---

## Reliability and distributed workflow terms

- **Service-to-service call**: One backend service calling another backend service (usually HTTP in this repo).
- **Orchestrator**: A service that coordinates a multi-step workflow across services. Here, the **Order Service** orchestrates checkout.
- **Saga (simplified)**: A multi-step workflow across services that uses **compensation** to undo earlier steps if something later fails.
- **Compensation / rollback**: The “undo” step. Example: if payment succeeded but stock reservation fails, refund the payment.
- **Idempotency**: “If I retry the same request twice, the system behaves as if it happened once.” This prevents double charges and duplicate orders.
- **Eventual consistency**: Data becomes correct “soon”, not instantly (common when using async messaging or search indexing).

---

## Communication terms

- **HTTP (sync)**: Request/response, user typically waits.
- **Message queue / service bus (async)**: Send a message and process later; improves decoupling but adds complexity (duplicates, ordering, observability).
- **BFF (Backend For Frontend)**: A backend layer built specifically to serve the UI, usually by aggregating multiple service calls into one response.

---

## Observability terms

- **Logs**: Text records of what happened.
- **Metrics**: Numbers over time (request count, latency, error rate).
- **Tracing**: A “map” of one request as it travels across services (distributed tracing).

