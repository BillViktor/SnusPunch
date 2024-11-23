using Microsoft.AspNetCore.Components;
using SnusPunch.Shared.Models.Auth.Email;
using SnusPunch.Shared.Models.Auth.Password;
using SnusPunch.Web.ViewModels.Snus;

namespace SnusPunch.Web.Pages.Auth
{
    public partial class ChangeEmailPage
    {
        [Inject] AuthViewModel AuthViewModel { get; set; }
        [Inject] NavigationManager NavigationManager { get; set; }

        private ConfirmChangeEmailRequestModel mConfirmChangeEmailRequestModel = new ConfirmChangeEmailRequestModel();

        private async Task ConfirmChangeEmail() 
        {
            //Hämta ut token från url
            mConfirmChangeEmailRequestModel.Token = NavigationManager.ToBaseRelativePath(NavigationManager.Uri).ToString().Replace("ChangeEmail?Token=", "");

            if (await AuthViewModel.ConfirmChangeEmail(mConfirmChangeEmailRequestModel))
            {
                await Task.Delay(1000);
                NavigationManager.NavigateTo("login");
            }
        }
    }
}
