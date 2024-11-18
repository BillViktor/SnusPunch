using SnusPunch.Shared.Models.Auth;
using SnusPunch.Shared.Models.Pagination;
using SnusPunch.Shared.Models.Snus;
using SnusPunch.Web.Clients.Snus;

namespace SnusPunch.Web.ViewModels.Snus
{
    public class UserViewModel : BaseViewModel
    {
        private readonly UserClient mUserClient;

        #region Fields
        private List<SnusPunchUserDto> mUsers = new List<SnusPunchUserDto>();
        #endregion


        #region Properties
        public List<SnusPunchUserDto> Snus { get { return mUsers; } set { SetValue(ref mUsers, value); } }
        #endregion

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
    }
}
