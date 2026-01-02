FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src

# Copy solution/project files
COPY ["AuthService.API/AuthService.API.csproj", "AuthService.API/"]
COPY ["AuthService.Core/AuthService.Core.csproj", "AuthService.Core/"]
COPY ["AuthService.Abstraction/AuthService.Abstraction.csproj", "AuthService.Abstraction/"]

# Copy nuget.config for Platform package
COPY ["nuget.config", "./"]

# Restore
RUN dotnet restore "AuthService.API/AuthService.API.csproj"

# Copy everything else
COPY . .

# Build and publish
WORKDIR "/src/AuthService.API"
RUN dotnet publish -c Release -o /app

# Runtime image
FROM mcr.microsoft.com/dotnet/aspnet:8.0
WORKDIR /app
COPY --from=build /app .

# Install curl and netcat for healthchecks
RUN apt-get update && apt-get install -y curl netcat-openbsd && rm -rf /var/lib/apt/lists/*

# Copy and setup entrypoint
COPY entrypoint.sh /entrypoint.sh
RUN sed -i 's/\r$//' /entrypoint.sh
RUN chmod +x /entrypoint.sh

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



