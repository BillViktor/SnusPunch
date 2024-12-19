using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using SnusPunch.Data.Models.Identity;
using SnusPunch.Data.Repository;
using SnusPunch.Services.NotificationService;
using SnusPunch.Shared.Models.Auth;
using SnusPunch.Shared.Models.Notification;
using SnusPunch.Shared.Models.Pagination;
using SnusPunch.Shared.Models.ResultModel;
using System.Security.Claims;

namespace SnusPunch.Services.Entry
{
    public class NotificationService
    {
        private readonly ILogger<NotificationService> mLogger;
        private readonly IConfiguration mConfiguration;
        private readonly SnusPunchRepository mSnusPunchRepository;
        private readonly UserManager<SnusPunchUserModel> mUserManager;
        private readonly NotificationHub mNotificationHub;

        public NotificationService(ILogger<NotificationService> aLogger, IConfiguration aConfiguration, SnusPunchRepository aSnusPunchRepository, UserManager<SnusPunchUserModel> aUserManager, NotificationHub aNotificationHub)
        {
            mLogger = aLogger;
            mConfiguration = aConfiguration;
            mSnusPunchRepository = aSnusPunchRepository;
            mUserManager = aUserManager;
            mNotificationHub = aNotificationHub;
        }

        public async Task<ResultModel<PaginationResponse<NotificationDto>>> GetNotificationsPaginated(PaginationParameters aPaginationParameters, ClaimsPrincipal aClaimsPrincipal)
        {
            ResultModel<PaginationResponse<NotificationDto>> sResultModel = new ResultModel<PaginationResponse<NotificationDto>>();

            try
            {
                var sUser = await mUserManager.GetUserAsync(aClaimsPrincipal);

                if (sUser == null)
                {
                    sResultModel.AddError("Användaren hittades ej");
                    sResultModel.Success = false;
                    return sResultModel;
                }

                sResultModel.ResultObject = await mSnusPunchRepository.GetNotificationsPaginated(aPaginationParameters, sUser.Id);
            }
            catch (Exception aException)
            {
                mLogger.LogError(aException, "Exception at GetNotificationsPaginated in NotificationService");
                sResultModel.Success = false;
                sResultModel.AddExceptionError(aException);
            }

            return sResultModel;
        }

        public async Task<ResultModel> SetAllNotificationsAsRead(ClaimsPrincipal aClaimsPrincipal)
        {
            ResultModel sResultModel = new ResultModel();

            try
            {
                var sUser = await mUserManager.GetUserAsync(aClaimsPrincipal);

                if (sUser == null)
                {
                    sResultModel.AddError("Användaren hittades ej");
                    sResultModel.Success = false;
                    return sResultModel;
                }

                await mSnusPunchRepository.SetAllNotificationsAsRead(sUser.Id);
            }
            catch (Exception aException)
            {
                mLogger.LogError(aException, "Exception at SetAllNotificationsAsRead in NotificationService");
                sResultModel.Success = false;
                sResultModel.AddExceptionError(aException);
            }

            return sResultModel;
        }
    }
}
