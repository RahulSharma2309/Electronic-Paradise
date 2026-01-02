FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src

COPY ["OrderService.API/OrderService.API.csproj", "OrderService.API/"]
COPY ["OrderService.Core/OrderService.Core.csproj", "OrderService.Core/"]
COPY ["OrderService.Abstraction/OrderService.Abstraction.csproj", "OrderService.Abstraction/"]
COPY ["nuget.config", "./"]

RUN dotnet restore "OrderService.API/OrderService.API.csproj"
COPY . .

WORKDIR "/src/OrderService.API"
RUN dotnet publish -c Release -o /app

FROM mcr.microsoft.com/dotnet/aspnet:8.0
WORKDIR /app
COPY --from=build /app .

RUN apt-get update && apt-get install -y curl netcat-openbsd && rm -rf /var/lib/apt/lists/*

COPY entrypoint.sh /entrypoint.sh
RUN sed -i 's/\r$//' /entrypoint.sh
RUN chmod +x /entrypoint.sh

ENV APP_DLL=OrderService.API.dll
ENV ASPNETCORE_URLS=http://+:80
ENV ConnectionStrings__DefaultConnection="Server=mssql,1433;Database=orderdb;User Id=sa;Password=Your_password123;TrustServerCertificate=True;"
ENV ServiceUrls__ProductService="http://product-service"
ENV ServiceUrls__PaymentService="http://payment-service"
ENV ServiceUrls__UserService="http://user-service"

EXPOSE 80
ENTRYPOINT ["/entrypoint.sh"]



