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

        // Get connection string from appsettings.json
        string connectionString = configuration.GetConnectionString("DefaultConnection")!;

        // Replace placeholders with environment variable values
        connectionString = connectionString.Replace("$MYSQL_HOST", Environment.GetEnvironmentVariable("MYSQL_HOST")!);
        connectionString = connectionString.Replace("$MYSQL_USER", Environment.GetEnvironmentVariable("MYSQL_USER")!);
        connectionString = connectionString.Replace("$MYSQL_PASSWORD", Environment.GetEnvironmentVariable("MYSQL_PASSWORD")!);
        connectionString = connectionString.Replace("$MYSQL_DATABASE", Environment.GetEnvironmentVariable("MYSQL_DATABASE")!);

        // Configure DbContext with MySQL provider
        services.AddDbContext<ApplicationDbContext>(options =>
        {
            options.UseMySQL(connectionString!,
                mySqlOptions =>
                {
                    mySqlOptions.UseRelationalNulls();
                });
        });

        // Register repositories
        services.AddScoped<IProductsRepository, ProductsRepository>();

        return services;
    }
}
