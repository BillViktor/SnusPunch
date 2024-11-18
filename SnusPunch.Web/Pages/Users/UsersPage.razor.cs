using Microsoft.AspNetCore.Components;
using SnusPunch.Shared.Models.Auth;
using SnusPunch.Shared.Models.Pagination;
using SnusPunch.Shared.Models.Snus;
using SnusPunch.Web.ViewModels.Snus;

namespace SnusPunch.Web.Pages.Users
{
    public partial class UsersPage
    {
        [Inject] UserViewModel UserViewModel { get; set; }

        private PaginationMetaData mPaginationMetaData = null;
        private List<SnusPunchUserDto> mUserList = new List<SnusPunchUserDto>();
        private PaginationParameters mPaginationParameters = new PaginationParameters
        {
            SearchPropertyName = "UserName",
            SortPropertyName = "UserName"
        };
        private Dictionary<string, string> mSortProperties = new Dictionary<string, string>
        {
            { "UserName", "Namn" }
        };

        protected override async Task OnInitializedAsync()
        {
            await GetUsers();
        }

        private async Task GetUsers()
        {
            var sResult = await UserViewModel.GetUsersPaginated(mPaginationParameters);
            mPaginationMetaData = sResult.PaginationMetaData;
            mUserList = sResult.Items;
        }
    }
}
