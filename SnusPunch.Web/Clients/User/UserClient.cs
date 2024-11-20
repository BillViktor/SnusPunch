using SnusPunch.Shared.Models.Auth;
using SnusPunch.Shared.Models.Pagination;
using SnusPunch.Shared.Models.ResultModel;
using System.Net.Http.Json;

namespace SnusPunch.Web.Clients.Snus
{
    public class UserClient
    {
        private readonly IHttpClientFactory mHttpClientFactory;

        public UserClient(IHttpClientFactory aHttpClientFactory)
        {
            mHttpClientFactory = aHttpClientFactory;
        }

        public async Task<ResultModel<PaginationResponse<SnusPunchUserDto>>> GetUsersPaginated(PaginationParameters aPaginationParameters)
        {
            ResultModel<PaginationResponse<SnusPunchUserDto>> sResultModel = new ResultModel<PaginationResponse<SnusPunchUserDto>>();

            try
            {
                var sHttpClient = mHttpClientFactory.CreateClient(HttpClientEnum.User.ToString());
                var sResponse = await sHttpClient.PostAsJsonAsync("GetUsersPaginated", aPaginationParameters);

                if (!sResponse.IsSuccessStatusCode)
                {
                    throw new Exception("Non-success status code at GetUsersPaginated in UserClient");
                }

                sResultModel = await sResponse.Content.ReadFromJsonAsync<ResultModel<PaginationResponse<SnusPunchUserDto>>>();
            }
            catch (Exception aException)
            {
                sResultModel.Success = false;
                sResultModel.AddExceptionError(aException);
            }

            return sResultModel;
        }

        public async Task<ResultModel> Delete()
        {
            ResultModel sResultModel = new ResultModel();

            try
            {
                var sHttpClient = mHttpClientFactory.CreateClient(HttpClientEnum.Auth.ToString());
                var sResponse = await sHttpClient.DeleteAsync("Delete");

                if (!sResponse.IsSuccessStatusCode)
                {
                    throw new Exception("Non-success status code at Delete in UserClient");
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

        public async Task<ResultModel> DeleteUser(string aUserName)
        {
            ResultModel sResultModel = new ResultModel();

            try
            {
                var sHttpClient = mHttpClientFactory.CreateClient(HttpClientEnum.Auth.ToString());
                var sResponse = await sHttpClient.DeleteAsync($"DeleteUser/{aUserName}");

                if (!sResponse.IsSuccessStatusCode)
                {
                    throw new Exception("Non-success status code at DeleteUser in UserClient");
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
