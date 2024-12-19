using Blazored.Modal;
using Blazored.Modal.Services;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Forms;
using SnusPunch.Shared.Models.Auth.Password;
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

        private IBrowserFile mBrowserFile = null;
        private Guid mInputFileId = Guid.NewGuid();
        private ChangePasswordRequestModel mChangePasswordRequestModel = new ChangePasswordRequestModel();

        //Visa alltid färsk information :)
        protected override async Task OnInitializedAsync()
        {
            await AuthViewModel.GetUserInfo();
        }

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

        private async Task PrivacySettings()
        {
            var sOptions = new ModalOptions
            {
                DisableBackgroundCancel = true,
                Size = ModalSize.Medium,
                Position = ModalPosition.Middle
            };
            var sModal = Modal.Show<UpdatePrivacySettingsComponent>("Uppdatera sekretess", sOptions);
            _ = await sModal.Result;
        }

        private async Task ChangeEmail()
        {
            var sOptions = new ModalOptions
            {
                DisableBackgroundCancel = true,
                Size = ModalSize.Custom,
                SizeCustomClass = "modal-large",
                Position = ModalPosition.Middle
            };
            var sModal = Modal.Show<ChangeEmailComponent>("Byt E-postadress", sOptions);
            _ = await sModal.Result;
        }

        private async Task ResendConfirmationEmail()
        {
            await AuthViewModel.ResendConfirmationEmail();
        }

        #region Profile Picture
        private async Task DeleteProfilePicture()
        {
            var sOptions = new ModalOptions
            {
                DisableBackgroundCancel = true,
                Size = ModalSize.Custom,
                SizeCustomClass = "modal-large",
                Position = ModalPosition.Middle
            };
            var sParameters = new ModalParameters { { "Message", $"Är du säker på att du vill radera din profilbild? Detta går inte att ångra!" } };
            var sModal = Modal.Show<ConfirmationComponent>("Bekräfta borttagning", sParameters, sOptions);
            var sResult = await sModal.Result;

            if (!sResult.Cancelled)
            {
                if (await AuthViewModel.DeleteProfilePicture())
                {
                    await AuthViewModel.GetUserInfo();
                }
            }
        }

        private async Task AddOrUpdateProfilePicture()
        {
            if(await AuthViewModel.AddOrUpdateProfilePicture(mBrowserFile))
            {
                mBrowserFile = null;
                mInputFileId = Guid.NewGuid();
                await InvokeAsync(StateHasChanged);
                await AuthViewModel.GetUserInfo();
            }
        }

        private void LoadImage(InputFileChangeEventArgs aInputFileChangeEventArgs)
        {
            mBrowserFile = aInputFileChangeEventArgs.File;
        }
        #endregion
    }
}
