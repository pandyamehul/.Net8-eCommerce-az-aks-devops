using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using MongoDB.Driver;
using eCommerce.OrderService.DataAccessLayer.RepositoryContracts;
using eCommerce.OrderService.DataAccessLayer.Repositories;

namespace eCommerce.OrderService.DataAccessLayer;

public static class DependencyInjection
{
    public static IServiceCollection AddDataAccessLayer(this IServiceCollection services, IConfiguration configuration)
    {
        //Add data access layer services into the IoC container
        string connectionStringTemplate = configuration.GetConnectionString("MongoDB")!;
        string connectionString = connectionStringTemplate
          .Replace("$MONGO_HOST", Environment.GetEnvironmentVariable("MONGODB_HOST") ?? "localhost")
          .Replace("$MONGO_PORT", Environment.GetEnvironmentVariable("MONGODB_PORT") ?? "27017");

        // Console.WriteLine($"MongoDB Connection String: {connectionString}");

        services.AddSingleton<IMongoClient>(new MongoClient(connectionString));

        services.AddScoped<IMongoDatabase>(provider =>
        {
            IMongoClient client = provider.GetRequiredService<IMongoClient>();
            return client.GetDatabase("OrdersDatabase");
        });

        // Register repositories
        services.AddScoped<IOrdersRepository, OrdersRepository>();

        return services;
    }
}