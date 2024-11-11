using SnusPunch.Shared.Models.Pagination;
using SnusPunch.Shared.Models.Snus;
using SnusPunch.Web.Clients.Snus;

namespace SnusPunch.Web.ViewModels.Snus
{
    public class SnusViewModel : BaseViewModel
    {
        private readonly SnusClient mSnusClient;

        #region Fields
        private List<SnusModel> mSnus = new List<SnusModel>();
        #endregion


        #region Properties
        public List<SnusModel> Snus { get { return mSnus; } set { SetValue(ref mSnus, value); } }
        #endregion

        public SnusViewModel(SnusClient aSnusClient)
        {
            mSnusClient = aSnusClient;
        }

        public async Task GetSnus()
        {
            IsBusy = true;

            var sResult = await mSnusClient.GetSnus();

            if (!sResult.Success)
            {
                Errors.AddRange(sResult.Errors);
            }
            else
            {
                Snus = sResult.ResultObject;
            }

            IsBusy = false;
        }

        public async Task<PaginationResponse<SnusModel>> GetSnusPaginated(PaginationParameters aPaginationParameters)
        {
            IsBusy = true;
            PaginationResponse<SnusModel> sPaginationResponse = new PaginationResponse<SnusModel>();

            var sResult = await mSnusClient.GetSnusPaginated(aPaginationParameters);

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

        public async Task<bool> RemoveSnus(SnusModel aSnusModel)
        {
            IsBusy = true;
            bool sSuccess = true;

            var sResult = await mSnusClient.RemoveSnus(aSnusModel.Id);

            if (!sResult.Success)
            {
                Errors.AddRange(sResult.Errors);
                sSuccess = false;
            }
            else
            {
                SuccessMessages.Add($"Raderade {aSnusModel.Name}!");
            }

            IsBusy = false;

            return sSuccess;
        }

        public async Task<bool> UpdateSnus(SnusModel aSnusModel)
        {
            IsBusy = true;
            bool sSuccess = true;

            var sResult = await mSnusClient.UpdateSnus(aSnusModel);

            if (!sResult.Success)
            {
                Errors.AddRange(sResult.Errors);
                sSuccess = false;
            }
            else
            {
                SuccessMessages.Add($"Uppdaterade {aSnusModel.Name}!");
            }

            IsBusy = false;

            return sSuccess;
        }

        public async Task<bool> AddSnus(SnusModel aSnusModel)
        {
            IsBusy = true;
            bool sSuccess = true;

            var sResult = await mSnusClient.AddSnus(aSnusModel);

            if (!sResult.Success)
            {
                Errors.AddRange(sResult.Errors);
                sSuccess = false;
            }
            else
            {
                SuccessMessages.Add($"Lade till {aSnusModel.Name}!");
            }

            IsBusy = false;

            return sSuccess;
        }
    }
}
