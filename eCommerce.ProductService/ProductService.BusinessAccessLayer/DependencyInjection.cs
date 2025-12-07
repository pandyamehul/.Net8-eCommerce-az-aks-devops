using eCommerce.ProductService.BusinessAccessLayer.Mappers;
using eCommerce.ProductsService.BusinessLogicLayer.Validators;
using Microsoft.Extensions.DependencyInjection;
using FluentValidation;

namespace eCommerce.ProductsService.BusinessLogicLayer;

public static class DependencyInjection
{
    public static IServiceCollection AddBusinessLogicLayer(this IServiceCollection services)
    {
        //TO DO: Add Business Logic Layer services into the IoC container

        // FluentValidation - Register validators from Core assembly
        services.AddValidatorsFromAssemblyContaining<ProductAddRequestValidator>();
        services.AddValidatorsFromAssemblyContaining<ProductUpdateRequestValidator>();

        // Configure Mapster
        ProductMappingProfile.RegisterMappings();

        return services;
    }
}