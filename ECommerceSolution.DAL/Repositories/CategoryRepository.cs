using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ECommerceSolution.DAL.Interfaces;
using ECommerceSolution.DAL.Models;
using MongoDB.Driver;
using MongoDB.Bson;

namespace ECommerceSolution.DAL.Repositories
{
    public class CategoryRepository : ICategoryRepository
    {
        private readonly IMongoCollection<Category> _category;

        public CategoryRepository(IMongoDBContext mongoDBContext)
        {
            _category = mongoDBContext.GetCollection<Category>("Categories");
        }

        public async Task CreateAsync(Category category)
        {
            await _category.InsertOneAsync(category);
        }

        public async Task<IEnumerable<Category>> GetAllAsync(bool includeInactive = false)
        {
            var filter = includeInactive ? Builders<Category>.Filter.Empty : Builders<Category>.Filter.Eq("IsActive", true);
            return await _category.Find(filter).ToListAsync();
        }

        public async Task<Category> GetByIdAsync(string id)
        {
            return await _category.Find<Category>(c => c.Id == id).FirstOrDefaultAsync();
        }

        public async Task<bool> UpdateAsync(string id, Category category)
        {
            var filter = Builders<Category>.Filter.Eq("Id", id);
            var result = await _category.ReplaceOneAsync(filter, category);
            return result.IsAcknowledged && result.ModifiedCount > 0;
        }

        public async Task<bool> DeleteAsync(string id)
        {
            var result = await _category.DeleteOneAsync(c => c.Id == id);
            return result.IsAcknowledged && result.DeletedCount > 0;
        }



    }
}
