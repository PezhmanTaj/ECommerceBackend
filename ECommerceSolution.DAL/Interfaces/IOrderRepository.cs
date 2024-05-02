using System.Collections.Generic;
using System.Threading.Tasks;
using ECommerceSolution.DAL.Models;

namespace ECommerceSolution.DAL.Interfaces
{
    public interface IOrderRepository
    {
        Task<Order> GetByIdAsync(string id);
        Task<IEnumerable<Order>> GetAllAsync();
        Task<bool> CreateAsync(Order order);
        Task<bool> UpdateAsync(string id, Order order);
        Task<bool> DeleteAsync(string id);
        Task<IEnumerable<Order>> GetFilteredOrdersAsync(string? sellerId, string? status, DateTime? startDate, DateTime? endDate);
    }
}
