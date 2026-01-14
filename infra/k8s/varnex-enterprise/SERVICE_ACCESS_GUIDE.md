# 🌐 Varnex Enterprise - Service Access Guide

## Overview
All services are now accessible via enterprise-level URLs through Kubernetes Ingress, just like the frontend!

---

## 🔧 One-Time Setup: Update Hosts File

1. **Open Notepad as Administrator**
   - Press Windows Key
   - Type "Notepad"
   - Right-click → **Run as Administrator**

2. **Open Hosts File**
   - Click File → Open
   - Navigate to: `C:\Windows\System32\drivers\etc\`
   - Change file type dropdown to "All Files (*.*)"
   - Select `hosts` file → Open

3. **Add These Lines at the Bottom**
   ```
   # Varnex Enterprise
   127.0.0.1 app.varnex-enterprise.local
   127.0.0.1 api.varnex-enterprise.local
   127.0.0.1 auth.varnex-enterprise.local
   127.0.0.1 user.varnex-enterprise.local
   127.0.0.1 product.varnex-enterprise.local
   127.0.0.1 order.varnex-enterprise.local
   127.0.0.1 payment.varnex-enterprise.local
   ```

4. **Save** (Ctrl+S) and **Close** Notepad

---

## 🚀 Access Your Services

### Frontend & Gateway
| Service | URL | Purpose |
|---------|-----|---------|
| **Frontend** | http://app.varnex-enterprise.local | Main application UI |
| **Gateway** | http://api.varnex-enterprise.local/swagger | API Gateway (routes to all services) |

### Individual Microservices (Swagger Endpoints)
| Service | URL | Purpose |
|---------|-----|---------|
| **Auth Service** | http://auth.varnex-enterprise.local/swagger | Authentication & JWT tokens |
| **User Service** | http://user.varnex-enterprise.local/swagger | User management |
| **Product Service** | http://product.varnex-enterprise.local/swagger | Product catalog |
| **Order Service** | http://order.varnex-enterprise.local/swagger | Order management |
| **Payment Service** | http://payment.varnex-enterprise.local/swagger | Payment processing |

---

## 💡 Why This Is Better Than Port-Forwarding

### Port-Forward Approach (Old Way)
```bash
kubectl port-forward svc/varnex-enterprise-auth 5001:80
# Access at: http://localhost:5001/swagger
```
- ❌ Need to run multiple commands
- ❌ Manual port management
- ❌ Multiple terminal windows
- ❌ Not production-like
- ❌ No DNS names

### Ingress Approach (Current Way)
```yaml
# Kubernetes Ingress handles all routing automatically
```
- ✅ Access via DNS names (enterprise-like URLs)
- ✅ Always available (no manual commands)
- ✅ Production-like setup
- ✅ Centralized routing through Nginx Ingress
- ✅ Easy to remember URLs
- ✅ Can add SSL/TLS easily in future

---

## 🔍 Verify Ingress Configuration

Check that all ingress rules are active:
```powershell
kubectl get ingress -n varnex-enterprise
```

Expected output:
```
NAME                              CLASS   HOSTS                              ADDRESS     PORTS   AGE
varnex-enterprise-ingress-all     nginx   app.varnex-enterprise.local +6     localhost   80      1m
```

View detailed ingress rules:
```powershell
kubectl describe ingress varnex-enterprise-ingress-all -n varnex-enterprise
```

---

## 🧪 Test Each Service

### Quick Health Check Script
Run this in PowerShell to test all endpoints:
```powershell
$services = @(
    "http://app.varnex-enterprise.local",
    "http://api.varnex-enterprise.local/swagger",
    "http://auth.varnex-enterprise.local/swagger",
    "http://user.varnex-enterprise.local/swagger",
    "http://product.varnex-enterprise.local/swagger",
    "http://order.varnex-enterprise.local/swagger",
    "http://payment.varnex-enterprise.local/swagger"
)

foreach ($url in $services) {
    try {
        $response = Invoke-WebRequest -Uri $url -Method Head -TimeoutSec 5
        Write-Host "✅ $url - Status: $($response.StatusCode)" -ForegroundColor Green
    } catch {
        Write-Host "❌ $url - Failed: $($_.Exception.Message)" -ForegroundColor Red
    }
}
```

---

## 🛠️ Troubleshooting

### Issue 1: "This site can't be reached"
**Cause:** Hosts file not updated or Nginx Ingress not running

**Solution:**
```powershell
# 1. Verify hosts file was saved correctly
Get-Content C:\Windows\System32\drivers\etc\hosts | Select-String "varnex"

# 2. Check if Nginx Ingress is running
kubectl get pods -n ingress-nginx

# 3. Restart Nginx Ingress if needed
kubectl rollout restart deployment ingress-nginx-controller -n ingress-nginx
```

### Issue 2: "502 Bad Gateway"
**Cause:** Backend pods are not running or not ready

**Solution:**
```powershell
# Check pod status
kubectl get pods -n varnex-enterprise

# Check pod logs if any are failing
kubectl logs <pod-name> -n varnex-enterprise
```

### Issue 3: "404 Not Found" on Swagger
**Cause:** Service doesn't have Swagger enabled or wrong path

**Solution:**
- Swagger should be at `/swagger` for all services
- Check service logs: `kubectl logs <pod-name> -n varnex-enterprise`

---

## 📚 Architecture Explanation

```
User Browser
    ↓
    | http://auth.varnex-enterprise.local/swagger
    ↓
Windows Hosts File (127.0.0.1)
    ↓
    | localhost:80
    ↓
Nginx Ingress Controller
    ↓
    | Reads Ingress rules
    | Routes based on hostname
    ↓
Kubernetes Service (varnex-enterprise-auth)
    ↓
    | Load balances between pods
    ↓
Auth Service Pod(s)
    ↓
    | Returns Swagger UI
    ↓
User sees Swagger documentation
```

---

## 🎯 Best Practices

1. **Use Gateway for Client Apps**: Frontend should call `api.varnex-enterprise.local`, not individual services
2. **Direct Service Access for Development**: Use direct URLs (auth.varnex-enterprise.local) only for testing/debugging
3. **Monitor Ingress Logs**: `kubectl logs -n ingress-nginx <ingress-controller-pod>`
4. **Check Service Health**: Use `/health` endpoints if implemented

---

## 🚀 Next Steps

1. ✅ All services accessible via DNS
2. 🔜 Add `/health` endpoints to all services
3. 🔜 Add SSL/TLS certificates (HTTPS)
4. 🔜 Add rate limiting and authentication to Ingress
5. 🔜 Set up monitoring dashboards

---

**You now have a production-like local Kubernetes setup! 🎉**
