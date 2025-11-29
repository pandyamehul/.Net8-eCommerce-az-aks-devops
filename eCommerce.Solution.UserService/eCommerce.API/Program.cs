using eCommerce.Infrastructure;
using eCommerce.Core;
using eCommerce.API.Middlewares;
using System.Text.Json.Serialization;
using eCommerce.Core.Mappers;

namespace eCommerce.API;

public class Program
{
    public static void Main(string[] args)
    {
        //------ Configure build pipeline ------ //
        var builder = WebApplication.CreateBuilder(args);

        //Add infrastructure services
        builder.Services.AddInfrastructure();

        //Add Core services
        builder.Services.AddCore();

        // Add controllers to the service collection
        builder.Services.AddControllers().AddJsonOptions(options =>
        {
            options.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter());
        });

        // Configure Mapster
        MappingConfig.RegisterMappings();

        //------ Configure request pipeline ------ //
        var app = builder.Build();

        // Add Generic Exception
        app.UseExceptionHandlingMiddleware();

        //Add Routings
        app.UseRouting();

        //Authentication & Authorization
        app.UseAuthentication();
        app.UseAuthorization();

        //Controller routes
        app.MapControllers();

        //Start and Run application
        app.Run();
    }
}
