using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System.Collections.Generic;

namespace ECommerceSolution.DAL.Models
{
    public class Product
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string? Id { get; set; }
        [BsonRepresentation(BsonType.ObjectId)]
        public string OwnerUserId { get; set; }
        public string? Title { get; set; }
        public string? Description { get; set; }
        public double Price { get; set; }
        public List<string>? Images { get; set; }
        [BsonRepresentation(BsonType.ObjectId)]
        public string? CategoryId { get; set; }
        public string? MeasurementsDescription { get; set; }
        public string? MaterialDescription { get; set; }
        public string? Features { get; set; }
        [BsonRepresentation(BsonType.ObjectId)]
        public List<string>? ColorIds { get; set; }
        public StockStatus? StockStatus { get; set; }

    }
}
