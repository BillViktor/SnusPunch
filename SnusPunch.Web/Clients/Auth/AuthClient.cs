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

        public async Task<ResultModel> Register(RegisterModel aRegisterModel)
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
    }
}
