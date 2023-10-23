namespace BlogsiteAPI.DataTransferObjects
{
    public class SignupResponseDTO
    {
        public bool IsSignupSuccess { get; set; }
        public IEnumerable<string>? SignupErrors { get; set; }
    }
}