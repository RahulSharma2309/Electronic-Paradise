# API Gateway (local)

This gateway is a simple reverse-proxy concept for the frontend. For the MVP you can use Ocelot or implement a tiny ASP.NET Core proxy. The frontend should call only the gateway (e.g., http://localhost:5000/api/... ) which will forward to each service's port.

Planned endpoints (examples):

- /api/auth/\* -> auth-service
- /api/users/\* -> user-service
- /api/products/\* -> product-service
- /api/orders/\* -> order-service
- /api/payments/\* -> payment-service

For now we start with individual services (each runs on its own port). Gateway implementation is next.
