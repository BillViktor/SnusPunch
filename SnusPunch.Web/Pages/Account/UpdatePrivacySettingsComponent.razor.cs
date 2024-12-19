using Blazored.Modal;
using Microsoft.AspNetCore.Components;
using SnusPunch.Shared.Models.Auth;
using SnusPunch.Shared.Models.Auth.Password;
using SnusPunch.Web.ViewModels.Snus;

namespace SnusPunch.Web.Pages.Account
{
    public partial class UpdatePrivacySettingsComponent
    {
        [CascadingParameter] BlazoredModalInstance BlazoredModal { get; set; } = default!;
        [Inject] AuthViewModel AuthViewModel { get; set; }

        private UpdatePrivacySettingsRequestModel mUpdatePrivacySettingsRequestModel = new UpdatePrivacySettingsRequestModel();

        protected override void OnInitialized()
        {
            mUpdatePrivacySettingsRequestModel.FriendPrivacySetting = AuthViewModel.UserInfoModel.FriendPrivacySettingEnum;
            mUpdatePrivacySettingsRequestModel.EntryPrivacySetting = AuthViewModel.UserInfoModel.EntryPrivacySettingEnum;
        }

        private async Task Confirm()
        {
            if(await AuthViewModel.UpdatePrivacySettings(mUpdatePrivacySettingsRequestModel))
            {
                await BlazoredModal.CloseAsync();
            }
        }

        private async Task Close() => await BlazoredModal.CancelAsync();
    }
}
