using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using ECommerceSolution.BLL.DTOs;
using ECommerceSolution.BLL.Interfaces;
using ECommerceSolution.DAL.Interfaces;
using ECommerceSolution.DAL.Models;
using ECommerceSolution.DAL.Repositories;

namespace ECommerceSolution.BLL.Services
{
    public class OrderService : IOrderService
    {
        private readonly IOrderRepository _orderRepository;
        private readonly IMapper _mapper;
        private readonly IUserContext _userContext;

        public OrderService(IOrderRepository orderRepository, IMapper mapper, IUserContext userContext)
        {
            _orderRepository = orderRepository;
            _mapper = mapper;
            _userContext = userContext;
        }

        public async Task<OrderDetailDTO> GetOrderByIdAsync(string id)
        {
            var order = await _orderRepository.GetByIdAsync(id);
            return _mapper.Map<OrderDetailDTO>(order);
        }

        public async Task<IEnumerable<OrderListDTO>> GetAllOrdersAsync()
        {
            if (_userContext.CurrentUserRole == "Seller")
            {
                
            }
            var orders = await _orderRepository.GetAllAsync();
            return orders.Select(order => _mapper.Map<OrderListDTO>(order));
        }

        public async Task<OrderDetailDTO> CreateOrderAsync(OrderDTO orderDto)
        {
            var order = _mapper.Map<Order>(orderDto);
            await _orderRepository.CreateAsync(order);
            return _mapper.Map<OrderDetailDTO>(order);
        }

        public async Task<bool> UpdateOrderAsync(string id, OrderDTO orderDto)
        {
            var existingOrder = await _orderRepository.GetByIdAsync(id);
            if (existingOrder == null)
                return false;

            var updatedOrder = _mapper.Map(orderDto, existingOrder);
            return await _orderRepository.UpdateAsync(id, updatedOrder);
        }

        public async Task<bool> DeleteOrderAsync(string id)
        {
            var existingOrder = await _orderRepository.GetByIdAsync(id);
            if (existingOrder == null)
                return false;

            return await _orderRepository.DeleteAsync(id);
        }

        public async Task<IEnumerable<Order>> GetFilteredOrdersAsync(string? sellerId, string? status, DateTime? startDate, DateTime? endDate)
        {
            return await _orderRepository.GetFilteredOrdersAsync(sellerId, status, startDate, endDate);
        }
    }
}
