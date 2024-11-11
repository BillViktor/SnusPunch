using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;
using SnusPunch.Shared.Models.Auth;
using SnusPunch.Shared.Models.Identity;
using SnusPunch.Shared.Models.ResultModel;

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

        public async Task<ResultModel> Register(RegisterModel aRegisterModel)
        {
            ResultModel sResultModel = new ResultModel();

            try
            {
                SnusPunchUserModel sSnusPunchUserModel = new SnusPunchUserModel
                {
                    UserName = aRegisterModel.UserName,
                    Email = aRegisterModel.Email,
                    LockoutEnabled = false
                };

                var sCreateUserResult = await mUserManager.CreateAsync(sSnusPunchUserModel, aRegisterModel.Password);

                if(!sCreateUserResult.Succeeded)
                {
                    sResultModel.AppendErrors(sCreateUserResult.Errors.Select(x => x.Description).ToList());
                    sResultModel.Success = false;
                    return sResultModel;
                }
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
