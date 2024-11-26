using SnusPunch.Shared.Models.ResultModel;
using SnusPunch.Shared.Models.Statistics;
using System.Net.Http.Json;

namespace SnusPunch.Web.Clients.Snus
{
    public class StatisticsClient
    {
        private readonly IHttpClientFactory mHttpClientFactory;

        public StatisticsClient(IHttpClientFactory aHttpClientFactory)
        {
            mHttpClientFactory = aHttpClientFactory;
        }

        public async Task<ResultModel<StatisticsTimePeriodResponseDto>> GetStatisticsForTimePeriod(StatisticsTimePeriodRequestDto aStatisticsTimePeriodRequestDto)
        {
            ResultModel<StatisticsTimePeriodResponseDto> sResultModel = new ResultModel<StatisticsTimePeriodResponseDto>();

            try
            {
                var sHttpClient = mHttpClientFactory.CreateClient(HttpClientEnum.Statistics.ToString());
                var sResponse = await sHttpClient.PostAsJsonAsync("GetStatisticsForTimePeriod", aStatisticsTimePeriodRequestDto);

                if (!sResponse.IsSuccessStatusCode)
                {
                    throw new Exception("Non-success status code at GetStatisticsForTimePeriod in StatisticsClient");
                }

                sResultModel = await sResponse.Content.ReadFromJsonAsync<ResultModel<StatisticsTimePeriodResponseDto>>();
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
