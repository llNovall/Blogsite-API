using System.ComponentModel.DataAnnotations;

namespace BlogsiteAPI.DataTransferObjects
{
    public class AppUserLoginDTO
    {
        [Required]
        [StringLength(20)]
        public string? Username { get; set; }

        [Required(ErrorMessage = "Password is required.")]
        public string? Password { get; set; }
    }
}