using eCommerce.UserService.Core.RepositoryContracts;
using eCommerce.UserService.Infrastructure.DbContext;
using eCommerce.UserService.Infrastructure.Repositories;
using Microsoft.Extensions.DependencyInjection;

namespace eCommerce.UserService.Infrastructure;

public static class DependencyInjection
{
    /// <summary>
    /// Extension method to add infrastructure service to dependency injection container
    /// </summary>
    /// <param name="services"></param>
    /// <returns></returns>
    public static IServiceCollection AddInfrastructure(this IServiceCollection services)
    {
        //TO DO: Add service to IoC container
        //Infrastructure service often include data access, caching & other low-level components

        services.AddSingleton<IUsersRepository, UsersRepository>();
        services.AddTransient<DapperDbContext>();

        return services;
    }
}
