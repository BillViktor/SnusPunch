using SnusPunch.Shared.Models.Auth;
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
    }
}
