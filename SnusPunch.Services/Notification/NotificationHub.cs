using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.SignalR;
using SnusPunch.Data.Models;
using SnusPunch.Data.Models.Entry;
using SnusPunch.Data.Repository;
using SnusPunch.Shared.Models.Notification;

namespace SnusPunch.Services.NotificationService
{
    [Authorize]
    public class NotificationHub : Hub
    {
        private readonly IHubContext<NotificationHub> mHubContext;
        private readonly SnusPunchRepository mSnusPunchRepository;

        public NotificationHub(IHubContext<NotificationHub> aHubContext, SnusPunchRepository aSnusPunchRepository)
        {
            mHubContext = aHubContext;
            mSnusPunchRepository = aSnusPunchRepository;
        }

        public async Task SendNotification(NotificationTypeEnum aNotificationTypeEnum, string aMessage, string aReceivingUserId)
        {
            await mHubContext.Clients.User(aReceivingUserId).SendAsync("ReceiveMessage", aNotificationTypeEnum.ToString(), aMessage);
        }

        public async Task AddNotification(string aReceivingUserId, string aSendingUserId, NotificationActionEnum aNotificationActionEnum, int aEntityId)
        {
            try
            {
                NotificationModel sNotificationModel = new NotificationModel
                {
                    SnusPunchUserModelIdOne = aReceivingUserId,
                    SnusPunchUserModelIdTwo = aSendingUserId,
                    NotificationViewed = false,
                    EntityId = aEntityId,
                    NotificationActionEnum = aNotificationActionEnum
                };

                await mSnusPunchRepository.AddNotification(sNotificationModel);
            }
            catch { }
        }
    }
}
