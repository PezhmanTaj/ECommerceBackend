using System;
namespace ECommerceSolution.BLL.DTOs
{
    public class OrderDTO
    {
        public string CustomerId { get; set; }
        public List<OrderItemDTO> OrderItems { get; set; }
        public string OrderOwnershipId { get; set; }
        public decimal TotalPrice { get; set; }
        public AddressDTO ShippingAddress { get; set; }
    }
}

