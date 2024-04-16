using AutoMapper;
using ECommerceSolution.BLL.DTOs;
using ECommerceSolution.BLL.Interfaces;
using ECommerceSolution.BLL.Mapper;
using ECommerceSolution.BLL.Services;
using ECommerceSolution.BLL.Validators;
using ECommerceSolution.DAL;
using ECommerceSolution.DAL.Interfaces;
using ECommerceSolution.DAL.Repositories;
using FluentValidation;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Threading.Tasks;

namespace ECommerceConsoleApp;

class Program
{
    static async Task Main(string[] args)
    {
        // Configure services
        var services = new ServiceCollection();

        // MongoDB context configuration - assuming you have a setup for this
        var connectionString = "mongodb://localhost:27017";
        var databaseName = "test";
        services.AddSingleton<IMongoDBContext>(new MongoDBContext(connectionString, databaseName));

        // Repository registration
        services.AddTransient<IProductRepository, ProductRepository>();

        // AutoMapper configuration
        services.AddAutoMapper(typeof(Program).Assembly, typeof(MappingProfile).Assembly);


        // Validator registration - Assuming you're using FluentValidation
        services.AddTransient<IValidator<ProductDTO>, ProductValidator>();

        // Service registration
        services.AddTransient<IProductService, ProductService>();

        // Build the service provider
        var serviceProvider = services.BuildServiceProvider();

        // Use the service provider to resolve your service
        var productService = serviceProvider.GetService<IProductService>();

        // Example usage of the productService
        var newProductDTO = new ProductDTO
        {
            Title = "Test39998 Product",
            Price = 8.88,
            // Other property setups...
        };

        // Assuming CreateProductAsync is a method in your IProductService interface
        IEnumerable<ProductDTO> products = await productService.GetAllProductsAsync();

        Console.WriteLine("Product inserted successfully.");
        foreach (var item in products)
        {
            Console.WriteLine(item.Title);
        }
        Console.ReadKey();
    }
}
