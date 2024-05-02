using System.Collections.Generic;
using System.Threading.Tasks;
using ECommerceSolution.BLL.DTOs;
using ECommerceSolution.DAL.Models;

namespace ECommerceSolution.BLL.Interfaces
{
    public interface IOrderService
    {
        Task<OrderDetailDTO> GetOrderByIdAsync(string id);
        Task<IEnumerable<OrderListDTO>> GetAllOrdersAsync();
        Task<OrderDetailDTO> CreateOrderAsync(OrderDTO orderDto);
        Task<bool> UpdateOrderAsync(string id, OrderDTO orderDto);
        Task<bool> DeleteOrderAsync(string id);
        Task<IEnumerable<Order>> GetFilteredOrdersAsync(string? sellerId, string? status, DateTime? startDate, DateTime? endDate);

    }
}

