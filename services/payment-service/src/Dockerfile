FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src

# Copy Platform project (now accessible with repo-root context)
COPY ["platform/Ep.Platform/Ep.Platform.csproj", "platform/Ep.Platform/"]
COPY ["platform/Ep.Platform/", "platform/Ep.Platform/"]

# Copy service .build folder with version definitions
COPY ["services/payment-service/.build/", "services/payment-service/.build/"]

# Copy service project files
COPY ["services/payment-service/src/PaymentService.API/PaymentService.API.csproj", "services/payment-service/src/PaymentService.API/"]
COPY ["services/payment-service/src/PaymentService.Core/PaymentService.Core.csproj", "services/payment-service/src/PaymentService.Core/"]
COPY ["services/payment-service/src/PaymentService.Abstraction/PaymentService.Abstraction.csproj", "services/payment-service/src/PaymentService.Abstraction/"]
COPY ["services/payment-service/src/Directory.Build.props", "services/payment-service/src/"]

# Copy nuget.config
COPY ["services/payment-service/nuget.config", "services/payment-service/"]

# Restore dependencies
WORKDIR /src/services/payment-service/src
RUN dotnet restore "PaymentService.API/PaymentService.API.csproj" --no-cache /p:DisableImplicitNuGetFallbackFolder=true

# Copy everything else
COPY ["services/payment-service/src/", "/src/services/payment-service/src/"]

# Build and publish
WORKDIR "/src/services/payment-service/src/PaymentService.API"
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
COPY ["services/payment-service/src/entrypoint.sh", "/entrypoint.sh"]
RUN sed -i 's/\r$//' /entrypoint.sh && chmod +x /entrypoint.sh

# Environment
ENV APP_DLL=PaymentService.API.dll
ENV ASPNETCORE_URLS=http://+:80
ENV ConnectionStrings__DefaultConnection="Server=mssql,1433;Database=paymentdb;User Id=sa;Password=Your_password123;TrustServerCertificate=True;"
ENV ServiceUrls__UserService="http://user-service:80"

EXPOSE 80
ENTRYPOINT ["/entrypoint.sh"]
