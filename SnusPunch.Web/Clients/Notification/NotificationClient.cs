using SnusPunch.Shared.Models.Entry;
using SnusPunch.Shared.Models.Notification;
using SnusPunch.Shared.Models.Pagination;
using SnusPunch.Shared.Models.ResultModel;
using System.Net.Http.Json;

namespace SnusPunch.Web.Clients.Notification
{
    public class NotificationClient
    {
        private readonly IHttpClientFactory mHttpClientFactory;

        public NotificationClient(IHttpClientFactory aHttpClientFactory)
        {
            mHttpClientFactory = aHttpClientFactory;
        }

        public async Task<ResultModel<PaginationResponse<NotificationDto>>> GetNotificationsPaginated(PaginationParameters aPaginationParameters)
        {
            ResultModel<PaginationResponse<NotificationDto>> sResultModel = new ResultModel<PaginationResponse<NotificationDto>>();

            try
            {
                var sHttpClient = mHttpClientFactory.CreateClient(HttpClientEnum.Notification.ToString());
                var sResponse = await sHttpClient.PostAsJsonAsync($"GetNotificationsPaginated", aPaginationParameters);

                if (!sResponse.IsSuccessStatusCode)
                {
                    throw new Exception("Non-success status code at GetNotificationsPaginated in NotificationClient");
                }

                sResultModel = await sResponse.Content.ReadFromJsonAsync<ResultModel<PaginationResponse<NotificationDto>>>();
            }
            catch (Exception aException)
            {
                sResultModel.Success = false;
                sResultModel.AddExceptionError(aException);
            }

            return sResultModel;
        }

        public async Task<ResultModel> SetAllNotificationsAsRead()
        {
            ResultModel sResultModel = new ResultModel();

            try
            {
                var sHttpClient = mHttpClientFactory.CreateClient(HttpClientEnum.Notification.ToString());
                var sResponse = await sHttpClient.PutAsync($"SetAllNotificationsAsRead", null);

                if (!sResponse.IsSuccessStatusCode)
                {
                    throw new Exception("Non-success status code at SetAllNotificationsAsRead in NotificationClient");
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
