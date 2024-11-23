using Microsoft.AspNetCore.Components;
using SnusPunch.Shared.Models.Auth.Password;
using SnusPunch.Web.ViewModels.Snus;

namespace SnusPunch.Web.Pages.Auth
{
    public partial class ResetPasswordPage
    {
        [Inject] AuthViewModel AuthViewModel { get; set; }
        [Inject] NavigationManager NavigationManager { get; set; }

        private ResetPasswordRequestModel mResetPasswordRequestModel = new ResetPasswordRequestModel();

        private async Task ResetPassword() 
        {
            //Hämta ut token från url
            mResetPasswordRequestModel.Token = NavigationManager.ToBaseRelativePath(NavigationManager.Uri).ToString().Replace("ResetPassword?Token=", "");

            if (await AuthViewModel.ResetPassword(mResetPasswordRequestModel))
            {
                await Task.Delay(1000);
                NavigationManager.NavigateTo("login");
            }
        }
    }
}
