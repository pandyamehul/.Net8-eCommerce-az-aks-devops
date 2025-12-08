using eCommerce.ProductsService.DataAccessLayer.Context;
using eCommerce.ProductsService.DataAccessLayer.Repositories;
using eCommerce.ProductsService.DataAccessLayer.RepositoryContracts;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace eCommerce.ProductsService.DataAccessLayer;

public static class DependencyInjection
{
    public static IServiceCollection AddDataAccessLayer(this IServiceCollection services, IConfiguration configuration)
    {
        //TO DO: Add Data Access Layer services into the IoC container
        services.AddDbContext<ApplicationDbContext>(options =>
        {
            options.UseMySQL(configuration.GetConnectionString("DefaultConnection")!,
                mySqlOptions =>
                {
                    mySqlOptions.UseRelationalNulls();
                });
        });

        services.AddScoped<IProductsRepository, ProductsRepository>();

        return services;
    }
}
