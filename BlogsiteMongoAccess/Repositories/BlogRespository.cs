using BlogsiteDomain.Context;
using BlogsiteDomain.Entities.AppContent;
using BlogsiteDomain.Repositories;
using MongoDB.Driver;

namespace BlogsiteMongoAccess.Repositories
{
    public class BlogRespository : BaseRepository<Blog>, IBlogRepository
    {
        public BlogRespository(IMongoDbContext dbContext) : base(dbContext)
        {
        }

        public async Task<bool> AddViewOfBlogByAsync(string blogId, int viewIncrement)
        {
            var result = await _collection.FindAsync(c => c.Id == blogId);
            Blog blog = result.FirstOrDefault();

            if (blog == null)
                return false;

            blog.Views += viewIncrement;

            await _collection.FindOneAndReplaceAsync(c => c.Id == blogId, blog);

            return true;
        }

        public async Task<IEnumerable<Blog>> FindBlogsByTagsAndYearsAsync(List<string> tagIds, List<int> years)
        {
            IAsyncCursor<Blog> result = await _collection.FindAsync(blog =>
            (blog.Tags.Any(tag => tagIds.Count == 0 || (tag.Id != null && tagIds.Contains(tag.Id))))
            && (years.Count == 0 || years.Contains(blog.CreatedAt.Year)));

            return await result.ToListAsync();
        }
    }
}