using eCommerce.ProductService.BusinessAccessLayer.Mappers;
using eCommerce.ProductsService.BusinessLogicLayer.ServiceContracts;
using eCommerce.ProductsService.BusinessLogicLayer.Validators;
using FluentValidation;
using Microsoft.Extensions.DependencyInjection;

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

        services.AddScoped<IProductsService, ProductsService.BusinessLogicLayer.Services.ProductsService>();

        return services;
    }
}