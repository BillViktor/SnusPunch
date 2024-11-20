using Blazored.Toast.Configuration;
using Blazored.Toast.Services;
using Microsoft.AspNetCore.Components;
using SnusPunch.Shared.Models.Errors;

namespace SnusPunch.Web.Components
{
    public partial class MessageComponent
    {
        [Parameter] public bool IsBusy { get; set; }
        [Parameter] public List<ErrorModel> Errors { get; set; } = new List<ErrorModel>();
        [Parameter]public List<string> SuccessMessages { get; set; } = new List<string>();
        [Inject] IToastService ToastService { get; set; }

        ToastSettings ToastSettingsError = new ToastSettings("", IconType.None, "", false, true, null, 0, true, false, 0, ToastPosition.BottomRight);
        ToastSettings ToastSettingsSuccess = new ToastSettings("", IconType.None, "", true, true, null, 5, false, true, 0, ToastPosition.BottomRight);

        protected override void OnParametersSet()
        {
            ShowErrorMessages();
            ShowSuccessMessages();
        }

        private void ShowErrorMessages()
        {
            foreach(var sError in Errors)
            {
                Console.WriteLine(sError.GetDetailedErrorString());
                ToastService.ShowError(sError.GetDetailedErrorString(), sSettings => sSettings.DisableTimeout = true);
            }

            Errors.Clear();
        }

        private void ShowSuccessMessages()
        {
            foreach (var sMessage in SuccessMessages)
            {
                ToastService.ShowSuccess(sMessage, sSettings =>
                {
                    sSettings.PauseProgressOnHover = true;
                    sSettings.Timeout = 3;
                });
            }

            SuccessMessages.Clear();
        }
    }
}
