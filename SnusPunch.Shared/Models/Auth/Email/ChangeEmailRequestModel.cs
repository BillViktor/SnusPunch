using System.ComponentModel.DataAnnotations;

namespace SnusPunch.Shared.Models.Auth.Email
{
    public class ChangeEmailRequestModel
    {
        [Required(ErrorMessage = "E-post är obligatoriskt!")]
        [EmailAddress(ErrorMessage = "E-postadressen är ogiltig!")]
        public string NewEmail { get; set; }
    }
}
