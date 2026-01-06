# Architecture

This document expands the architecture diagram in the main README and provides per-service sequence diagrams showing typical call flows and database interactions.

## **High-level overview**

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

## **Service details and environment notes**

- User Service
  - Container env defaults (from Dockerfile): `POSTGRESDB_HOST=localhost`, `POSTGRESDB_PORT=5433` (DB: PostgreSQL)
  - Exposed container ports: `9090`, `9091` (Kestrel endpoint configured at `http://0.0.0.0:9090/`)

- Product Service
  - Container env defaults (from Dockerfile): `MYSQL_HOST=localhost`, `MYSQL_PORT=3306`, `MYSQL_DATABASE=eCommerceProducts`
  - Exposed container ports: `8080`, `8081`

- Order Service
  - Container env defaults (from Dockerfile): `MONGODB_HOST=localhost`, `MONGODB_PORT=27017`
  - Also configured to call other services using env keys: `UserServiceName`, `UserServicePort`, `ProductServiceName`, `ProductServicePort` (defaults point to `localhost:9090` and `localhost:5247` respectively)
  - Exposed container ports: `8080`, `8081`

## **Per-service sequence diagrams**

### **User Service — typical auth flow**

```mermaid
sequenceDiagram
    participant UI as Angular UI
    participant User as UserService (9090)
    participant PG as PostgreSQL (5433)

    UI->>User: POST /api/auth/login {credentials}
    User->>PG: SELECT user WHERE email=... (Dapper)
    PG-->>User: user row
    User-->>UI: 200 OK + JWT / profile
```

### **Product Service — product fetch flow**

```mermaid
sequenceDiagram
    participant UI as Angular UI
    participant Prod as ProductService (8080)
    participant MySQL as MySQL (3306)

    UI->>Prod: GET /api/products
    Prod->>MySQL: SELECT * FROM Products (EF Core)
    MySQL-->>Prod: rows
    Prod-->>UI: 200 OK + product list
```

### **Order Service — place order flow**

```mermaid
sequenceDiagram
    participant UI as Angular UI
    participant Order as OrderService (8080)
    participant Prod as ProductService (5247)
    participant User as UserService (9090)
    participant Mongo as MongoDB (27017)

    UI->>Order: POST /api/orders {cart, userId}
    Order->>User: GET /api/users/{userId} (validate user)
    User-->>Order: 200 OK + user
    Order->>Prod: GET /api/products/{id} (validate price/stock)
    Prod-->>Order: 200 OK + product
    Order->>Mongo: Insert order document
    Mongo-->>Order: Insert result
    Order-->>UI: 201 Created + orderId
```

Notes

- Sequence diagrams show typical synchronous JSON HTTP calls between services. In production you might replace some flows with asynchronous messaging (e.g., event bus) to improve resilience and scalability.
- Container ports shown are the exposed container ports. When using `docker-compose` you may map these to different host ports — diagram shows container-level values found in each service's Dockerfile and Kestrel config.

How to preview

- In VS Code: open the file and press `Ctrl+Shift+V` (or `Command+Shift+V` on macOS) to open the Markdown preview which renders Mermaid diagrams if you have a Mermaid preview extension installed.
- Alternatively, push to GitHub and view the Markdown on the repo (GitHub will render Mermaid diagrams in the web UI if enabled for the repo).

Example git commands to commit this doc (optional)

```bash
git add docs/architecture.md ReadMe.md
git commit -m "docs: add architecture.md with expanded diagrams and per-service flows"
git push origin main
```
