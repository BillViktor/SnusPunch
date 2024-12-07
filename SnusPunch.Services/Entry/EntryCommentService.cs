using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using SnusPunch.Data.Models.Entry;
using SnusPunch.Data.Models.Identity;
using SnusPunch.Data.Repository;
using SnusPunch.Shared.Models.Entry;
using SnusPunch.Shared.Models.Entry.Likes;
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

        public EntryCommentService(ILogger<EntryCommentService> aLogger, IConfiguration aConfiguration, SnusPunchRepository aSnusPunchRepository, UserManager<SnusPunchUserModel> aUserManager)
        {
            mLogger = aLogger;
            mConfiguration = aConfiguration;
            mSnusPunchRepository = aSnusPunchRepository;
            mUserManager = aUserManager;
        }

        public async Task<ResultModel<PaginationResponse<EntryCommentDto>>> GetEntryCommentsPaginated(PaginationParameters aPaginationParameters, int aEntryCommentModelId, ClaimsPrincipal aClaimsPrincipal)
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

                sResultModel.ResultObject = await mSnusPunchRepository.GetEntryCommentsPaginated(aPaginationParameters, aEntryCommentModelId, sUser.Id);
            }
            catch (Exception aException)
            {
                mLogger.LogError(aException, "Exception at GetEntryCommentsPaginated in EntryCommentService");
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

                EntryCommentModel sEntryCommentModel = new EntryCommentModel
                {
                    EntryId = aAddEntryCommentDto.EntryModelId,
                    SnusPunchUserModelId = sUser.Id,
                    Comment = aAddEntryCommentDto.Comment,
                };

                sResultModel.ResultObject = await mSnusPunchRepository.AddEntryComment(sEntryCommentModel);
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
