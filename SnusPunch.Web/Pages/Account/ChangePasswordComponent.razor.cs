using Blazored.Modal;
using Microsoft.AspNetCore.Components;
using SnusPunch.Shared.Models.Auth;
using SnusPunch.Web.ViewModels.Snus;

namespace SnusPunch.Web.Pages.Account
{
    public partial class ChangePasswordComponent
    {
        [CascadingParameter] BlazoredModalInstance BlazoredModal { get; set; } = default!;
        [Inject] AuthViewModel AuthViewModel { get; set; }

        private ChangePasswordRequestModel mChangePasswordRequestModel = new ChangePasswordRequestModel();

        private async Task Confirm()
        {
            if(await AuthViewModel.ChangePassword(mChangePasswordRequestModel))
            {
                await BlazoredModal.CloseAsync();
            }
        }

        private async Task Close() => await BlazoredModal.CancelAsync();
    }
}
