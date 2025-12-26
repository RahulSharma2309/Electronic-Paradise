Service run notes

Each service is a standalone ASP.NET Core Web API. To run a service:

1. Open a terminal in the service folder, e.g. `services\auth-service`
2. Run `dotnet restore` then `dotnet run`.

Each service includes a health endpoint at `/api/health` and Swagger UI in development.

Ensure SQL Server is running (see `infra/docker-compose.yml`). The connection strings in each service point to `localhost:1433` and the DB name is per-service (authdb, userdb, productdb, orderdb, paymentdb).

## Docker vs Local

You can run the stack in two ways:

1. Local (recommended for development of a single service)

- Run SQL Server locally (we provide a docker-compose for MSSQL only), then run services with `dotnet run`.

  Example:

  ```powershell
  # start only the database
  cd infra
  docker-compose up -d mssql

  # run product service locally
  cd ..\services\product-service
  dotnet restore
  dotnet run
  ```

  In this mode, services bind to the ports defined in their `Properties/launchSettings.json` (5001..5005). The gateway (if run locally) reads `gateway/ocelot.json` which proxies to `http://localhost:5001`, etc.

2. Full stack with Docker Compose (recommended for end-to-end integration)

- This will build container images for each service, start SQL Server, and the YARP gateway. The compose file maps container ports to the host ports used for local runs, so you can still hit `http://localhost:5000` for the gateway.

  From `infra/` run:

  ```powershell
  cd infra
  docker-compose up --build -d
  ```

  Notes:

  - The gateway inside Docker uses `ocelot.docker.json` which points to container hostnames (e.g. `http://product-service:80`).
  - Containers expose their services on the same host ports (5000..5005) so you can test via `http://localhost:5000/api/products`.

Stopping and cleanup:

```powershell
cd infra
docker-compose down
```

## Healthchecks and readiness

I added container healthchecks for SQL Server and all services. You can inspect container health with:

```powershell
docker ps
docker inspect --format='{{.State.Health}}' <container-name>
```

If you prefer a script to block until a service is available, use the included `infra/wait-for.sh` script:

```powershell
# wait for SQL Server TCP port
bash infra/wait-for.sh localhost:1433 60

# wait for gateway HTTP health endpoint
bash infra/wait-for.sh http://localhost:5000/api/health 60
```

Note: the compose file uses healthchecks to report containers' status. For local development you can still run services with `dotnet run` — the healthchecks are primarily for Docker-managed runs.

## CI workflow

I added a GitHub Actions workflow at `.github/workflows/ci.yml` that:

- builds and starts the full stack using `infra/docker-compose.yml`
- waits for the gateway health endpoint
- runs `infra/smoke-test.sh` to perform an end-to-end smoke test (register, login, wallet creation, order)
- tears down the stack

This provides a lightweight CI check that the stack can build and the main flows work.
