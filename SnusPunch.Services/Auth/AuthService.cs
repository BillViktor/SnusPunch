using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using SnusPunch.Services.Email;
using SnusPunch.Shared.Models.Auth;
using SnusPunch.Shared.Models.Identity;
using SnusPunch.Shared.Models.ResultModel;
using System.Security.Claims;

namespace SnusPunch.Services.Snus
{
    public class AuthService
    {
        private readonly ILogger<AuthService> mLogger;
        private readonly UserManager<SnusPunchUserModel> mUserManager;
        private readonly SignInManager<SnusPunchUserModel> mSignInManager;
        private readonly EmailService mEmailService;

        public AuthService(ILogger<AuthService> aLogger, UserManager<SnusPunchUserModel> aUserManager, SignInManager<SnusPunchUserModel> aSignInManager, EmailService aEmailService)
        {
            mLogger = aLogger;
            mUserManager = aUserManager;
            mSignInManager = aSignInManager;
            mEmailService = aEmailService;
        }

        public async Task<ResultModel> Register(RegisterRequestModel aRegisterRequest)
        {
            ResultModel sResultModel = new ResultModel();

            try
            {
                SnusPunchUserModel sSnusPunchUserModel = new SnusPunchUserModel
                {
                    UserName = aRegisterRequest.UserName,
                    Email = aRegisterRequest.Email,
                    LockoutEnabled = false
                };

                var sCreateUserResult = await mUserManager.CreateAsync(sSnusPunchUserModel, aRegisterRequest.Password);

                if (!sCreateUserResult.Succeeded)
                {
                    sResultModel.AppendErrors(sCreateUserResult.Errors.Select(x => x.Description).ToList());
                    sResultModel.Success = false;
                    return sResultModel;
                }

                await SendVerificationEmail(sSnusPunchUserModel);
            }
            catch (Exception ex)
            {
                sResultModel.Success = false;
                sResultModel.AddExceptionError(ex);
            }

            return sResultModel;
        }

        #region Login
        public async Task<ResultModel> Login(LoginRequestModel aLoginRequest)
        {
            ResultModel sResultModel = new ResultModel();

            try
            {
                var sUser = await mUserManager.Users.FirstOrDefaultAsync(x => x.UserName == aLoginRequest.UserName);

                if (sUser == null)
                {
                    sResultModel.AddError("Inloggningen misslyckades.");
                    sResultModel.Success = false;
                    return sResultModel;
                }

                var sResult = await mSignInManager.PasswordSignInAsync(sUser, aLoginRequest.Password, aLoginRequest.RememberMe, false);

                if(!sResult.Succeeded)
                {
                    sResultModel.AddError("Inloggningen misslyckades.");
                    sResultModel.Success = false;
                    return sResultModel;
                }
            }
            catch (Exception ex)
            {
                sResultModel.Success = false;
                sResultModel.AddExceptionError(ex);
            }

            return sResultModel;
        }

        public async Task<ResultModel> Logout()
        {
            ResultModel sResultModel = new ResultModel();

            try
            {
                await mSignInManager.SignOutAsync();
            }
            catch (Exception ex)
            {
                sResultModel.Success = false;
                sResultModel.AddExceptionError(ex);
            }

            return sResultModel;
        }

        public async Task<ResultModel<string>> Info(ClaimsPrincipal aClaimsPrincipal)
        {
            ResultModel<string> sResultModel = new ResultModel<string>();

            try
            {
                var sUser = await mUserManager.GetUserAsync(aClaimsPrincipal);

                sResultModel.ResultObject = sUser.UserName;
            }
            catch (Exception ex)
            {
                sResultModel.Success = false;
                sResultModel.AddExceptionError(ex);
            }

            return sResultModel;
        }

        public async Task<ResultModel<List<RoleClaimModel>>> Roles(ClaimsPrincipal aClaimsPrincipal)
        {
            ResultModel<List<RoleClaimModel>> sResultModel = new ResultModel<List<RoleClaimModel>>();

            try
            {
                var sIdentity = (ClaimsIdentity)aClaimsPrincipal.Identity;
                var sRoles = sIdentity.FindAll(sIdentity.RoleClaimType).Select(c => new RoleClaimModel
                {
                    Issuer = c.Issuer,
                    OriginalIssuer = c.OriginalIssuer,
                    Type = c.Type,
                    Value = c.Value,
                    ValueType = c.ValueType
                }).ToList();

                sResultModel.ResultObject = sRoles;
            }
            catch (Exception ex)
            {
                sResultModel.Success = false;
                sResultModel.AddExceptionError(ex);
            }

            return sResultModel;
        }
        #endregion


        #region Email Verification
        public async Task<ResultModel> SendVerificationEmail(SnusPunchUserModel aSnusPunchUserModel)
        {
            ResultModel sResultModel = new ResultModel();

            try
            {
                var sToken = await mUserManager.GenerateEmailConfirmationTokenAsync(aSnusPunchUserModel);

                string sLink = sToken;

                var sEmailResult = await mEmailService.SendEmail(aSnusPunchUserModel.Email, "SnusPunch - Bekräfta din e-postadress", EmailHelper.GetConfirmationEmail(sLink));

                if(!sEmailResult.Success)
                {
                    sResultModel = sEmailResult;
                }
            }
            catch (Exception ex)
            {
                sResultModel.Success = false;
                sResultModel.AddExceptionError(ex);
            }

            return sResultModel;
        }

        public async Task<ResultModel> ResendVerificationEmail(string aUserId)
        {
            ResultModel sResultModel = new ResultModel();

            try
            {
                var sUser = await mUserManager.Users.FirstOrDefaultAsync(x => x.Id == aUserId);

                if (sUser == null)
                {
                    sResultModel.Success = false;
                    sResultModel.AddError("Användaren hittades ej.");
                    return sResultModel;
                }

                if(sUser.EmailConfirmed)
                {
                    sResultModel.Success = false;
                    sResultModel.AddError("Användaren redan verifierad.");
                    return sResultModel;
                }

                var sResult = await SendVerificationEmail(sUser);

                if(!sResult.Success)
                {
                    sResultModel = sResult;
                }
            }
            catch (Exception ex)
            {
                sResultModel.Success = false;
                sResultModel.AddExceptionError(ex);
            }

            return sResultModel;
        }

        public async Task<ResultModel> VerifyEmail(string aUserId, string aToken)
        {
            ResultModel sResultModel = new ResultModel();

            try
            {
                var sUser = await mUserManager.Users.FirstOrDefaultAsync(x => x.Id == aUserId);

                if(sUser == null)
                {
                    sResultModel.Success = false;
                    sResultModel.AddError("Användaren hittades ej.");
                    return sResultModel;
                }

                var sResult = await mUserManager.ConfirmEmailAsync(sUser, aToken);

                if(!sResult.Succeeded)
                {
                    sResultModel.Success = false;
                    sResultModel.AppendErrors(sResult.Errors.Select(x => x.Description).ToList());
                    return sResultModel;
                }
            }
            catch (Exception ex)
            {
                sResultModel.Success = false;
                sResultModel.AddExceptionError(ex);
            }

            return sResultModel;
        }
        #endregion


        #region Password
        public async Task<ResultModel> ForgotPassword(string aEmail)
        {
            ResultModel sResultModel = new ResultModel();

            try
            {
                var sUser = await mUserManager.Users.FirstOrDefaultAsync(x => x.Email == aEmail);

                if(sUser == null)
                {
                    return sResultModel;
                }

                var sToken = await mUserManager.GeneratePasswordResetTokenAsync(sUser);

                string sLink = sToken;

                var sEmailResult = await mEmailService.SendEmail(sUser.Email, "SnusPunch - Återställ Lösenord", EmailHelper.GetResetPasswordEmail(sLink));

                if (!sEmailResult.Success)
                {
                    sResultModel = sEmailResult;
                }
            }
            catch (Exception ex)
            {
                sResultModel.Success = false;
                sResultModel.AddExceptionError(ex);
            }

            return sResultModel;
        }

        public async Task<ResultModel> ResetPassword(ResetPasswordRequestModel aResetPasswordRequest)
        {
            ResultModel sResultModel = new ResultModel();

            try
            {
                var sUser = await mUserManager.Users.FirstOrDefaultAsync(x => x.Email == aResetPasswordRequest.Email);

                if (sUser == null)
                {
                    sResultModel.Success = false;
                    sResultModel.AddError("Användaren hittades ej.");
                    return sResultModel;
                }

                var sResult = await mUserManager.ResetPasswordAsync(sUser, aResetPasswordRequest.Token, aResetPasswordRequest.Password);

                if (!sResult.Succeeded)
                {
                    sResultModel.Success = false;
                    sResultModel.AppendErrors(sResult.Errors.Select(x => x.Description).ToList());
                    return sResultModel;
                }
            }
            catch (Exception ex)
            {
                sResultModel.Success = false;
                sResultModel.AddExceptionError(ex);
            }

            return sResultModel;
        }
        #endregion
    }
}
