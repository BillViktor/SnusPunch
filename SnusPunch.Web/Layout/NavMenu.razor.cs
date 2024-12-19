using Blazored.Modal;
using Blazored.Modal.Services;
using Blazored.Toast.Services;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.SignalR.Client;
using Microsoft.JSInterop;
using SnusPunch.Shared.Models.Auth;
using SnusPunch.Shared.Models.Notification;
using SnusPunch.Web.Components.Entry;
using SnusPunch.Web.Identity;
using SnusPunch.Web.ViewModels.Snus;

namespace SnusPunch.Web.Layout
{
    public partial class NavMenu : IAsyncDisposable
    {
        [CascadingParameter] public IModalService Modal { get; set; } = default!;
        [Inject] EntryViewModel EntryViewModel { get; set; }
        [Inject] IToastService ToastService { get; set; }
        [Inject] IConfiguration Configuration { get; set; }
        [Inject] IJSRuntime JSRuntime { get; set; }
        [Inject] AuthViewModel AuthViewModel { get; set; }
        [Inject] UserViewModel UserViewModel { get; set; }

        private HubConnection? mHubConnection;

        protected override async Task OnInitializedAsync()
        {
            try
            {
                mHubConnection = new HubConnectionBuilder().WithUrl(Configuration["BaseUrl"] + "Notifications", opt =>
                {
                    opt.HttpMessageHandlerFactory = _ => new CookieHandler
                    {
                        InnerHandler = new HttpClientHandler()
                    };
                }).Build();

                await mHubConnection.StartAsync();

                mHubConnection.On<string, string>("ReceiveMessage", MessageReceived);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Kunde inte ansluta till notifikationscentret: {ex.Message}");
            }
        }

        private async Task MessageReceived(string aNotificationType, string aMessage)
        {
            if (Enum.TryParse<NotificationTypeEnum>(aNotificationType, true, out NotificationTypeEnum sNotificationTypeEnum))
            {
                switch (sNotificationTypeEnum)
                {
                    case NotificationTypeEnum.EntryEvent:
                    case NotificationTypeEnum.FriendRequest:
                        await OnNotificationReceived(sNotificationTypeEnum);
                        break;
                    case NotificationTypeEnum.FriendRequestRemoved:
                        OnFriendRequestRemoved();
                        break;
                    default:
                        break;
                }

                ToastService.ShowInfo(aMessage);
                StateHasChanged();
            }
        }

        #region Events
        private async Task OnNotificationReceived(NotificationTypeEnum aNotificationTypeEnum)
        {
            await JSRuntime.InvokeVoidAsync("playNotificationSound");

            if(aNotificationTypeEnum == NotificationTypeEnum.FriendRequest)
            {
                AuthViewModel.UserInfoModel.FriendRequests++;
            }
            else if(aNotificationTypeEnum == NotificationTypeEnum.EntryEvent)
            {
                AuthViewModel.UserInfoModel.UnreadNotifications++;
            }
        }

        private void OnFriendRequestRemoved()
        {
            if (AuthViewModel.UserInfoModel.FriendRequests > 0)
            {
                AuthViewModel.UserInfoModel.FriendRequests--;
            }
        }
        #endregion


        #region Friend Requests
        private async Task ShowFriendRequests()
        {
            if (AuthViewModel.UserInfoModel?.FriendRequests > 0)
            {
                await UserViewModel.GetFriendRequests();
            }

            await JSRuntime.InvokeVoidAsync("toggleFriendRequestsDropDown");
        }

        private async Task AcceptFriendRequest(FriendRequestDto aFriendRequestDto)
        {
            if (await UserViewModel.AcceptFriendRequest(aFriendRequestDto.UserName))
            {
                UserViewModel.FriendRequests.Remove(aFriendRequestDto);
                AuthViewModel.UserInfoModel.FriendRequests--;
            }
        }

        private async Task DenyFriendRequest(FriendRequestDto aFriendRequestDto)
        {
            if (await UserViewModel.DenyFriendRequest(aFriendRequestDto.UserName))
            {
                UserViewModel.FriendRequests.Remove(aFriendRequestDto);
                AuthViewModel.UserInfoModel.FriendRequests--;
            }
        }
        #endregion


        #region Notifications
        private async Task ShowNotifications()
        {
            await UserViewModel.GetNotificationsPaginated();

            if (await UserViewModel.SetAllNotificationsAsRead())
            {
                AuthViewModel.UserInfoModel.UnreadNotifications = 0;
            }

            await JSRuntime.InvokeVoidAsync("toggleNotificationDropDown");
        }

        private async Task OnNotificationClick(NotificationDto aNotification)
        {
            await JSRuntime.InvokeVoidAsync("toggleNotificationDropDown");

            if(aNotification.NotificationType == NotificationActionEnum.EntryLike)
            {
                //Hämta mer data om inlägget
                var sEntry = await EntryViewModel.GetEntryById(aNotification.EntityId);

                if (sEntry == null) return;

                var sOptions = new ModalOptions
                {
                    DisableBackgroundCancel = true,
                    Size = ModalSize.Custom,
                    SizeCustomClass = "modal-website-width",
                    Position = ModalPosition.Middle
                };

                var sParametes = new ModalParameters { { "EntryDto", sEntry } };

                Modal.Show<ShowEntryComponent>($"{sEntry.UserName}'s inlägg", sParametes, sOptions);
            }
            else if(aNotification.NotificationType== NotificationActionEnum.CommentLike)
            {
                ToastService.ShowError("Go to comment not implemented yet!");
            }
            else if(aNotification.NotificationType == NotificationActionEnum.Comment)
            {
                ToastService.ShowError("Go to comment not implemented yet!");
            }
        }
        #endregion

        public async ValueTask DisposeAsync()
        {
            await mHubConnection.StopAsync();
            await mHubConnection.DisposeAsync();
        }
    }
}
