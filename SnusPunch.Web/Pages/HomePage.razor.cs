using Blazored.Modal;
using Blazored.Modal.Services;
using Microsoft.AspNetCore.Components;
using SnusPunch.Shared.Models.Entry;
using SnusPunch.Shared.Models.Pagination;
using SnusPunch.Shared.Models.Snus;
using SnusPunch.Web.Components;
using SnusPunch.Web.Components.Entry;
using SnusPunch.Web.Components.Snus;
using SnusPunch.Web.ViewModels.Snus;

namespace SnusPunch.Web.Pages
{
    public partial class HomePage
    {
        [CascadingParameter] public IModalService Modal { get; set; } = default!;
        [Inject] AuthViewModel AuthViewModel { get; set; }
        [Inject] EntryViewModel EntryViewModel { get; set; }

        private string mDescription = "";
        private SnusDto? mChosenSnus = null;
        private PaginationMetaData mPaginationMetaData = null;
        private List<EntryDto> mEntryList = new List<EntryDto>();
        private PaginationParameters mPaginationParameters = new PaginationParameters
        {
            SearchPropertyNames = new List<string> { "SnusPunchUserModel.UserName", "Snus.Name", "Description" },
            SortPropertyName = "CreateDate",
            SortOrder = SortOrderEnum.Descending
        };
        private Dictionary<string, string> mSortProperties = new Dictionary<string, string>
        {
            { "CreateDate", "Datum" },
            { "SnusPunchUserModel.UserName", "Användarnamn" },
        };

        protected override async Task OnInitializedAsync()
        {
            await GetEntries();

            GetFavouriteSnus();
        }

        private async Task GetEntries()
        {
            var sResult = await EntryViewModel.GetEntriesPaginated(mPaginationParameters);
            mPaginationMetaData = sResult.PaginationMetaData;
            mEntryList = sResult.Items;
        }

        private void GetFavouriteSnus()
        {
            if (AuthViewModel.UserInfoModel?.FavouriteSnusId != null && !string.IsNullOrEmpty(AuthViewModel.UserInfoModel?.FavouriteSnusName))
            {
                mChosenSnus = new SnusDto
                {
                    Id = (int)AuthViewModel.UserInfoModel.FavouriteSnusId,
                    Name = AuthViewModel.UserInfoModel.FavouriteSnusName,
                };
            }
        }

        #region Actions
        public async Task AddEntry()
        {
            if(mChosenSnus?.Id == null)
            {
                EntryViewModel.AddError("Du har inte valt något snus!");
                return;
            }

            var sResult = await EntryViewModel.AddEntry(mChosenSnus.Id, mDescription);

            if(sResult != null)
            {
                mDescription = "";

                mEntryList.Insert(0, sResult);
                if (mPaginationMetaData.TotalCount >= mPaginationParameters.PageSize)
                {
                    mEntryList.Remove(mEntryList[mPaginationParameters.PageSize - 1]);
                }
            }
        }

        public async Task RemoveEntry(EntryDto aEntryDto)
        {
            if (!await ConfirmDeleteEntry()) return;

            if(await EntryViewModel.RemoveEntry(aEntryDto.Id))
            {
                await GetEntries();
            }
        }

        public async Task AdminRemoveEntry(EntryDto aEntryDto)
        {
            if (!await ConfirmDeleteEntry()) return;

            if (await EntryViewModel.AdminRemoveEntry(aEntryDto.Id))
            {
                await GetEntries();
            }
        }

        private async Task<bool> ConfirmDeleteEntry()
        {
            var sOptions = new ModalOptions
            {
                DisableBackgroundCancel = true,
                Size = ModalSize.Custom,
                SizeCustomClass = "modal-large",
                Position = ModalPosition.Middle
            };
            var sParameters = new ModalParameters { { "Message", $"Är du säker på att du vill radera detta inlägg? Det går inte att ångra!" } };
            var sModal = Modal.Show<ConfirmationComponent>("Bekräfta borttagning", sParameters, sOptions);
            var sResult = await sModal.Result;

            if (!sResult.Cancelled)
            {
                return true;
            }

            return false;
        }

        private async Task ChangeSnus()
        {
            var sOptions = new ModalOptions
            {
                DisableBackgroundCancel = false,
                Size = ModalSize.Custom,
                SizeCustomClass = "modal-large",
                Position = ModalPosition.Middle
            };
            var sModal = Modal.Show<ChooseSnusComponent>("Välj Snus", sOptions);
            var sResult = await sModal.Result;

            if (!sResult.Cancelled)
            {
                var sSnus = sResult.Data as SnusModel;

                mChosenSnus = new SnusDto
                {
                    Id = sSnus.Id,
                    Name = sSnus.Name,
                };
            }
        }

        private async Task ToggleLike(EntryDto aEntryDto)
        {
            if(aEntryDto.LikedByUser)
            {
                if(await EntryViewModel.UnlikeEntry(aEntryDto.Id))
                {
                    aEntryDto.Likes -= 1;
                    aEntryDto.LikedByUser = false;
                }
            }
            else
            {
                if (await EntryViewModel.LikeEntry(aEntryDto.Id))
                {
                    aEntryDto.Likes += 1;
                    aEntryDto.LikedByUser = true;
                }
            }
        }

        private void ShowLikes(EntryDto aEntryDto)
        {
            if(aEntryDto.Likes == 0)
            {
                EntryViewModel.AddError("Inlägget har inga likes :(");
                return;
            }

            var sOptions = new ModalOptions
            {
                DisableBackgroundCancel = false,
                Size = ModalSize.Medium,
                Position = ModalPosition.Middle
            };

            var sParametes = new ModalParameters { { "EntryModelId", aEntryDto.Id } };

            Modal.Show<ShowEntryLikesComponent>("Likes", sParametes, sOptions);
        }

        private async Task Comment(EntryDto aEntryDto)
        {
            EntryViewModel.AddError("Not implemented yet! :(");
            await Task.Delay(0);
        }

        private void ShowProfilePicture(EntryDto aEntryDto)
        {
            var sParameters = new ModalParameters { { "ProfilePictureUrl", aEntryDto.UserProfilePictureUrl } };
            var sOptions = new ModalOptions { Size = ModalSize.Automatic, Position = ModalPosition.Middle };

            Modal.Show<ProfilePictureComponent>($"{aEntryDto.UserName}'s Profilbild", sParameters, sOptions);
        }
        #endregion
    }
}
