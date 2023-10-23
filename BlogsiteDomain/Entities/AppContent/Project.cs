using System.ComponentModel.DataAnnotations;

namespace BlogsiteDomain.Entities.AppContent
{
    public class Project : BaseEntity
    {
        [StringLength(500, MinimumLength = 1)]
        public string ProjectName { get; set; }

        [StringLength(8000, MinimumLength = 1)]
        public string ProjectDescription { get; set; }

        [StringLength(500, MinimumLength = 1)]
        [RegularExpression(@"https{0,1}://.*")]
        public string ProjectLink { get; set; }

        public Project(string projectName, string projectDescription, string projectLink)
        {
            ProjectName = projectName;
            ProjectDescription = projectDescription;
            ProjectLink = projectLink;
        }
    }
}