using System.ComponentModel.DataAnnotations;

namespace SnusPunch.Shared.Models.Auth.Password
{
    public class ChangePasswordRequestModel
    {
        [Required(ErrorMessage = "Du måste bekräfta ditt nuvarande lösenord!")]
        public string CurrentPassword { get; set; }

        [Required(ErrorMessage = "Ett nytt lösenord är obligatoriskt!")]
        [RegularExpression(@"^(?=.*[a-z])(?=.*[A-Z])(?=.*\d)(?=.*[#$^+=!*()@%&]).{8,}$", ErrorMessage = "Lösenordet måste innehålla minst: en stor bokstav, en liten bokstav, en siffra, ett specialtecken samt vara 8 tecken långt!")]
        public string NewPassword { get; set; }

        [Required(ErrorMessage = "Du måste bekräfta ditt nya lösenord!")]
        [Compare("NewPassword", ErrorMessage = "Lösenorden matchar inte!")]
        public string ConfirmNewPassword { get; set; }
    }
}
