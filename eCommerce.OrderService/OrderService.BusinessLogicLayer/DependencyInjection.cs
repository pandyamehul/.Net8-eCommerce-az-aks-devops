using eCommerce.OrderService.BusinessLogicLayer.Mappers;
using eCommerce.OrderService.BusinessLogicLayer.ServiceContracts;
using eCommerce.OrderService.BusinessLogicLayer.Services;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System.Reflection;
using FluentValidation;
using Mapster;
using MapsterMapper;
using StackExchange.Redis;

namespace eCommerce.OrderService.BusinessLogicLayer;

public static class DependencyInjection
{
    public static IServiceCollection AddBusinessLogicLayer(this IServiceCollection services, IConfiguration configuration)
    {
        //Add business logic layer services into the IoC container
        //services.AddScoped<IOrdersService, OrdersService>();

        //services.AddValidatorsFromAssemblyContaining<OrderAddRequestValidator>();
        //services.AddValidatorsFromAssemblyContaining<OrderUpdateRequestValidator>();

        //services.AddValidatorsFromAssemblyContaining<OrderItemAddRequestValidator>();
        //services.AddValidatorsFromAssemblyContaining<OrderItemUpdateRequestValidator>();

        services.AddValidatorsFromAssemblyContaining<OrderAddRequestToOrderMappingProfile>();

        // Configure and Register Mapster - Scan current assembly
        TypeAdapterConfig.GlobalSettings.Scan(Assembly.GetExecutingAssembly());

        // Register Mapster IMapper
        services.AddSingleton(TypeAdapterConfig.GlobalSettings);

        services.AddScoped<IMapper, Mapper>();

        services.AddScoped<IOrdersService, OrdersService>();

        services.AddStackExchangeRedisCache(options =>
        {
            options.Configuration = $"{configuration["REDIS_HOST"]}:{configuration["REDIS_PORT"]}";
        });

        return services;
    }
}