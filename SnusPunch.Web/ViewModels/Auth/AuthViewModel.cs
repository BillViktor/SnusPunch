using SnusPunch.Shared.Models.Auth;
using SnusPunch.Web.Clients.Snus;
using SnusPunch.Web.Identity;

namespace SnusPunch.Web.ViewModels.Snus
{
    public class AuthViewModel : BaseViewModel
    {
        private readonly AuthClient mAuthClient;
        private readonly CookieAuthenticationStateProvider mCookieAuthenticationStateProvider;

        public AuthViewModel(AuthClient aAuthClient, CookieAuthenticationStateProvider aCookieAuthenticationStateProvider)
        {
            mAuthClient = aAuthClient;
            mCookieAuthenticationStateProvider = aCookieAuthenticationStateProvider;
        }

        public async Task<bool> Register(RegisterRequestModel aRegisterModel)
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

        public async Task<bool> Login(LoginRequestModel aLoginRequestModel)
        {
            bool sSuccess = true;
            IsBusy = true;

            var sResult = await mAuthClient.Login(aLoginRequestModel);

            if (!sResult.Success)
            {
                Errors.AddRange(sResult.Errors);
                sSuccess = false;
            }
            else
            {
                mCookieAuthenticationStateProvider.NotifyAuthenticationStateChanged();
                SuccessMessages.Add("Inloggningen lyckades!");
            }

            IsBusy = false;
            return sSuccess;
        }

        public async Task<bool> Logout()
        {
            bool sSuccess = true;
            IsBusy = true;

            var sResult = await mAuthClient.Logout();

            if (!sResult.Success)
            {
                Errors.AddRange(sResult.Errors);
                sSuccess = false;
            }
            else
            {
                mCookieAuthenticationStateProvider.NotifyAuthenticationStateChanged();
                SuccessMessages.Add("Utloggningen lyckades!");
            }

            IsBusy = false;
            return sSuccess;
        }
    }
}
