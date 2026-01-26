FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src

# Copy Platform project (now accessible with repo-root context)
COPY ["platform/Ep.Platform/Ep.Platform.csproj", "platform/Ep.Platform/"]
COPY ["platform/Ep.Platform/", "platform/Ep.Platform/"]

# Copy service .build folder with version definitions
COPY ["services/order-service/.build/", "services/order-service/.build/"]

# Copy service project files
COPY ["services/order-service/src/OrderService.API/OrderService.API.csproj", "services/order-service/src/OrderService.API/"]
COPY ["services/order-service/src/OrderService.Core/OrderService.Core.csproj", "services/order-service/src/OrderService.Core/"]
COPY ["services/order-service/src/OrderService.Abstraction/OrderService.Abstraction.csproj", "services/order-service/src/OrderService.Abstraction/"]
COPY ["services/order-service/src/Directory.Build.props", "services/order-service/src/"]

# Copy nuget.config
COPY ["services/order-service/nuget.config", "services/order-service/"]

# Restore dependencies
WORKDIR /src/services/order-service/src
RUN dotnet restore "OrderService.API/OrderService.API.csproj" --no-cache /p:DisableImplicitNuGetFallbackFolder=true

# Copy everything else
COPY ["services/order-service/src/", "/src/services/order-service/src/"]

# Build and publish
WORKDIR "/src/services/order-service/src/OrderService.API"
RUN dotnet publish -c Release -o /app /p:UseAppHost=false /p:DisableImplicitNuGetFallbackFolder=true

# Runtime image
FROM mcr.microsoft.com/dotnet/aspnet:8.0
WORKDIR /app
COPY --from=build /app .

# Install curl and netcat for healthchecks
RUN apt-get update && \
    apt-get install -y --no-install-recommends curl netcat-openbsd && \
    apt-get clean && \
    rm -rf /var/lib/apt/lists/* /tmp/* /var/tmp/*

# Copy and setup entrypoint
COPY ["services/order-service/src/entrypoint.sh", "/entrypoint.sh"]
RUN sed -i 's/\r$//' /entrypoint.sh && chmod +x /entrypoint.sh

# Environment
ENV APP_DLL=OrderService.API.dll
ENV ASPNETCORE_URLS=http://+:80
ENV ConnectionStrings__DefaultConnection="Server=mssql,1433;Database=orderdb;User Id=sa;Password=Your_password123;TrustServerCertificate=True;"
ENV ServiceUrls__ProductService="http://product-service:80"
ENV ServiceUrls__PaymentService="http://payment-service:80"
ENV ServiceUrls__UserService="http://user-service:80"

EXPOSE 80
ENTRYPOINT ["/entrypoint.sh"]
