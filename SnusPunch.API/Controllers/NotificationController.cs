using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SnusPunch.Services.Entry;
using SnusPunch.Shared.Models.Entry.Likes;
using SnusPunch.Shared.Models.Notification;
using SnusPunch.Shared.Models.Pagination;
using SnusPunch.Shared.Models.ResultModel;

namespace SnusPunch.API.Controllers
{
    [ApiController]
    [Authorize]
    [Route("[controller]")]
    public class NotificationController : ControllerBase
    {
        private readonly ILogger<NotificationController> mLogger;
        private readonly NotificationService mNotificationService;

        public NotificationController(ILogger<NotificationController> aLogger, NotificationService aNotificationService)
        {
            mLogger = aLogger;
            mNotificationService = aNotificationService;
        }

        [HttpPost("GetNotificationsPaginated")]
        public async Task<ResultModel<PaginationResponse<NotificationDto>>> GetNotificationsPaginated(PaginationParameters aPaginationParameters)
        {
            return await mNotificationService.GetNotificationsPaginated(aPaginationParameters, User);
        }

        [HttpPut("SetAllNotificationsAsRead")]
        public async Task<ResultModel> SetAllNotificationsAsRead()
        {
            return await mNotificationService.SetAllNotificationsAsRead(User);
        }
    }
}
