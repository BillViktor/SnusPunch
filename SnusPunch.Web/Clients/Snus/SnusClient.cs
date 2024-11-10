using SnusPunch.Shared.Models.Pagination;
using SnusPunch.Shared.Models.ResultModel;
using SnusPunch.Shared.Models.Snus;
using System.Net.Http.Json;

namespace SnusPunch.Web.Clients.Snus
{
    public class SnusClient
    {
        private readonly IHttpClientFactory mHttpClientFactory;

        public SnusClient(IHttpClientFactory aHttpClientFactory)
        {
            mHttpClientFactory = aHttpClientFactory;
        }

        public async Task<ResultModel<SnusModel>> AddSnus(SnusModel aSnusModel)
        {
            ResultModel<SnusModel> sResultModel = new ResultModel<SnusModel>();

            try
            {
                var sHttpClient = mHttpClientFactory.CreateClient(HttpClientEnum.Snus.ToString());
                var sResponse = await sHttpClient.PostAsJsonAsync("AddSnus", aSnusModel);

                if (!sResponse.IsSuccessStatusCode)
                {
                    throw new Exception("Non-success status code at AddSnus in SnusClient");
                }

                sResultModel = await sResponse.Content.ReadFromJsonAsync<ResultModel<SnusModel>>();
            }
            catch (Exception aException)
            {
                sResultModel.Success = false;
                sResultModel.AddExceptionError(aException);
            }

            return sResultModel;
        }

        public async Task<ResultModel<List<SnusModel>>> GetSnus()
        {
            ResultModel<List<SnusModel>> sResultModel = new ResultModel<List<SnusModel>>();

            try
            {
                var sHttpClient = mHttpClientFactory.CreateClient(HttpClientEnum.Snus.ToString());
                var sResponse = await sHttpClient.GetAsync("GetSnus");

                if (!sResponse.IsSuccessStatusCode)
                {
                    throw new Exception("Non-success status code at GetSnus in SnusClient");
                }

                sResultModel = await sResponse.Content.ReadFromJsonAsync<ResultModel<List<SnusModel>>>();
            }
            catch (Exception aException)
            {
                sResultModel.Success = false;
                sResultModel.AddExceptionError(aException);
            }

            return sResultModel;
        }

        public async Task<ResultModel<PaginationResponse<SnusModel>>> GetSnusPaginated(PaginationParameters aPaginationParameters)
        {
            ResultModel<PaginationResponse<SnusModel>> sResultModel = new ResultModel<PaginationResponse<SnusModel>>();

            try
            {
                var sHttpClient = mHttpClientFactory.CreateClient(HttpClientEnum.Snus.ToString());
                var sResponse = await sHttpClient.PostAsJsonAsync("GetSnusPaginated", aPaginationParameters);

                if (!sResponse.IsSuccessStatusCode)
                {
                    throw new Exception("Non-success status code at GetSnusPaginated in SnusClient");
                }

                sResultModel = await sResponse.Content.ReadFromJsonAsync<ResultModel<PaginationResponse<SnusModel>>>();
            }
            catch (Exception aException)
            {
                sResultModel.Success = false;
                sResultModel.AddExceptionError(aException);
            }

            return sResultModel;
        }

        public async Task<ResultModel<SnusModel>> UpdateSnus(SnusModel aSnusModel)
        {
            ResultModel<SnusModel> sResultModel = new ResultModel<SnusModel>();

            try
            {
                var sHttpClient = mHttpClientFactory.CreateClient(HttpClientEnum.Snus.ToString());
                var sResponse = await sHttpClient.PutAsJsonAsync("UpdateSnus", aSnusModel);

                if (!sResponse.IsSuccessStatusCode)
                {
                    throw new Exception("Non-success status code at UpdateSnus in SnusClient");
                }

                sResultModel = await sResponse.Content.ReadFromJsonAsync<ResultModel<SnusModel>>();
            }
            catch (Exception aException)
            {
                sResultModel.Success = false;
                sResultModel.AddExceptionError(aException);
            }

            return sResultModel;
        }

        public async Task<ResultModel> RemoveSnus(int aSnusModelId)
        {
            ResultModel sResultModel = new ResultModel();

            try
            {
                var sHttpClient = mHttpClientFactory.CreateClient(HttpClientEnum.Snus.ToString());
                var sResponse = await sHttpClient.DeleteAsync($"RemoveSnus/{aSnusModelId}");

                if (!sResponse.IsSuccessStatusCode)
                {
                    throw new Exception("Non-success status code at RemoveSnus in SnusClient");
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
