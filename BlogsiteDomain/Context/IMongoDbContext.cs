using MongoDB.Driver;

namespace BlogsiteDomain.Context
{
    public interface IMongoDbContext
    {
        public IMongoCollection<T> GetCollection<T>(string collectionName);
    }
}