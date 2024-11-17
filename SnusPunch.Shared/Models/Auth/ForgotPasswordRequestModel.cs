using System.ComponentModel.DataAnnotations;

namespace SnusPunch.Shared.Models.Auth
{
    public class ForgotPasswordRequestModel
    {
        [Required(ErrorMessage = "E-post är obligatoriskt!")]
        [EmailAddress(ErrorMessage = "E-postadressen är ogiltig!")]
        public string Email { get; set; }
    }
}
