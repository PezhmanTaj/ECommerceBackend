using System;
using ECommerceSolution.DAL.Interfaces;
using ECommerceSolution.DAL.Models;
using MongoDB.Driver;

namespace ECommerceSolution.DAL.Repositories
{
	public class UserRepository : IUserRepository
	{
        private readonly IMongoCollection<User> _users;

		public UserRepository(IMongoDBContext mongoDBContext)
		{
            _users = mongoDBContext.GetCollection<User>("Users");
		}

        public async Task AddAsync(User user)
        {
            await _users.InsertOneAsync(user);
        }

        public async Task<bool> DeleteAsync(string id)
        {
            var result = await _users.DeleteOneAsync(u => u.UserId == id);
            return result.IsAcknowledged && result.DeletedCount > 0;
        }

        public async Task<IEnumerable<User>> GetAllAsync()
        {
            return await _users.Find(_ => true).ToListAsync();
        }

        public async Task<User> GetByIdAsync(string id)
        {
            return await _users.Find(u => u.UserId == id).FirstOrDefaultAsync();
        }

        public async Task<User> GetByUsernameAsync(string username)
        {
            return await _users.Find(u => u.Username == username).FirstOrDefaultAsync();
        }

        public async Task<bool> UpdateAsync(string id, User user)
        {
            var result = await _users.ReplaceOneAsync(u => u.UserId == id, user);
            return result.IsAcknowledged && result.ModifiedCount > 0;
        }
    }
}

