using SnusPunch.Shared.Models.Auth;
using SnusPunch.Shared.Models.Pagination;
using SnusPunch.Shared.Models.ResultModel;
using System.Net.Http.Json;

namespace SnusPunch.Web.Clients.Snus
{
    public class FriendClient
    {
        private readonly IHttpClientFactory mHttpClientFactory;

        public FriendClient(IHttpClientFactory aHttpClientFactory)
        {
            mHttpClientFactory = aHttpClientFactory;
        }

        public async Task<ResultModel<PaginationResponse<SnusPunchUserDto>>> GetFriendsForUser(PaginationParameters aPaginationParameters, string aUserName)
        {
            ResultModel<PaginationResponse<SnusPunchUserDto>> sResultModel = new ResultModel<PaginationResponse<SnusPunchUserDto>>();

            try
            {
                var sHttpClient = mHttpClientFactory.CreateClient(HttpClientEnum.Friend.ToString());
                var sResponse = await sHttpClient.PostAsJsonAsync($"GetFriendsForUser/{aUserName}", aPaginationParameters);

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

        public async Task<ResultModel> SendFriendRequest(string aUserName)
        {
            ResultModel sResultModel = new ResultModel();

            try
            {
                var sHttpClient = mHttpClientFactory.CreateClient(HttpClientEnum.Friend.ToString());
                var sResponse = await sHttpClient.PostAsync($"SendFriendRequest/{aUserName}", null);

                if (!sResponse.IsSuccessStatusCode)
                {
                    throw new Exception("Non-success status code at SendFriendRequest in UserClient");
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

        public async Task<ResultModel> RemoveFriendRequest(string aUserName)
        {
            ResultModel sResultModel = new ResultModel();

            try
            {
                var sHttpClient = mHttpClientFactory.CreateClient(HttpClientEnum.Friend.ToString());
                var sResponse = await sHttpClient.DeleteAsync($"RemoveFriendRequest/{aUserName}");

                if (!sResponse.IsSuccessStatusCode)
                {
                    throw new Exception("Non-success status code at RemoveFriendRequest in UserClient");
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

        public async Task<ResultModel> DenyFriendRequest(string aUserName)
        {
            ResultModel sResultModel = new ResultModel();

            try
            {
                var sHttpClient = mHttpClientFactory.CreateClient(HttpClientEnum.Friend.ToString());
                var sResponse = await sHttpClient.PutAsync($"DenyFriendRequest/{aUserName}", null);

                if (!sResponse.IsSuccessStatusCode)
                {
                    throw new Exception("Non-success status code at DenyFriendRequest in UserClient");
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

        public async Task<ResultModel> AcceptFriendRequest(string aUserName)
        {
            ResultModel sResultModel = new ResultModel();

            try
            {
                var sHttpClient = mHttpClientFactory.CreateClient(HttpClientEnum.Friend.ToString());
                var sResponse = await sHttpClient.PutAsync($"AcceptFriendRequest/{aUserName}", null);

                if (!sResponse.IsSuccessStatusCode)
                {
                    throw new Exception("Non-success status code at AcceptFriendRequest in UserClient");
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

        public async Task<ResultModel> RemoveFriend(string aUserName)
        {
            ResultModel sResultModel = new ResultModel();

            try
            {
                var sHttpClient = mHttpClientFactory.CreateClient(HttpClientEnum.Friend.ToString());
                var sResponse = await sHttpClient.DeleteAsync($"RemoveFriend/{aUserName}");

                if (!sResponse.IsSuccessStatusCode)
                {
                    throw new Exception("Non-success status code at RemoveFriend in UserClient");
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
