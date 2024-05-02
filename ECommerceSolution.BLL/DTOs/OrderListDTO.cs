using System;
namespace ECommerceSolution.BLL.DTOs
{
    public class OrderListDTO
    {
        public string Id { get; set; }
        public decimal TotalPrice { get; set; }
        public DateTime OrderDate { get; set; }
    }
}

