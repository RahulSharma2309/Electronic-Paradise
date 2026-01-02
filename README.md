# 🛒 MVP E-Commerce Microservices Application

A production-ready, full-stack e-commerce application built with microservices architecture, featuring user authentication, product catalog, shopping cart, order management, and payment processing.

---

## 📋 Table of Contents

- [Overview](#overview)
- [Architecture](#architecture)
- [Features](#features)
- [Tech Stack](#tech-stack)
- [Getting Started](#getting-started)
- [Documentation](#documentation)
- [Project Structure](#project-structure)
- [API Endpoints](#api-endpoints)
- [Contributing](#contributing)

---

## 🎯 Overview

This is a **complete e-commerce platform** demonstrating modern microservices architecture with:

- **5 Backend Microservices** (Auth, User, Product, Order, Payment)
- **API Gateway** (YARP-based reverse proxy)
- **React Frontend** (Modern UI with routing and state management)
- **SQL Server** (Separate databases per service)
- **Docker Compose** (Full containerization)

**Perfect for:**
- Learning microservices architecture
- Understanding service-to-service communication
- Practicing Docker & containerization
- Building production-grade applications

---

## 🏗️ Architecture

```
┌──────────────────────────────────────────────────────────┐
│                    Frontend (React)                      │
│                   Port: 3000 (Nginx)                     │
└────────────────────────┬─────────────────────────────────┘
                         │
                         │ HTTP/REST
                         ▼
┌──────────────────────────────────────────────────────────┐
│              API Gateway (YARP)                          │
│                    Port: 5000                            │
│  • Routes requests to microservices                      │
│  • JWT validation (future)                               │
│  • Rate limiting (future)                                │
└───┬────────┬──────────┬──────────┬──────────────────────┘
    │        │          │          │
    ▼        ▼          ▼          ▼
┌────────┐ ┌────────┐ ┌────────┐ ┌────────┐ ┌────────┐
│  Auth  │ │  User  │ │Product │ │ Order  │ │Payment │
│Service │ │Service │ │Service │ │Service │ │Service │
│ :5001  │ │ :5005  │ │ :5002  │ │ :5004  │ │ :5003  │
└───┬────┘ └───┬────┘ └───┬────┘ └───┬────┘ └───┬────┘
    │          │           │          │          │
    └──────────┴───────────┴──────────┴──────────┘
                         │
                         ▼
            ┌─────────────────────────┐
            │   SQL Server (MSSQL)    │
            │   Port: 1433            │
            │                         │
            │  • authdb               │
            │  • userdb               │
            │  • productdb            │
            │  • orderdb              │
            │  • paymentdb            │
            └─────────────────────────┘
```

### Service-to-Service Communication

Some services communicate directly (bypassing gateway):

```
Auth Service ──────► User Service (profile creation, phone check)
Order Service ─────► User Service (get profile)
Order Service ─────► Product Service (validate, reserve stock)
Order Service ─────► Payment Service (process payment)
Payment Service ───► User Service (wallet debit/credit)
```

---

## ✨ Features

### User Features
- ✅ **User Registration** with email, phone, password validation
- ✅ **User Login** with JWT token generation
- ✅ **Profile Management** with wallet balance
- ✅ **Add Balance** to wallet
- ✅ **Order History** with filtering (amount, date) and sorting

### Shopping Features
- ✅ **Product Catalog** with stock management
- ✅ **Add to Cart** (frontend state)
- ✅ **Checkout** with wallet payment
- ✅ **Stock Reservation** during order
- ✅ **Payment Processing** with transaction recording

### Technical Features
- ✅ **Microservices Architecture** (5 independent services)
- ✅ **API Gateway** (YARP reverse proxy)
- ✅ **Atomic Transactions** (user registration, order creation)
- ✅ **Rollback Mechanisms** (failed orders refund automatically)
- ✅ **Guid Consistency** across all services
- ✅ **BCrypt Password Hashing**
- ✅ **JWT Authentication**
- ✅ **Docker Compose** for easy deployment
- ✅ **Swagger Documentation** for all services
- ✅ **Health Checks** for all services
- ✅ **INR Currency** support

---

## 🛠️ Tech Stack

### Frontend
- **React 18** - UI framework
- **React Router v6** - Client-side routing
- **Axios** - HTTP client
- **CSS3** - Modern styling
- **Nginx** - Web server (in Docker)

### Backend
- **.NET 8** - Microservices framework
- **Entity Framework Core** - ORM
- **YARP** - API Gateway (reverse proxy)
- **BCrypt.NET** - Password hashing
- **JWT** - Authentication tokens

### Database
- **SQL Server 2019** - All databases
- **Separate DB per service** (authdb, userdb, productdb, orderdb, paymentdb)

### DevOps
- **Docker** - Containerization
- **Docker Compose** - Multi-container orchestration
- **Nginx** - Frontend web server

---

## 🚀 Getting Started

### Prerequisites

- **Docker Desktop** installed
- **Git** installed
- **Ports available**: 3000, 5000, 5001-5005, 1433

### Quick Start

1. **Clone the repository**
   ```bash
   git clone <repository-url>
   cd MY_Practice
   ```

2. **Start all services with Docker Compose**
   ```bash
   cd infra
   docker-compose up --build -d
   ```

3. **Wait for services to be healthy** (~2-3 minutes)
   ```bash
   docker-compose ps
   ```

4. **Access the application**
   - **Frontend**: http://localhost:3000
   - **API Gateway**: http://localhost:5000
   - **Swagger Docs**:
     - Auth: http://localhost:5001/swagger
     - User: http://localhost:5005/swagger
     - Product: http://localhost:5002/swagger
     - Order: http://localhost:5004/swagger
     - Payment: http://localhost:5003/swagger

5. **Stop all services**
   ```bash
   docker-compose down
   ```

### VS Code Tasks

Use VS Code tasks for common operations:

- **Ctrl+Shift+P** → "Tasks: Run Task"
- Available tasks:
  - `Docker: Start All Services`
  - `Docker: Stop All Services`
  - `Docker: Rebuild All Services`
  - `Docker: View All Logs`
  - Individual service tasks

---

## 📚 Documentation

### 🧭 Start Here
- **[START_HERE.md](docs/START_HERE.md)** - Choose your role-based learning path
- **[DOCUMENTATION_INDEX.md](docs/DOCUMENTATION_INDEX.md)** - Complete file catalog (53 files)

### 📖 Key Documentation Categories

#### 🚀 Getting Started
- **[Project Overview](docs/1-getting-started/PROJECT_OVERVIEW.md)** - Vision, goals, and quickstart
- **[Tech Stack](docs/1-getting-started/TECH_STACK.md)** - Technologies and rationale

#### 🎓 Learning Resources
- **[Learning Guide](docs/2-learning-guide/LEARNING_GUIDE.md)** - Novel-style end-to-end walkthrough
- **[Learning Roadmap](docs/2-learning-guide/LEARNING_ROADMAP.md)** - Skills & patterns you'll master
- **[Engineering Playbook](docs/2-learning-guide/ENGINEERING_PLAYBOOK.md)** - Design decisions

#### 👤 User Flows
- **[Signup](docs/5-user-flows/SIGNUP_FLOW.md)**, **[Login](docs/5-user-flows/LOGIN_FLOW.md)**, **[Checkout](docs/5-user-flows/CHECKOUT_ORDER_FLOW.md)**, and more
- **[All Flows Index](docs/5-user-flows/README.md)**

#### 🔧 Service Documentation
- **[API Gateway](docs/7-services/API_GATEWAY.md)**, **[Auth](docs/7-services/AUTH_SERVICE.md)**, **[User](docs/7-services/USER_SERVICE.md)**, **[Product](docs/7-services/PRODUCT_SERVICE.md)**, **[Order](docs/7-services/ORDER_SERVICE.md)**, **[Payment](docs/7-services/PAYMENT_SERVICE.md)**
- **[All Services Index](docs/7-services/README.md)**

#### 🏛️ Architecture
- **[System Architecture](docs/6-architecture/SYSTEM_ARCHITECTURE.md)** - High-level design
- **[Platform Architecture](docs/6-architecture/PLATFORM_ARCHITECTURE.md)** - Ep.Platform NuGet design

---

## 📁 Project Structure

```
MY_Practice/
├── frontend/                    # React application
│   ├── src/
│   │   ├── components/         # React components
│   │   ├── config/             # Configuration files
│   │   ├── hooks/              # Custom React hooks
│   │   ├── styles/             # CSS modules
│   │   ├── utils/              # Utility functions
│   │   └── api/                # API client functions
│   ├── Dockerfile
│   └── nginx.conf
│
├── gateway/                     # API Gateway (YARP)
│   ├── Program.cs
│   ├── Startup.cs
│   ├── ocelot.json             # Local config
│   └── ocelot.docker.json      # Docker config
│
├── services/                    # Microservices
│   ├── auth-service/           # Authentication
│   ├── user-service/           # User profiles & wallet
│   ├── product-service/        # Products & inventory
│   ├── order-service/          # Order management
│   └── payment-service/        # Payment processing
│
├── infra/                       # Infrastructure
│   └── docker-compose.yml      # All services orchestration
│
├── docs/                        # Documentation (53 files)
│   ├── START_HERE.md           # Role-based entry point
│   ├── DOCUMENTATION_INDEX.md  # Complete catalog
│   ├── 1-getting-started/      # Project overview & tech stack
│   ├── 2-learning-guide/       # Learning resources
│   ├── 3-product-owner/        # PM/PO docs
│   ├── 4-epics-and-pbis/       # Product backlog
│   ├── 5-user-flows/           # End-to-end workflows
│   ├── 6-architecture/         # System design & diagrams
│   ├── 7-services/             # Service documentation
│   ├── 8-platform/             # Ep.Platform NuGet guide
│   ├── 9-roadmap-and-tracking/ # Project planning
│   └── 10-tools-and-automation/ # Scripts & tools
│
└── .vscode/                     # VS Code configuration
    └── tasks.json              # Docker tasks
```

---

## 🔌 API Endpoints

### Auth Service (Port 5001)
```
POST   /api/auth/register    - Register new user
POST   /api/auth/login        - Login user
POST   /api/auth/reset-password - Reset password
GET    /api/auth/me           - Get current user (requires JWT)
```

### User Service (Port 5005)
```
GET    /api/users/{id}                    - Get user by ID
GET    /api/users/by-userid/{userId}      - Get user by Auth userId
GET    /api/users/phone-exists/{phone}    - Check phone exists
POST   /api/users                         - Create user profile
PUT    /api/users/{id}                    - Update user profile
POST   /api/users/{id}/wallet/debit       - Debit wallet
POST   /api/users/{id}/wallet/credit      - Credit wallet
POST   /api/users/add-balance              - Add balance to wallet
```

### Product Service (Port 5002)
```
GET    /api/products              - Get all products
GET    /api/products/{id}         - Get product by ID
POST   /api/products              - Create product
PUT    /api/products/{id}         - Update product
POST   /api/products/{id}/reserve - Reserve stock
```

### Order Service (Port 5004)
```
POST   /api/orders/create         - Create order
GET    /api/orders/{id}           - Get order by ID
GET    /api/orders/user/{userId}  - Get user's orders
```

### Payment Service (Port 5003)
```
POST   /api/payments/process      - Process payment
POST   /api/payments/refund       - Refund payment
POST   /api/payments/record       - Record payment (legacy)
GET    /api/payments/status/{orderId} - Get payment status
```

---

## 🎯 Key Concepts

### Guid Consistency
- All IDs (user, product, order) are **Guid** type
- Ensures type safety across microservices
- Frontend stores as strings (JavaScript compatibility)

### Atomic Operations
- **User Registration**: Auth user + User profile created atomically
- **Order Creation**: Payment + Stock reservation + Order creation
- **Rollback mechanisms** ensure no partial operations

### Payment Service Integration
- Central payment handling for all wallet operations
- Records every transaction in paymentdb
- Automatic refunds on order failures

### Service-to-Service Communication
- Direct HTTP calls between services (bypassing gateway)
- Uses HttpClientFactory for connection pooling
- Configured via appsettings.json and docker-compose

---

## 🤝 Contributing

This is a learning/practice project. Feel free to:

1. Fork the repository
2. Create a feature branch
3. Make your changes
4. Update documentation
5. Submit a pull request

---

## 📝 License

This project is licensed under the MIT License - see the [LICENSE](LICENSE) file for details.

---

## 🙏 Acknowledgments

- Built with **.NET 8** and **React 18**
- Uses **YARP** for API Gateway
- Inspired by modern microservices patterns
- Documentation follows industry best practices

---

## 📞 Support

For questions or issues:
- Check the [documentation index](docs/DOCUMENTATION_INDEX.md)
- Start with [START_HERE.md](docs/START_HERE.md)
- Review [service architecture docs](docs/7-services/)
- Open an issue on GitHub

---

**Last Updated:** December 24, 2025  
**Version:** 1.0.0  
**Status:** Production Ready ✅
