using System.ComponentModel.DataAnnotations;

namespace BlogsiteDomain.Entities.AppContent
{
    public class Blog : BaseEntity
    {
        [StringLength(200, MinimumLength = 1)]
        public string Title { get; set; } = null!;

        [StringLength(500, MinimumLength = 1)]
        [RegularExpression(@"https{0,1}://.*")]
        public string ImageUrl { get; set; } = null!;

        [StringLength(10000, MinimumLength = 1)]
        public string Content { get; set; } = null!;

        public IEnumerable<BlogTag> Tags { get; set; } = new List<BlogTag>();
        public DateTime CreatedAt { get; set; }
        public DateTime EditedAt { get; set; }
        public bool IsFeatured { get; set; }
        public int Views { get; set; }
    }
}