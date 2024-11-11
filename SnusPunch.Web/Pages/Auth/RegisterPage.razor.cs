using Microsoft.AspNetCore.Components;
using SnusPunch.Shared.Models.Auth;
using SnusPunch.Web.ViewModels.Snus;

namespace SnusPunch.Web.Pages.Auth
{
    public partial class RegisterPage
    {
        [Inject] AuthViewModel AuthViewModel { get; set; }
        [Inject] NavigationManager NavigationManager { get; set; }

        private RegisterModel mRegisterModel = new RegisterModel();

        private async Task Register()
        {
            if(await AuthViewModel.Register(mRegisterModel))
            {
                AuthViewModel.SuccessMessages.Add("Navigerar till login-sidan...");
                await InvokeAsync(StateHasChanged);
                await Task.Delay(2000);
                NavigationManager.NavigateTo("login");
            }
        }
    }
}
