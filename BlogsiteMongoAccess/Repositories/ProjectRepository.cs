using BlogsiteDomain.Context;
using BlogsiteDomain.Entities.AppContent;
using BlogsiteDomain.Repositories;

namespace BlogsiteMongoAccess.Repositories
{
    public class ProjectRepository : BaseRepository<Project>, IProjectRepository
    {
        public ProjectRepository(IMongoDbContext dbContext) : base(dbContext)
        {
        }
    }
}