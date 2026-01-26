# Architecture

This document expands the architecture diagram in the main README and provides per-service sequence diagrams showing typical call flows and database interactions.

## **High-level overview**

```mermaid
graph TB
    subgraph Presentation Layer
        UI[Angular 17 UI<br/>Port: 4200]
    end

    subgraph Microservices Business Layer
        US[User Service<br/>ASP.NET Core 9<br/>Port: 5001/7001<br/>Dapper ORM]
        PS[Product Service<br/>ASP.NET Core 9<br/>Port: 5002/7002<br/>EF Core]
        OS[Order Service<br/>ASP.NET Core 9<br/>Port: 5071/7182<br/>MongoDB Driver]
    end

    subgraph Messaging
        RMQ[RabbitMQ<br/>Exchange: product.exchange<br/>RoutingKey: net9.ecomm.aks.product.update.name]
    end

    subgraph Data Layer
        PG[(PostgreSQL<br/>User DB<br/>Port: 5432)]
        MY[(MySQL<br/>Product DB<br/>Port: 3306)]
        MG[(MongoDB<br/>Order DB<br/>Port: 27018)]
        RED[(Redis Cache<br/>Port: 6379)]
    end

    subgraph Container Orchestration
        Docker[Docker Compose<br/>Multi-container Management]
        GHCR[GitHub Container Registry<br/>Image Storage]
    end

    %% subgraph Infrastructure
    %%     LinuxVM[Linux VM <br> Docker Host <br> Deployment Target]
    %% end

    UI --> |HTTP/REST API| AG
    AG[API Gateway - Ocelot<br/>Port: 8080]

    AG --> |HTTP/REST API| US
    AG --> |HTTP/REST API| PS
    AG --> |HTTP/REST API| OS

    PS --> | Async comm. - PUB: message/event via queues | RMQ   
    OS --> | Async comm. - SUB: message/event via queues | RMQ
    
    US --> |Dapper Queries| PG
    PS --> |EF Core| MY
    OS --> |MongoDB Driver| MG    
    OS --> |Redis Cache| RED    

    US -.-> |Docker Image| GHCR
    PS -.-> |Docker Image| GHCR
    OS -.-> |Docker Image| GHCR

    Docker --> |Orchestrates| US
    Docker --> |Orchestrates| PS
    Docker --> |Orchestrates| OS
    Docker --> |Manages| PG
    Docker --> |Manages| MY
    Docker --> |Manages| MG
    Docker --> |Manages| RED
    %% GHCR -.->|Deploy| LinuxVM
    %% Docker -.->|Runs On| LinuxVM

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
    style RMQ fill:#BB5566,stroke:#333,stroke-width:2px,color:#fff,text-align:left
    style RED fill:#D82C20,stroke:#333,stroke-width:2px,color:#fff
        
```

## **Cache (Redis)**

- **Role:** Distributed in-memory cache for frequently read data (product/catalog lookups), session state, and short-lived data to reduce DB load and lower request latency.
- **Placement:** A shared cache instance used by all services (User, Product, Order). In cloud deployments prefer a managed service such as **Azure Cache for Redis**.
- **Common usage patterns:**  
  - **Read-through / Write-through cache:** Cache product details, pricing snapshots, and computed DTOs.
  - **Session cache:** Store minimal session data or JWT blacklists (avoid storing PII).
  - **Distributed locks / counters:** Use Redis primitives (SETNX, INCR) with care.
  - **Pub/Sub:** Optionally use Redis Pub/Sub for lightweight invalidation or cache-churn signals (RabbitMQ remains primary event bus).
- **Configuration (env vars):** `REDIS_HOST`, `REDIS_PORT` (default `6379`), `REDIS_PASSWORD` (when enabled), `REDIS_SSL=true|false`.
- **.NET example (StackExchange.Redis):**

```csharp
var conn = ConnectionMultiplexer.Connect($"{env:REDIS_HOST}:{env:REDIS_PORT},password={env:REDIS_PASSWORD}");
var db = conn.GetDatabase();
await db.StringSetAsync("product:123", json, TimeSpan.FromMinutes(10));
var cached = await db.StringGetAsync("product:123");
```

- **Resilience & best practices:**
  - Set sensible TTLs (e.g., 5–60 minutes) depending on staleness tolerance.
  - Design for cache misses — always fall back to the authoritative DB and repopulate the cache.
  - Use connection pooling and a singleton `ConnectionMultiplexer` per process.
  - Monitor cache hit rate and eviction metrics; provision memory and enable persistence or replication per SLAs.
  - For production on Azure, enable clustering or premium tiers for replication and better throughput.

> Dev tip: run Redis locally via Docker (`docker run -p 6379:6379 redis:7`) for local dev and integration tests.
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

## **UI Component**

- **Framework**: Angular 17 (project at `eCommerce.UI`).
- **Responsibilities**: client-side application — routing, product catalog, cart management, authentication UI, and calling backend APIs through the API Gateway (Ocelot).
- **Key paths**: `eCommerce.UI/src` contains app code; `eCommerce.UI/package.json` contains dev scripts.
- **Local dev**: run from repo root in the `eCommerce.UI` folder:

```bash
cd eCommerce.UI
npm install
npm run start    # starts Angular dev server (default port 4200)
```

- **Production build**:

```bash
cd eCommerce.UI
npm run build    # outputs production assets in dist/
```

- **Container**: application can be containerized and served by a static web server or via the same Docker Compose setup used for services; map the container port `4200` (or serve built `dist/` from Nginx).

- **Integration**: UI calls the API Gateway at `http://<gateway-host>:8080` (configured in `eCommerce.UI/src/app/app.config.ts` or environment files). When running locally with Docker Compose the gateway proxies to service container ports.

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

### **ProductService — publish product update event**

```mermaid

sequenceDiagram
    participant Prod as Product Service
    participant RMQ as RabbitMQ (product.exchange)
    participant Queue as orders.product.update.name.queue

    Prod->>RMQ: Publish { ProductID, NewName } with routing key `net9.ecomm.aks.product.update.name`
    Note over RMQ: Exchange routes message to bound queues by routing key
    RMQ-->>Queue: Store message
    Prod-->>Prod: Log publish result
```

### **OrderService — consume product update event**

```mermaid
sequenceDiagram
    participant RMQ as RabbitMQ (product.exchange)
    participant Queue as orders.product.update.name.queue
    participant Order as OrderService (background consumer)
    participant Mongo as MongoDB (Order DB)

    Queue->>Order: Deliver message { ProductID, NewName }
    Order->>Order: Deserialize message
    Order->>Mongo: Update orders / log change
    Note right of Order: Ack to RabbitMQ (if using manual ack) the current consumer may use auto-ack
    Order-->>Order: Continue processing
```

Notes

- Sequence diagrams show typical synchronous JSON HTTP calls between services. The solution now uses RabbitMQ for async product events:
  - `ProductService` publishes product events to the `product.exchange` exchange (routing key `net9.ecomm.aks.product.update.name`).
  - `OrderService` runs a background consumer (hosted service) that subscribes to the queue bound to that exchange and applies updates (consumer implemented as `ProductNameUpdateConsumer`).
  - You can run RabbitMQ locally using the included Docker Compose service (image `rabbitmq:management`).
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
