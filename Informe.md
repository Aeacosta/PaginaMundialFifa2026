# Proyecto: Aplicación Web Asistida por IA

Estudiante: Allan Acosta Porras
Correo: aacostap@ucenfotec.ac.cr

## Introducción

El siguiento documento se elaboró con el objetivo de elaborar una aplicación Web. A lo largo de su desarrollo, se explica como fue aplicada la Inteligencia Artificial en cada una de sus etaps.

## Arquitectura

La arquitectura consiste en la interfaz desarrollada en React. El Back End desde .NET 9. Finalmente la base de datos correr bajo SQLite

```
┌─────────────────────────────────────────────────────────────┐
│                         Frontend (React)                     │
│  ┌──────────────┐  ┌──────────────┐  ┌──────────────┐      │
│  │   Pages      │  │  Components  │  │   Services   │      │
│  └──────────────┘  └──────────────┘  └──────────────┘      │
└─────────────────────────────────────────────────────────────┘
                            │ HTTP/REST
                            ▼
┌─────────────────────────────────────────────────────────────┐
│                    Backend (.NET 9 API)                      │
│  ┌──────────────────────────────────────────────────────┐   │
│  │              Presentation Layer                       │   │
│  │  Controllers, DTOs, Filters, Middleware              │   │
│  └──────────────────────────────────────────────────────┘   │
│                            │                                 │
│  ┌──────────────────────────────────────────────────────┐   │
│  │              Application Layer                        │   │
│  │  Services, Business Logic, Validators                │   │
│  └──────────────────────────────────────────────────────┘   │
│                            │                                 │
│  ┌──────────────────────────────────────────────────────┐   │
│  │              Domain Layer                             │   │
│  │  Entities, Interfaces, Domain Logic                  │   │
│  └──────────────────────────────────────────────────────┘   │
│                            │                                 │
│  ┌──────────────────────────────────────────────────────┐   │
│  │              Infrastructure Layer                     │   │
│  │  DbContext, Repositories, External Services          │   │
│  └──────────────────────────────────────────────────────┘   │
└─────────────────────────────────────────────────────────────┘
                            │
                            ▼
                    ┌───────────────┐
                    │ SQLite/       │
                    │ PostgreSQL    │
                    └───────────────┘
```
### Front End

### Back End

### CICD

Se integró un archivo formato YAML para la ejecución de las pruebas unitarias. Adicionalmente, esta reporta la cobertura al concluir esta acción. 

```YAML
name: .NET 9 Unit Tests

on:
  pull_request:
    branches: [ "*" ]
  workflow_dispatch:

jobs:
  test:
    name: Run Unit Tests
    runs-on: ubuntu-latest
    permissions:
      contents: read
      pull-requests: write

    steps:
    - name: Checkout Code
      uses: actions/checkout@v4

    - name: Setup .NET SDK
      uses: actions/setup-dotnet@v4
      with:
        dotnet-version: '9.0.x'

    - name: Restore Dependencies
      run: dotnet restore backend/WorldCup2026.slnx

    - name: Build Solution
      run: dotnet build backend/WorldCup2026.slnx --configuration Release --no-restore

    - name: Run Tests with Coverage
      run: dotnet test backend/src/WorldCup2026.Tests/WorldCup2026.Tests.csproj --configuration Release --no-build --verbosity normal --collect:"XPlat Code Coverage" --results-directory ./coverage

    - name: Generate Coverage Report
      uses: irongut/CodeCoverageSummary@v1.3.0
      with:
        filename: coverage/**/coverage.cobertura.xml
        badge: true
        fail_below_min: false
        format: markdown
        hide_branch_rate: false
        hide_complexity: true
        indicators: true
        output: both
        thresholds: '60 80'

    - name: Add Coverage to Job Summary
      run: cat code-coverage-results.md >> $GITHUB_STEP_SUMMARY

    - name: Add Coverage PR Comment
      uses: marocchino/sticky-pull-request-comment@v2
      if: github.event_name == 'pull_request'
      with:
        recreate: true
        path: code-coverage-results.md

    - name: Upload Coverage Report
      uses: actions/upload-artifact@v4
      with:
        name: coverage-report
        path: coverage/**/coverage.cobertura.xml
        retention-days: 30
```


### Docker

Para una buena escalabilidad, se tiene a dispoción una solución Docker. Esta se encarga de levantar las principales aristas de la aplicación: Front End, Back End y Base de Datos.

```yaml
version: '3.8'

services:
  # Backend API Service
  backend:
    build:
      context: ./backend
      dockerfile: Dockerfile
    container_name: worldcup2026-backend
    ports:
      - "5004:5004"
    environment:
      - ASPNETCORE_ENVIRONMENT=Production
      - ASPNETCORE_URLS=http://+:5004
      - ConnectionStrings__DefaultConnection=Data Source=/app/data/WorldCup2026.db
    volumes:
      - backend-data:/app/data
    networks:
      - worldcup-network
    healthcheck:
      test: ["CMD", "curl", "-f", "http://localhost:5004/health"]
      interval: 30s
      timeout: 10s
      retries: 3
      start_period: 40s
    restart: unless-stopped

  # Frontend Service
  frontend:
    build:
      context: ./frontend
      dockerfile: Dockerfile
    container_name: worldcup2026-frontend
    ports:
      - "80:80"
    environment:
      - VITE_API_URL=http://localhost:5004
    depends_on:
      backend:
        condition: service_healthy
    networks:
      - worldcup-network
    healthcheck:
      test: ["CMD", "wget", "--no-verbose", "--tries=1", "--spider", "http://localhost:80/health"]
      interval: 30s
      timeout: 10s
      retries: 3
      start_period: 10s
    restart: unless-stopped

# Named volumes for data persistence
volumes:
  backend-data:
    driver: local

# Network configuration
networks:
  worldcup-network:
    driver: bridge

# Made with Bob
```
### Documentación

A lo largo de la elaboración de este proyecto, la IA fue de gran aporte para documentar y resumenes ejecutivos el progreso en cada punto del desarrollo.
