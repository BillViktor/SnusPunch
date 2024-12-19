using SnusPunch.Shared.Models.Auth;
using SnusPunch.Shared.Models.Notification;
using SnusPunch.Shared.Models.Pagination;
using SnusPunch.Web.Clients.Notification;
using SnusPunch.Web.Clients.Snus;
using System.Collections.Generic;

namespace SnusPunch.Web.ViewModels.Snus
{
    public class UserViewModel : BaseViewModel
    {
        private readonly UserClient mUserClient;
        private readonly FriendClient mFriendClient;
        private readonly NotificationClient mNotificationClient;

        //Friend requests
        private List<FriendRequestDto> mFriendRequests = new List<FriendRequestDto>();
        public List<FriendRequestDto> FriendRequests { get { return mFriendRequests; } set { SetValue(ref mFriendRequests, value); } }

        //Notifications
        private List<NotificationDto> mNofitications = new List<NotificationDto>();
        private PaginationMetaData mNotificationPaginationMetaData = null;
        private PaginationParameters mNotificationPaginationParameters = new PaginationParameters
        {
            PageSize = 10,
        };
        public List<NotificationDto> Notifications { get { return mNofitications; } set { SetValue(ref mNofitications, value); } }
        public PaginationMetaData NotificationPaginationMetaData { get { return mNotificationPaginationMetaData; } set { SetValue(ref mNotificationPaginationMetaData, value); } }
        public PaginationParameters NotificationPaginationParameters { get { return mNotificationPaginationParameters; } set { SetValue(ref mNotificationPaginationParameters, value); } }

        public UserViewModel(UserClient aUserClient, FriendClient aFriendClient, NotificationClient aNotificationClient)
        {
            mUserClient = aUserClient;
            mFriendClient = aFriendClient;
            mNotificationClient = aNotificationClient;
        }

        public async Task<PaginationResponse<SnusPunchUserDto>> GetUsersPaginated(PaginationParameters aPaginationParameters)
        {
            IsBusy = true;
            PaginationResponse<SnusPunchUserDto> sPaginationResponse = new PaginationResponse<SnusPunchUserDto>();

            var sResult = await mUserClient.GetUsersPaginated(aPaginationParameters);

            if (!sResult.Success)
            {
                Errors.AddRange(sResult.Errors);
            }
            else
            {
                sPaginationResponse = sResult.ResultObject;
            }

            IsBusy = false;
            return sPaginationResponse;
        }

        public async Task<PaginationResponse<SnusPunchUserDto>> GetFriendsForUser(PaginationParameters aPaginationParameters, string aUserName)
        {
            IsBusy = true;
            PaginationResponse<SnusPunchUserDto> sPaginationResponse = new PaginationResponse<SnusPunchUserDto>();

            var sResult = await mFriendClient.GetFriendsForUser(aPaginationParameters, aUserName);

            if (!sResult.Success)
            {
                Errors.AddRange(sResult.Errors);
            }
            else
            {
                sPaginationResponse = sResult.ResultObject;
            }

            IsBusy = false;
            return sPaginationResponse;
        }

        public async Task<SnusPunchUserProfileDto> GetUserProfile(string aUserName)
        {
            IsBusy = true;
            SnusPunchUserProfileDto sSnusPunchUserProfileDto = null;

            var sResult = await mUserClient.GetUserProfile(aUserName);

            if (!sResult.Success)
            {
                Errors.AddRange(sResult.Errors);
            }
            else
            {
                sSnusPunchUserProfileDto = sResult.ResultObject;
            }

            IsBusy = false;

            return sSnusPunchUserProfileDto;
        }

        public async Task<bool> DeleteUser(SnusPunchUserDto aSnusPunchUserDto)
        {
            IsBusy = true;
            bool sSuccess = true;

            var sResult = await mUserClient.DeleteUser(aSnusPunchUserDto.UserName);

            if (!sResult.Success)
            {
                Errors.AddRange(sResult.Errors);
                sSuccess = false;
            }
            else
            {
                SuccessMessages.Add($"Raderade användaren: {aSnusPunchUserDto.UserName}!");
            }

            IsBusy = false;

            return sSuccess;
        }

        public async Task<bool> Delete()
        {
            IsBusy = true;
            bool sSuccess = true;

            var sResult = await mUserClient.Delete();

            if (!sResult.Success)
            {
                Errors.AddRange(sResult.Errors);
                sSuccess = false;
            }
            else
            {
                SuccessMessages.Add($"Ditt konto är nu borttaget.");
            }

            IsBusy = false;

            return sSuccess;
        }

        #region Friends
        public async Task<bool> SendFriendRequest(string aUserName)
        {
            IsBusy = true;
            bool sSuccess = true;

            var sResult = await mFriendClient.SendFriendRequest(aUserName);

            if (!sResult.Success)
            {
                Errors.AddRange(sResult.Errors);
                sSuccess = false;
            }
            else
            {
                SuccessMessages.Add($"Vänförfrågan skickad!");
            }

            IsBusy = false;

            return sSuccess;
        }

        public async Task<bool> RemoveFriendRequest(string aUserName)
        {
            IsBusy = true;
            bool sSuccess = true;

            var sResult = await mFriendClient.RemoveFriendRequest(aUserName);

            if (!sResult.Success)
            {
                Errors.AddRange(sResult.Errors);
                sSuccess = false;
            }
            else
            {
                SuccessMessages.Add($"Vänförfrågan borttagen!");
            }

            IsBusy = false;

            return sSuccess;
        }

        public async Task<bool> DenyFriendRequest(string aUserName)
        {
            IsBusy = true;
            bool sSuccess = true;

            var sResult = await mFriendClient.DenyFriendRequest(aUserName);

            if (!sResult.Success)
            {
                Errors.AddRange(sResult.Errors);
                sSuccess = false;
            }
            else
            {
                SuccessMessages.Add($"Vänförfrågan nekad!");
            }

            IsBusy = false;

            return sSuccess;
        }

        public async Task<bool> AcceptFriendRequest(string aUserName)
        {
            IsBusy = true;
            bool sSuccess = true;

            var sResult = await mFriendClient.AcceptFriendRequest(aUserName);

            if (!sResult.Success)
            {
                Errors.AddRange(sResult.Errors);
                sSuccess = false;
            }
            else
            {
                SuccessMessages.Add($"Vänförfrågan accepterad!");
            }

            IsBusy = false;

            return sSuccess;
        }

        public async Task<bool> RemoveFriend(string aUserName)
        {
            IsBusy = true;
            bool sSuccess = true;

            var sResult = await mFriendClient.RemoveFriend(aUserName);

            if (!sResult.Success)
            {
                Errors.AddRange(sResult.Errors);
                sSuccess = false;
            }
            else
            {
                SuccessMessages.Add($"Du är nu inte längre vän med {aUserName}!");
            }

            IsBusy = false;

            return sSuccess;
        }

        public async Task GetFriendRequests()
        {
            IsBusy = true;

            var sResult = await mFriendClient.GetAllFriendRequests();

            if (sResult.Success)
            {
                mFriendRequests = sResult.ResultObject;
            }
            else
            {
                AddError($"Kunde inte hämta vänförfrågningar!");
            }

            IsBusy = false;
        }
        #endregion


        #region Notifications
        public async Task GetNotificationsPaginated()
        {
            IsBusy = true;

            var sResult = await mNotificationClient.GetNotificationsPaginated(mNotificationPaginationParameters);

            if (!sResult.Success)
            {
                Errors.AddRange(sResult.Errors);
            }
            else
            {
                Notifications = sResult.ResultObject.Items;
                NotificationPaginationMetaData =  sResult.ResultObject.PaginationMetaData;
            }

            IsBusy = false;
        }

        public async Task FetchMoreNotifications()
        {
            NotificationPaginationParameters.PageNumber++;

            var sNotifications = await mNotificationClient.GetNotificationsPaginated(mNotificationPaginationParameters);

            if(!sNotifications.Success)
            {
                Errors.AddRange(sNotifications.Errors);
                return;
            }

            NotificationPaginationMetaData = sNotifications.ResultObject.PaginationMetaData;

            Notifications.AddRange(sNotifications.ResultObject.Items);
            Notifications = Notifications.Distinct().ToList();
        }

        public async Task<bool> SetAllNotificationsAsRead()
        {
            IsBusy = true;

            var sResult = await mNotificationClient.SetAllNotificationsAsRead();

            if (!sResult.Success)
            {
                Errors.AddRange(sResult.Errors);
                IsBusy = false;
                return false;
            }

            IsBusy = false;
            return true;
        }
        #endregion
    }
}
