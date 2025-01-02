using ECommerce.Infrastructure;
using ECommerce.Core;
using ECommerce.API.Middlewares;
using System.Text.Json.Serialization;
using ECommerce.Core.Mappers;
using FluentValidation.AspNetCore;
using MicroElements.Swashbuckle.FluentValidation.AspNetCore;

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
builder.Services.AddFluentValidationRulesToSwagger();

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddCors(opts =>
{
    opts.AddDefaultPolicy(builderr =>
    {
        builderr.WithOrigins("http://localhost:4200")
            .AllowAnyMethod()
            .AllowAnyHeader();
    });
});

// Build the web application
var app = builder.Build();

app.UseExceptionHandlingMiddleware();

// Routing
app.UseRouting();

app.UseSwagger();
app.UseSwaggerUI();

app.UseCors();

// Auth
app.UseAuthentication();
app.UseAuthorization();

// Controller Routes
app.MapControllers();

app.Run();
