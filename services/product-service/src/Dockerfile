FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src

COPY ["ProductService.API/ProductService.API.csproj", "ProductService.API/"]
COPY ["ProductService.Core/ProductService.Core.csproj", "ProductService.Core/"]
COPY ["ProductService.Abstraction/ProductService.Abstraction.csproj", "ProductService.Abstraction/"]
COPY ["nuget.config", "./"]

RUN dotnet restore "ProductService.API/ProductService.API.csproj"
COPY . .

WORKDIR "/src/ProductService.API"
RUN dotnet publish -c Release -o /app

FROM mcr.microsoft.com/dotnet/aspnet:8.0
WORKDIR /app
COPY --from=build /app .

RUN apt-get update && apt-get install -y curl netcat-openbsd && rm -rf /var/lib/apt/lists/*

COPY entrypoint.sh /entrypoint.sh
RUN sed -i 's/\r$//' /entrypoint.sh
RUN chmod +x /entrypoint.sh

ENV APP_DLL=ProductService.API.dll
ENV ASPNETCORE_URLS=http://+:80
ENV ConnectionStrings__DefaultConnection="Server=mssql,1433;Database=productdb;User Id=sa;Password=Your_password123;TrustServerCertificate=True;"

EXPOSE 80
ENTRYPOINT ["/entrypoint.sh"]



