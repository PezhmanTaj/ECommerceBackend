using System;
using System.Collections.Generic;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace ECommerceSolution.DAL.Models
{
    public class Order
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string Id { get; set; }
        [BsonRepresentation(BsonType.ObjectId)]
        public string CustomerId { get; set; }
        [BsonRepresentation(BsonType.ObjectId)]
        public string OrderOwnershipId { get; set; }
        public DateTime OrderDate { get; set; }
        public List<OrderItem> OrderItems { get; set; }
        public decimal TotalPrice { get; set; }
        public Address ShippingAddress { get; set; }
        public OrderStatus Status { get; set; }

        public Order()
        {
            OrderItems = new List<OrderItem>();
        }
    }
}
