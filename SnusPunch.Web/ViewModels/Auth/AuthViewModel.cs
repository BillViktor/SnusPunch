using SnusPunch.Shared.Models.Auth;
using SnusPunch.Web.Clients.Snus;
using SnusPunch.Web.Identity;

namespace SnusPunch.Web.ViewModels.Snus
{
    public class AuthViewModel : BaseViewModel
    {
        private readonly AuthClient mAuthClient;
        private readonly CookieAuthenticationStateProvider mCookieAuthenticationStateProvider;

        private UserInfoModel mUserInfoModel;

        public UserInfoModel UserInfoModel { get { return mUserInfoModel; } }

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
                SuccessMessages.Add("Du är nu utloggad!");
            }

            IsBusy = false;
            return sSuccess;
        }

        public async Task GetUserInfo()
        {
            IsBusy = true;

            var sResult = await mAuthClient.Info();

            if (!sResult.Success)
            {
                Errors.AddRange(sResult.Errors);
            }
            else
            {
                mUserInfoModel = sResult.ResultObject;
            }

            IsBusy = false;
        }


        #region Email
        public async Task<bool> VerifyEmail(VerifyEmailRequest aVerifyEmailRequest)
        {
            bool sSuccess = true;
            IsBusy = true;

            var sResult = await mAuthClient.VerifyEmail(aVerifyEmailRequest);

            if (!sResult.Success)
            {
                Errors.AddRange(sResult.Errors);
                sSuccess = false;
            }
            else
            {
                SuccessMessages.Add("E-postadressen är nu verifierad!");
            }

            IsBusy = false;
            return sSuccess;
        }

        public async Task<bool> ResendConfirmationEmail()
        {
            bool sSuccess = true;
            IsBusy = true;

            var sResult = await mAuthClient.ResendConfirmationEmail();

            if (!sResult.Success)
            {
                Errors.AddRange(sResult.Errors);
                sSuccess = false;
            }
            else
            {
                SuccessMessages.Add($"Ett nytt mejl har skickats till {sResult.ResultObject}");
            }

            IsBusy = false;
            return sSuccess;
        }
        #endregion


        #region Password
        public async Task<bool> ForgotPassword(ForgotPasswordRequestModel aForgotPasswordRequestModel)
        {
            bool sSuccess = true;
            IsBusy = true;

            var sResult = await mAuthClient.ForgotPassword(aForgotPasswordRequestModel);

            if (!sResult.Success)
            {
                Errors.AddRange(sResult.Errors);
                sSuccess = false;
            }
            else
            {
                SuccessMessages.Add("Ett e-postmeddelande för att återställa e-postadressen har skickats om det fanns någon användare med given e-post.");
            }

            IsBusy = false;
            return sSuccess;
        }

        public async Task<bool> ResetPassword(ResetPasswordRequestModel aResetPasswordRequestModel)
        {
            bool sSuccess = true;
            IsBusy = true;

            var sResult = await mAuthClient.ResetPassword(aResetPasswordRequestModel);

            if (!sResult.Success)
            {
                Errors.AddRange(sResult.Errors);
                sSuccess = false;
            }
            else
            {
                SuccessMessages.Add("Lösenordet bytt! Du kan nu logga in.");
            }

            IsBusy = false;
            return sSuccess;
        }
        #endregion


        #region Profile Picture
        public async Task<bool> DeleteProfilePicture()
        {
            bool sSuccess = true;
            IsBusy = true;

            var sResult = await mAuthClient.DeleteProfilePicture();

            if (!sResult.Success)
            {
                Errors.AddRange(sResult.Errors);
                sSuccess = false;
            }
            else
            {
                SuccessMessages.Add($"Din profilbild har nu raderats.");
            }

            IsBusy = false;
            return sSuccess;
        }
        #endregion
    }
}
