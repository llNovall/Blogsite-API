using System.ComponentModel.DataAnnotations;

namespace BlogsiteAPI.DataTransferObjects
{
    public class BlogAddViewDTO
    {
        public string? Id { get; set; }

        public int ViewsToAdd { get; set; }
    }
}