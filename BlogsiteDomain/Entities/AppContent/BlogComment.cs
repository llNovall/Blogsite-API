using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlogsiteDomain.Entities.AppContent
{
    public class BlogComment : BaseEntity
    {
        [Required]
        public string BlogId { get; set; } = null!;

        public string? ParentCommentId { get; set; }

        [Required]
        [StringLength(30, MinimumLength = 3)]
        public string Sender { get; set; } = null!;

        [Required]
        [StringLength(1000, MinimumLength = 3)]
        public string CommentBody { get; set; } = null!;

        public DateTime CreatedAt { get; set; }
    }
}