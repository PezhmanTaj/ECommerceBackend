using System;
using ECommerceSolution.DAL.Models;

namespace ECommerceSolution.DAL.Interfaces
{
	public interface IUserRepository
	{
        Task<User> GetByIdAsync(string id);
        Task<User> GetByUsernameAsync(string username);
        Task<IEnumerable<User>> GetAllAsync();
        Task AddAsync(User user);
        Task<bool> UpdateAsync(string id,User user);
        Task<bool> DeleteAsync(string id);
    }
}

