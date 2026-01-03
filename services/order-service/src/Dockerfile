FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src

# Copy solution/project files
COPY ["OrderService.API/OrderService.API.csproj", "OrderService.API/"]
COPY ["OrderService.Core/OrderService.Core.csproj", "OrderService.Core/"]
COPY ["OrderService.Abstraction/OrderService.Abstraction.csproj", "OrderService.Abstraction/"]
COPY ["Directory.Build.props", "./"]

# Copy nuget.config for Platform package
COPY ["nuget.config", "./"]

# Accept GitHub token as build argument
ARG GITHUB_TOKEN
# Handle GitHub token: replace if provided, remove GitHub source and credentials if not
RUN if [ -n "$GITHUB_TOKEN" ] && [ "$GITHUB_TOKEN" != "" ]; then \
      sed -i "s|%GITHUB_TOKEN%|$GITHUB_TOKEN|g" nuget.config; \
      echo "GitHub token provided, using GitHub Packages source."; \
    else \
      echo "Warning: GITHUB_TOKEN not provided. Removing GitHub package source and credentials."; \
      sed -i '/<add key="github"/d' nuget.config; \
      sed -i '/<packageSourceCredentials>/,/<\/packageSourceCredentials>/d' nuget.config; \
      echo "GitHub source removed. Ep.Platform package must be available from nuget.org or local source."; \
    fi

# Restore dependencies (this layer will be cached if dependencies don't change)
# Disable NuGet fallback folders to prevent Windows path issues in Linux containers
RUN dotnet restore "OrderService.API/OrderService.API.csproj" --no-cache /p:DisableImplicitNuGetFallbackFolder=true

# Copy everything else
COPY . .

# Build and publish (removed --no-restore to avoid using cached Windows paths)
WORKDIR "/src/OrderService.API"
RUN dotnet publish -c Release -o /app /p:UseAppHost=false /p:DisableImplicitNuGetFallbackFolder=true

# Runtime image
FROM mcr.microsoft.com/dotnet/aspnet:8.0
WORKDIR /app
COPY --from=build /app .

# Install curl and netcat for healthchecks (optimized to reduce layer size)
RUN apt-get update && \
    apt-get install -y --no-install-recommends curl netcat-openbsd && \
    apt-get clean && \
    rm -rf /var/lib/apt/lists/* /tmp/* /var/tmp/*

# Copy and setup entrypoint
COPY entrypoint.sh /entrypoint.sh
RUN sed -i 's/\r$//' /entrypoint.sh && chmod +x /entrypoint.sh

ENV APP_DLL=OrderService.API.dll
ENV ASPNETCORE_URLS=http://+:80
ENV ConnectionStrings__DefaultConnection="Server=mssql,1433;Database=orderdb;User Id=sa;Password=Your_password123;TrustServerCertificate=True;"
ENV ServiceUrls__ProductService="http://product-service"
ENV ServiceUrls__PaymentService="http://payment-service"
ENV ServiceUrls__UserService="http://user-service"

EXPOSE 80
ENTRYPOINT ["/entrypoint.sh"]



