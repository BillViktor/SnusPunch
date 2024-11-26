using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using SnusPunch.Data.Models.Entry;
using SnusPunch.Data.Models.Identity;
using SnusPunch.Data.Repository;
using SnusPunch.Shared.Models.Entry.Likes;
using SnusPunch.Shared.Models.Pagination;
using SnusPunch.Shared.Models.ResultModel;
using System.Security.Claims;

namespace SnusPunch.Services.Entry
{
    public class EntryLikeService
    {
        private readonly ILogger<EntryLikeService> mLogger;
        private readonly IConfiguration mConfiguration;
        private readonly SnusPunchRepository mSnusPunchRepository;
        private readonly UserManager<SnusPunchUserModel> mUserManager;

        public EntryLikeService(ILogger<EntryLikeService> aLogger, IConfiguration aConfiguration, SnusPunchRepository aSnusPunchRepository, UserManager<SnusPunchUserModel> aUserManager)
        {
            mLogger = aLogger;
            mConfiguration = aConfiguration;
            mSnusPunchRepository = aSnusPunchRepository;
            mUserManager = aUserManager;
        }

        public async Task<ResultModel> LikeEntry(int aEntryModelId, ClaimsPrincipal aClaimsPrincipal)
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

                var sEntry = await mSnusPunchRepository.GetEntryById(aEntryModelId);

                if (sEntry == null)
                {
                    sResultModel.Success = false;
                    sResultModel.AddError("Inlägget hittades ej.");
                    return sResultModel;
                }

                EntryLikeModel sEntryLikeModel = new EntryLikeModel
                {
                    EntryId = sEntry.Id,
                    SnusPunchUserModelId = sUser.Id
                };

                await mSnusPunchRepository.LikeEntry(sEntryLikeModel);
            }
            catch (Exception aException)
            {
                mLogger.LogError(aException, "Exception at LikeEntry in EntryLikeService");
                sResultModel.Success = false;
                sResultModel.AddExceptionError(aException);
            }

            return sResultModel;
        }

        public async Task<ResultModel> UnlikeEntry(int aEntryModelId, ClaimsPrincipal aClaimsPrincipal)
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

                var sLike = await mSnusPunchRepository.GetEntryLike(aEntryModelId, sUser.Id);

                await mSnusPunchRepository.UnlikeEntry(sLike);
            }
            catch (Exception aException)
            {
                mLogger.LogError(aException, "Exception at UnlikeEntry in EntryLikeService");
                sResultModel.Success = false;
                sResultModel.AddExceptionError(aException);
            }

            return sResultModel;
        }

        public async Task<ResultModel<PaginationResponse<EntryLikeDto>>> GetEntryLikesPaginated(PaginationParameters aPaginationParameters, int aEntryModelId)
        {
            ResultModel<PaginationResponse<EntryLikeDto>> sResultModel = new ResultModel<PaginationResponse<EntryLikeDto>>();

            try
            {
                sResultModel.ResultObject = await mSnusPunchRepository.GetEntryLikesPaginated(aPaginationParameters, aEntryModelId);
            }
            catch (Exception aException)
            {
                mLogger.LogError(aException, "Exception at GetEntryLikesPaginated in EntryLikeService");
                sResultModel.Success = false;
                sResultModel.AddExceptionError(aException);
            }

            return sResultModel;
        }
    }
}
