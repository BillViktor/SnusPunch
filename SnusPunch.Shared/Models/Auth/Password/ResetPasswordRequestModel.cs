using System.ComponentModel.DataAnnotations;

namespace SnusPunch.Shared.Models.Auth.Password
{
    public class ResetPasswordRequestModel
    {
        public string Email { get; set; }
        public string Token { get; set; }
        public string Password { get; set; }
        public string ConfirmPassword { get; set; }
    }
}
