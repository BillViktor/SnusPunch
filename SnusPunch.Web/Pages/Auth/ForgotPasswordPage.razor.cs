using Microsoft.AspNetCore.Components;
using SnusPunch.Shared.Models.Auth.Password;
using SnusPunch.Web.Identity;
using SnusPunch.Web.ViewModels.Snus;

namespace SnusPunch.Web.Pages.Auth
{
    public partial class ForgotPasswordPage
    {
        [Inject] AuthViewModel AuthViewModel { get; set; }
        [Inject] NavigationManager NavigationManager { get; set; }

        private ForgotPasswordRequestModel mForgotPasswordRequestModel = new ForgotPasswordRequestModel();

        private async Task ForgotPassword() 
        {
            if(await AuthViewModel.ForgotPassword(mForgotPasswordRequestModel))
            {
                mForgotPasswordRequestModel.Email = "";
            }
        }
    }
}
