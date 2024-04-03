using ECommerceSolution.DAL.Models;
using ECommerceSolution.DAL.Repositories;

namespace ECommerceConsolApp;

class Program
{
    static async Task Main(string[] args)
    {
        var connectionString = "mongodb://localhost:27017";
        var databaseName = "test";
        var context = new MongoDBContext(connectionString, databaseName);

        var productRepository = new ProductRepository(context);

        // Create a new product
        var newProduct = new Product
        {
            Name = "Test Product",
            Price = 9.99,
        };

        // Insert the new product
        await productRepository.AddProductAsync(newProduct);

        Console.WriteLine("Product inserted successfully.");
    }
}

