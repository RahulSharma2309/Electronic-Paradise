# Frontend (React)

This folder will contain a React app (create-react-app or Vite). The app will call the API Gateway for all operations (auth, products, cart, checkout). For now it's a placeholder and will be scaffolded when backend endpoints are ready.

## Docker

You can also run the frontend as a container and let the full compose bring up everything. From `infra/` run:

```powershell
cd infra
docker-compose up --build -d
```

This maps the frontend to `http://localhost:3000` (nginx serving the production build). The frontend still talks to the gateway at `http://localhost:5000` which the compose file exposes on the host.
