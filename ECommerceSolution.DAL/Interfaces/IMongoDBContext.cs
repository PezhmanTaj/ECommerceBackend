using System;
using MongoDB.Driver;

namespace ECommerceSolution.DAL.Interfaces
{
	public interface IMongoDBContext
	{
        IMongoCollection<TDocument> GetCollection<TDocument>(string collectionName);
    }
}

