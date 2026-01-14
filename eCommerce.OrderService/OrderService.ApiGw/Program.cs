using Ocelot.DependencyInjection;
using Ocelot.Middleware;
using Ocelot.Provider.Polly;

var builder = WebApplication.CreateBuilder(args);

// Ocelot Configuration
// Load File based on environment variable in ASPNETCORE_ENVIRONMENT

builder.Configuration.AddJsonFile(
    builder.Configuration["OCELOT_CONFIGFILE"] ?? "ocelot.json",
    optional: false,
    reloadOnChange: true
);
builder.Services
    .AddOcelot()
    .AddPolly();

var app = builder.Build();
await app.UseOcelot();

app.Run();
