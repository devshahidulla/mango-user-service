using Amazon.EventBridge;
using Amazon.EventBridge.Model;
using FluentValidation;
using FluentValidation.AspNetCore;
using Mango.UserService.Application.Interfaces;
using Mango.UserService.Application.Services;
using Mango.UserService.Application.Validators;
using Mango.UserService.Infrastructure.Repositories;
using System.Data;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddFluentValidationAutoValidation()
   .AddFluentValidationClientsideAdapters();
builder.Services.AddValidatorsFromAssemblyContaining<RegisterRequestValidator>();

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
// PostgreSQL + Dapper connection
builder.Services.AddScoped<IDbConnection>(sp =>
    new Npgsql.NpgsqlConnection(
        builder.Configuration.GetConnectionString("DefaultConnection")
    ));

// Register application services  
builder.Services.AddScoped<IUserRepository, UserRepository>();
builder.Services.AddScoped<IUserService, UserService>();
builder.Services.AddAWSService<IAmazonEventBridge>();
builder.Services.AddScoped<IEventPublisher, EventBridgePublisher>();


var app = builder.Build();

if (app.Environment.IsDevelopment())
{
  app.UseSwagger();               // generates Swagger JSON  
  app.UseSwaggerUI();             // enables Swagger UI at /swagger  
}

app.UseHttpsRedirection();
app.UseAuthorization();
app.MapControllers();
app.Run();
