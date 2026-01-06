# eCommerce - Web Application (ASP .NET Core Microservices with Azure DevOps & AKS)

- [eCommerce - Web Application (ASP .NET Core Microservices with Azure DevOps \& AKS)](#ecommerce---web-application-asp-net-core-microservices-with-azure-devops--aks)
  - [Background](#background)
  - [Architecture Overview](#architecture-overview)
    - [Microservices Architecture](#microservices-architecture)
    - [Service Communication](#service-communication)
    - [Deployment Strategy](#deployment-strategy)
  - [Technologies \& Patterns Used](#technologies--patterns-used)

## Background

- Build comprehensive eCommerce application with ASP .NET Core microservices in .net 8
- Real-world experience in microservices development
- Complete eCommerce Project: Build a fully functional eCommerce platform featuring users, products, and orders microservices.

## Architecture Overview

```mermaid
graph TB
    subgraph Presentation Layer
        UI[Angular 17 UI<br/>Port: 4200]
    end

    subgraph Microservices Business Layer
        US[User Service<br/>ASP.NET Core 8<br/>Port: 5001/7001<br/>Dapper ORM]
        PS[Product Service<br/>ASP.NET Core 8<br/>Port: 5002/7002<br/>EF Core]
        OS[Order Service<br/>ASP.NET Core 9<br/>Port: 5071/7182<br/>MongoDB Driver]
    end

    subgraph Data Layer
        PG[(PostgreSQL<br/>User DB<br/>Port: 5432)]
        MY[(MySQL<br/>Product DB<br/>Port: 3306)]
        MG[(MongoDB<br/>Order DB<br/>Port: 27018)]
    end

    subgraph Container Orchestration
        Docker[Docker Compose<br/>Multi-container Management]
        GHCR[GitHub Container Registry<br/>Image Storage]
    end

    subgraph Infrastructure
        LinuxVM[Linux VM <br> Docker Host <br> Deployment Target]
    end

    UI --> |HTTP/REST API| US
    UI --> |HTTP/REST API| PS
    UI --> |HTTP/REST API| OS

    US --> |Dapper Queries| PG
    PS --> |EF Core| MY
    OS --> |MongoDB Driver| MG

    US -.-> |Docker Image| GHCR
    PS -.-> |Docker Image| GHCR
    OS -.-> |Docker Image| GHCR

    Docker --> |Orchestrates| US
    Docker --> |Orchestrates| PS
    Docker --> |Orchestrates| OS
    Docker --> |Manages| PG
    Docker --> |Manages| MY
    Docker --> |Manages| MG

    GHCR -.->|Deploy| LinuxVM
    Docker -.->|Runs On| LinuxVM

    style UI fill:#61DAFB,stroke:#333,stroke-width:2px,color:#000
    style US fill:#512BD4,stroke:#333,stroke-width:2px,color:#fff
    style PS fill:#512BD4,stroke:#333,stroke-width:2px,color:#fff
    style OS fill:#512BD4,stroke:#333,stroke-width:2px,color:#fff
    style PG fill:#336791,stroke:#333,stroke-width:2px,color:#fff
    style MY fill:#4479A1,stroke:#333,stroke-width:2px,color:#fff
    style MG fill:#47A248,stroke:#333,stroke-width:2px,color:#fff
    style Docker fill:#2496ED,stroke:#333,stroke-width:2px,color:#fff
    style GHCR fill:#181717,stroke:#333,stroke-width:2px,color:#fff
    style Docker fill:#2496ED,stroke:#333,stroke-width:2px,color:#fff
    style GHCR fill:#181717,stroke:#333,stroke-width:2px,color:#fff
```

> **Note**: OrderService uses HTTP clients to call other services. Default container targets (from Dockerfile envs): ProductService at port 5247, UserService at port 9090.

### Microservices Architecture

Each microservice follows a **clean architecture** pattern with three layers:

1. **API Layer** (`*.API` project)
   - RESTful endpoints
   - Middleware (Exception Handling, CORS)
   - Swagger documentation
   - Minimal API / Controllers

2. **Business Logic Layer** (`*.BusinessLogicLayer` / `*.Core` project)
   - Service contracts and implementations
   - DTOs (Data Transfer Objects)
   - Object mapping (Mapster)
   - FluentValidation validators
   - Business rules

3. **Data Access Layer** (`*.DataAccessLayer` / `*.Infrastructure` project)
   - Repository pattern
   - Database context
   - Entity models
   - Data access logic

### Service Communication

- **User Service**: Manages user authentication, registration, and profiles
- **Product Service**: Handles product catalog, inventory management
- **Order Service**: Processes orders, order items, and order history

Each service:

- Exposes REST APIs
- Maintains its own database (Database per Service pattern)
- Can be deployed independently
- Runs in isolated Docker containers

### Deployment Strategy

1. **Development**: Local development with docker-compose
2. **CI/CD**: GitHub Actions → GitHub Container Registry
3. **Production**: Linux VM with Docker Compose orchestration
4. **Future**: Azure Kubernetes Service (AKS) deployment

## Technologies & Patterns Used

- ASP.NET Core 8 (Web API in c#)
- Microservices Architecture
- REST API (HTTP Methods - GET, POST, PUT, DELETE)
- Mapster (Object to Object Mapping), earlier used AutoMapper
- Repository Pattern
- Dapper (Micro ORM for data access)
- PostgreSQL (Relational Database) in docker container
- FluentValidation (Request validation)
- Dependency Injection (DI) Pattern
- Swagger (API Documentation)
- CORS Policy Configuration
- Angular 17 (Frontend application)
- MySQL (Relational Database) in docker container
- Docker (Containerization)
- Docker Compose (Multi-container orchestration)
- Deployment to Dockerized environment on Linux VM box
- MongoDb (NoSQL Database) in docker container
- Fault Tolerance and Resilience (Polly - Future Implementation: Retry, Circuit Breaker, Fallback, Bulkhead Isolation, Timeout)
- Logging and Monitoring (ILogger - Future Implementation)
