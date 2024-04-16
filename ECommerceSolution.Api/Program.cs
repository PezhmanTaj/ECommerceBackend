using ECommerceSolution.BLL.DTOs;
using ECommerceSolution.BLL.Interfaces;
using ECommerceSolution.BLL.Mapper;
using ECommerceSolution.BLL.Services;
using ECommerceSolution.BLL.Validators;
using ECommerceSolution.DAL.Interfaces;
using ECommerceSolution.DAL.Repositories;
using FluentValidation;
using Microsoft.Extensions.DependencyInjection; // Ensure you have this using directive for IServiceCollection
using AutoMapper; // Ensure this if you're using AutoMapper

var builder = WebApplication.CreateBuilder(args);

// Add services to the DI container.
builder.Services.AddControllers().AddJsonOptions(options =>
{
    options.JsonSerializerOptions.PropertyNamingPolicy = null;
});
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowLocalhost3000",
        builder =>
        {
            builder.WithOrigins("http://localhost:3000")
                   .AllowAnyHeader()
                   .AllowAnyMethod()
                   .AllowCredentials();
                    
        });
});

// MongoDB context configuration
var connectionString = "mongodb://localhost:27017";
var databaseName = "test";
builder.Services.AddSingleton<IMongoDBContext>(_ => new MongoDBContext(connectionString, databaseName));

// Repository registration
builder.Services.AddTransient<IProductRepository, ProductRepository>();

// AutoMapper configuration
builder.Services.AddAutoMapper(typeof(Program).Assembly, typeof(MappingProfile).Assembly); 

// Validator registration - Assuming you're using FluentValidation
builder.Services.AddTransient<IValidator<ProductDTO>, ProductValidator>();

// Service registration
builder.Services.AddTransient<IProductService, ProductService>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseCors("AllowLocalhost3000");
app.UseHttpsRedirection();
app.UseAuthorization();
app.MapControllers();
app.Run();
