using eCommerce.OrderService.BusinessLogicLayer;
using eCommerce.OrderService.DataAccessLayer;
using eCommerce.OrderService.API.Middleware;
using eCommerce.OrderService.BusinessLogicLayer.HttpClients;
using eCommerce.OrderService.BusinessLogicLayer.Policies;

//-----------------------------------------//
//------ Configure build pipeline -------- //
//-----------------------------------------//
var builder = WebApplication.CreateBuilder(args);

//Add DAL and BLL services
builder.Services.AddDataAccessLayer(builder.Configuration);
builder.Services.AddBusinessLogicLayer(builder.Configuration);

builder.Services.AddControllers();

//FluentValidations
//builder.Services.AddFluentValidationAutoValidation();

//Swagger
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

//Cors
builder.Services.AddCors(options =>
{
    options.AddDefaultPolicy(builder =>
    {
        builder.WithOrigins("http://localhost:4200")
        .AllowAnyMethod()
        .AllowAnyHeader();
    });
});

builder.Services.AddTransient<IUserServicePolicies, UserServicePolicies>();
builder.Services.AddTransient<IProductServicePolicies, ProductServicePolicies>();

builder.Services.AddHttpClient<UserServiceClient>(client =>
        {
            client.BaseAddress = new Uri(
                $"http://{builder.Configuration["UserServiceName"]}:" +
                $"{builder.Configuration["UserServicePort"]}"
            );
        }
    )
    .AddPolicyHandler(builder.Services.BuildServiceProvider().GetRequiredService<IUserServicePolicies>().GetRetryPolicy())
    .AddPolicyHandler(builder.Services.BuildServiceProvider().GetRequiredService<IUserServicePolicies>().GetCircuitBreakerPolicy())
    .AddPolicyHandler(builder.Services.BuildServiceProvider().GetRequiredService<IUserServicePolicies>().GetTimeoutPolicy());

builder.Services.AddHttpClient<ProductServiceClient>(client =>
{
    client.BaseAddress = new Uri(
        $"http://{builder.Configuration["ProductServiceName"]}:" +
        $"{builder.Configuration["ProductServicePort"]}"
    );
})
    .AddPolicyHandler(builder.Services.BuildServiceProvider().GetRequiredService<IProductServicePolicies>().GetFallbackPolicy());

//-----------------------------------------//
//------ Configure request pipeline ------ //
//-----------------------------------------//
var app = builder.Build();

app.UseExceptionHandlingMiddleware();
app.UseRouting();

//Cors
app.UseCors();

//Swagger
app.UseSwagger();
app.UseSwaggerUI();

//Auth
app.UseHttpsRedirection();
app.UseAuthentication();
app.UseAuthorization();

//Endpoints
app.MapControllers();

app.Run();