using Blazored.Modal;
using Blazored.Modal.Services;
using Microsoft.AspNetCore.Components;
using SnusPunch.Shared.Models.Errors;
using SnusPunch.Shared.Models.Pagination;
using SnusPunch.Shared.Models.Snus;
using SnusPunch.Web.Components;
using SnusPunch.Web.Identity;
using SnusPunch.Web.Pages.Snus.Components;
using SnusPunch.Web.ViewModels.Snus;

namespace SnusPunch.Web.Pages.Snus
{
    public partial class SnusPage
    {
        [CascadingParameter] public IModalService Modal { get; set; } = default!;
        [Inject] AuthViewModel AuthViewModel { get; set; }
        [Inject] SnusViewModel SnusViewModel { get; set; }

        private int? mFavouriteSnusId = null;
        private PaginationMetaData mPaginationMetaData = null;
        private List<SnusModel> mSnusList = new List<SnusModel>();
        private PaginationParameters mPaginationParameters = new PaginationParameters
        {
            SearchPropertyNames = new List<string> { "Name" },
            SortPropertyName = "Name"
        };
        private Dictionary<string, string> mSortProperties = new Dictionary<string, string>
        {
            { "Name", "Namn" },
            { "PortionCount", "Antal Portioner" },
            { "PriceInSek", "Pris (SEK)" },
            { "WeightInGrams", "Vikt (g)" },
            { "NicotineInMgPerGram", "Nikotin (mg/g)" },
            { "PricePerPortion", "Pris(SEK/portion)" },
            { "NicotinePerPortion", "Nikotin (mg/portion)" },
        };

        protected override async Task OnInitializedAsync()
        {
            await GetSnus();

            mFavouriteSnusId = AuthViewModel.UserInfoModel?.FavouriteSnusId;
        }

        private async Task GetSnus()
        {
            var sResult = await SnusViewModel.GetSnusPaginated(mPaginationParameters);
            mPaginationMetaData = sResult.PaginationMetaData;
            mSnusList = sResult.Items;
        }

        #region Actions
        private async Task SetFavouriteSnus(SnusModel aSnusModel)
        {
            if(aSnusModel.Id == mFavouriteSnusId)
            {
                SnusViewModel.Errors.Add(new ErrorModel { ErrorText = $"{aSnusModel.Name} är redan ditt favoritsnus!" });
                return;
            }
            var sOptions = new ModalOptions
            {
                DisableBackgroundCancel = true,
                Size = ModalSize.Custom,
                SizeCustomClass = "modal-large",
                Position = ModalPosition.Middle
            };
            var sParameters = new ModalParameters { { "Message", $"Vill du sätta {aSnusModel.Name} som favoritsnus?" } };
            var sModal = Modal.Show<ConfirmationComponent>("Bekräfta nytt favoritsnus", sParameters, sOptions);
            var sResult = await sModal.Result;

            if (!sResult.Cancelled)
            {
                if(await SnusViewModel.SetFavouriteSnus(aSnusModel))
                {
                    mFavouriteSnusId = aSnusModel.Id;
                    await AuthViewModel.GetUserInfo();
                }
            }
        }

        private async Task DeleteSnus(SnusModel aSnusModel)
        {
            var sOptions = new ModalOptions
            {
                DisableBackgroundCancel = true,
                Size = ModalSize.Custom,
                SizeCustomClass = "modal-large",
                Position = ModalPosition.Middle
            };
            var sParameters = new ModalParameters { { "Message", $"Är du säker på att du vill radera {aSnusModel.Name}? Detta går inte att ångra!" } };
            var sModal = Modal.Show<ConfirmationComponent>("Bekräfta borttagning", sParameters, sOptions);
            var sResult = await sModal.Result;

            if (!sResult.Cancelled)
            {
                if(await SnusViewModel.RemoveSnus(aSnusModel))
                {
                    await GetSnus();
                }
            }
        }

        private async Task EditSnus(SnusModel aSnusModel)
        {
            var sOptions = new ModalOptions
            {
                DisableBackgroundCancel = true,
                Size = ModalSize.Custom,
                SizeCustomClass = "modal-large",
                Position = ModalPosition.Middle
            };

            var sParameters = new ModalParameters { { "SnusModel", aSnusModel } };
            var sModal = Modal.Show<AddEditSnusComponent>($"Redigera {aSnusModel.Name}", sParameters, sOptions);
            var sResult = await sModal.Result;

            if (!sResult.Cancelled)
            {
                var sSnus = sResult.Data as SnusModel;

                if (await SnusViewModel.UpdateSnus(sSnus))
                {
                    await GetSnus();
                }
            }
        }

        private async Task AddSnus()
        {
            var sOptions = new ModalOptions
            {
                DisableBackgroundCancel = true,
                Size = ModalSize.Custom,
                SizeCustomClass = "modal-large",
                Position = ModalPosition.Middle
            };

            var sModal = Modal.Show<AddEditSnusComponent>($"Lägg till nytt Snus", sOptions);

            var sResult = await sModal.Result;

            if (!sResult.Cancelled)
            {
                var sSnus = sResult.Data as SnusModel;

                if (await SnusViewModel.AddSnus(sSnus))
                {
                    await ShowAddedSnus(sSnus);
                }
            }
        }

        private async Task ShowAddedSnus(SnusModel aSnusModel)
        {
            var sParameters = new ModalParameters { { "Message", $"Snus \"{aSnusModel.Name}\" skapat! Vill du se den?" } };

            var sModal = Modal.Show<ConfirmationComponent>("Snus skapat!", sParameters);

            var sResult = await sModal.Result;

            if (!sResult.Cancelled)
            {
                mPaginationParameters.SearchString = aSnusModel.Name;
                mPaginationParameters.PageNumber = 1;
                await GetSnus();
            }
            else
            {
                await GetSnus();
            }
        }
        #endregion
    }
}
