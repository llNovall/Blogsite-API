using System.ComponentModel.DataAnnotations;

namespace BlogsiteDomain.Entities.AppContent
{
    public class BlogTag : BaseEntity
    {
        [Required]
        public string TagName { get; set; } = null!;
    }
}