using Microsoft.AspNetCore.Components;
using SnusPunch.Shared.Models.Auth;
using SnusPunch.Web.ViewModels.Snus;

namespace SnusPunch.Web.Pages.Auth
{
    public partial class LoginPage
    {
        [Inject] AuthViewModel AuthViewModel { get; set; }
        [Inject] NavigationManager NavigationManager { get; set; }

        private LoginModel mLoginModel = new LoginModel();

        private async Task Login() 
        {
            await Task.Delay(0);
        }
    }
}
