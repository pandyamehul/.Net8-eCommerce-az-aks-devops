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
- Added CORS services and configured a default policy to allow requests from the Angular frontend running on http://localhost:5013.
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
