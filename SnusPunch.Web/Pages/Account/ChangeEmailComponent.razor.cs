using Blazored.Modal;
using Microsoft.AspNetCore.Components;
using SnusPunch.Shared.Models.Auth.Email;
using SnusPunch.Web.ViewModels.Snus;

namespace SnusPunch.Web.Pages.Account
{
    public partial class ChangeEmailComponent
    {
        [CascadingParameter] BlazoredModalInstance BlazoredModal { get; set; } = default!;
        [Inject] AuthViewModel AuthViewModel { get; set; }

        private ChangeEmailRequestModel mChangeEmailRequestModel = new ChangeEmailRequestModel();

        private async Task Confirm()
        {
            if(mChangeEmailRequestModel.NewEmail == AuthViewModel.UserInfoModel.Email)
            {
                AuthViewModel.AddError("Din nya e-post kan inte vara likadan som din nuvarande!");
                return;
            }

            if(await AuthViewModel.ChangeEmail(mChangeEmailRequestModel))
            {
                await BlazoredModal.CloseAsync();
            }
        }

        private async Task Close() => await BlazoredModal.CancelAsync();
    }
}
