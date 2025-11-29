# eCommerce - Web Application (ASP .NET Core Microservices with Azure DevOps & AKS)

- [eCommerce - Web Application (ASP .NET Core Microservices with Azure DevOps \& AKS)](#ecommerce---web-application-asp-net-core-microservices-with-azure-devops--aks)
  - [Background](#background)
  - [Features (Technical and non-technical)](#features-technical-and-non-technical)
  - [Technologies \& Patterns Used](#technologies--patterns-used)
  - [Session# 1 : Implementation of User Microservice](#session-1--implementation-of-user-microservice)

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
- Automapper (Object to Object Mapping)

## Session# 1 : Implementation of User Microservice

- Created new folder ".NET Microservices w Azure DevOps & AKS"
- Initialized git to track changes
- Created Blank solution
- Added Class Library projects - core and Infra project.
- Added "ASP.net web API" project
- Added Dependency injection for core and Infra project.
- API solution configured and implemented request pipeline.
- Created User Controller to serve register and login flow.
- Created rest client to test API endpoints.
- Implemented automapper to map request and response objects.
