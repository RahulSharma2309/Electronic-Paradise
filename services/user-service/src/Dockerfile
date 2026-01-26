FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src

# Copy Platform project (now accessible with repo-root context)
COPY ["platform/Ep.Platform/Ep.Platform.csproj", "platform/Ep.Platform/"]
COPY ["platform/Ep.Platform/", "platform/Ep.Platform/"]

# Copy service .build folder with version definitions
COPY ["services/user-service/.build/", "services/user-service/.build/"]

# Copy service project files
COPY ["services/user-service/src/UserService.API/UserService.API.csproj", "services/user-service/src/UserService.API/"]
COPY ["services/user-service/src/UserService.Core/UserService.Core.csproj", "services/user-service/src/UserService.Core/"]
COPY ["services/user-service/src/UserService.Abstraction/UserService.Abstraction.csproj", "services/user-service/src/UserService.Abstraction/"]
COPY ["services/user-service/src/Directory.Build.props", "services/user-service/src/"]

# Copy nuget.config
COPY ["services/user-service/nuget.config", "services/user-service/"]

# Restore dependencies
WORKDIR /src/services/user-service/src
RUN dotnet restore "UserService.API/UserService.API.csproj" --no-cache /p:DisableImplicitNuGetFallbackFolder=true

# Copy everything else
COPY ["services/user-service/src/", "/src/services/user-service/src/"]

# Build and publish
WORKDIR "/src/services/user-service/src/UserService.API"
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
COPY ["services/user-service/src/entrypoint.sh", "/entrypoint.sh"]
RUN sed -i 's/\r$//' /entrypoint.sh && chmod +x /entrypoint.sh

# Environment
ENV APP_DLL=UserService.API.dll
ENV ASPNETCORE_URLS=http://+:80
ENV ConnectionStrings__DefaultConnection="Server=mssql,1433;Database=userdb;User Id=sa;Password=Your_password123;TrustServerCertificate=True;"

EXPOSE 80
ENTRYPOINT ["/entrypoint.sh"]
