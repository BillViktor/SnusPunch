using Microsoft.AspNetCore.Components.Forms;
using SnusPunch.Shared.Constants;
using SnusPunch.Shared.Models.Auth;
using SnusPunch.Shared.Models.Auth.Email;
using SnusPunch.Shared.Models.Auth.Password;
using SnusPunch.Web.Clients.Snus;
using SnusPunch.Web.Identity;
using System.Net.Http.Headers;
using System.Net.Mime;

namespace SnusPunch.Web.ViewModels.Snus
{
    public class AuthViewModel : BaseViewModel
    {
        private readonly AuthClient mAuthClient;
        private readonly CookieAuthenticationStateProvider mCookieAuthenticationStateProvider;

        private UserInfoModel mUserInfoModel;

        public UserInfoModel UserInfoModel { get { return mCookieAuthenticationStateProvider.UserInfoModel; } }

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
            await mCookieAuthenticationStateProvider.CheckAuthenticatedAsync();
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

        public async Task<bool> ChangeEmail(ChangeEmailRequestModel aChangeEmailRequestModel)
        {
            bool sSuccess = true;
            IsBusy = true;

            var sResult = await mAuthClient.ChangeEmail(aChangeEmailRequestModel);

            if (!sResult.Success)
            {
                Errors.AddRange(sResult.Errors);
                sSuccess = false;
            }
            else
            {
                SuccessMessages.Add($"Ett bekräftelsemejl för att byta e-postadress till {aChangeEmailRequestModel.NewEmail} har skickats till din nuvarande e-postadress!");
            }

            IsBusy = false;
            return sSuccess;
        }

        public async Task<bool> ConfirmChangeEmail(ConfirmChangeEmailRequestModel aConfirmChangeEmailRequestModel)
        {
            bool sSuccess = true;
            IsBusy = true;

            var sResult = await mAuthClient.ConfirmChangeEmail(aConfirmChangeEmailRequestModel);

            if (!sResult.Success)
            {
                Errors.AddRange(sResult.Errors);
                sSuccess = false;
            }
            else
            {
                SuccessMessages.Add($"Bytet av e-postadress lyckades!");
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

        public async Task<bool> ChangePassword(ChangePasswordRequestModel aChangePasswordRequestModel)
        {
            bool sSuccess = true;
            IsBusy = true;

            var sResult = await mAuthClient.ChangePassword(aChangePasswordRequestModel);

            if (!sResult.Success)
            {
                Errors.AddRange(sResult.Errors);
                sSuccess = false;
            }
            else
            {
                SuccessMessages.Add("Ditt lösenord har nu ändrats!");
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

        public async Task<bool> AddOrUpdateProfilePicture(IBrowserFile aBrowserFile)
        {
            bool sSuccess = true;
            IsBusy = true;

            try
            {
                if(!ValidateProfilePicture(aBrowserFile))
                {
                    IsBusy = false;
                    return false;
                }

                using var sContent = new MultipartFormDataContent();
                var sFileContent = new StreamContent(aBrowserFile.OpenReadStream(AllowedImageFileTypes.ImageMaximumBytes));
                sFileContent.Headers.ContentType = new MediaTypeHeaderValue(aBrowserFile.ContentType);

                sContent.Add(sFileContent, "aFormFile", aBrowserFile.Name);

                var sResult = await mAuthClient.AddOrUpdateProfilePicture(sContent);

                if (!sResult.Success)
                {
                    sSuccess = false;
                    Errors.AddRange(sResult.Errors);
                }
                else
                {
                    SuccessMessages.Add($"Din profilbild har uppdaterats");
                }

            }
            catch(Exception ex)
            {
                AddError(ex.Message);
                sSuccess = false;
            }

            

            IsBusy = false;
            return sSuccess;
        }

        private bool ValidateProfilePicture(IBrowserFile aBrowserFile)
        {
            bool sSuccess = true;

            try
            {
                if(aBrowserFile == null)
                {
                    throw new Exception("Ingen fil vald.");
                }

                if (!AllowedImageFileTypes.AllowedMimeTypes.Any(x => string.Equals(x, aBrowserFile.ContentType, StringComparison.OrdinalIgnoreCase)))
                {
                    throw new Exception("Ogiltigt filformat.");
                }

                if (aBrowserFile.Size > AllowedImageFileTypes.ImageMaximumBytes)
                {
                    throw new Exception("Filen är för stor.");
                }
            }
            catch(Exception ex)
            {
                AddError(ex.Message);
                sSuccess = false;
            }

            return sSuccess;
        }
        #endregion
    }
}
