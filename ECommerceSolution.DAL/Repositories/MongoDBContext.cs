using System;
using ECommerceSolution.DAL.Interfaces;
using MongoDB.Driver;

namespace ECommerceSolution.DAL.Repositories
{
	public class MongoDBContext : IMongoDBContext
	{
        private readonly IMongoDatabase _database;
        private readonly MongoClient _mongoClient;

        public MongoDBContext(string connectionString, string databaseName)
        {
            if (string.IsNullOrEmpty(connectionString))
                throw new ArgumentException("connectionString must be provided", nameof(connectionString));

            if (string.IsNullOrEmpty(databaseName))
                throw new ArgumentException("databaseName must be provided", nameof(databaseName));

            _mongoClient = new MongoClient(connectionString);
            _database = _mongoClient.GetDatabase(databaseName);
        }

        public IMongoCollection<TDocument> GetCollection<TDocument>(string collectionName)
        {
            if (string.IsNullOrEmpty(collectionName))
                throw new ArgumentException("collectionName must be provided", nameof(collectionName));

            return _database.GetCollection<TDocument>(collectionName);
        }

    }
}

