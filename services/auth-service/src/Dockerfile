FROM mcr.microsoft.com/dotnet/sdk:10.0 AS build
WORKDIR /src

# Copy Platform project (now accessible with repo-root context)
COPY ["platform/Ep.Platform/Ep.Platform.csproj", "platform/Ep.Platform/"]
COPY ["platform/Ep.Platform/", "platform/Ep.Platform/"]
COPY ["platform/Directory.Build.props", "platform/"]
COPY ["platform/.build/", "platform/.build/"]

# Copy service .build folder with version definitions
COPY ["services/auth-service/.build/", "services/auth-service/.build/"]

# Copy service project files
COPY ["services/auth-service/src/AuthService.API/AuthService.API.csproj", "services/auth-service/src/AuthService.API/"]
COPY ["services/auth-service/src/AuthService.Core/AuthService.Core.csproj", "services/auth-service/src/AuthService.Core/"]
COPY ["services/auth-service/src/AuthService.Abstraction/AuthService.Abstraction.csproj", "services/auth-service/src/AuthService.Abstraction/"]
COPY ["services/auth-service/src/Directory.Build.props", "services/auth-service/src/"]

# Copy nuget.config
COPY ["services/auth-service/nuget.config", "services/auth-service/"]

# Restore dependencies
WORKDIR /src/services/auth-service/src
RUN dotnet restore "AuthService.API/AuthService.API.csproj" --no-cache /p:DisableImplicitNuGetFallbackFolder=true

# Copy everything else
COPY ["services/auth-service/src/", "/src/services/auth-service/src/"]

# Build and publish
WORKDIR "/src/services/auth-service/src/AuthService.API"
RUN dotnet publish -c Release -o /app /p:UseAppHost=false /p:DisableImplicitNuGetFallbackFolder=true /p:TreatWarningsAsErrors=false

# Runtime image
FROM mcr.microsoft.com/dotnet/aspnet:10.0
WORKDIR /app
COPY --from=build /app .

# Install curl and netcat for healthchecks
RUN apt-get update && \
    apt-get install -y --no-install-recommends curl netcat-openbsd && \
    apt-get clean && \
    rm -rf /var/lib/apt/lists/* /tmp/* /var/tmp/*

# Copy and setup entrypoint
COPY ["services/auth-service/src/entrypoint.sh", "/entrypoint.sh"]
RUN sed -i 's/\r$//' /entrypoint.sh && chmod +x /entrypoint.sh

# Environment
ENV APP_DLL=AuthService.API.dll
ENV ASPNETCORE_URLS=http://+:80
ENV ConnectionStrings__DefaultConnection="Server=mssql,1433;Database=authdb;User Id=sa;Password=Your_password123;TrustServerCertificate=True;"
ENV ServiceUrls__UserService="http://user-service:80"
ENV JwtOptions__Key="your-super-secret-key-that-should-be-at-least-32-characters-long-for-security"
ENV JwtOptions__Issuer="Electronic-Paradise"
ENV JwtOptions__Audience="Electronic-Paradise-Users"

EXPOSE 80
ENTRYPOINT ["/entrypoint.sh"]
