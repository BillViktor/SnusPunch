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
                var sResponse = await sHttpClient.PostAsync("login", null);

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

        public async Task<ResultModel<string>> Info()
        {
            ResultModel<string> sResultModel = new ResultModel<string>();

            try
            {
                var sHttpClient = mHttpClientFactory.CreateClient(HttpClientEnum.Auth.ToString());
                var sResponse = await sHttpClient.GetAsync("Info");

                if (!sResponse.IsSuccessStatusCode)
                {
                    throw new Exception("Non-success status code at Info in AuthClient");
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

        public async Task<ResultModel<List<RoleClaimModel>>> Roles()
        {
            ResultModel<List<RoleClaimModel>> sResultModel = new ResultModel<List<RoleClaimModel>>();

            try
            {
                var sHttpClient = mHttpClientFactory.CreateClient(HttpClientEnum.Auth.ToString());
                var sResponse = await sHttpClient.GetAsync("Roles");

                if (!sResponse.IsSuccessStatusCode)
                {
                    throw new Exception("Non-success status code at Roles in AuthClient");
                }

                sResultModel = await sResponse.Content.ReadFromJsonAsync<ResultModel<List<RoleClaimModel>>>();
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

        #endregion

        #region Password

        #endregion

    }
}
