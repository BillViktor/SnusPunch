using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using SnusPunch.Data.Models.Entry;
using SnusPunch.Data.Models.Identity;
using SnusPunch.Data.Repository;
using SnusPunch.Services.NotificationService;
using SnusPunch.Shared.Models.Entry;
using SnusPunch.Shared.Models.Notification;
using SnusPunch.Shared.Models.Pagination;
using SnusPunch.Shared.Models.ResultModel;
using System.Security.Claims;

namespace SnusPunch.Services.Entry
{
    public class EntryCommentService
    {
        private readonly ILogger<EntryCommentService> mLogger;
        private readonly IConfiguration mConfiguration;
        private readonly SnusPunchRepository mSnusPunchRepository;
        private readonly UserManager<SnusPunchUserModel> mUserManager;
        private readonly NotificationHub mNotificationHub;

        public EntryCommentService(ILogger<EntryCommentService> aLogger, IConfiguration aConfiguration, SnusPunchRepository aSnusPunchRepository, UserManager<SnusPunchUserModel> aUserManager, NotificationHub aNotificationHub)
        {
            mLogger = aLogger;
            mConfiguration = aConfiguration;
            mSnusPunchRepository = aSnusPunchRepository;
            mUserManager = aUserManager;
            mNotificationHub = aNotificationHub;
        }

        public async Task<ResultModel<PaginationResponse<EntryCommentDto>>> GetEntryCommentsPaginated(PaginationParameters aPaginationParameters, int aEntryModelId, ClaimsPrincipal aClaimsPrincipal)
        {
            ResultModel<PaginationResponse<EntryCommentDto>> sResultModel = new ResultModel<PaginationResponse<EntryCommentDto>>();

            try
            {
                var sUser = await mUserManager.GetUserAsync(aClaimsPrincipal);

                if (sUser == null)
                {
                    sResultModel.Success = false;
                    sResultModel.AddError("Användaren hittades ej.");
                    return sResultModel;
                }

                sResultModel.ResultObject = await mSnusPunchRepository.GetEntryCommentsPaginated(aPaginationParameters, aEntryModelId, sUser.Id);
            }
            catch (Exception aException)
            {
                mLogger.LogError(aException, "Exception at GetEntryCommentsPaginated in EntryCommentService");
                sResultModel.Success = false;
                sResultModel.AddExceptionError(aException);
            }

            return sResultModel;
        }

        public async Task<ResultModel<PaginationResponse<EntryCommentDto>>> GetEntryCommentRepliesPaginated(PaginationParameters aPaginationParameters, int aEntryCommentModelId, ClaimsPrincipal aClaimsPrincipal)
        {
            ResultModel<PaginationResponse<EntryCommentDto>> sResultModel = new ResultModel<PaginationResponse<EntryCommentDto>>();

            try
            {
                var sUser = await mUserManager.GetUserAsync(aClaimsPrincipal);

                if (sUser == null)
                {
                    sResultModel.Success = false;
                    sResultModel.AddError("Användaren hittades ej.");
                    return sResultModel;
                }

                sResultModel.ResultObject = await mSnusPunchRepository.GetEntryCommentRepliesPaginated(aPaginationParameters, aEntryCommentModelId, sUser.Id);
            }
            catch (Exception aException)
            {
                mLogger.LogError(aException, "Exception at GetEntryCommentRepliesPaginated in EntryCommentService");
                sResultModel.Success = false;
                sResultModel.AddExceptionError(aException);
            }

            return sResultModel;
        }

        public async Task<ResultModel<EntryCommentDto>> AddEntryComment(AddEntryCommentDto aAddEntryCommentDto, ClaimsPrincipal aClaimsPrincipal)
        {
            ResultModel<EntryCommentDto> sResultModel = new ResultModel<EntryCommentDto>();

            try
            {
                var sUser = await mUserManager.GetUserAsync(aClaimsPrincipal);

                if (sUser == null)
                {
                    sResultModel.Success = false;
                    sResultModel.AddError("Användaren hittades ej.");
                    return sResultModel;
                }

                string? sUserIdRepliedTo = null;
                if(aAddEntryCommentDto.SnusPunchUserNameRepliedTo != null)
                {
                    var sUserRepliedTo = await mUserManager.Users.FirstOrDefaultAsync(x => x.UserName == aAddEntryCommentDto.SnusPunchUserNameRepliedTo);

                    if (sUserRepliedTo == null)
                    {
                        sResultModel.Success = false;
                        sResultModel.AddError("Användaren hittades ej.");
                        return sResultModel;
                    }

                    sUserIdRepliedTo = sUserRepliedTo.Id;
                }

                EntryCommentModel sEntryCommentModel = new EntryCommentModel
                {
                    EntryId = aAddEntryCommentDto.EntryModelId,
                    SnusPunchUserModelId = sUser.Id,
                    Comment = aAddEntryCommentDto.Comment,
                    ParentCommentId = aAddEntryCommentDto.ParentId,
                    SnusPunchUserModelIdRepliedTo = sUserIdRepliedTo
                };

                sResultModel.ResultObject = await mSnusPunchRepository.AddEntryComment(sEntryCommentModel);

                //Skicka notis
                var sEntry = await mSnusPunchRepository.GetEntryById(aAddEntryCommentDto.EntryModelId);
                if (sUser.Id != sEntry.SnusPunchUserModelId)
                {
                    //Till den som gjort inlägget
                    await mNotificationHub.AddNotification(sEntry.SnusPunchUserModelId, sUser.Id, NotificationActionEnum.Comment, sEntry.Id);
                    await mNotificationHub.SendNotification(NotificationTypeEnum.EntryEvent, $"{sUser.UserName} har kommenterat ditt inlägg!", sEntry.SnusPunchUserModelId);
                }

                //Till den vars kommentar man svarade på, om man inte svarade sig själv
                if (sUserIdRepliedTo != null && sUserIdRepliedTo != sUser.Id)
                {
                    await mNotificationHub.AddNotification(sUserIdRepliedTo, sUser.Id, NotificationActionEnum.CommentAnswered, sEntry.Id);
                    await mNotificationHub.SendNotification(NotificationTypeEnum.EntryEvent, $"{sUser.UserName} har svarat på din kommentar!", sUserIdRepliedTo);
                }
            }
            catch (Exception aException)
            {
                mLogger.LogError(aException, "Exception at AddEntryComment in EntryCommentService");
                sResultModel.Success = false;
                sResultModel.AddExceptionError(aException);
            }

            return sResultModel;
        }

        public async Task<ResultModel> RemoveEntryComment(int aEntryCommentModelId, ClaimsPrincipal aClaimsPrincipal)
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

                var sEntryComment = await mSnusPunchRepository.GetEntryCommentById(aEntryCommentModelId);

                if(sEntryComment == null)
                {
                    sResultModel.Success = false;
                    sResultModel.AddError("Kommentaren hittades ej!");
                    return sResultModel;
                }

                //Verifiera konto
                if (sEntryComment.SnusPunchUserModelId != sUser.Id)
                {
                    sResultModel.Success = false;
                    sResultModel.AddError("Du saknar behörighet för att radera detta inlägg.");
                    return sResultModel;
                }

                await mSnusPunchRepository.RemoveEntryComment(sEntryComment);
            }
            catch (Exception aException)
            {
                mLogger.LogError(aException, "Exception at RemoveEntryComment in EntryCommentService");
                sResultModel.Success = false;
                sResultModel.AddExceptionError(aException);
            }

            return sResultModel;
        }

        public async Task<ResultModel> RemoveEntryComment(int aEntryCommentModelId)
        {
            ResultModel sResultModel = new ResultModel();

            try
            {
                var sEntryComment = await mSnusPunchRepository.GetEntryCommentById(aEntryCommentModelId);

                if (sEntryComment == null)
                {
                    sResultModel.Success = false;
                    sResultModel.AddError("Kommentaren hittades ej.");
                    return sResultModel;
                }

                await mSnusPunchRepository.RemoveEntryComment(sEntryComment);
            }
            catch (Exception aException)
            {
                mLogger.LogError(aException, "Exception at RemoveEntryComment in EntryCommentService");
                sResultModel.Success = false;
                sResultModel.AddExceptionError(aException);
            }

            return sResultModel;
        }
    }
}
