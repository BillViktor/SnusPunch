using System.ComponentModel.DataAnnotations;

namespace SnusPunch.Shared.Models.Auth.Password
{
    public class ForgotPasswordRequestModel
    {
        [Required(ErrorMessage = "E-post är obligatoriskt!")]
        [EmailAddress(ErrorMessage = "E-postadressen är ogiltig!")]
        public string Email { get; set; }
    }
}
