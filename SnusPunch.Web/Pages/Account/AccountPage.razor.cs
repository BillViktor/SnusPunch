using Blazored.Modal;
using Blazored.Modal.Services;
using Microsoft.AspNetCore.Components;
using SnusPunch.Shared.Models.Auth;
using SnusPunch.Web.Components;
using SnusPunch.Web.ViewModels.Snus;

namespace SnusPunch.Web.Pages.Account
{
    public partial class AccountPage
    {
        [CascadingParameter] public IModalService Modal { get; set; } = default!;
        [Inject] AuthViewModel AuthViewModel { get; set; }
        [Inject] UserViewModel UserViewModel { get; set; }
        [Inject] NavigationManager NavigationManager { get; set; }

        private ChangePasswordRequestModel mChangePasswordRequestModel = new ChangePasswordRequestModel();

        private async Task DeleteAccount()
        {
            var sOptions = new ModalOptions
            {
                DisableBackgroundCancel = true,
                Size = ModalSize.Custom,
                SizeCustomClass = "modal-large",
                Position = ModalPosition.Middle
            };
            var sParameters = new ModalParameters { { "Message", $"Är du helt säker på att du vill radera ditt konto? Detta går inte att ångra." } };
            var sModal = Modal.Show<ConfirmationComponent>("Bekräfta borttagning av konto", sParameters, sOptions);
            var sResult = await sModal.Result;

            if (!sResult.Cancelled)
            {
                if (await UserViewModel.Delete())
                {
                    NavigationManager.NavigateTo("login", true);
                }
            }
        }

        private async Task ChangePassword()
        {
            var sOptions = new ModalOptions
            {
                DisableBackgroundCancel = true,
                Size = ModalSize.Custom,
                SizeCustomClass = "modal-large",
                Position = ModalPosition.Middle
            };
            var sModal = Modal.Show<ChangePasswordComponent>("Byt Lösenord", sOptions);
            _ = await sModal.Result;
        }
    }
}
