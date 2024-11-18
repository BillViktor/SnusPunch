using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;
using SnusPunch.Data.Models.Identity;
using SnusPunch.Data.Repository;
using SnusPunch.Shared.Models.Auth;
using SnusPunch.Shared.Models.Pagination;
using SnusPunch.Shared.Models.ResultModel;
using SnusPunch.Shared.Models.Snus;
using System.Security.Claims;

namespace SnusPunch.Services.Snus
{
    public class SnusService
    {
        private readonly ILogger<SnusService> mLogger;
        private readonly SnusPunchRepository mSnusPunchRepository;
        private readonly UserManager<SnusPunchUserModel> mUserManager;

        public SnusService(ILogger<SnusService> aLogger, SnusPunchRepository aSnusPunchRepository, UserManager<SnusPunchUserModel> aUserManager)
        {
            mLogger = aLogger;
            mSnusPunchRepository = aSnusPunchRepository;
            mUserManager = aUserManager;
        }

        public async Task<ResultModel<SnusModel>> AddSnus(SnusModel aSnusModel)
        {
            ResultModel<SnusModel> sResultModel = new ResultModel<SnusModel>();

            try
            {
                sResultModel.ResultObject = await mSnusPunchRepository.AddSnus(aSnusModel);
            }
            catch (Exception aException)
            {
                mLogger.LogError(aException, "Exception at AddSnus in SnusService");
                sResultModel.Success = false;
                sResultModel.AddExceptionError(aException);
            }

            return sResultModel;
        }

        public async Task<ResultModel<List<SnusModel>>> GetSnus()
        {
            ResultModel<List<SnusModel>> sResultModel = new ResultModel<List<SnusModel>>();

            try
            {
                sResultModel.ResultObject = await mSnusPunchRepository.GetSnus();
            }
            catch(Exception aException)
            {
                mLogger.LogError(aException, "Exception at GetSnus in SnusService");
                sResultModel.Success = false;
                sResultModel.AddExceptionError(aException);
            }

            return sResultModel;
        }

        public async Task<ResultModel<SnusModel>> GetSnusById(int aSnusModelId)
        {
            ResultModel<SnusModel> sResultModel = new ResultModel<SnusModel>();

            try
            {
                sResultModel.ResultObject = await mSnusPunchRepository.GetSnusById(aSnusModelId);
            }
            catch (Exception aException)
            {
                mLogger.LogError(aException, "Exception at GetSnusById in SnusService");
                sResultModel.Success = false;
                sResultModel.AddExceptionError(aException);
            }

            return sResultModel;
        }

        public async Task<ResultModel<PaginationResponse<SnusModel>>> GetSnusPaginated(PaginationParameters aPaginationParameters)
        {
            ResultModel<PaginationResponse<SnusModel>> sResultModel = new ResultModel<PaginationResponse<SnusModel>>();

            try
            {
                sResultModel.ResultObject = await mSnusPunchRepository.GetSnusPaginated(aPaginationParameters);
            }
            catch (Exception aException)
            {
                mLogger.LogError(aException, "Exception at GetSnusPaginated in SnusService");
                sResultModel.Success = false;
                sResultModel.AddExceptionError(aException);
            }

            return sResultModel;
        }

        public async Task<ResultModel<SnusModel>> UpdateSnus(SnusModel aSnusModel)
        {
            ResultModel<SnusModel> sResultModel = new ResultModel<SnusModel>();

            try
            {
                sResultModel.ResultObject = await mSnusPunchRepository.UpdateSnus(aSnusModel);
            }
            catch (Exception aException)
            {
                mLogger.LogError(aException, "Exception at UpdateSnus in SnusService");
                sResultModel.Success = false;
                sResultModel.AddExceptionError(aException);
            }

            return sResultModel;
        }

        public async Task<ResultModel> RemoveSnus(int aSnusModelId)
        {
            ResultModel sResultModel = new ResultModel();

            try
            {
                await mSnusPunchRepository.RemoveSnus(aSnusModelId);
            }
            catch (Exception aException)
            {
                mLogger.LogError(aException, "Exception at RemoveSnus in SnusService");
                sResultModel.Success = false;
                sResultModel.AddExceptionError(aException);
            }

            return sResultModel;
        }

        public async Task<ResultModel> SetFavouriteSnus(int aSnusId, ClaimsPrincipal aClaimsPrincipal)
        {
            ResultModel sResultModel = new ResultModel();

            try
            {
                var sUser = await mUserManager.GetUserAsync(aClaimsPrincipal);

                if(sUser == null)
                {
                    sResultModel.Success = false;
                    sResultModel.AddError("Användaren hittades ej");
                }
                else
                {
                    await mSnusPunchRepository.SetFavouriteSnus(sUser.Id, aSnusId);
                }

                
            }
            catch (Exception aException)
            {
                mLogger.LogError(aException, "Exception at RemoveSnus in SnusService");
                sResultModel.Success = false;
                sResultModel.AddExceptionError(aException);
            }

            return sResultModel;
        }
    }
}
