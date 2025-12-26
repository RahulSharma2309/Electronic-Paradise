This repository contains a microservices e-commerce MVP example. See CONTRIBUTING.md for developer workflow and guidelines.

Quick start

1. Start SQL Server container

```powershell
cd infra
docker compose up -d mssql
```

2. Run a backend service locally

```powershell
cd services\product-service
dotnet run
```

3. Run the frontend

```powershell
cd frontend
npm install
npm start
```

Repository layout

- frontend/: React app
- gateway/: YARP API gateway
- services/: microservices (auth, user, product, order, payment)
- infra/: docker-compose and helper scripts
