using System;
namespace ECommerceSolution.DAL.Models
{
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

