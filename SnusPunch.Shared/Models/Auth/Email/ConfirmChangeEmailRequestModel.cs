using System.ComponentModel.DataAnnotations;

namespace SnusPunch.Shared.Models.Auth.Email
{
    public class ConfirmChangeEmailRequestModel
    {
        [Required(ErrorMessage = "E-post är obligatoriskt!")]
        [EmailAddress(ErrorMessage = "E-postadressen är ogiltig!")]
        public string NewEmail { get; set; }

        public string Token { get; set; } 
    }
}
