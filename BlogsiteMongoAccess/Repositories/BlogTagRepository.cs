using BlogsiteDomain.Context;
using BlogsiteDomain.Entities.AppContent;
using BlogsiteDomain.Repositories;

namespace BlogsiteMongoAccess.Repositories
{
    public class BlogTagRepository : BaseRepository<BlogTag>, IBlogTagRepository
    {
        public BlogTagRepository(IMongoDbContext dbContext) : base(dbContext)
        {
        }
    }
}