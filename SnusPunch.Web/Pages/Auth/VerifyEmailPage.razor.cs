using Microsoft.AspNetCore.Components;
using SnusPunch.Shared.Models.Auth;
using SnusPunch.Web.ViewModels.Snus;

namespace SnusPunch.Web.Pages.Auth
{
    public partial class VerifyEmailPage
    {
        [Parameter] public string Token { get; set; }
        [Inject] AuthViewModel AuthViewModel { get; set; }
        [Inject] NavigationManager NavigationManager { get; set; }

        private VerifyEmailRequest mVerifyEmailRequest = new VerifyEmailRequest();

        private async Task VerifyEmail() 
        {
            //Hämta ut token från url
            var sToken = 

            mVerifyEmailRequest.Token = NavigationManager.ToBaseRelativePath(NavigationManager.Uri).ToString().Replace("Verify?Token=", "");

            if(await AuthViewModel.VerifyEmail(mVerifyEmailRequest))
            {
                await Task.Delay(1000);
                NavigationManager.NavigateTo("");
            }
        }
    }
}
