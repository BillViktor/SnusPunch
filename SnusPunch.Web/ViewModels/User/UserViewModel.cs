using SnusPunch.Shared.Models.Auth;
using SnusPunch.Shared.Models.Pagination;
using SnusPunch.Shared.Models.Snus;
using SnusPunch.Web.Clients.Snus;

namespace SnusPunch.Web.ViewModels.Snus
{
    public class UserViewModel : BaseViewModel
    {
        private readonly UserClient mUserClient;
        private readonly FriendClient mFriendClient;
        public UserViewModel(UserClient aUserClient, FriendClient aFriendClient)
        {
            mUserClient = aUserClient;
            mFriendClient = aFriendClient;
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
        #endregion
    }
}
