using System.ComponentModel.DataAnnotations;

namespace BlogsiteAPI.DataTransferObjects
{
    public class AppUserSignupDTO
    {
        [StringLength(20)]
        public string? Username { get; set; }

        [EmailAddress(ErrorMessage = "Must enter a valid email address.")]
        [Required(ErrorMessage = "Email is required.")]
        public string? EmailAddress { get; set; }

        [Required(ErrorMessage = "Password is required.")]
        public string? Password { get; set; }

        [Compare("Password", ErrorMessage = "The password and confirmation password must match.")]
        public string? ConfirmPassword { get; set; }
    }
}