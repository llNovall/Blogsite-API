using BlogsiteDomain.Context;
using Microsoft.Extensions.Options;
using MongoDB.Driver;

namespace BlogsiteMongoAccess.Context
{
    public class MongoDbContext : IMongoDbContext
    {
        private readonly IMongoDatabase _db;
        private readonly MongoCollectionSettings _settings;

        public MongoDbContext(IOptions<MongoConnectionSetting> connectionSetting)
        {
            MongoClient client = new(connectionSetting.Value.ConnectionString);

            _db = client.GetDatabase(connectionSetting.Value.DatabaseName);

            _settings = new()
            {
                AssignIdOnInsert = true
            };
        }

        public IMongoCollection<T> GetCollection<T>(string collectionName) => _db.GetCollection<T>(collectionName, _settings);
    }
}