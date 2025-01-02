using ECommerce.Infrastructure;
using ECommerce.Core;
using ECommerce.API.Middlewares;
using System.Text.Json.Serialization;
using ECommerce.Core.Mappers;
using FluentValidation.AspNetCore;

var builder = WebApplication.CreateBuilder(args);

// Add controllers to service collection
builder.Services.AddControllers();
//builder.Services.AddControllers()
//    .AddJsonOptions(opt =>
//    {
//        opt.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter());
//    });

builder.Services.AddInfrastructure();
builder.Services.AddCore();

builder.Services.AddAutoMapper(typeof(AppUserMappingProfile).Assembly);

builder.Services.AddFluentValidationAutoValidation();

// Build the web application
var app = builder.Build();

app.UseExceptionHandlingMiddleware();

// Routing
app.UseRouting();

// Auth
app.UseAuthentication();
app.UseAuthorization();

// Controller Routes
app.MapControllers();

app.Run();
