using Blazored.Modal;
using Blazored.Modal.Services;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Forms;
using Microsoft.AspNetCore.Components.Web;
using SnusPunch.Shared.Constants;
using SnusPunch.Shared.Models.Entry;
using SnusPunch.Shared.Models.Pagination;
using SnusPunch.Shared.Models.Snus;
using SnusPunch.Web.Components.Snus;
using SnusPunch.Web.ViewModels.Snus;

namespace SnusPunch.Web.Pages
{
    public partial class HomePage
    {
        [CascadingParameter] public IModalService Modal { get; set; } = default!;
        [Inject] AuthViewModel AuthViewModel { get; set; }
        [Inject] EntryViewModel EntryViewModel { get; set; }

        private bool mFetchEmptyPunches = false;
        private string? mImagePreview = "";
        private EntryFilterEnum mEntryFilterEnum = EntryFilterEnum.All;
        private IBrowserFile mBrowserFile = null;
        private Guid mInputFileId = Guid.NewGuid();
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
            var sResult = await EntryViewModel.GetEntriesPaginated(mPaginationParameters, mFetchEmptyPunches, mEntryFilterEnum);
            mPaginationMetaData = sResult.PaginationMetaData;
            mEntryList = sResult.Items;
        }

        private async Task OnSearch()
        {
            mPaginationParameters.PageNumber = 1;
            await GetEntries();
        }

        private async Task OnKeyDown(KeyboardEventArgs aKeyboardEventArgs)
        {
            if (aKeyboardEventArgs.Key == "Enter")
            {
                await OnSearch();
            }
        }


        #region Images
        private async void LoadImage(InputFileChangeEventArgs aInputFileChangeEventArgs)
        {
            try
            {
                if(aInputFileChangeEventArgs.File == null)
                {
                    throw new Exception("Ingen fil vald!");
                }

                if(!AllowedImageFileTypes.AllowedMimeTypes.Any(x => string.Equals(x, aInputFileChangeEventArgs.File.ContentType, StringComparison.OrdinalIgnoreCase)))
                {
                    throw new Exception($"Ogiltigt filformat, endast: {string.Join(", ", AllowedImageFileTypes.AllowedExtensions)} tillåts");
                }

                if(AllowedImageFileTypes.ImageMaximumBytes <= aInputFileChangeEventArgs.File.Size)
                {
                    throw new Exception($"Filen är för stor!");
                }

                var sBuf = new byte[aInputFileChangeEventArgs.File.Size];

                using (var sStram = aInputFileChangeEventArgs.File.OpenReadStream())
                {
                    await sStram.ReadAsync(sBuf);
                }

                mImagePreview = "data:image/jpeg;base64, " + Convert.ToBase64String(sBuf);
                
                mBrowserFile = aInputFileChangeEventArgs.File;
            }
            catch(Exception ex)
            {
                mInputFileId = Guid.NewGuid();
                EntryViewModel.AddError($"Ett fel uppstod: {ex.Message}");
            }

            StateHasChanged();
        }

        private void RemoveImage()
        {
            mImagePreview = null;
            mBrowserFile = null;
            mInputFileId = Guid.NewGuid();
        }
        #endregion


        #region CRUD
        private async Task AddEntry()
        {
            if (mChosenSnus?.Id == null)
            {
                EntryViewModel.AddError("Du har inte valt något snus!");
                return;
            }

            var sResult = await EntryViewModel.AddEntry(mChosenSnus.Id, mDescription, mBrowserFile);

            if (sResult != null)
            {
                mDescription = "";

                mEntryList.Insert(0, sResult);
                if (mPaginationMetaData.TotalCount >= mPaginationParameters.PageSize)
                {
                    mEntryList.Remove(mEntryList[mPaginationParameters.PageSize - 1]);
                }

                RemoveImage();
            }
        }
        #endregion

        #region Snus
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
        #endregion
    }
}
