using System;
using ECommerceSolution.DAL.Models;

namespace ECommerceSolution.BLL.DTOs
{
    public class OrderDetailDTO : OrderDTO
    {
        public string Id { get; set; }
        public string OrderOwnershipId { get; set; }
        public DateTime OrderDate { get; set; }
        public OrderStatus Status { get; set; }
    }
}

