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
    }
}
