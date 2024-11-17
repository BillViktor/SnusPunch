namespace SnusPunch.Shared.Models.Auth
{
    public class VerifyEmailRequest
    {
        public string UserId { get; set; }
        public string Token { get; set; }
    }
}
