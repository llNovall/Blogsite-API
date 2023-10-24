using BlogsiteDomain.Entities.AppContent;

namespace BlogsiteDomain.Repositories
{
    public interface IBlogRepository : IRepository<Blog>
    {
        Task<IEnumerable<Blog>> FindBlogsByTagsAndYearsAsync(List<string> tagIds, List<int> years);

        Task<bool> AddViewOfBlogByAsync(string blogId, int viewIncrement);
    }
}