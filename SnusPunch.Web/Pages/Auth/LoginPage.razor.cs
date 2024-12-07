using Microsoft.AspNetCore.Components;
using SnusPunch.Shared.Models.Auth;
using SnusPunch.Web.Identity;
using SnusPunch.Web.ViewModels.Snus;

namespace SnusPunch.Web.Pages.Auth
{
    public partial class LoginPage
    {
        [Inject] AuthViewModel AuthViewModel { get; set; }
        [Inject] NavigationManager NavigationManager { get; set; }

        private LoginRequestModel mLoginModel = new LoginRequestModel();

        private async Task Login() 
        {
            if(await AuthViewModel.Login(mLoginModel))
            {
                NavigationManager.NavigateTo("");
            }
        }
    }
}
