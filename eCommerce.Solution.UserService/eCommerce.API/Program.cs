using eCommerce.API.Middlewares;
using eCommerce.Core;
using eCommerce.Infrastructure;

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

        //Add Controller to service collection
        builder.Services.AddControllers();


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
