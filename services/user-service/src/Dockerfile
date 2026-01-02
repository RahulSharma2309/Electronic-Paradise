FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src

# Copy solution/project files
COPY ["UserService.API/UserService.API.csproj", "UserService.API/"]
COPY ["UserService.Core/UserService.Core.csproj", "UserService.Core/"]
COPY ["UserService.Abstraction/UserService.Abstraction.csproj", "UserService.Abstraction/"]

# Copy nuget.config for Platform package
COPY ["nuget.config", "./"]

# Restore
RUN dotnet restore "UserService.API/UserService.API.csproj"

# Copy everything else
COPY . .

# Build and publish
WORKDIR "/src/UserService.API"
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
ENV APP_DLL=UserService.API.dll
ENV ASPNETCORE_URLS=http://+:80
ENV ConnectionStrings__DefaultConnection="Server=mssql,1433;Database=userdb;User Id=sa;Password=Your_password123;TrustServerCertificate=True;"

EXPOSE 80
ENTRYPOINT ["/entrypoint.sh"]



