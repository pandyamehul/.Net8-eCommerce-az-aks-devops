using eCommerce.ProductService.BusinessAccessLayer.Mappers;
using eCommerce.ProductsService.BusinessLogicLayer.ServiceContracts;
using eCommerce.ProductsService.BusinessLogicLayer.Validators;
using eCommerce.ProductService.BusinessAccessLayer.Publisher;
using Microsoft.Extensions.DependencyInjection;
using FluentValidation;
using Mapster;
using MapsterMapper;

namespace eCommerce.ProductsService.BusinessLogicLayer;

public static class DependencyInjection
{
    public static IServiceCollection AddBusinessLogicLayer(this IServiceCollection services)
    {
        //TO DO: Add Business Logic Layer services into the IoC container'

        // FluentValidation - Register validators from Core assembly
        services.AddValidatorsFromAssemblyContaining<ProductAddRequestValidator>();
        services.AddValidatorsFromAssemblyContaining<ProductUpdateRequestValidator>();

        // Configure Mapster
        ProductMappingProfile.RegisterMappings();

        // Register Mapster mapper
        var config = TypeAdapterConfig.GlobalSettings;
        services.AddSingleton(config);
        services.AddScoped<IMapper, ServiceMapper>();

        services.AddScoped<IProductsService, ProductsService.BusinessLogicLayer.Services.ProductsService>();
        services.AddTransient<IProductEvent, ProductEventPublish>();

        return services;
    }
}