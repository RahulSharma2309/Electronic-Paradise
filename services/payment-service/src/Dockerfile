FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src

# Copy project files
COPY ["PaymentService.API/PaymentService.API.csproj", "PaymentService.API/"]
COPY ["PaymentService.Core/PaymentService.Core.csproj", "PaymentService.Core/"]
COPY ["PaymentService.Abstraction/PaymentService.Abstraction.csproj", "PaymentService.Abstraction/"]

# Copy nuget.config for Platform package
COPY ["nuget.config", "./"]

# Restore
RUN dotnet restore "PaymentService.API/PaymentService.API.csproj"

# Copy everything
COPY . .

# Build and publish
WORKDIR "/src/PaymentService.API"
RUN dotnet publish -c Release -o /app

# Runtime
FROM mcr.microsoft.com/dotnet/aspnet:8.0
WORKDIR /app
COPY --from=build /app .

# Health check tools
RUN apt-get update && apt-get install -y curl netcat-openbsd && rm -rf /var/lib/apt/lists/*

# Entrypoint
COPY entrypoint.sh /entrypoint.sh
RUN sed -i 's/\r$//' /entrypoint.sh
RUN chmod +x /entrypoint.sh

ENV APP_DLL=PaymentService.API.dll
ENV ASPNETCORE_URLS=http://+:80
ENV ConnectionStrings__DefaultConnection="Server=mssql,1433;Database=paymentdb;User Id=sa;Password=Your_password123;TrustServerCertificate=True;"
ENV ServiceUrls__UserService="http://user-service"

EXPOSE 80
ENTRYPOINT ["/entrypoint.sh"]



