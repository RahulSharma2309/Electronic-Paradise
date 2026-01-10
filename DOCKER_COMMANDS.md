# Electronic Paradise - Quick Commands

## 🚀 Build and Start Everything (RECOMMENDED)

```powershell
# Run the automated script
.\scripts\docker-build-start.ps1
```

This script will:
1. Disable BuildKit (fixes ordering issues)
2. Clean existing containers
3. Build all services
4. Start all services
5. Show service status and URLs

---

## 🛠️ Manual Commands

### Build All Services
```powershell
cd infra
$env:DOCKER_BUILDKIT=0
docker-compose build
```

### Start All Services
```powershell
docker-compose up -d
```

### Check Service Status
```powershell
docker ps --format "table {{.Names}}\t{{.Status}}\t{{.Ports}}"
```

### View Logs
```powershell
# All services
docker-compose logs -f

# Specific service
docker-compose logs -f user-service
```

### Stop All Services
```powershell
docker-compose down
```

### Clean Everything (Fresh Start)
```powershell
docker-compose down -v
docker system prune -af --volumes
```

---

## 🌐 Service URLs

| Service | URL | Description |
|---------|-----|-------------|
| Frontend | http://localhost:3000 | React UI |
| API Gateway | http://localhost:5000 | Main API entry |
| Auth Service | http://localhost:5001/swagger | Auth API docs |
| Product Service | http://localhost:5002/swagger | Product API docs |
| Payment Service | http://localhost:5003/swagger | Payment API docs |
| Order Service | http://localhost:5004/swagger | Order API docs |
| User Service | http://localhost:5005/swagger | User API docs |

---

## 🧪 Testing

### Health Check All Services
```powershell
curl http://localhost:5001/api/health
curl http://localhost:5002/api/health
curl http://localhost:5003/api/health
curl http://localhost:5004/api/health
curl http://localhost:5005/api/health
```

### Register a User (via Swagger)
1. Go to http://localhost:5001/swagger
2. Use POST `/api/auth/register`
3. Try the login endpoint

---

## ⚠️ Troubleshooting

### BuildKit "changes out of order" Error
**Solution:** Always set `$env:DOCKER_BUILDKIT=0` before building

### Port Already in Use
```powershell
# Find process using port
netstat -ano | findstr :5001

# Kill process
taskkill /PID <process_id> /F
```

### Service Unhealthy
```powershell
# Check logs
docker logs <service-name>

# Restart service
docker-compose restart <service-name>
```

### Database Issues
```powershell
# Check SQL Server
docker logs infra_mssql_1

# Restart SQL Server
docker-compose restart mssql
```

---

## 📝 Notes

- **BuildKit must be disabled** for builds to work correctly
- Wait 30-60 seconds after `docker-compose up -d` for services to be fully healthy
- First build takes 5-10 minutes, subsequent builds are faster with caching
- All services use SQL Server running in Docker
- Platform library is referenced directly (no NuGet needed)
