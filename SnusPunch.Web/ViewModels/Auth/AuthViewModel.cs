using SnusPunch.Shared.Models.Auth;
using SnusPunch.Web.Clients.Snus;

namespace SnusPunch.Web.ViewModels.Snus
{
    public class AuthViewModel : BaseViewModel
    {
        private readonly AuthClient mAuthClient;

        public AuthViewModel(AuthClient aAuthClient)
        {
            mAuthClient = aAuthClient;
        }

        public async Task<bool> Register(RegisterModel aRegisterModel)
        {
            bool sSuccess = true;
            IsBusy = true;

            var sResult = await mAuthClient.Register(aRegisterModel);

            if (!sResult.Success)
            {
                Errors.AddRange(sResult.Errors);
                sSuccess = false;
            }
            else
            {
                SuccessMessages.Add("Registreringen lyckades! Du kan nu logga in.");
            }

            IsBusy = false;
            return sSuccess;
        }
    }
}
