using eCommerce.ProductsMicroService.API.Middleware;
using eCommerce.ProductsService.API.APIEndpoints;
using eCommerce.ProductsService.BusinessLogicLayer;
using eCommerce.ProductsService.DataAccessLayer;
using System.Text.Json.Serialization;

//-----------------------------------------//
//------ Configure build pipeline -------- //
//-----------------------------------------//
var builder = WebApplication.CreateBuilder(args);

//Add DAL and BLL services
builder.Services.AddDataAccessLayer(builder.Configuration);
builder.Services.AddBusinessLogicLayer();

builder.Services.AddControllers();

//Add model binder to read values from JSON to enum
builder.Services.ConfigureHttpJsonOptions(options =>
{
    options.SerializerOptions.Converters.Add(new JsonStringEnumConverter());
});

//// FluentValidation - Register validators from Core assembly
//builder.Services.AddValidatorsFromAssemblyContaining<ProductAddRequestValidator>();
//builder.Services.AddValidatorsFromAssemblyContaining<ProductUpdateRequestValidator>();

//// Configure Mapster
//ProductMappingProfile.RegisterMappings();

//-----------------------------------------//
//------ Configure request pipeline ------ //
//-----------------------------------------//
var app = builder.Build();

app.UseExceptionHandlingMiddleware();
app.UseRouting();

//Auth
app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();
app.MapProductAPIEndpoints();

app.Run();