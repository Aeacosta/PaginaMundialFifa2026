# Stage 1: Build
FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build
WORKDIR /src

# Copy solution and project files
COPY ["src/WorldCup2026.API/WorldCup2026.API.csproj", "WorldCup2026.API/"]
COPY ["src/WorldCup2026.Application/WorldCup2026.Application.csproj", "WorldCup2026.Application/"]
COPY ["src/WorldCup2026.Domain/WorldCup2026.Domain.csproj", "WorldCup2026.Domain/"]
COPY ["src/WorldCup2026.Infrastructure/WorldCup2026.Infrastructure.csproj", "WorldCup2026.Infrastructure/"]

# Restore dependencies
RUN dotnet restore "WorldCup2026.API/WorldCup2026.API.csproj"

# Copy all source files
COPY src/ .

# Build the application
WORKDIR "/src/WorldCup2026.API"
RUN dotnet build "WorldCup2026.API.csproj" -c Release -o /app/build

# Publish the application
RUN dotnet publish "WorldCup2026.API.csproj" -c Release -o /app/publish /p:UseAppHost=false

# Stage 2: Runtime
FROM mcr.microsoft.com/dotnet/aspnet:9.0 AS runtime
WORKDIR /app

# Install curl for health checks
RUN apt-get update && apt-get install -y curl && rm -rf /var/lib/apt/lists/*

# Create data directory for SQLite database
RUN mkdir -p /app/data

# Copy published application
COPY --from=build /app/publish .

# Expose port
EXPOSE 5004

# Set environment variables
ENV ASPNETCORE_URLS=http://+:5004
ENV ASPNETCORE_ENVIRONMENT=Production

# Health check endpoint
HEALTHCHECK --interval=30s --timeout=10s --start-period=40s --retries=3 \
  CMD curl -f http://localhost:5004/health || exit 1

# Run the application
ENTRYPOINT ["dotnet", "WorldCup2026.API.dll"]

# Made with Bob
