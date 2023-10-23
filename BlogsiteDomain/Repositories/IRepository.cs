namespace BlogsiteDomain.Repositories
{
    public interface IRepository<T> where T : class
    {
        public Task<T?> GetEntityAsync(string id);

        public Task<IEnumerable<T>> GetAllEntitiesAsync();

        public Task<bool> AddEntityAsync(T entity);

        public Task<bool> UpdateEntityAsync(T entity);

        public Task<bool> DeleteEntityAsync(string id);
    }
}