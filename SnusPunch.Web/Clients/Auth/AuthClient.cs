using Microsoft.AspNetCore.Components.Forms;
using Microsoft.AspNetCore.Http;
using SnusPunch.Shared.Models.Auth;
using SnusPunch.Shared.Models.Auth.Email;
using SnusPunch.Shared.Models.Auth.Password;
using SnusPunch.Shared.Models.Pagination;
using SnusPunch.Shared.Models.ResultModel;
using SnusPunch.Shared.Models.Snus;
using System.Net.Http.Json;

namespace SnusPunch.Web.Clients.Snus
{
    public class AuthClient
    {
        private readonly IHttpClientFactory mHttpClientFactory;

        public AuthClient(IHttpClientFactory aHttpClientFactory)
        {
            mHttpClientFactory = aHttpClientFactory;
        }

        public async Task<ResultModel> Register(RegisterRequestModel aRegisterModel)
        {
            ResultModel sResultModel = new ResultModel();

            try
            {
                var sHttpClient = mHttpClientFactory.CreateClient(HttpClientEnum.Auth.ToString());
                var sResponse = await sHttpClient.PostAsJsonAsync("Register", aRegisterModel);

                if (!sResponse.IsSuccessStatusCode)
                {
                    throw new Exception("Non-success status code at Register in AuthClient");
                }

                sResultModel = await sResponse.Content.ReadFromJsonAsync<ResultModel>();
            }
            catch (Exception aException)
            {
                sResultModel.Success = false;
                sResultModel.AddExceptionError(aException);
            }

            return sResultModel;
        }

        #region Login/Info
        public async Task<ResultModel> Login(LoginRequestModel aLoginRequestModel)
        {
            ResultModel sResultModel = new ResultModel();

            try
            {
                var sHttpClient = mHttpClientFactory.CreateClient(HttpClientEnum.Auth.ToString());
                var sResponse = await sHttpClient.PostAsJsonAsync("login", aLoginRequestModel);

                if (!sResponse.IsSuccessStatusCode)
                {
                    throw new Exception("Non-success status code at Login in AuthClient");
                }

                sResultModel = await sResponse.Content.ReadFromJsonAsync<ResultModel>();
            }
            catch (Exception aException)
            {
                sResultModel.Success = false;
                sResultModel.AddExceptionError(aException);
            }

            return sResultModel;
        }

        public async Task<ResultModel> Logout()
        {
            ResultModel sResultModel = new ResultModel();

            try
            {
                var sHttpClient = mHttpClientFactory.CreateClient(HttpClientEnum.Auth.ToString());
                var sResponse = await sHttpClient.PostAsync("logout", null);

                if (!sResponse.IsSuccessStatusCode)
                {
                    throw new Exception("Non-success status code at Logout in AuthClient");
                }

                sResultModel = await sResponse.Content.ReadFromJsonAsync<ResultModel>();
            }
            catch (Exception aException)
            {
                sResultModel.Success = false;
                sResultModel.AddExceptionError(aException);
            }

            return sResultModel;
        }

        public async Task<ResultModel<UserInfoModel>> Info()
        {
            ResultModel<UserInfoModel> sResultModel = new ResultModel<UserInfoModel>();

            try
            {
                var sHttpClient = mHttpClientFactory.CreateClient(HttpClientEnum.Auth.ToString());
                var sResponse = await sHttpClient.GetAsync("Info");

                if (!sResponse.IsSuccessStatusCode)
                {
                    throw new Exception("Non-success status code at Info in AuthClient");
                }

                sResultModel = await sResponse.Content.ReadFromJsonAsync<ResultModel<UserInfoModel>>();
            }
            catch (Exception aException)
            {
                sResultModel.Success = false;
                sResultModel.AddExceptionError(aException);
            }

            return sResultModel;
        }
        #endregion


        #region Email
        public async Task<ResultModel<string>> ResendConfirmationEmail()
        {
            ResultModel<string> sResultModel = new ResultModel<string>();

            try
            {
                var sHttpClient = mHttpClientFactory.CreateClient(HttpClientEnum.Auth.ToString());
                var sResponse = await sHttpClient.PostAsync("ResendConfirmationEmail", null);

                if (!sResponse.IsSuccessStatusCode)
                {
                    throw new Exception("Non-success status code at ResendConfirmationEmail in AuthClient");
                }

                sResultModel = await sResponse.Content.ReadFromJsonAsync<ResultModel<string>>();
            }
            catch (Exception aException)
            {
                sResultModel.Success = false;
                sResultModel.AddExceptionError(aException);
            }

            return sResultModel;
        }

        public async Task<ResultModel> VerifyEmail(VerifyEmailRequest aVerifyEmailRequest)
        {
            ResultModel sResultModel = new ResultModel();

            try
            {
                var sHttpClient = mHttpClientFactory.CreateClient(HttpClientEnum.Auth.ToString());
                var sResponse = await sHttpClient.PostAsJsonAsync("VerifyEmail", aVerifyEmailRequest);

                if (!sResponse.IsSuccessStatusCode)
                {
                    throw new Exception("Non-success status code at VerifyEmail in AuthClient");
                }

                sResultModel = await sResponse.Content.ReadFromJsonAsync<ResultModel>();
            }
            catch (Exception aException)
            {
                sResultModel.Success = false;
                sResultModel.AddExceptionError(aException);
            }

            return sResultModel;
        }

        public async Task<ResultModel> ChangeEmail(ChangeEmailRequestModel aChangeEmailRequestModel)
        {
            ResultModel sResultModel = new ResultModel();

            try
            {
                var sHttpClient = mHttpClientFactory.CreateClient(HttpClientEnum.Auth.ToString());
                var sResponse = await sHttpClient.PostAsJsonAsync("ChangeEmail", aChangeEmailRequestModel);

                if (!sResponse.IsSuccessStatusCode)
                {
                    throw new Exception("Non-success status code at ChangeEmail in AuthClient");
                }

                sResultModel = await sResponse.Content.ReadFromJsonAsync<ResultModel>();
            }
            catch (Exception aException)
            {
                sResultModel.Success = false;
                sResultModel.AddExceptionError(aException);
            }

            return sResultModel;
        }

        public async Task<ResultModel> ConfirmChangeEmail(ConfirmChangeEmailRequestModel aConfirmChangeEmailRequestModel)
        {
            ResultModel sResultModel = new ResultModel();

            try
            {
                var sHttpClient = mHttpClientFactory.CreateClient(HttpClientEnum.Auth.ToString());
                var sResponse = await sHttpClient.PostAsJsonAsync("ConfirmChangeEmail", aConfirmChangeEmailRequestModel);

                if (!sResponse.IsSuccessStatusCode)
                {
                    throw new Exception("Non-success status code at ConfirmChangeEmail in AuthClient");
                }

                sResultModel = await sResponse.Content.ReadFromJsonAsync<ResultModel>();
            }
            catch (Exception aException)
            {
                sResultModel.Success = false;
                sResultModel.AddExceptionError(aException);
            }

            return sResultModel;
        }
        #endregion


        #region Password
        public async Task<ResultModel> ForgotPassword(ForgotPasswordRequestModel aForgotPasswordRequestModel)
        {
            ResultModel sResultModel = new ResultModel();

            try
            {
                var sHttpClient = mHttpClientFactory.CreateClient(HttpClientEnum.Auth.ToString());
                var sResponse = await sHttpClient.PostAsJsonAsync("ForgotPassword", aForgotPasswordRequestModel);

                if (!sResponse.IsSuccessStatusCode)
                {
                    throw new Exception("Non-success status code at ForgotPassword in AuthClient");
                }

                sResultModel = await sResponse.Content.ReadFromJsonAsync<ResultModel>();
            }
            catch (Exception aException)
            {
                sResultModel.Success = false;
                sResultModel.AddExceptionError(aException);
            }

            return sResultModel;
        }

        public async Task<ResultModel> ChangePassword(ChangePasswordRequestModel aChangePasswordRequestModel)
        {
            ResultModel sResultModel = new ResultModel();

            try
            {
                var sHttpClient = mHttpClientFactory.CreateClient(HttpClientEnum.Auth.ToString());
                var sResponse = await sHttpClient.PostAsJsonAsync("ChangePassword", aChangePasswordRequestModel);

                if (!sResponse.IsSuccessStatusCode)
                {
                    throw new Exception("Non-success status code at ChangePassword in AuthClient");
                }

                sResultModel = await sResponse.Content.ReadFromJsonAsync<ResultModel>();
            }
            catch (Exception aException)
            {
                sResultModel.Success = false;
                sResultModel.AddExceptionError(aException);
            }

            return sResultModel;
        }

        public async Task<ResultModel> ResetPassword(ResetPasswordRequestModel aResetPasswordRequestModel)
        {
            ResultModel sResultModel = new ResultModel();

            try
            {
                var sHttpClient = mHttpClientFactory.CreateClient(HttpClientEnum.Auth.ToString());
                var sResponse = await sHttpClient.PostAsJsonAsync("ResetPassword", aResetPasswordRequestModel);

                if (!sResponse.IsSuccessStatusCode)
                {
                    throw new Exception("Non-success status code at ResetPassword in AuthClient");
                }

                sResultModel = await sResponse.Content.ReadFromJsonAsync<ResultModel>();
            }
            catch (Exception aException)
            {
                sResultModel.Success = false;
                sResultModel.AddExceptionError(aException);
            }

            return sResultModel;
        }
        #endregion


        #region Profile Picture
        public async Task<ResultModel> AddOrUpdateProfilePicture(MultipartFormDataContent aMultipartFormDataContent)
        {
            ResultModel sResultModel = new ResultModel();

            try
            {
                var sHttpClient = mHttpClientFactory.CreateClient(HttpClientEnum.Auth.ToString());
                var sResponse = await sHttpClient.PutAsync("AddOrUpdateProfilePicture", aMultipartFormDataContent);

                if (!sResponse.IsSuccessStatusCode)
                {
                    throw new Exception("Non-success status code at AddOrUpdateProfilePicture in AuthClient");
                }

                sResultModel = await sResponse.Content.ReadFromJsonAsync<ResultModel>();
            }
            catch (Exception aException)
            {
                sResultModel.Success = false;
                sResultModel.AddExceptionError(aException);
            }

            return sResultModel;
        }

        public async Task<ResultModel> DeleteProfilePicture()
        {
            ResultModel sResultModel = new ResultModel();

            try
            {
                var sHttpClient = mHttpClientFactory.CreateClient(HttpClientEnum.Auth.ToString());
                var sResponse = await sHttpClient.DeleteAsync("DeleteProfilePicture");

                if (!sResponse.IsSuccessStatusCode)
                {
                    throw new Exception("Non-success status code at DeleteProfilePicture in AuthClient");
                }

                sResultModel = await sResponse.Content.ReadFromJsonAsync<ResultModel>();
            }
            catch (Exception aException)
            {
                sResultModel.Success = false;
                sResultModel.AddExceptionError(aException);
            }

            return sResultModel;
        }

        public async Task<ResultModel> DeleteUserProfilePicture(string aUserName)
        {
            ResultModel sResultModel = new ResultModel();

            try
            {
                var sHttpClient = mHttpClientFactory.CreateClient(HttpClientEnum.Auth.ToString());
                var sResponse = await sHttpClient.DeleteAsync($"DeleteProfilePicture/{aUserName}");

                if (!sResponse.IsSuccessStatusCode)
                {
                    throw new Exception("Non-success status code at DeleteUserProfilePicture in AuthClient");
                }

                sResultModel = await sResponse.Content.ReadFromJsonAsync<ResultModel>();
            }
            catch (Exception aException)
            {
                sResultModel.Success = false;
                sResultModel.AddExceptionError(aException);
            }

            return sResultModel;
        }
        #endregion
    }
}
