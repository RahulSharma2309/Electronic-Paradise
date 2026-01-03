# Electronic Paradise - E-Commerce Microservices Platform

A comprehensive microservices-based e-commerce platform built with .NET 8, demonstrating modern software architecture patterns, best practices, and scalable design.

## 🚀 Quick Start

> **📖 For detailed step-by-step instructions, see [SETUP_GUIDE.md](SETUP_GUIDE.md)**

### Prerequisites

- **Docker Desktop** (or Docker Engine + Docker Compose)
- **Git** (for cloning)
- **GitHub Account** (for Personal Access Token)

### Quick Setup (5 Steps)

1. **Clone the repository**

   ```bash
   git clone <repository-url>
   cd Electronic-Paradise
   ```

2. **Set up GitHub Token** (one-time setup)

   **Windows:**

   ```powershell
   cd infra
   .\setup-env.ps1
   ```

   **Linux/Mac:**

   ```bash
   cd infra
   chmod +x setup-env.sh
   ./setup-env.sh
   ```

3. **Build Docker images**

   ```bash
   cd infra
   docker-compose build
   ```

4. **Start all services**

   ```bash
   docker-compose up -d
   ```

5. **Access the application**
   - **Frontend**: http://localhost:3000
   - **API Gateway**: http://localhost:5000
   - **Swagger Docs**: http://localhost:5001/swagger (Auth), http://localhost:5005/swagger (User), etc.

**📚 Need detailed instructions?** See [SETUP_GUIDE.md](SETUP_GUIDE.md) for complete step-by-step guide with troubleshooting.

## 📁 Project Structure

```
Electronic-Paradise/
├── services/              # Microservices
│   ├── auth-service/      # Authentication & Authorization
│   ├── user-service/      # User profiles & wallet management
│   ├── product-service/   # Product catalog & inventory
│   ├── order-service/     # Order management & orchestration
│   └── payment-service/   # Payment processing
├── gateway/               # API Gateway (YARP)
├── frontend/              # React frontend application
├── platform/              # Ep.Platform shared library
└── infra/                 # Infrastructure & Docker setup
    ├── docker-compose.yml # All services orchestration
    ├── setup-env.ps1      # Windows setup script
    └── setup-env.sh       # Linux/Mac setup script
```

## 🛠️ Development

### Running Services Locally

Each service can be run independently:

```bash
cd services/user-service
dotnet run
```

### Running with Docker

```bash
cd infra
docker-compose up -d          # Start all services
docker-compose logs -f         # View logs
docker-compose down            # Stop all services
```

### VS Code Tasks

Use VS Code tasks for common operations:

- **Ctrl+Shift+P** → "Tasks: Run Task"
- Available tasks:
  - `Docker: Start All Services`
  - `Docker: Stop All Services`
  - `Docker: Build All Services`
  - Individual service tasks

## 📚 Documentation

### 🧭 Start Here

- **[START_HERE.md](docs/START_HERE.md)** - Choose your role-based learning path
- **[DOCUMENTATION_INDEX.md](docs/DOCUMENTATION_INDEX.md)** - Complete file catalog

### 📖 Key Documentation

- **[Project Overview](docs/1-getting-started/PROJECT_OVERVIEW.md)** - Vision, goals, and quickstart
- **[Tech Stack](docs/1-getting-started/TECH_STACK.md)** - Technologies and rationale
- **[Learning Guide](docs/2-learning-guide/LEARNING_GUIDE.md)** - End-to-end walkthrough
- **[System Architecture](docs/6-architecture/SYSTEM_ARCHITECTURE.md)** - High-level design
- **[Platform Architecture](docs/6-architecture/PLATFORM_ARCHITECTURE.md)** - Ep.Platform NuGet design

### 🔧 Setup

- **[Complete Setup Guide](SETUP_GUIDE.md)** - ⭐ **Start here!** Complete step-by-step instructions for running locally

## 🔐 GitHub Token Setup

The services require a GitHub Personal Access Token to access the `Ep.Platform` NuGet package.

**Quick Setup:**

```powershell
cd infra
.\setup-env.ps1
```

The `.env` file created by the setup script will be automatically used by Docker Compose on every build, so you only need to set it up once per machine.

**Manual Setup:**

1. Copy `infra/.env.example` to `infra/.env`
2. Edit `infra/.env` and add your GitHub token
3. Docker Compose will automatically use it

See [SETUP_GUIDE.md](SETUP_GUIDE.md) for complete step-by-step instructions.

## 🏗️ Architecture

- **Microservices**: Independent, scalable services
- **API Gateway**: Single entry point with routing
- **N-tier Architecture**: Abstraction, Core, API layers
- **Platform Library**: Shared infrastructure via NuGet
- **Docker**: Containerized services for easy deployment
- **SQL Server**: Database per service pattern

## 🤝 Contributing

See [CONTRIBUTING.md](CONTRIBUTING.md) for guidelines.

## 📄 License

[Add your license here]

---

**Note:** Make sure to set up your GitHub token before building Docker images. The setup script makes this easy and ensures the token persists across sessions.
