using System;
using ECommerceSolution.DAL.Interfaces;
using ECommerceSolution.DAL.Models;
using MongoDB.Driver;

namespace ECommerceSolution.DAL.Repositories
{
	public class ProductRepository : IProductRepository
	{
        private readonly IMongoCollection<Product> _products;

		public ProductRepository(IMongoDBContext mongoDBContext)
		{
            _products = mongoDBContext.GetCollection<Product>("Products");
		}

        public async Task AddProductAsync(Product product)
        {
            await _products.InsertOneAsync(product);
        }

        public async Task<bool> DeleteProductAsync(string id)
        {
            var result = await _products.DeleteOneAsync(p => p.Id == id);
            return result.IsAcknowledged && result.DeletedCount > 0;
        }

        public async Task<IEnumerable<Product>> GetAllProductsAsync()
        {
            return await _products.Find(_ => true).ToListAsync();
        }

        public async Task<Product> GetProductByIdAsync(string id)
        {
            return await _products.Find<Product>(p => p.Id == id).FirstOrDefaultAsync();
        }

        public async Task<bool> UpdateProductAsync(string id, Product product)
        {
            var result = await _products.ReplaceOneAsync(p => p.Id == id, product);
            return result.IsAcknowledged && result.ModifiedCount > 0;
        }
    }
}

