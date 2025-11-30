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
