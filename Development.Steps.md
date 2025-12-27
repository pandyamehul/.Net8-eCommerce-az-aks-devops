# eCommerce - Web Application (ASP .NET Core Microservices with Azure DevOps & AKS)

- [eCommerce - Web Application (ASP .NET Core Microservices with Azure DevOps \& AKS)](#ecommerce---web-application-asp-net-core-microservices-with-azure-devops--aks)
  - [Background](#background)
  - [Features (Technical and non-technical)](#features-technical-and-non-technical)
  - [Technologies \& Patterns Used](#technologies--patterns-used)
  - [Step# 1 : Implementation of User Microservice](#step-1--implementation-of-user-microservice)
    - [Initial Project setup](#initial-project-setup)
    - [User Microservice Controller Implementation](#user-microservice-controller-implementation)
    - [Automapper Integration](#automapper-integration)
    - [Switch from Automapper to Mapster](#switch-from-automapper-to-mapster)
    - [Postgres Integration with Dapper (ORM tool for data access)](#postgres-integration-with-dapper-orm-tool-for-data-access)
    - [FluentValidation Integration for Request Validation](#fluentvalidation-integration-for-request-validation)
    - [Added Swagger Support with CORS Policy](#added-swagger-support-with-cors-policy)
  - [Step# 2: Implementation of Product Microservice](#step-2-implementation-of-product-microservice)
    - [Initial Project setup (Product Microservice)](#initial-project-setup-product-microservice)
    - [MySQL Db Setup in Docker Container](#mysql-db-setup-in-docker-container)
    - [Data Access Layer Implementation](#data-access-layer-implementation)
    - [Business Access Layer Implementation](#business-access-layer-implementation)
    - [API Endpoints Implementation](#api-endpoints-implementation)
    - [Build Docker Images in local Docker Registry](#build-docker-images-in-local-docker-registry)
    - [Steps to delete and recreate databases in docker environment on Linux VM](#steps-to-delete-and-recreate-databases-in-docker-environment-on-linux-vm)
  - [Step # 3: Implementation of Order Microservice](#step--3-implementation-of-order-microservice)

## Background

- Build comprehensive eCommerce application with ASP .NET Core microservices in .net 8
- Real-world experience in microservices development
- Complete eCommerce Project: Build a fully functional eCommerce platform featuring users, products, and orders microservices.

## Features (Technical and non-technical)

- Implement and integrate these using ASP.NET Core Web API and various technologies.
- Microservices Architecture: Develop expertise in microservices with diverse databases like Postgres, MySQL, and MongoDB, along with different architectural patterns.
- Containerization & Orchestration: Master Docker and Kubernetes. Learn how to containerize your applications, orchestrate them with AKS (Azure Kubernetes Service), and achieve zero downtime deployments.
- Fault Tolerance & Caching: Implement Polly for advanced fault tolerance strategies, use Redis for caching, and leverage RabbitMQ for reliable messaging.
- DevOps Integration: Gain hands-on experience with Azure DevOps. Set up CI/CD pipelines, manage environments, and integrate with Azure Key Vault for secure deployment.
- API Management & Authentication: Configure an API Gateway using Ocelot, manage your APIs with Azure API Management, and secure your application with Microsoft Entra ID B2C authentication.

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

## Step# 1 : Implementation of User Microservice

### Initial Project setup

- Created new folder ".NET Microservices w Azure DevOps & AKS"
- Initialized git to track changes
- Created Blank solution
- Added Class Library projects - core and Infra project.
- Added "ASP.net web API" project
- Added Dependency injection for core and Infra project.
- API solution configured and implemented request pipeline.

### User Microservice Controller Implementation

- Created User Controller to serve register and login flow.
- Created rest client to test API endpoints.

### Automapper Integration

- Implemented automapper to map request and response objects.

### Switch from Automapper to Mapster

- Removed automapper nuget package from core project.
- Added Mapster and Mapster.DependencyInjection nuget packages to core project.
- Updated mapping profiles and DI registrations to use Mapster instead of Automapper.
- Tested user registration and login endpoints to ensure mapping works correctly.
- Verified that all unit tests pass after the migration.

### Postgres Integration with Dapper (ORM tool for data access)

- Added Npgsql.EntityFrameworkCore.PostgreSQL nuget package to Infra project.
- Configured Postgres connection string in appsettings.json file.
- Implemented DbContext and entity configurations for User entity.
- Used Dapper for data access in User repository.
- Tested user registration and login flows with Postgres database.
- Verified data is correctly stored and retrieved from Postgres.

### FluentValidation Integration for Request Validation

- Added FluentValidation and FluentValidation.DependencyInjectionExtensions nuget packages to Core project.
- Created validators for User registration and login request models.
- Registered validators in the DI container in DependencyInjection.cs.
- Updated UserController to use FluentValidation for validating incoming requests.
- Tested validation scenarios to ensure invalid requests are properly handled.
- Verified that valid requests pass validation and are processed correctly.

### Added Swagger Support with CORS Policy

- Added Swagger services in Program.cs to generate API documentation.
- Configured Swagger middleware in the request pipeline to serve swagger.json and Swagger UI.
- Added CORS services and configured a default policy to allow requests from the [Angular frontend](http://localhost:5013).
- Tested Swagger UI to ensure API endpoints are documented and accessible.
- Verified CORS policy allows requests from the specified origin without issues.
- Tested API endpoints from the Angular frontend to ensure proper communication with the User microservice.
- Verified that Swagger UI and CORS configurations work as expected in development environment.
- Completed Step#1 implementation of User Microservice with all required features.

## Step# 2: Implementation of Product Microservice

### Initial Project setup (Product Microservice)

- Created new ASP.net web API project for Product Microservice.
- Added Class Library projects (data access layer and business access layer) - core and Infra project for Product Microservice.
- Added necessary Nuget packages - Mapster, FluentValidation, Dependency Injection extensions etc.

### MySQL Db Setup in Docker Container

- Pulled MySQL Docker image from Docker Hub.
- Created and started MySQL container with necessary environment variables (root password, database name etc.)
- Verified MySQL container is running and accessible.
- Created Product database and necessary tables with data using MySQL Workbench.
- Updated docker-compose file to include MySQL service for Product Microservice.

### Data Access Layer Implementation

- Implemented Product entity and DbContext for MySQL database.
- Created Product repository using Entity Framework Core for data access.
- Added Dependency Injection for DbContext and Product repository in the DI container.
- Created Product service interface and implementation for data access layer.
- Implemented Repository pattern for data access layer to abstract database operations, ref - ProductsRepository.cs and IProductsRepository.cs.
- Tested data access layer methods to ensure correct CRUD operations on Product entity.

### Business Access Layer Implementation

- Added DTO and IProductsService.cs in Business Access Layer project.
- Added ProductMappingProfile.cs to configure Mapster mappings for Product entity and DTOs.
- Implemented ProductsService.cs to handle business logic for Product operations.
- Registered Business Access Layer services in the DI container.
- Added FluentValidation validators for ProductAddRequest and ProductUpdateRequest DTOs.
- Updated DependencyInjection.cs to register FluentValidation validators for Product requests.

### API Endpoints Implementation

- Implemented Product Service end point using Minimal API pattern.
- Created endpoints for CRUD operations on Product entity (GetAllProducts, GetProductById, AddProduct, UpdateProduct, DeleteProduct).
- Added request validation using FluentValidation for ProductAddRequest and ProductUpdateRequest.
- Fix mapster mapping issues in ProductsService.cs and ProductMappingProfile.cs.
- Added missing dependency injection registrations in DependencyInjection.cs in data access layer and business access layer projects.
- Created new api client using rest client to test Product Service endpoints.
- Tested all Product Service endpoints to ensure correct functionality and data integrity.
- search/searchText end point implementation is not working yet due to db collation issues.

### Build Docker Images in local Docker Registry

- Created local docker image for User Microservice and Product Microservice.
- Verified docker images are created successfully and available in local docker registry.

used below command to build docker images

  ```pwsh
  docker build -t net9-ecomm-userapi:initial -f .\UserService.API\Dockerfile .
  ```

  ```pwsh
  docker build -t net9-ecomm-productapi:initial -f .\ProductService.API\Dockerfile .
  ```

- Push docker images to Git Hub Container Registry (GHCR) for versioning and sharing. Followed below steps:

  - Created a personal access token (PAT) in GitHub with appropriate scopes for package read and write.
  - Logged in to GitHub Container Registry using Docker CLI with the PAT.

    ```pwsh
    $env:GHCR_TOKEN = "ghp_YOUR_GITHUB_PAT"
    $env:GHCR_TOKEN | docker login ghcr.io -u <GITHUB_USERNAME> --password-stdin
    ```

- Tagged the local Docker images with the GHCR repository name.

  ```pwsh
  docker build -t net9-ecomm-productapi:1.0 -f .\ProductService.API\Dockerfile .
  docker tag  net9-ecomm-productapi:1.0 ghcr.io/GITHUB_USERNAME/net9-ecomm-productapi:1.0
  ```

- Pushed the tagged images to GHCR.

  ```pwsh
  docker push ghcr.io/GITHUB_USERNAME/net9-ecomm-productapi:1.0
  ```

- Verified the images are successfully pushed to GHCR by checking the Packages section in the GitHub repository.
- Logged out from GHCR in Docker CLI for security.

  ```pwsh
  docker logout ghcr.io
  ```

- Documented the steps and commands used for building and pushing Docker images to GHCR for future reference.
- Linked GHCR image with Git Hub repository for easy access and management. Followed below steps:

  - Navigated to the GitHub repository where the Docker images are stored.
  - Clicked on the "Settings" tab in the repository.
  - Selected "Packages" from the left sidebar.
  - Located the specific Docker image package (e.g., net9-ecomm-productapi).
  - Clicked on the package to view its details.
  - In the package details, clicked on "Link to repository" button.
  - Selected the appropriate repository from the dropdown menu to link with the Docker image.
  - Confirmed the linking action.
  - Verified that the Docker image is now linked to the GitHub repository by checking the package details page.

- Installed Oracle VirtualBox with Linux 24.04 VM to setup local docker setup for testing application.

  - Installed Docker in Linux VM, pulled images from GHCR and ran containers to test application locally.
  - Following commands used to setup docker in Linux VM:
  
    ```pwsh
    # update Linux
    sudo apt update && sudo apt upgrade -y

    # install docker
    sudo apt install docker.io -y

    # check the status of docker service
    sudo systemctl status docker

    # start and enable docker service
    sudo systemctl start docker
    sudo systemctl enable docker

    # verify docker installation
    docker --version

    # set GHCR token as environment variable
    export GHCR_TOKEN="ghp_YOUR_GITHUB_PAT"

    # login to GHCR 
    echo $GHCR_TOKEN | docker login ghcr.io -u <GITHUB_USERNAME> --password-stdin

    # pull docker images from GHCR
    docker pull ghcr.io/GITHUB_USERNAME/net9-ecomm-userapi:1.0
    docker pull ghcr.io/GITHUB_USERNAME/net9-ecomm-productapi:1.0

    # run docker containers
    docker run -p 5000:80 ghcr.io/GITHUB_USERNAME/net9-ecomm-userapi:1.0
    docker run -p 8080:8080 ghcr.io/GITHUB_USERNAME/net9-ecomm-productapi:1.0

    # verify containers are running
    docker ps
    ```

- Created docker network in Linuc VM docket to allow communication between Product microservice and MySQL container.

  ```pwsh
  sudo docker network create ecomm_app
  sudo docker network ls # to verify network creation
  sudo docker network inspect ecomm_app # to view network details
  ```

- Created and started MySQL container in ecomm-network.

  ```pwsh
  ## get latest mysql image
  # sudo docker run --name mysql-container --network ecomm_app -e MYSQL_ROOT_PASSWORD=YourPassword -e MYSQL_DATABASE=ProductDB -p 3306:3306 -d mysql:latest
  
  ## run mysql container with hostname for product service to connect to mysql container using hostname instead of ip address 
  # sudo docker run -it -e MYSQL_ROOT_PASSWORD=admin -p 3306:3306 --network ecomm_app --hostname=mysql-host-product-service mysql:latest

  ## Provide necessary rights to mysql-init folder to allow mysql container to read sql files
  sudo chmod -R 755 ./mysql-init

  ## use below command to run sql file to create tables and insert data
  sudo docker run -it -e MYSQL_ROOT_PASSWORD=admin -p 3306:3306 --network ecomm_app --hostname=mysql-host-product-service -v ./mysql-init:/docker-entrypoint-initdb.d mysql:latest

  ## run product service container in same network to connect to mysql container using hostname
  sudo docker run -p 8080:8080 --network ecomm_app -e MYSQL_HOST=mysql-host-product-service -e MYSQL_USER=root -e MYSQL_PASSWORD=admin ghcr.io/GITHUB_USERNAME/net9-ecomm-productapi:1.0
  ```
  
- Created docker-compose-linux.yaml file to setup MySQL container and Product microservice container together in ecomm_app network. Used below command to start containers using docker-compose-linux.yaml file

  ```pwsh
  sudo apt install docker-compose -y

  sudo apt docker-compose version
  sudo apt docker --version

  sudo docker-compose -f docker-compose-linux.yaml config # to verify docker compose file syntax
  
  sudo docker-compose -f docker-compose-linux.yaml up -d # to start containers in detached mode
  ```

- Updated User Microservice Dockerfile to expose ports 9090 and 9091 instead of 8080 and 8081 and parameterized Postgres connection settings using ENV variables. Build docket image again using updated Dockerfile.

  ```pwsh
  docker build -t net9-ecomm-userapi:initial -f .\eCommerce.API\Dockerfile .
  $env:GHCR_TOKEN = "ghp_YOUR_GITHUB_PAT"
  $env:GHCR_TOKEN | docker login ghcr.io -u <GITHUB_USERNAME> --password-stdin
  docker tag  net9-ecomm-userapi:initial ghcr.io/GITHUB_USERNAME/net9-ecomm-userapi:1.0
  docker push ghcr.io/GITHUB_USERNAME/net9-ecomm-userapi:1.0
  ```

- used below command to start and stop ecommerce application services

  ```bash
  sudo docker-compose -f docker-compose-product-service-linux.yaml up

  sudo docker-compose -f docker-compose-product-service-linux.yaml down
  ```

### Steps to delete and recreate databases in docker environment on Linux VM

  ```bash
  # 1. Stop all containers
  docker-compose -f docker-compose-product-service-linux.yaml down

  # 2. Remove volumes (this deletes all database data)
  docker-compose -f docker-compose-product-service-linux.yaml down -v

  # 3. Verify volumes are removed
  docker volume ls | grep ecommerce

  # 4. Optional: Fix file permissions for init scripts
  sudo chmod 644 ./postgres-init/*.sql
  sudo chmod 644 ./mysql-init/*.sql

  # 5. Start containers fresh (databases will be recreated from init scripts)
  docker-compose -f docker-compose-product-service-linux.yaml up -d

  # 6. Watch logs to ensure successful initialization
  docker-compose -f docker-compose-product-service-linux.yaml logs -f

  # 7. Verify databases are created
  docker exec -it ecommerce.aks.userdb psql -U postgres -c "\l"
  docker exec -it ecommerce.aks.productdb mysql -u root -pmysql-secret-pw -e "SHOW DATABASES;"

  # 8. Check if tables exist
  docker exec -it ecommerce.aks.userdb psql -U postgres -d eCommerceUsers -c "\dt"
  docker exec -it ecommerce.aks.productdb mysql -u mysqluser -pmysqlpassword eCommerceProducts -e "SHOW TABLES;"
  ```

- Steps to connect to Postgres and MySQL containers and run some useful commands

```bash
# For PostgreSQL - use the userdb container
sudo docker exec -it ecommerce.aks.userdb psql -U postgres -d eCommerceUsers

-- List all databases
\l
-- or
\list

-- Connect to a specific database
\c eCommerceUsers

-- List all tables in current database
\dt

-- Describe a specific table structure
\d Users
-- or
\d+ Users  -- (more detailed)

-- List all schemas
\dn

-- List all tables in all schemas
\dt *.*

-- Show table columns with details
\d+ tablename

-- List all users/roles
\du

-- Show current database
SELECT current_database();

-- Show current user
SELECT current_user;

-- View table data
SELECT * FROM Users;

-- Show table size
\dt+

-- Exit psql
\q

-- Use double quotes for case-sensitive names
DROP DATABASE "eCommerceUsers";

-- Or force drop if there are active connections
DROP DATABASE "eCommerceUsers" WITH (FORCE);
```

```bash
# For MySQL - use the productdb container\
sudo docker exec -it ecommerce.aks.productdb mysql -u root -pmysql-secret-pw
-- List all databases
SHOW DATABASES;
-- Use a specific database
USE eCommerceProducts;
-- Show all tables in the current database
SHOW TABLES;
-- Describe a specific table structure
DESCRIBE Products;
-- View table data
SELECT * FROM Products;
-- Exit MySQL shell
EXIT;
```

## Step # 3: Implementation of Order Microservice

- Created new ASP.net web API project for Order Microservice.
- Added Class Library projects (data access layer and business access layer) - core and Infra project for Order Microservice.
- Added necessary Nuget packages - Mapster, FluentValidation, Dependency Injection extensions etc.
- Implemented Order entity and DbContext for MongoDB database.
- Created Order repository using MongoDB.Driver for data access.
- Added Dependency Injection for DbContext and Order repository in the DI container.
- Created Order service interface and implementation for data access layer.
- Implemented Repository pattern for data access layer to abstract database operations, ref - OrdersRepository.cs and IOrdersRepository.cs.
- Added required configuration settings for MongoDB connection in appsettings.json file along with other environment variables for mongo connection in docker-compose file and launchsettings.json file.
- Added DTOs and IOrdersService interface in Business Access Layer project.
