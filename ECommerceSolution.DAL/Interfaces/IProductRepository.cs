using System;
using ECommerceSolution.DAL.Models;

namespace ECommerceSolution.DAL.Interfaces
{
	public interface IProductRepository
	{
        Task<IEnumerable<Product>> GetAllProductsAsync();
        Task<IEnumerable<Product>> GetProductsBySellerId(string sellerId);
        Task<Product> GetProductByIdAsync(string id);
        Task AddProductAsync(Product product);
        Task<bool> UpdateProductAsync(string id, Product product);
        Task<bool> DeleteProductAsync(string id);
    }
}

