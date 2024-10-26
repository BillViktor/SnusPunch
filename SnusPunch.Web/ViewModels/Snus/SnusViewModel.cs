using SnusPunch.Web.Clients;

namespace SnusPunch.Web.ViewModels.Snus
{
    public class SnusViewModel : BaseViewModel
    {
        private readonly SnusClient mSnusClient;

        #region Fields
        
        #endregion


        #region Properties

        #endregion

        public SnusViewModel(SnusClient aSnusClient)
        {
            mSnusClient = aSnusClient;
        }
    }
}
