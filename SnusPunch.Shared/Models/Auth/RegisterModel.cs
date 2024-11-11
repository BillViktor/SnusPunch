using System.ComponentModel.DataAnnotations;

namespace SnusPunch.Shared.Models.Auth
{
    public class RegisterModel
    {
        [Required(ErrorMessage = "Användarnamn är obligatoriskt!")]
        [MinLength(5)]
        public string UserName { get; set; }

        [Required(ErrorMessage = "E-post är obligatoriskt!")]
        [EmailAddress(ErrorMessage = "E-postadressen är ogiltig!")]
        public string Email { get; set; }

        [Required(ErrorMessage = "Lösenord är obligatoriskt!")]
        [RegularExpression(@"^(?=.*[a-z])(?=.*[A-Z])(?=.*\d)(?=.*[#$^+=!*()@%&]).{8,}$", ErrorMessage = "Lösenordet måste innehålla minst: en stor bokstav, en liten bokstav, en siffra, ett specialtecken samt vara 8 tecken långt!")]
        public string Password { get; set; }

        [Required(ErrorMessage = "Du måste bekräfta ditt lösenord!")]
        [Compare("Password", ErrorMessage = "Lösenorden matchar inte!")]
        public string ConfirmPassword { get; set; }
    }
}
