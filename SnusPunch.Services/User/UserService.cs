using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using SnusPunch.Data.Models.Identity;
using SnusPunch.Data.Repository;
using SnusPunch.Shared.Models.Auth;
using SnusPunch.Shared.Models.Entry;
using SnusPunch.Shared.Models.Pagination;
using SnusPunch.Shared.Models.ResultModel;
using SnusPunch.Shared.Models.Snus;
using System.Security.Claims;

namespace SnusPunch.Services.Snus
{
    public class UserService
    {
        private readonly ILogger<UserService> mLogger;
        private readonly SnusPunchRepository mSnusPunchRepository;
        private readonly UserManager<SnusPunchUserModel> mUserManager;

        public UserService(ILogger<UserService> aLogger, SnusPunchRepository aSnusPunchRepository, UserManager<SnusPunchUserModel> aUserManager)
        {
            mLogger = aLogger;
            mSnusPunchRepository = aSnusPunchRepository;
            mUserManager = aUserManager;
        }

        public async Task<ResultModel<PaginationResponse<SnusPunchUserDto>>> GetUsersPaginated(PaginationParameters aPaginationParameters)
        {
            ResultModel<PaginationResponse<SnusPunchUserDto>> sResultModel = new ResultModel<PaginationResponse<SnusPunchUserDto>>();

            try
            {
                sResultModel.ResultObject = await mSnusPunchRepository.GetUsersPaginated(aPaginationParameters);
            }
            catch (Exception aException)
            {
                mLogger.LogError(aException, "Exception at GetUsersPaginated in SnusService");
                sResultModel.Success = false;
                sResultModel.AddExceptionError(aException);
            }

            return sResultModel;
        }

        public async Task<ResultModel<SnusPunchUserProfileDto>> GetUserProfile(string aUserName, ClaimsPrincipal aClaimsPrincipal)
        {
            ResultModel<SnusPunchUserProfileDto> sResultModel = new ResultModel<SnusPunchUserProfileDto>();

            try
            {
                var sUserToFetch = await mUserManager.Users.FirstOrDefaultAsync(x => x.UserName == aUserName);
                if (sUserToFetch == null)
                {
                    sResultModel.Success = false;
                    sResultModel.AddError($"Användaren {aUserName} existerar inte");
                    return sResultModel;
                }

                var sCurrentUser = await mUserManager.GetUserAsync(aClaimsPrincipal);

                if (sCurrentUser == null)
                {
                    sResultModel.AddError("Användaren hittades ej");
                    sResultModel.Success = false;
                    return sResultModel;
                }

                sResultModel.ResultObject = await mSnusPunchRepository.GetUserProfile(sUserToFetch.UserName, sUserToFetch.Id, sCurrentUser.Id);

                sResultModel.ResultObject.FriendshipStatusEnum = await mSnusPunchRepository.GetFriendshipStatus(sUserToFetch.Id, sCurrentUser.Id);
            }
            catch (Exception aException)
            {
                mLogger.LogError(aException, "Exception at GetUserProfile in SnusService");
                sResultModel.Success = false;
                sResultModel.AddExceptionError(aException);
            }

            return sResultModel;
        }
    }
}
