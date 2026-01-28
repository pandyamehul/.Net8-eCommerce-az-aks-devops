#See https://aka.ms/customizecontainer to learn how to customize your debug container and how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:9.0 AS base
USER app
WORKDIR /app
EXPOSE 8080

FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["OrderService.ApiGw/OrderService.ApiGw.csproj", "ApiGw/"]
RUN dotnet restore "./OrderService.ApiGw/OrderService.ApiGw.csproj"
COPY . .
WORKDIR "/src/ApiGw"
RUN dotnet build "./OrderService.ApiGw.csproj" -c $BUILD_CONFIGURATION -o /app/build
FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "./OrderService.ApiGw.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .

# IMPORTANT: Explicitly copy ocelot.json
COPY OrderService.ApiGw/ocelot.json ./ocelot.json

ENTRYPOINT ["dotnet", "OrderService.ApiGw.dll"]