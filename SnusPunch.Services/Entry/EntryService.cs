using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;
using SnusPunch.Data.Models.Entry;
using SnusPunch.Data.Models.Identity;
using SnusPunch.Data.Repository;
using SnusPunch.Services.Snus;
using SnusPunch.Shared.Models.Entry;
using SnusPunch.Shared.Models.Entry.Likes;
using SnusPunch.Shared.Models.Pagination;
using SnusPunch.Shared.Models.ResultModel;
using SnusPunch.Shared.Models.Snus;
using System.Security.Claims;

namespace SnusPunch.Services.Entry
{
    public class EntryService
    {
        private readonly ILogger<EntryService> mLogger;
        private readonly SnusPunchRepository mSnusPunchRepository;
        private readonly UserManager<SnusPunchUserModel> mUserManager;

        public EntryService(ILogger<EntryService> aLogger, SnusPunchRepository aSnusPunchRepository, UserManager<SnusPunchUserModel> aUserManager)
        {
            mLogger = aLogger;
            mSnusPunchRepository = aSnusPunchRepository;
            mUserManager = aUserManager;
        }

        public async Task<ResultModel<PaginationResponse<EntryDto>>> GetEntriesPaginated(PaginationParameters aPaginationParameters, ClaimsPrincipal aClaimsPrincipal)
        {
            ResultModel<PaginationResponse<EntryDto>> sResultModel = new ResultModel<PaginationResponse<EntryDto>>();

            try
            {
                var sUser = await mUserManager.GetUserAsync(aClaimsPrincipal);

                if(sUser == null)
                {
                    sResultModel.AddError("Användaren hittades ej");
                    sResultModel.Success = false;
                    return sResultModel;
                }

                sResultModel.ResultObject = await mSnusPunchRepository.GetEntriesPaginated(aPaginationParameters, sUser.Id);
            }
            catch (Exception aException)
            {
                mLogger.LogError(aException, "Exception at GetEntriesPaginated in EntryService");
                sResultModel.Success = false;
                sResultModel.AddExceptionError(aException);
            }

            return sResultModel;
        }

        public async Task<ResultModel<EntryDto>> AddEntry(AddEntryDto aAddEntryDto, ClaimsPrincipal aClaimsPrincipal)
        {
            ResultModel<EntryDto> sResultModel = new ResultModel<EntryDto>();

            try
            {
                var sUserId = mUserManager.GetUserId(aClaimsPrincipal);

                if(sUserId == null)
                {
                    sResultModel.Success = false;
                    sResultModel.AddError("Användaren hittades ej.");
                    return sResultModel;
                }

                EntryModel sEntryModel = new EntryModel
                {
                    Description = aAddEntryDto.Description,
                    SnusId = aAddEntryDto.SnusId,
                    SnusPunchUserModelId = sUserId
                };

                sResultModel.ResultObject = await mSnusPunchRepository.AddEntry(sEntryModel);
            }
            catch (Exception aException)
            {
                mLogger.LogError(aException, "Exception at AddEntry in EntryService");
                sResultModel.Success = false;
                sResultModel.AddExceptionError(aException);
            }

            return sResultModel;
        }

        public async Task<ResultModel> RemoveEntry(int aEntryModelId, ClaimsPrincipal aClaimsPrincipal)
        {
            ResultModel sResultModel = new ResultModel();

            try
            {
                var sUserId = mUserManager.GetUserId(aClaimsPrincipal);

                if (sUserId == null)
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

                //Verifiera konto
                if (sEntry.SnusPunchUserModelId != sUserId)
                {
                    sResultModel.Success = false;
                    sResultModel.AddError("Du saknar behörighet för att radera detta inlägg.");
                    return sResultModel;
                }

                await mSnusPunchRepository.RemoveEntry(sEntry);
            }
            catch (Exception aException)
            {
                mLogger.LogError(aException, "Exception at RemoveEntry in EntryService");
                sResultModel.Success = false;
                sResultModel.AddExceptionError(aException);
            }

            return sResultModel;
        }

        public async Task<ResultModel> RemoveEntry(int aEntryModelId)
        {
            ResultModel sResultModel = new ResultModel();

            try
            {
                var sEntry = await mSnusPunchRepository.GetEntryById(aEntryModelId);

                if (sEntry == null)
                {
                    sResultModel.Success = false;
                    sResultModel.AddError("Inlägget hittades ej.");
                    return sResultModel;
                }

                await mSnusPunchRepository.RemoveEntry(sEntry);
            }
            catch (Exception aException)
            {
                mLogger.LogError(aException, "Exception at RemoveEntry in EntryService");
                sResultModel.Success = false;
                sResultModel.AddExceptionError(aException);
            }

            return sResultModel;
        }

        #region Likes
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
                mLogger.LogError(aException, "Exception at LikeEntry in EntryService");
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
                mLogger.LogError(aException, "Exception at UnlikeEntry in EntryService");
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
                mLogger.LogError(aException, "Exception at UnlikeEntry in EntryService");
                sResultModel.Success = false;
                sResultModel.AddExceptionError(aException);
            }

            return sResultModel;
        }
        #endregion
    }
}
