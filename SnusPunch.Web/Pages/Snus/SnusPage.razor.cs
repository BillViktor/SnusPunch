using Microsoft.AspNetCore.Components;
using SnusPunch.Shared.Models.Pagination;
using SnusPunch.Shared.Models.Snus;
using SnusPunch.Web.ViewModels.Snus;

namespace SnusPunch.Web.Pages.Snus
{
    public partial class SnusPage
    {
        [Inject]
        SnusViewModel SnusViewModel { get; set; }

        private PaginationMetaData mPaginationMetaData = null;
        private List<SnusModel> mSnusList = new List<SnusModel>();
        private PaginationParameters mPaginationParameters = new PaginationParameters
        {
            SearchPropertyName = "name",
            SortPropertyName = "name"
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
        }

        private async Task GetSnus()
        {
            var sResult = await SnusViewModel.GetSnusPaginated(mPaginationParameters);
            mPaginationMetaData = sResult.PaginationMetaData;
            mSnusList = sResult.Items;
        }
    }
}
