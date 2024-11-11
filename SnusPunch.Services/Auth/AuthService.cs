using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;
using SnusPunch.Data.Repository;
using SnusPunch.Shared.Models.Identity;
using SnusPunch.Shared.Models.Pagination;
using SnusPunch.Shared.Models.ResultModel;
using SnusPunch.Shared.Models.Snus;

namespace SnusPunch.Services.Snus
{
    public class AuthService
    {
        private readonly ILogger<AuthService> mLogger;
        private readonly UserManager<SnusPunchUserModel> mUserManager;

        public AuthService(ILogger<AuthService> aLogger, UserManager<SnusPunchUserModel> aUserManager)
        {
            mLogger = aLogger;
            mUserManager = aUserManager;
        }

        public async Task<ResultModel> Register()
        {
            ResultModel sResultModel = new ResultModel();

            try
            {
                await Task.Delay(0);
            }
            catch(Exception ex)
            {
                sResultModel.Success = false;
                sResultModel.AddExceptionError(ex);
            }

            return sResultModel;
        }
    }
}
