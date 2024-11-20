using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;
using SnusPunch.Data.Models.Entry;
using SnusPunch.Data.Models.Identity;
using SnusPunch.Data.Repository;
using SnusPunch.Services.Snus;
using SnusPunch.Shared.Models.Entry;
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

        public async Task<ResultModel<PaginationResponse<EntryDto>>> GetEntriesPaginated(PaginationParameters aPaginationParameters)
        {
            ResultModel<PaginationResponse<EntryDto>> sResultModel = new ResultModel<PaginationResponse<EntryDto>>();

            try
            {
                sResultModel.ResultObject = await mSnusPunchRepository.GetEntriesPaginated(aPaginationParameters);
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
    }
}
