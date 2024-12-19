using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using SnusPunch.Data.Models.Entry;
using SnusPunch.Data.Models.Identity;
using SnusPunch.Data.Repository;
using SnusPunch.Services.NotificationService;
using SnusPunch.Shared.Models.Entry;
using SnusPunch.Shared.Models.Entry.Likes;
using SnusPunch.Shared.Models.Notification;
using SnusPunch.Shared.Models.Pagination;
using SnusPunch.Shared.Models.ResultModel;
using System.Security.Claims;

namespace SnusPunch.Services.Entry
{
    public class EntryCommentLikeService
    {
        private readonly ILogger<EntryCommentLikeService> mLogger;
        private readonly IConfiguration mConfiguration;
        private readonly SnusPunchRepository mSnusPunchRepository;
        private readonly UserManager<SnusPunchUserModel> mUserManager;
        private readonly NotificationHub mNotificationHub;

        public EntryCommentLikeService(ILogger<EntryCommentLikeService> aLogger, IConfiguration aConfiguration, SnusPunchRepository aSnusPunchRepository, UserManager<SnusPunchUserModel> aUserManager, NotificationHub aNotificationHub)
        {
            mLogger = aLogger;
            mConfiguration = aConfiguration;
            mSnusPunchRepository = aSnusPunchRepository;
            mUserManager = aUserManager;
            mNotificationHub = aNotificationHub;
        }

        public async Task<ResultModel> LikeComment(int aEntryCommentModelid, ClaimsPrincipal aClaimsPrincipal)
        {
            ResultModel sResultModel = new ResultModel();

            try
            {
                var sUser = await mUserManager.GetUserAsync(aClaimsPrincipal);

                if(sUser == null)
                {
                    sResultModel.Success = false;
                    sResultModel.AddError("Användaren hittades ej.");
                    return sResultModel;
                }

                var sComment = await mSnusPunchRepository.GetEntryCommentById(aEntryCommentModelid);

                if (sComment == null)
                {
                    sResultModel.Success = false;
                    sResultModel.AddError("Kommentaren hittades ej.");
                    return sResultModel;
                }

                EntryCommentLikeModel sEntryLikeModel = new EntryCommentLikeModel
                {
                    EntryCommentId = sComment.Id,
                    SnusPunchUserModelId = sUser.Id
                };

                await mSnusPunchRepository.LikeComment(sEntryLikeModel);

                //Skicka notis
                if (sUser.Id != sComment.SnusPunchUserModelId)
                {
                    await mNotificationHub.AddNotification(sComment.SnusPunchUserModelId, sUser.Id, NotificationActionEnum.CommentLike, sComment.Id);
                    await mNotificationHub.SendNotification(NotificationTypeEnum.EntryEvent, $"{sUser.UserName} har gillat din kommentar!", sComment.SnusPunchUserModelId);
                }
            }
            catch (Exception aException)
            {
                mLogger.LogError(aException, "Exception at LikeComment in EntryCommentLikeService");
                sResultModel.Success = false;
                sResultModel.AddExceptionError(aException);
            }

            return sResultModel;
        }

        public async Task<ResultModel> UnlikeComment(int aEntryModelId, ClaimsPrincipal aClaimsPrincipal)
        {
            ResultModel sResultModel = new ResultModel();

            try
            {
                var sUser = await mUserManager.GetUserAsync(aClaimsPrincipal);

                if (sUser == null)
                {
                    sResultModel.Success = false;
                    sResultModel.AddError("Användaren hittades ej.");
                    return sResultModel;
                }

                var sLike = await mSnusPunchRepository.GetCommentLike(aEntryModelId, sUser.Id);

                await mSnusPunchRepository.UnlikeComment(sLike);
            }
            catch (Exception aException)
            {
                mLogger.LogError(aException, "Exception at UnlikeComment in EntryCommentLikeService");
                sResultModel.Success = false;
                sResultModel.AddExceptionError(aException);
            }

            return sResultModel;
        }

        public async Task<ResultModel<PaginationResponse<EntryLikeDto>>> GetCommentLikesPaginated(PaginationParameters aPaginationParameters, int aEntryModelId)
        {
            ResultModel<PaginationResponse<EntryLikeDto>> sResultModel = new ResultModel<PaginationResponse<EntryLikeDto>>();

            try
            {
                sResultModel.ResultObject = await mSnusPunchRepository.GetCommentLikesPaginated(aPaginationParameters, aEntryModelId);
            }
            catch (Exception aException)
            {
                mLogger.LogError(aException, "Exception at GetCommentLikesPaginated in EntryCommentLikeService");
                sResultModel.Success = false;
                sResultModel.AddExceptionError(aException);
            }

            return sResultModel;
        }
    }
}
