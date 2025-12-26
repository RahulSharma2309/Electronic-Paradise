FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src
COPY . ./
RUN dotnet restore
RUN dotnet publish -c Release -o /app

FROM mcr.microsoft.com/dotnet/aspnet:8.0
WORKDIR /app
COPY --from=build /app .
RUN apt-get update && apt-get install -y curl netcat-openbsd && rm -rf /var/lib/apt/lists/*
COPY entrypoint.sh /entrypoint.sh
RUN chmod +x /entrypoint.sh
# When running in docker, instruct gateway to use docker network config
ENV USE_DOCKER_NETWORK=true
ENV APP_DLL=Gateway.dll
ENV ASPNETCORE_URLS=http://+:80
EXPOSE 80
ENTRYPOINT ["/entrypoint.sh"]
