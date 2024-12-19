using Blazored.Modal;
using Blazored.Modal.Services;
using Microsoft.AspNetCore.Components;
using SnusPunch.Shared.Models.Auth;
using SnusPunch.Shared.Models.Entry;
using SnusPunch.Shared.Models.Pagination;
using SnusPunch.Web.Components;
using SnusPunch.Web.Components.Entry;
using SnusPunch.Web.Components.Friends;
using SnusPunch.Web.Components.Photos;
using SnusPunch.Web.ViewModels.Snus;

namespace SnusPunch.Web.Pages.Profile
{
    public partial class ProfilePage
    {
        [Parameter] public string UserName { get; set; }
        [CascadingParameter] public IModalService Modal { get; set; } = default!;
        [Inject] AuthViewModel AuthViewModel { get; set; }
        [Inject] UserViewModel UserViewModel { get; set; }
        [Inject] EntryViewModel EntryViewModel { get; set; }
        [Inject] NavigationManager NavigationManager { get; set; }

        private SnusPunchUserProfileDto mSnusPunchUserProfileDto;

        private bool mFetchEmptyPunches = true;
        private PaginationMetaData mPaginationMetaData = null;
        private List<EntryDto> mEntryList = new List<EntryDto>();
        private PaginationParameters mPaginationParameters = new PaginationParameters
        {
            SearchPropertyNames = new List<string> { "SnusPunchUserModel.UserName" },
            SortPropertyName = "CreateDate",
            SortOrder = SortOrderEnum.Descending,
        };

        protected override async Task OnParametersSetAsync()
        {
            mSnusPunchUserProfileDto = await UserViewModel.GetUserProfile(UserName);
            await InvokeAsync(StateHasChanged);
            
            if(mSnusPunchUserProfileDto == null)
            {
                NavigationManager.NavigateTo("NotFound");
            }

            mPaginationParameters.SearchString = UserName;
            await GetEntries();
        }

        private async Task ToggleEmpty()
        {
            mEntryList.Clear();
            mPaginationParameters.PageNumber = 1;
            await GetEntries();
        }

        private async Task GetEntries()
        {
            if(mSnusPunchUserProfileDto.IsAllowedToSeeEntries(AuthViewModel.UserInfoModel.UserName))
            {
                var sResult = await EntryViewModel.GetEntriesPaginated(mPaginationParameters, mFetchEmptyPunches, EntryFilterEnum.All);
                mPaginationMetaData = sResult.PaginationMetaData;
                mEntryList = sResult.Items;
            }
        }

        private async Task FetchMoreEntries()
        {
            mPaginationParameters.PageNumber++;

            var sEntries = await EntryViewModel.GetEntriesPaginated(mPaginationParameters, mFetchEmptyPunches, EntryFilterEnum.All);
            mPaginationMetaData = sEntries.PaginationMetaData;

            mEntryList.AddRange(sEntries.Items);
            mEntryList = mEntryList.DistinctBy(x => x.Id).ToList();

            StateHasChanged();
        }

        #region Actions
        private void SendMessage()
        {
            UserViewModel.AddError("Inte implementerat än! :(");
        }

        private void ShowAllFriends()
        {
            if (mSnusPunchUserProfileDto.FriendsCount == 0)
            {
                EntryViewModel.AddError($"{mSnusPunchUserProfileDto.UserName} har inga vänner :(");
                return;
            }

            var sOptions = new ModalOptions
            {
                DisableBackgroundCancel = true,
                Size = ModalSize.Custom,
                SizeCustomClass = "modal-website-width",
                Position = ModalPosition.Middle
            };

            var sParametes = new ModalParameters { { "UserName", mSnusPunchUserProfileDto.UserName } };

            Modal.Show<ShowFriendsComponent>($"{mSnusPunchUserProfileDto.UserName}'s vänner", sParametes, sOptions);
        }

        private void ShowAllPhotos()
        {
            if(mSnusPunchUserProfileDto.PhotoEntries.Count == 0)
            {
                EntryViewModel.AddError($"{mSnusPunchUserProfileDto.UserName} har inte lagt upp några foton :(");
                return;
            }

            var sOptions = new ModalOptions
            {
                DisableBackgroundCancel = true,
                Size = ModalSize.Custom,
                SizeCustomClass = "modal-website-width",
                Position = ModalPosition.Middle
            };

            var sParametes = new ModalParameters { { "UserName", mSnusPunchUserProfileDto.UserName } };

            Modal.Show<ShowPhotosComponent>($"{mSnusPunchUserProfileDto.UserName}'s fotoinlägg", sParametes, sOptions);
        }

        private async Task ShowEntry(EntryDto aEntryDto)
        {
            //Hämta mer data om inlägget
            var sEntry = await EntryViewModel.GetEntryById(aEntryDto.Id);

            if (sEntry == null) return;

            await InvokeAsync(StateHasChanged);

            var sOptions = new ModalOptions
            {
                DisableBackgroundCancel = true,
                Size = ModalSize.Custom,
                SizeCustomClass = "modal-website-width",
                Position = ModalPosition.Middle
            };

            var sParametes = new ModalParameters { { "EntryDto", sEntry } };

            var sModal = Modal.Show<ShowEntryComponent>($"{sEntry.UserName}'s inlägg", sParametes, sOptions);

            var sResult = await sModal.Result;

            //Det raderades, ta bort det
            if(!sResult.Cancelled)
            {
                mSnusPunchUserProfileDto.PhotoEntries.Remove(aEntryDto);
            }
        }

        private void ShowProfilePicture(string aUserName, string aProfilePicturePath)
        {
            var sParameters = new ModalParameters { { "ProfilePictureUrl", aProfilePicturePath } };
            var sOptions = new ModalOptions { Size = ModalSize.Automatic, Position = ModalPosition.Middle };

            Modal.Show<ProfilePictureComponent>($"{aUserName}'s Profilbild", sParameters, sOptions);
        }
        #endregion


        #region Friendship Actions
        private async Task AddFriend()
        {
            if(await UserViewModel.SendFriendRequest(mSnusPunchUserProfileDto.UserName))
            {
                mSnusPunchUserProfileDto.FriendshipStatusEnum = FriendshipStatusEnum.Pending;
            }
        }

        private async Task RemoveFriend()
        {
            //Bekräfta
            var sOptions = new ModalOptions
            {
                DisableBackgroundCancel = true,
                Size = ModalSize.Custom,
                SizeCustomClass = "modal-large",
                Position = ModalPosition.Middle
            };
            var sParameters = new ModalParameters { { "Message", $"Är du säker på att du vill ta bort {UserName} som vän?" } };
            var sModal = Modal.Show<ConfirmationComponent>("Bekräfta borttagning", sParameters, sOptions);
            var sResult = await sModal.Result;

            if (sResult.Cancelled) return;

            if (await UserViewModel.RemoveFriend(mSnusPunchUserProfileDto.UserName))
            {
                mSnusPunchUserProfileDto.FriendshipStatusEnum = FriendshipStatusEnum.None;
                mSnusPunchUserProfileDto.FriendsCount--;
                mSnusPunchUserProfileDto.Friends.RemoveAll(x => x.UserName == AuthViewModel.UserInfoModel.UserName);
            }
        }

        private async Task RemoveFriendRequest()
        {
            if (await UserViewModel.RemoveFriendRequest(mSnusPunchUserProfileDto.UserName))
            {
                mSnusPunchUserProfileDto.FriendshipStatusEnum = FriendshipStatusEnum.None;
            }
        }

        private async Task AcceptFriendRequest()
        {
            if (await UserViewModel.AcceptFriendRequest(mSnusPunchUserProfileDto.UserName))
            {
                mSnusPunchUserProfileDto.FriendshipStatusEnum = FriendshipStatusEnum.Friends;
                mSnusPunchUserProfileDto.FriendsCount++;
                mSnusPunchUserProfileDto.Friends.Insert(0, new SnusPunchUserDto { UserName = AuthViewModel.UserInfoModel.UserName, ProfilePictureUrl = AuthViewModel.UserInfoModel.ProfilePictureUrl });
            }
        }

        private async Task DenyFriendRequest()
        {
            if (await UserViewModel.DenyFriendRequest(mSnusPunchUserProfileDto.UserName))
            {
                mSnusPunchUserProfileDto.FriendshipStatusEnum = FriendshipStatusEnum.None;
            }
        }
        #endregion
    }
}
