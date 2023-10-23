using BlogsiteDomain.Context;
using BlogsiteDomain.Entities.AppContent;
using BlogsiteDomain.Repositories;
using MongoDB.Driver;

namespace BlogsiteMongoAccess.Repositories
{
    public abstract class BaseRepository<T> : IRepository<T> where T : BaseEntity
    {
        protected readonly IMongoDbContext _dbContext;
        protected readonly IMongoCollection<T> _collection;

        public BaseRepository(IMongoDbContext dbContext)
        {
            _dbContext = dbContext;
            _collection = _dbContext.GetCollection<T>(typeof(T).Name);
        }

        public virtual async Task<bool> AddEntityAsync(T entity)
        {
            try
            {
                await _collection.InsertOneAsync(entity);
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        public virtual async Task<bool> DeleteEntityAsync(string id)
        {
            try
            {
                await _collection.DeleteOneAsync(c => c.Id == id);
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        public virtual async Task<IEnumerable<T>> GetAllEntitiesAsync()
        {
            try
            {
                return await _collection.Find(_ => true).ToListAsync();
            }
            catch (Exception)
            {
                return Enumerable.Empty<T>();
            }
        }

        public virtual async Task<T?> GetEntityAsync(string id)
        {
            try
            {
                return await _collection.Find(c => c.Id == id).FirstOrDefaultAsync();
            }
            catch (Exception)
            {
                return null;
            }
        }

        public virtual async Task<bool> UpdateEntityAsync(T entity)
        {
            try
            {
                await _collection.ReplaceOneAsync(c => c.Id == entity.Id, entity);
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }
    }
}