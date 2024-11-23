namespace SnusPunch.Shared.Models.Auth.Email
{
    public class VerifyEmailRequest
    {
        public string Email { get; set; }
        public string Token { get; set; }
    }
}
