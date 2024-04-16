using System;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace ECommerceSolution.BLL.DTOs
{
	public class ProductDTO
	{
            public string? Id { get; set; }
            public string? Title { get; set; }
            public string? Description { get; set; }
            public double Price { get; set; }
            public List<string>? Images { get; set; }
            public string? CategoryId { get; set; }
            public string? MeasurementsDescription { get; set; }
            public string? MaterialDescription { get; set; }
            public string? Features { get; set; }
            public List<string>? ColorIds { get; set; }
            public StockStatus StockStatus { get; set; }
    }
    public enum StockStatus
    {
        Available,
        OutOfStock,
        AvailableForPreOrder,
        Backordered,
        AvailableByOrder,
        Discontinued
    }
}

