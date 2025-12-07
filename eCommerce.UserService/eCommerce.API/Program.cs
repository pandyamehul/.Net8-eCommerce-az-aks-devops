using eCommerce.UserService.Infrastructure;
using eCommerce.UserService.Core;
using eCommerce.API.Middlewares;
using eCommerce.UserService.Core.Validators;
using eCommerce.UserService.Core.Mappers;
using System.Text.Json.Serialization;
using FluentValidation;

namespace eCommerce.UserService.API;

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

        // FluentValidation - Register validators from Core assembly
        builder.Services.AddValidatorsFromAssemblyContaining<LoginRequestValidator>();
        builder.Services.AddValidatorsFromAssemblyContaining<RegisterRequestValidator>();

        // Configure Mapster
        MappingConfig.RegisterMappings();

        // Add API Explorer Service - Swagger
        builder.Services.AddEndpointsApiExplorer();

        // Add swagger generation service to create swagger specification
        builder.Services.AddSwaggerGen();

        //Add cors services
        builder.Services.AddCors(options =>
        {
            options.AddDefaultPolicy(builder =>
            {
                builder.WithOrigins("http://localhost:5013")
                .AllowAnyMethod()
                .AllowAnyHeader();
            });
        });

        //-----------------------------------------//
        //------ Configure request pipeline ------ //
        var app = builder.Build();

        // Add Generic Exception
        app.UseExceptionHandlingMiddleware();

        //Add Routings
        app.UseRouting();

        // Add Swagger Support
        app.UseSwagger(); //Adds endpoint that can serve the swagger.json
        app.UseSwaggerUI(); //Adds swagger UI (interactive page to explore and test API endpoints)

        //Authentication & Authorization
        app.UseAuthentication();
        app.UseAuthorization();

        //Controller routes
        app.MapControllers();

        //Start and Run application
        app.Run();
    }
}
