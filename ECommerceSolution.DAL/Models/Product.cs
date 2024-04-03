using System;
using MongoDB.Bson.Serialization.Attributes;

namespace ECommerceSolution.DAL.Models
{
	public class Product
	{
		[BsonId]
		[BsonRepresentation(MongoDB.Bson.BsonType.ObjectId)]
        public string Id { get; internal set; }
		public string Name { get; set; }
		public string Category { get; set; }
		public double Price { get; set; }
	}
}

