FROM mcr.microsoft.com/dotnet/sdk:10.0 AS build
WORKDIR /src

# Copy project file first for better layer caching
COPY ["gateway/src/Gateway.csproj", "Gateway/"]
COPY ["gateway/src/Directory.Build.props", "./"]
COPY ["gateway/.build/", ".build/"]

# Restore dependencies
RUN dotnet restore "Gateway/Gateway.csproj" --no-cache /p:DisableImplicitNuGetFallbackFolder=true

# Copy everything else
COPY gateway/src/ Gateway/

# Build and publish
WORKDIR "/src/Gateway"
RUN dotnet publish -c Release -o /app /p:UseAppHost=false /p:DisableImplicitNuGetFallbackFolder=true /p:TreatWarningsAsErrors=false

# Runtime image
FROM mcr.microsoft.com/dotnet/aspnet:10.0
WORKDIR /app
COPY --from=build /app .

# Install health check tools
RUN apt-get update && \
    apt-get install -y --no-install-recommends curl netcat-openbsd && \
    apt-get clean && \
    rm -rf /var/lib/apt/lists/* /tmp/* /var/tmp/*

# Copy and setup entrypoint
COPY gateway/src/entrypoint.sh /entrypoint.sh
RUN sed -i 's/\r$//' /entrypoint.sh && chmod +x /entrypoint.sh

# Environment
ENV USE_DOCKER_NETWORK=true
ENV APP_DLL=Gateway.dll
ENV ASPNETCORE_URLS=http://+:80

EXPOSE 80
ENTRYPOINT ["/entrypoint.sh"]
