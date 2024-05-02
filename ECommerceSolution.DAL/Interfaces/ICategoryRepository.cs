using System.Collections.Generic;
using System.Threading.Tasks;
using ECommerceSolution.DAL.Models;


namespace ECommerceSolution.DAL.Interfaces
{
    public interface ICategoryRepository
    {
        Task<Category> GetByIdAsync(string id);
        Task<IEnumerable<Category>> GetAllAsync(bool includeInactive = false);
        Task CreateAsync(Category category);
        Task<bool> UpdateAsync(string id, Category category);
        Task<bool> DeleteAsync(string id);

    }


}
