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
            PaginationResponse<SnusModel> sPaginationResponse = new PaginationResponse<SnusModel>();
            IsBusy = true;


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
    }
}
