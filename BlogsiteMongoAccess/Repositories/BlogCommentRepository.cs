using BlogsiteDomain.Context;
using BlogsiteDomain.Entities.AppContent;
using BlogsiteDomain.Repositories;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlogsiteMongoAccess.Repositories
{
    public class BlogCommentRepository : BaseRepository<BlogComment>, IBlogCommentRepository
    {
        public BlogCommentRepository(IMongoDbContext dbContext) : base(dbContext)
        {
        }

        public async Task<IEnumerable<BlogComment>> GetBlogComments(string blogId, string? parentCommentId = null, int depth = 1)
        {
            if (depth < 0)
                depth = 0;

            List<BlogComment> comments = new List<BlogComment>();

            IAsyncCursor<BlogComment> result = await _collection.FindAsync(c => c.BlogId == blogId & c.ParentCommentId == parentCommentId);
            comments.AddRange(await result.ToListAsync());

            List<string?> parentIds = comments.Select(c => c.Id).ToList();

            for (int i = 0; i < depth; i++)
            {
                result = await _collection.FindAsync(c => c.BlogId == blogId & parentIds.Contains(c.ParentCommentId));
                List<BlogComment> foundComments = await result.ToListAsync();
                comments.AddRange(foundComments);

                if (i + 1 < depth)
                {
                    parentIds = foundComments.Select(c => c.Id).ToList();
                }
            }

            return comments;
        }
    }
}