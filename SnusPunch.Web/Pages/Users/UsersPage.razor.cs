using Blazored.Modal;
using Blazored.Modal.Services;
using Microsoft.AspNetCore.Components;
using SnusPunch.Shared.Models.Auth;
using SnusPunch.Shared.Models.Pagination;
using SnusPunch.Web.Components;
using SnusPunch.Web.ViewModels.Snus;

namespace SnusPunch.Web.Pages.Users
{
    public partial class UsersPage
    {
        [CascadingParameter] public IModalService Modal { get; set; } = default!;
        [Inject] UserViewModel UserViewModel { get; set; }

        private PaginationMetaData mPaginationMetaData = null;
        private List<SnusPunchUserDto> mUserList = new List<SnusPunchUserDto>();
        private PaginationParameters mPaginationParameters = new PaginationParameters
        {
            SearchPropertyNames = new List<string> { "UserName", "FavoriteSnus.Name" },
            SortPropertyName = "UserName"
        };
        private Dictionary<string, string> mSortProperties = new Dictionary<string, string>
        {
            { "UserName", "Namn" },
            { "FavoriteSnus.Name", "Favoritsnus" }
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

        #region Actions
        private async Task DeleteUser(SnusPunchUserDto aSnusPunchUserDto)
        {
            var sOptions = new ModalOptions
            {
                DisableBackgroundCancel = true,
                Size = ModalSize.Custom,
                SizeCustomClass = "modal-large",
                Position = ModalPosition.Middle
            };
            var sParameters = new ModalParameters { { "Message", $"Är du säker på att du vill radera användaren \"{aSnusPunchUserDto.UserName}\"? Detta går inte att ångra!" } };
            var sModal = Modal.Show<ConfirmationComponent>("Bekräfta borttagning av användare", sParameters, sOptions);
            var sResult = await sModal.Result;

            if (!sResult.Cancelled)
            {
                if (await UserViewModel.DeleteUser(aSnusPunchUserDto))
                {
                    await GetUsers();
                }
            }
        }
        #endregion
    }
}
