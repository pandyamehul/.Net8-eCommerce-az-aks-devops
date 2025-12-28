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
    subgraph "Frontend Layer"
        UI[Angular 17 UI<br/>Port: 4200]
    end

    subgraph "API Gateway / Load Balancer"
        Gateway[API Gateway / NGINX<br/>Future Implementation]
    end

    subgraph "Microservices Layer"
        UserService[User Service<br/>ASP.NET Core 8<br/>Port: 5001/7001<br/>Dapper ORM]
        ProductService[Product Service<br/>ASP.NET Core 8<br/>Port: 5002/7002<br/>EF Core]
        OrderService[Order Service<br/>ASP.NET Core 8<br/>Port: 5071/7182<br/>MongoDB Driver]
    end

    subgraph "Data Layer"
        PostgreSQL[(PostgreSQL<br/>User DB<br/>Port: 5432)]
        MySQL[(MySQL<br/>Product DB<br/>Port: 3306)]
        MongoDB[(MongoDB<br/>Orders DB<br/>Port: 27018)]
    end

    subgraph "Container Orchestration"
        Docker[Docker Compose<br/>Multi-container Management]
        GHCR[GitHub Container Registry<br/>Image Storage]
    end

    subgraph "Infrastructure"
        LinuxVM[Linux VM<br/>Deployment Target]
    end

    UI -->|HTTP/REST API| UserService
    UI -->|HTTP/REST API| ProductService
    UI -->|HTTP/REST API| OrderService

    UserService -->|Dapper Queries| PostgreSQL
    ProductService -->|EF Core| MySQL
    OrderService -->|MongoDB Driver| MongoDB

    UserService -.->|Docker Image| GHCR
    ProductService -.->|Docker Image| GHCR
    OrderService -.->|Docker Image| GHCR

    Docker -->|Orchestrates| UserService
    Docker -->|Orchestrates| ProductService
    Docker -->|Orchestrates| OrderService
    Docker -->|Manages| PostgreSQL
    Docker -->|Manages| MySQL
    Docker -->|Manages| MongoDB

    GHCR -.->|Deploy| LinuxVM
    Docker -.->|Runs On| LinuxVM

    style UI fill:#61DAFB,stroke:#333,stroke-width:2px,color:#000
    style UserService fill:#512BD4,stroke:#333,stroke-width:2px,color:#fff
    style ProductService fill:#512BD4,stroke:#333,stroke-width:2px,color:#fff
    style OrderService fill:#512BD4,stroke:#333,stroke-width:2px,color:#fff
    style PostgreSQL fill:#336791,stroke:#333,stroke-width:2px,color:#fff
    style MySQL fill:#4479A1,stroke:#333,stroke-width:2px,color:#fff
    style MongoDB fill:#47A248,stroke:#333,stroke-width:2px,color:#fff
    style Docker fill:#2496ED,stroke:#333,stroke-width:2px,color:#fff
    style GHCR fill:#181717,stroke:#333,stroke-width:2px,color:#fff
```

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
