using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using SnusPunch.Data.Models.Identity;
using SnusPunch.Data.Repository;
using SnusPunch.Services.Email;
using SnusPunch.Services.Helpers;
using SnusPunch.Shared.Models.Auth;
using SnusPunch.Shared.Models.Auth.Email;
using SnusPunch.Shared.Models.Auth.Password;
using SnusPunch.Shared.Models.ResultModel;
using System.Security.Claims;

namespace SnusPunch.Services.Snus
{
    public class AuthService
    {
        private readonly ILogger<AuthService> mLogger;
        private readonly SnusPunchRepository mSnusPunchRepository;
        private readonly UserManager<SnusPunchUserModel> mUserManager;
        private readonly SignInManager<SnusPunchUserModel> mSignInManager;
        private readonly RoleManager<IdentityRole> mRoleManager;
        private readonly EmailService mEmailService;
        private readonly IConfiguration mConfiguration;

        public AuthService(ILogger<AuthService> aLogger, SnusPunchRepository aSnusPunchRepository, UserManager<SnusPunchUserModel> aUserManager, SignInManager<SnusPunchUserModel> aSignInManager, RoleManager<IdentityRole> aRoleManager, EmailService aEmailService, IConfiguration aConfiguration)
        {
            mLogger = aLogger;
            mSnusPunchRepository = aSnusPunchRepository;
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

        public async Task<ResultModel> Delete(ClaimsPrincipal aClaimsPrincipal)
        {
            ResultModel sResultModel = new ResultModel();

            try
            {
                var sUser = await mUserManager.GetUserAsync(aClaimsPrincipal);
                string sUserName = sUser.UserName;
                string sEmail = sUser.Email;

                if(sUser == null)
                {
                    sResultModel.Success = false;
                    sResultModel.AddError("Användaren hittades ej");
                    return sResultModel;
                }

                var sResult = await mUserManager.DeleteAsync(sUser);

                if(!sResult.Succeeded)
                {
                    sResultModel.Success = false;
                    sResultModel.AppendErrors(sResult.Errors.Select(x => x.Description).ToList());
                    return sResultModel;
                }

                await DeleteProfilePicture(sUserName);
                await mEmailService.SendEmail(sEmail, "Bekräftelse på raderat konto", EmailHelper.GetAccountDeletedConfirmation());

                await Logout();
            }
            catch (Exception ex)
            {
                sResultModel.Success = false;
                sResultModel.AddExceptionError(ex);
            }

            return sResultModel;
        }

        public async Task<ResultModel> DeleteUser(string aUserName)
        {
            ResultModel sResultModel = new ResultModel();

            try
            {
                var sUser = await mUserManager.Users.FirstOrDefaultAsync(x => x.UserName == aUserName);
                string sEmail = sUser.Email;

                if (sUser == null)
                {
                    sResultModel.Success = false;
                    sResultModel.AddError("Användaren hittades ej");
                    return sResultModel;
                }

                var sResult = await mUserManager.DeleteAsync(sUser);

                if (!sResult.Succeeded)
                {
                    sResultModel.Success = false;
                    sResultModel.AppendErrors(sResult.Errors.Select(x => x.Description).ToList());
                    return sResultModel;
                }

                await DeleteProfilePicture(aUserName);
                await mEmailService.SendEmail(sEmail, "Ditt konto har blivit raderat", EmailHelper.GetAccountDeletedByAdminEmail());
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

                var sResult = await mSignInManager.PasswordSignInAsync(sUser, aLoginRequest.Password, aLoginRequest.RememberMe, true);

                if(!sResult.Succeeded)
                {
                    if (sResult.IsLockedOut)
                    {
                        sUser = await mUserManager.Users.FirstOrDefaultAsync(x => x.UserName == aLoginRequest.UserName);
                        sResultModel.AddError($"Kontot är låst, prova logga in igen om {(sUser.LockoutEnd - DateTime.Now).Value.Minutes} minuter.");
                    }
                    else
                    {
                        sResultModel.AddError("Inloggningen misslyckades.");
                        sResultModel.Success = false;
                    }
                    
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
                    RoleClaims = sRoles,
                    FavouriteSnusId = sUser.FavoriteSnusId,
                    ProfilePictureUrl = $"{mConfiguration["ProfilePicturePathFull"]}{sUser.ProfilePicturePath ?? "default.jpg"}"
                };

                if(sResultModel.ResultObject.FavouriteSnusId != null)
                {
                    var sSnus = await mSnusPunchRepository.GetSnusById((int)sResultModel.ResultObject.FavouriteSnusId);

                    if(sSnus != null)
                    {
                        sResultModel.ResultObject.FavouriteSnusName = sSnus.Name;
                    }
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


        #region Email
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

                if(sUser.EmailConfirmed)
                {
                    sResultModel.Success = false;
                    sResultModel.AddError("Användaren har redan bekräftat sin e-psot.");
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

        public async Task<ResultModel> ChangeEmail(ChangeEmailRequestModel aChangeEmailRequestModel, ClaimsPrincipal aClaimsPrincipal)
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

                if(await mUserManager.Users.AnyAsync(x => x.Email == aChangeEmailRequestModel.NewEmail))
                {
                    sResultModel.Success = false;
                    sResultModel.AddError("Denna e-postadress används redan.");
                    return sResultModel;
                }

                var sToken = await mUserManager.GenerateChangeEmailTokenAsync(sUser, aChangeEmailRequestModel.NewEmail);

                string sLink = mConfiguration["FrontendUrl"] + $"/ChangeEmail?Token={sToken}";

                var sEmailResult = await mEmailService.SendEmail(sUser.Email, "SnusPunch - Bekräfta byte av e-postadress", EmailHelper.GetChangeEmailEmail(sLink));

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

        public async Task<ResultModel> ConfirmChangeEmail(ConfirmChangeEmailRequestModel aConfirmChangeEmailRequestModel, ClaimsPrincipal aClaimsPrincipal)
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

                var sResult = await mUserManager.ChangeEmailAsync(sUser, aConfirmChangeEmailRequestModel.NewEmail, aConfirmChangeEmailRequestModel.Token);

                if(!sResult.Succeeded)
                {
                    sResultModel.Success = false;
                    sResultModel.AppendErrors(sResult.Errors.Select(x => x.Description).ToList());
                }

                //Ta bort verifieringen
                sUser.EmailConfirmed = false;
                await mUserManager.UpdateAsync(sUser);
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
                }
            }
            catch (Exception ex)
            {
                sResultModel.Success = false;
                sResultModel.AddExceptionError(ex);
            }

            return sResultModel;
        }

        public async Task<ResultModel> ChangePassword(ChangePasswordRequestModel aChangePasswordRequestModel, ClaimsPrincipal aClaimsPrincipal)
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

                var sResult = await mUserManager.ChangePasswordAsync(sUser, aChangePasswordRequestModel.CurrentPassword, aChangePasswordRequestModel.NewPassword);

                if(!sResult.Succeeded)
                {
                    sResultModel.Success = false;
                    sResultModel.AppendErrors(sResult.Errors.Select(x => x.Description).ToList());
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


        #region ProfilePicture
        public async Task<ResultModel> AddOrUpdateProfilePicture(IFormFile aFormFile, ClaimsPrincipal aClaimsPrincipal)
        {
            ResultModel sResultModel = new ResultModel();

            try
            {
                //Hämta ut användare
                var sUser = await mUserManager.GetUserAsync(aClaimsPrincipal);

                if(sUser == null)
                {
                    sResultModel.AddError("Användaren hittades ej");
                    sResultModel.Success = false;
                    return sResultModel;
                }

                var sCurrentProfilePicture = sUser.ProfilePicturePath;

                //Verifiera filen
                var sVerifyFileResult = ImageFileVerification.IsValidImage(aFormFile);
                if(!sVerifyFileResult.Success)
                {
                    sResultModel = sVerifyFileResult;
                    return sResultModel;
                }

                //Fil OK, generera en Guid för filnamnet (för att undvika att användare gissar sig till en annans profilbild)
                Guid sFileName = Guid.NewGuid();

                string sFilePath = mConfiguration["ProfilePicturePath"] +  $"{sFileName}.jpg";

                //Konvertera filen till jpg & spara på disk
                using(var sStream = new MemoryStream())
                {
                    await aFormFile.CopyToAsync(sStream);

                    using(var sImage = System.Drawing.Image.FromStream(sStream))
                    {
                        sImage.Save(sFilePath, System.Drawing.Imaging.ImageFormat.Jpeg);
                    }
                }

                //Sätt path på user
                sUser.ProfilePicturePath = sFileName + ".jpg";

                var sResult = await mUserManager.UpdateAsync(sUser);

                if(!sResult.Succeeded)
                {
                    sResultModel.AppendErrors(sResult.Errors.Select(x => x.Description).ToList());
                    sResultModel.Success = false;
                    return sResultModel;
                }

                //Om användaren hade en profilbild sedan innan, radera den gamla.
                if(sCurrentProfilePicture != null)
                {
                    string sFile = mConfiguration["ProfilePicturePath"] + $"/{sCurrentProfilePicture}";

                    if(File.Exists(sFile))
                    {
                        File.Delete(sFile);
                    }
                }
            }
            catch (Exception ex)
            {
                sResultModel.Success = false;
                sResultModel.AddExceptionError(ex);
            }

            return sResultModel;
        }

        public async Task<ResultModel> DeleteProfilePicture(ClaimsPrincipal aClaimsPrincipal)
        {
            ResultModel sResultModel = new ResultModel();

            try
            {
                //Hämta ut användare
                var sUser = await mUserManager.GetUserAsync(aClaimsPrincipal);

                if (sUser == null)
                {
                    sResultModel.AddError("Användaren hittades ej");
                    sResultModel.Success = false;
                    return sResultModel;
                }

                return await DeleteProfilePicture(sUser);
            }
            catch (Exception ex)
            {
                sResultModel.Success = false;
                sResultModel.AddExceptionError(ex);
            }

            return sResultModel;
        }

        public async Task<ResultModel> DeleteProfilePicture(string aUserName)
        {
            ResultModel sResultModel = new ResultModel();

            try
            {
                //Hämta ut användare
                var sUser = await mUserManager.Users.FirstOrDefaultAsync(x => x.UserName == aUserName);

                if (sUser == null)
                {
                    sResultModel.AddError("Användaren hittades ej");
                    sResultModel.Success = false;
                    return sResultModel;
                }

                return await DeleteProfilePicture(sUser);
            }
            catch (Exception ex)
            {
                sResultModel.Success = false;
                sResultModel.AddExceptionError(ex);
            }

            return sResultModel;
        }

        private async Task<ResultModel> DeleteProfilePicture(SnusPunchUserModel aSnusPunchUserModel)
        {
            ResultModel sResultModel = new ResultModel();

            try
            {
                string sProfilePicturePath = aSnusPunchUserModel.ProfilePicturePath;

                if (aSnusPunchUserModel.ProfilePicturePath == null)
                {
                    sResultModel.AddError("Användaren har ingen profilbild");
                    sResultModel.Success = false;
                    return sResultModel;
                }

                aSnusPunchUserModel.ProfilePicturePath = null;
                var sUpdateResult = await mUserManager.UpdateAsync(aSnusPunchUserModel);

                if(!sUpdateResult.Succeeded)
                {
                    sResultModel.AppendErrors(sUpdateResult.Errors.Select(x => x.Description).ToList());
                    sResultModel.Success = false;
                    return sResultModel;
                }

                var sFilePath = mConfiguration["ProfilePicturePath"] + sProfilePicturePath;
                if (File.Exists(sFilePath))
                {
                    File.Delete(sFilePath);
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
