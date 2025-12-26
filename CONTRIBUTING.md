# Contributing

Thank you for your interest in contributing to this microservices example repository.

Ways to contribute

- Report bugs using the issue tracker (use the Bug report template).
- Suggest features or improvements (open a discussion or issue).
- Submit pull requests with bug fixes, tests, or documentation improvements.

Development workflow

1. Fork the repository and create a feature branch from `main`:

```bash
git checkout -b feat/my-change
```

2. Implement your change. Follow the existing code layout:

- Backend: `services/*` (each is an independent ASP.NET Core project)
- Frontend: `frontend/`
- Gateway: `gateway/`

3. Run and test locally. Typical commands:

```powershell
# Start only the SQL Server container
cd infra
docker compose up -d mssql

# Run a service (example)
cd services/product-service
dotnet run

# Run frontend
cd frontend
npm install
npm start
```

4. Keep commits small and focused. Use clear commit messages and reference issues when applicable.

Pull request checklist

- [ ] My code builds and runs locally
- [ ] I added/updated tests where appropriate
- [ ] I updated documentation where necessary
- [ ] The PR description explains the change and why

Code style

- Keep code consistent with the existing project style (C# 10/11 conventions for backend, JS/React style for frontend).

Thank you for your contributions!
