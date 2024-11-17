using System.ComponentModel.DataAnnotations;

namespace SnusPunch.Shared.Models.Auth
{
    public class LoginRequestModel
    {
        [Required(ErrorMessage = "Du måste fylla i användarnamn!")]
        public string UserName { get; set; }

        [Required(ErrorMessage = "Du måste fylla i lösenord!")]
        public string Password { get; set; }

        public bool RememberMe { get; set; }
    }
}
