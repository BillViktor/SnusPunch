using SnusPunch.Shared.Models.Auth;
using SnusPunch.Shared.Models.Pagination;
using SnusPunch.Shared.Models.Snus;
using SnusPunch.Web.Clients.Snus;

namespace SnusPunch.Web.ViewModels.Snus
{
    public class UserViewModel : BaseViewModel
    {
        private readonly UserClient mUserClient;

        public UserViewModel(UserClient aUserClient)
        {
            mUserClient = aUserClient;
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
    }
}
