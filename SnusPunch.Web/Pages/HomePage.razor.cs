﻿using Blazored.Modal;
using Blazored.Modal.Services;
using Microsoft.AspNetCore.Components;
using SnusPunch.Shared.Models.Auth;
using SnusPunch.Shared.Models.Entry;
using SnusPunch.Shared.Models.Pagination;
using SnusPunch.Shared.Models.Snus;
using SnusPunch.Web.Components;
using SnusPunch.Web.Identity;
using SnusPunch.Web.ViewModels.Snus;

namespace SnusPunch.Web.Pages
{
    public partial class HomePage
    {
        [CascadingParameter] public IModalService Modal { get; set; } = default!;
        [Inject] AuthViewModel AuthViewModel { get; set; }
        [Inject] EntryViewModel EntryViewModel { get; set; }

        private string mDescription = "";
        private SnusDto? mFavouriteSnus = null;
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
                mFavouriteSnus = new SnusDto
                {
                    Id = (int)AuthViewModel.UserInfoModel.FavouriteSnusId,
                    Name = AuthViewModel.UserInfoModel.FavouriteSnusName,
                };
            }
        }

        #region Actions
        public async Task AddEntry()
        {
            if(mFavouriteSnus?.Id == null)
            {
                EntryViewModel.AddError("Du har inget favoritsnus!");
                return;
            }

            var sResult = await EntryViewModel.AddEntry(mFavouriteSnus.Id, mDescription);

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
            EntryViewModel.AddError("Not implemented yet! :(");
            await Task.Delay(0);
        }

        public async Task ToggleLike(EntryDto aEntryDto)
        {
            EntryViewModel.AddError("Not implemented yet! :(");
            await Task.Delay(0);
        }

        public async Task Comment(EntryDto aEntryDto)
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
