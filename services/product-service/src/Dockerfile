FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src

# Copy Platform project (now accessible with repo-root context)
COPY ["platform/Ep.Platform/Ep.Platform.csproj", "platform/Ep.Platform/"]
COPY ["platform/Ep.Platform/", "platform/Ep.Platform/"]

# Copy service .build folder with version definitions
COPY ["services/product-service/.build/", "services/product-service/.build/"]

# Copy service project files
COPY ["services/product-service/src/ProductService.API/ProductService.API.csproj", "services/product-service/src/ProductService.API/"]
COPY ["services/product-service/src/ProductService.Core/ProductService.Core.csproj", "services/product-service/src/ProductService.Core/"]
COPY ["services/product-service/src/ProductService.Abstraction/ProductService.Abstraction.csproj", "services/product-service/src/ProductService.Abstraction/"]
COPY ["services/product-service/src/Directory.Build.props", "services/product-service/src/"]

# Copy nuget.config
COPY ["services/product-service/nuget.config", "services/product-service/"]

# Accept GitHub token as build argument
ARG GITHUB_TOKEN
# Handle GitHub token and remove local source (not available in Docker)
RUN sed -i '/<add key="local"/d' services/product-service/nuget.config && \
    if [ -n "$GITHUB_TOKEN" ] && [ "$GITHUB_TOKEN" != "" ]; then \
      sed -i "s|%GITHUB_TOKEN%|$GITHUB_TOKEN|g" services/product-service/nuget.config; \
      echo "GitHub token provided, using GitHub Packages source."; \
    else \
      echo "Warning: GITHUB_TOKEN not provided. Removing GitHub package source and credentials."; \
      sed -i '/<add key="github"/d' services/product-service/nuget.config; \
      sed -i '/<packageSourceCredentials>/,/<\\/packageSourceCredentials>/d' services/product-service/nuget.config; \
      echo "GitHub source removed. Ep.Platform package must be available from nuget.org."; \
    fi

# Restore dependencies
WORKDIR /src/services/product-service/src
RUN dotnet restore "ProductService.API/ProductService.API.csproj" --no-cache /p:DisableImplicitNuGetFallbackFolder=true

# Copy everything else
COPY ["services/product-service/src/", "/src/services/product-service/src/"]

# Build and publish
WORKDIR "/src/services/product-service/src/ProductService.API"
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
COPY ["services/product-service/src/entrypoint.sh", "/entrypoint.sh"]
RUN sed -i 's/\r$//' /entrypoint.sh && chmod +x /entrypoint.sh

# Environment
ENV APP_DLL=ProductService.API.dll
ENV ASPNETCORE_URLS=http://+:80
ENV ConnectionStrings__DefaultConnection="Server=mssql,1433;Database=productdb;User Id=sa;Password=Your_password123;TrustServerCertificate=True;"

EXPOSE 80
ENTRYPOINT ["/entrypoint.sh"]
