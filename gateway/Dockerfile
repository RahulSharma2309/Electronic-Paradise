FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src

# Copy project file first for better layer caching
COPY ["Gateway.csproj", "./"]

# Restore dependencies (disable NuGet fallback folders to prevent Windows path issues)
RUN dotnet restore "Gateway.csproj" --no-cache /p:DisableImplicitNuGetFallbackFolder=true

# Copy everything else
COPY . .

# Build and publish (removed --no-restore to avoid using cached Windows paths)
RUN dotnet publish "Gateway.csproj" -c Release -o /app /p:UseAppHost=false /p:DisableImplicitNuGetFallbackFolder=true

# Runtime image
FROM mcr.microsoft.com/dotnet/aspnet:8.0
WORKDIR /app
COPY --from=build /app .

# Install health check tools (optimized to reduce layer size)
RUN apt-get update && \
    apt-get install -y --no-install-recommends curl netcat-openbsd && \
    apt-get clean && \
    rm -rf /var/lib/apt/lists/* /tmp/* /var/tmp/*

# Copy and setup entrypoint
COPY entrypoint.sh /entrypoint.sh
RUN sed -i 's/\r$//' /entrypoint.sh && chmod +x /entrypoint.sh

# Environment
ENV USE_DOCKER_NETWORK=true
ENV APP_DLL=Gateway.dll
ENV ASPNETCORE_URLS=http://+:80

EXPOSE 80
ENTRYPOINT ["/entrypoint.sh"]
