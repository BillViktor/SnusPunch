using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
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
        private readonly RoleManager<IdentityRole> mRoleManager;
        private readonly EmailService mEmailService;
        private readonly IConfiguration mConfiguration;

        public AuthService(ILogger<AuthService> aLogger, UserManager<SnusPunchUserModel> aUserManager, SignInManager<SnusPunchUserModel> aSignInManager, RoleManager<IdentityRole> aRoleManager, EmailService aEmailService, IConfiguration aConfiguration)
        {
            mLogger = aLogger;
            mUserManager = aUserManager;
            mSignInManager = aSignInManager;
            mRoleManager = aRoleManager;
            mEmailService = aEmailService;
            mConfiguration = aConfiguration;
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

        public async Task<ResultModel<UserInfoModel>> Info(ClaimsPrincipal aClaimsPrincipal)
        {
            ResultModel<UserInfoModel> sResultModel = new ResultModel<UserInfoModel>();

            try
            {
                var sUser = await mUserManager.GetUserAsync(aClaimsPrincipal);
                var sIdentity = (ClaimsIdentity)aClaimsPrincipal.Identity;
                var sRoles = sIdentity.FindAll(sIdentity.RoleClaimType).Select(c => new RoleClaimModel
                {
                    Issuer = c.Issuer,
                    OriginalIssuer = c.OriginalIssuer,
                    Type = c.Type,
                    Value = c.Value,
                    ValueType = c.ValueType
                }).ToList();

                sResultModel.ResultObject = new UserInfoModel
                {
                    Email = sUser.Email,
                    IsEmailConfirmed = sUser.EmailConfirmed,
                    UserName = sUser.UserName,
                    RoleClaims = sRoles
                };
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

                string sLink = mConfiguration["FrontendUrl"] + $"/Verify?Token={sToken}";

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

        public async Task<ResultModel<string>> ResendVerificationEmail(ClaimsPrincipal aClaimsPrincipal)
        {
            ResultModel<string> sResultModel = new ResultModel<string>();

            try
            {
                var sUser = await mUserManager.GetUserAsync(aClaimsPrincipal);

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
                    sResultModel.Errors = sResult.Errors;
                    sResult.Success = false;
                }
                else
                {
                    sResultModel.ResultObject = sUser.Email;
                }
            }
            catch (Exception ex)
            {
                sResultModel.Success = false;
                sResultModel.AddExceptionError(ex);
            }

            return sResultModel;
        }

        public async Task<ResultModel> VerifyEmail(VerifyEmailRequest aVerifyEmailRequest)
        {
            ResultModel sResultModel = new ResultModel();

            try
            {
                var sUser = await mUserManager.Users.FirstOrDefaultAsync(x => x.Email == aVerifyEmailRequest.Email);

                if(sUser == null)
                {
                    sResultModel.Success = false;
                    sResultModel.AddError("Användaren hittades ej.");
                    return sResultModel;
                }

                var sResult = await mUserManager.ConfirmEmailAsync(sUser, aVerifyEmailRequest.Token);

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

                string sLink = mConfiguration["FrontendUrl"] + $"/ResetPassword?Token={sToken}";

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


        #region Roles
        public async Task<ResultModel> AddUserToRole(UserRoleRequestModel aUserRoleRequestModel)
        {
            ResultModel sResultModel = new ResultModel();

            try
            {
                var sUser = await mUserManager.Users.FirstOrDefaultAsync(x => x.UserName == aUserRoleRequestModel.UserName);

                if(sUser == null)
                {
                    sResultModel.Success = false;
                    sResultModel.AddError($"User: {aUserRoleRequestModel.UserName} not found.");
                    return sResultModel;
                }

                if(!await mRoleManager.RoleExistsAsync(aUserRoleRequestModel.RoleName))
                {
                    sResultModel.Success = false;
                    sResultModel.AddError($"Role: {aUserRoleRequestModel.RoleName} not found.");
                    return sResultModel;
                }

                await mUserManager.AddToRoleAsync(sUser, aUserRoleRequestModel.RoleName);
            }
            catch (Exception ex)
            {
                sResultModel.Success = false;
                sResultModel.AddExceptionError(ex);
            }

            return sResultModel;
        }

        public async Task<ResultModel> RemoveUserFromRole(UserRoleRequestModel aUserRoleRequestModel)
        {
            ResultModel sResultModel = new ResultModel();

            try
            {
                var sUser = await mUserManager.Users.FirstOrDefaultAsync(x => x.UserName == aUserRoleRequestModel.UserName);

                if (sUser == null)
                {
                    sResultModel.Success = false;
                    sResultModel.AddError($"User: {aUserRoleRequestModel.UserName} not found.");
                    return sResultModel;
                }

                if (!await mRoleManager.RoleExistsAsync(aUserRoleRequestModel.RoleName))
                {
                    sResultModel.Success = false;
                    sResultModel.AddError($"Role: {aUserRoleRequestModel.RoleName} not found.");
                    return sResultModel;
                }

                if(!await mUserManager.IsInRoleAsync(sUser, aUserRoleRequestModel.RoleName))
                {
                    sResultModel.Success = false;
                    sResultModel.AddError($"User: {aUserRoleRequestModel.UserName} is not in role {aUserRoleRequestModel.RoleName}.");
                    return sResultModel;
                }

                await mUserManager.RemoveFromRoleAsync(sUser, aUserRoleRequestModel.RoleName);
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
