# Docker Deployment Guide - FIFA World Cup 2026

This guide explains how to build and deploy the FIFA World Cup 2026 application using Docker and Docker Compose.

## 📋 Prerequisites

- **Docker Desktop** (Windows/Mac) or **Docker Engine** (Linux)
  - Download: https://www.docker.com/products/docker-desktop
  - Minimum version: Docker 20.10+, Docker Compose 2.0+
- **Git** (to clone the repository)
- **8GB RAM** minimum (recommended: 16GB)
- **10GB free disk space**

## 🚀 Quick Start

### 1. Clone the Repository

```bash
git clone <repository-url>
cd WorldCup2026
```

### 2. Build and Run with Docker Compose

```bash
# Build and start all services
docker-compose up --build

# Or run in detached mode (background)
docker-compose up -d --build
```

### 3. Access the Application

- **Frontend**: http://localhost
- **Backend API**: http://localhost:5004
- **Swagger UI**: http://localhost:5004/swagger

### 4. Seed the Database

```bash
# Using PowerShell
Invoke-RestMethod -Uri "http://localhost:5004/api/seed" -Method Post

# Using curl
curl -X POST http://localhost:5004/api/seed

# Using browser
# Open http://localhost:5004/swagger and execute POST /api/seed
```

## 📦 Docker Services

### Backend Service
- **Image**: worldcup2026-backend
- **Port**: 5004
- **Technology**: .NET 9.0 ASP.NET Core
- **Database**: SQLite (persisted in Docker volume)
- **Health Check**: http://localhost:5004/health

### Frontend Service
- **Image**: worldcup2026-frontend
- **Port**: 80
- **Technology**: React 18 + TypeScript + Nginx
- **Health Check**: http://localhost/health

## 🛠️ Docker Commands

### Build Services

```bash
# Build all services
docker-compose build

# Build specific service
docker-compose build backend
docker-compose build frontend

# Build without cache (clean build)
docker-compose build --no-cache
```

### Start/Stop Services

```bash
# Start services
docker-compose up

# Start in background
docker-compose up -d

# Stop services
docker-compose down

# Stop and remove volumes (WARNING: deletes database)
docker-compose down -v
```

### View Logs

```bash
# View all logs
docker-compose logs

# Follow logs in real-time
docker-compose logs -f

# View specific service logs
docker-compose logs backend
docker-compose logs frontend

# View last 100 lines
docker-compose logs --tail=100
```

### Service Management

```bash
# Restart services
docker-compose restart

# Restart specific service
docker-compose restart backend

# Stop specific service
docker-compose stop backend

# Start specific service
docker-compose start backend

# View running containers
docker-compose ps

# View service status
docker-compose ps -a
```

### Database Management

```bash
# Access backend container
docker exec -it worldcup2026-backend /bin/bash

# View database file
docker exec worldcup2026-backend ls -lh /app/data/

# Backup database
docker cp worldcup2026-backend:/app/data/WorldCup2026.db ./backup/

# Restore database
docker cp ./backup/WorldCup2026.db worldcup2026-backend:/app/data/
```

### Clean Up

```bash
# Remove stopped containers
docker-compose rm

# Remove all containers, networks, and volumes
docker-compose down -v

# Remove unused images
docker image prune -a

# Complete cleanup (WARNING: removes everything)
docker system prune -a --volumes
```

## 🔧 Configuration

### Environment Variables

Create a `.env` file in the project root (copy from `.env.example`):

```env
# Backend Configuration
ASPNETCORE_ENVIRONMENT=Production
ASPNETCORE_URLS=http://+:5004

# Database Configuration
DB_CONNECTION_STRING=Data Source=/app/data/WorldCup2026.db

# Frontend Configuration
VITE_API_URL=http://localhost:5004

# Docker Configuration
BACKEND_PORT=5004
FRONTEND_PORT=80
```

### Custom Ports

To change default ports, edit `docker-compose.yml`:

```yaml
services:
  backend:
    ports:
      - "8080:5004"  # Change 8080 to your desired port
  
  frontend:
    ports:
      - "3000:80"    # Change 3000 to your desired port
```

Then update the frontend environment variable:

```yaml
frontend:
  environment:
    - VITE_API_URL=http://localhost:8080
```

## 🏗️ Build Process

### Backend Build Stages

1. **Build Stage**: Restores dependencies and compiles .NET application
2. **Publish Stage**: Creates optimized production build
3. **Runtime Stage**: Copies published files to minimal runtime image

### Frontend Build Stages

1. **Build Stage**: Installs npm dependencies and builds React app
2. **Production Stage**: Copies built files to Nginx for serving

## 📊 Health Checks

Both services include health checks:

### Backend Health Check
```bash
curl http://localhost:5004/health
```

### Frontend Health Check
```bash
curl http://localhost/health
```

### View Health Status
```bash
docker-compose ps
```

## 🔍 Troubleshooting

### Port Already in Use

```bash
# Find process using port 5004
netstat -ano | findstr :5004  # Windows
lsof -i :5004                 # Linux/Mac

# Kill the process or change port in docker-compose.yml
```

### Container Won't Start

```bash
# View detailed logs
docker-compose logs backend

# Check container status
docker-compose ps

# Rebuild without cache
docker-compose build --no-cache backend
docker-compose up backend
```

### Database Issues

```bash
# Reset database (WARNING: deletes all data)
docker-compose down -v
docker-compose up -d
# Then seed again
```

### Frontend Can't Connect to Backend

1. Check backend is running: `docker-compose ps`
2. Verify backend health: `curl http://localhost:5004/health`
3. Check frontend environment: `docker-compose logs frontend`
4. Ensure VITE_API_URL is correct in docker-compose.yml

### Out of Disk Space

```bash
# Remove unused images and containers
docker system prune -a

# Remove unused volumes
docker volume prune
```

## 🚢 Production Deployment

### Using Docker Compose

```bash
# Production build
docker-compose -f docker-compose.yml up -d --build

# View logs
docker-compose logs -f
```

### Using Individual Containers

```bash
# Build images
docker build -t worldcup2026-backend:latest ./backend
docker build -t worldcup2026-frontend:latest ./frontend

# Run backend
docker run -d \
  --name worldcup2026-backend \
  -p 5004:5004 \
  -v worldcup-data:/app/data \
  worldcup2026-backend:latest

# Run frontend
docker run -d \
  --name worldcup2026-frontend \
  -p 80:80 \
  --link worldcup2026-backend:backend \
  worldcup2026-frontend:latest
```

### Push to Docker Registry

```bash
# Tag images
docker tag worldcup2026-backend:latest your-registry/worldcup2026-backend:latest
docker tag worldcup2026-frontend:latest your-registry/worldcup2026-frontend:latest

# Push to registry
docker push your-registry/worldcup2026-backend:latest
docker push your-registry/worldcup2026-frontend:latest
```

## 📈 Performance Optimization

### Multi-stage Builds
Both Dockerfiles use multi-stage builds to minimize image size:
- Backend: ~200MB (vs ~2GB with SDK)
- Frontend: ~50MB (vs ~1GB with node_modules)

### Caching
Docker caches layers for faster rebuilds:
- Dependencies are cached separately from source code
- Only changed layers are rebuilt

### Resource Limits

Add resource limits in `docker-compose.yml`:

```yaml
services:
  backend:
    deploy:
      resources:
        limits:
          cpus: '2'
          memory: 2G
        reservations:
          cpus: '1'
          memory: 1G
```

## 🔐 Security Best Practices

1. **Don't commit .env files** - Use .env.example as template
2. **Use secrets for sensitive data** - Consider Docker secrets for production
3. **Run as non-root user** - Images use non-root users
4. **Keep images updated** - Regularly update base images
5. **Scan for vulnerabilities** - Use `docker scan` command

## 📝 Additional Resources

- [Docker Documentation](https://docs.docker.com/)
- [Docker Compose Documentation](https://docs.docker.com/compose/)
- [.NET Docker Images](https://hub.docker.com/_/microsoft-dotnet)
- [Nginx Docker Images](https://hub.docker.com/_/nginx)

## 🆘 Support

For issues or questions:
1. Check logs: `docker-compose logs`
2. Review this guide
3. Check Docker status: `docker-compose ps`
4. Open an issue in the repository

---

**Last Updated**: June 6, 2026
**Docker Version**: 20.10+
**Docker Compose Version**: 2.0+