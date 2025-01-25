using OrdersService.DataAccessLayer;
using OrdersService.BusinessLogicLayer;
using FluentValidation.AspNetCore;
using OrdersService.API.Middlewares;
using OrdersService.API.Extenstions;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddCors(opts =>
{
    opts.AddDefaultPolicy(plcy =>
    {
        plcy.WithOrigins("http://localhost:4200").AllowAnyMethod().AllowAnyHeader();
    });
});

builder.Services.AddDataAccessLayer(builder.Configuration);
builder.Services.AddBusinessLogicLayer(builder.Configuration);
builder.Services.AddHttpClients(builder.Configuration);

builder.Services.AddFluentValidationAutoValidation();

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

app.UseExceptionHandlingMiddleware();

app.UseRouting();

app.UseCors();

app.UseSwagger();
app.UseSwaggerUI();

app.UseHttpsRedirection();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();
