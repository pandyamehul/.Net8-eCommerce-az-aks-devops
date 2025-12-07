using eCommerce.ProductService.BusinessAccessLayer.Mappers;
using eCommerce.ProductsMicroService.API.Middleware;
using eCommerce.ProductsService.API.APIEndpoints;
using eCommerce.ProductsService.BusinessLogicLayer;
using eCommerce.ProductsService.BusinessLogicLayer.Validators;
using eCommerce.ProductsService.DataAccessLayer;
using FluentValidation;

//-----------------------------------------//
//------ Configure build pipeline -------- //
//-----------------------------------------//
var builder = WebApplication.CreateBuilder(args);

//Add DAL and BLL services
builder.Services.AddDataAccessLayer(builder.Configuration);
builder.Services.AddBusinessLogicLayer();

builder.Services.AddControllers();

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