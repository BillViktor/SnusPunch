﻿using Microsoft.AspNetCore.Components;
using SnusPunch.Shared.Models.Auth;
using SnusPunch.Web.ViewModels.Snus;

namespace SnusPunch.Web.Pages.Auth
{
    public partial class ResetPasswordPage
    {
        [Parameter] public string Token { get; set; }
        [Inject] AuthViewModel AuthViewModel { get; set; }
        [Inject] NavigationManager NavigationManager { get; set; }

        private ResetPasswordRequestModel mResetPasswordRequestModel = new ResetPasswordRequestModel();

        private async Task ResetPassword() 
        {
            //Hämta ut token från url
            mResetPasswordRequestModel.Token = NavigationManager.ToBaseRelativePath(NavigationManager.Uri).ToString().Replace("resetpassword?Token=", "");

            if (await AuthViewModel.ResetPassword(mResetPasswordRequestModel))
            {
                await Task.Delay(1000);
                NavigationManager.NavigateTo("login");
            }
        }
    }
}