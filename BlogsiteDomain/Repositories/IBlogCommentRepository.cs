using BlogsiteDomain.Entities.AppContent;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlogsiteDomain.Repositories
{
    public interface IBlogCommentRepository : IRepository<BlogComment>
    {
        public Task<IEnumerable<BlogComment>> GetBlogComments(string blogId, string? parentCommentId, int depth = 0);
    }
}