using System.Collections.Generic;
using System.Threading.Tasks;
using ECommerceSolution.DAL.Interfaces;
using ECommerceSolution.DAL.Models;
using MongoDB.Driver;

namespace ECommerceSolution.DAL.Repositories
{
    public class OrderRepository : IOrderRepository
    {
        private readonly IMongoCollection<Order> _orderCollection;

        public OrderRepository(IMongoDBContext dbContext)
        {
            _orderCollection = dbContext.GetCollection<Order>("Orders");
        }

        public async Task<Order> GetByIdAsync(string id)
        {
            return await _orderCollection.Find(order => order.Id == id).FirstOrDefaultAsync();
        }

        public async Task<IEnumerable<Order>> GetAllAsync()
        {
            return await _orderCollection.Find(order => true).ToListAsync();
        }

        public async Task<bool> CreateAsync(Order order)
        {
            try
            {
                await _orderCollection.InsertOneAsync(order);
                return true;
            }
            catch
            {
                return false;
            }
        }

        public async Task<bool> UpdateAsync(string id, Order order)
        {
            var result = await _orderCollection.ReplaceOneAsync(o => o.Id == id, order);
            return result.IsAcknowledged && result.ModifiedCount > 0;
        }

        public async Task<IEnumerable<Order>> GetFilteredOrdersAsync(string? sellerId = null, string? status = null, DateTime? startDate = null, DateTime? endDate = null)
        {
            var filterBuilder = Builders<Order>.Filter;
            var filter = filterBuilder.Empty; 

            if (!string.IsNullOrEmpty(sellerId))
            {
                filter &= filterBuilder.Eq(order => order.OrderOwnershipId, sellerId);
            }
            if (!string.IsNullOrEmpty(status))
            {
                OrderStatus statusEnum = (OrderStatus)Enum.Parse(typeof(OrderStatus), status);
                filter &= filterBuilder.Eq(order => order.Status, statusEnum);
            }

            if (startDate.HasValue)
            {
                filter &= filterBuilder.Gte(order => order.OrderDate, startDate);
            }
            if ( endDate.HasValue)
            {
                filter &= filterBuilder.Lt(order => order.OrderDate, endDate);
            }

            return await _orderCollection.Find(filter).ToListAsync();
        }


        public async Task<IEnumerable<Order>> GetOrdersBySellerId(string sellerId)
        {
            return await _orderCollection.Find(o => o.OrderOwnershipId == sellerId).ToListAsync();
        }

        public async Task<bool> DeleteAsync(string id)
        {
            var result = await _orderCollection.DeleteOneAsync(order => order.Id == id);
            return result.IsAcknowledged && result.DeletedCount > 0;
        }

    }
}
