using Microsoft.AspNetCore.Authentication.BearerToken;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SnusPunch.Services.Email;
using SnusPunch.Services.Snus;
using SnusPunch.Shared.Models.Auth;
using SnusPunch.Shared.Models.ResultModel;
using System.Security.Claims;

namespace SnusPunch.API.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class AuthController : ControllerBase
    {
        private readonly ILogger<AuthController> mLogger;
        private readonly AuthService mAuthService;

        private readonly EmailService mEmailService;

        public AuthController(ILogger<AuthController> aLogger, AuthService aAuthService, EmailService aEmailService)
        {
            mLogger = aLogger;
            mAuthService = aAuthService;
            mEmailService = aEmailService;
        }

        [HttpPost("Register")]
        public async Task<ResultModel> Register(RegisterRequestModel aRegisterRequest)
        {
            return await mAuthService.Register(aRegisterRequest);
        }

        [HttpPost("Login")]
        public async Task<ResultModel> Login(LoginRequestModel aLoginRequest)
        {
            return await mAuthService.Login(aLoginRequest);
        }

        [Authorize]
        [HttpPost("Logout")]
        public async Task<ResultModel> Logout()
        {
            return await mAuthService.Logout();
        }

        [Authorize]
        [HttpGet("Info")]
        public async Task<ResultModel<UserInfoModel>> Info()
        {
            return await mAuthService.Info(User);
        }

        [Authorize]
        [HttpDelete("Delete")]
        public async Task<ResultModel> Delete()
        {
            return await mAuthService.Delete(User);
        }

        [Authorize(Roles = "Admin")]
        [HttpDelete("DeleteUser/{aUserName}")]
        public async Task<ResultModel> DeleteUser(string aUserName)
        {
            return await mAuthService.DeleteUser(aUserName);
        }

        #region Profile Picture
        [Authorize]
        [HttpPut("AddOrUpdateProfilePicture")]
        public async Task<ResultModel<string>> AddOrUpdateProfilePicture(IFormFile aFormFile)
        {
            return await mAuthService.AddOrUpdateProfilePicture(aFormFile, User);
        }

        [Authorize]
        [HttpDelete("DeleteProfilePicture")]
        public async Task<ResultModel> DeleteProfilePicture()
        {
            return await mAuthService.DeleteProfilePicture(User);
        }

        [Authorize(Roles = "Admin")]
        [HttpDelete("DeleteProfilePicture/{aUserName}")]
        public async Task<ResultModel> DeleteProfilePicture(string aUserName)
        {
            return new ResultModel();
        }
        #endregion

        #region Email
        [HttpPost("VerifyEmail")]
        public async Task<ResultModel> VerifyEmail(VerifyEmailRequest aVerifyEmailRequest)
        {
            return await mAuthService.VerifyEmail(aVerifyEmailRequest);
        }

        [HttpPost("ResendConfirmationEmail")]
        public async Task<ResultModel> ResendConfirmationEmail()
        {
            return await mAuthService.ResendVerificationEmail(User);
        }
        #endregion

        #region Password
        [HttpPost("ForgotPassword")]
        public async Task<ResultModel> ForgotPassword(ForgotPasswordRequestModel aForgotPasswordRequest)
        {
            return await mAuthService.ForgotPassword(aForgotPasswordRequest.Email);
        }

        [HttpPost("ResetPassword")]
        public async Task<ResultModel> ResetPassword(ResetPasswordRequestModel aResetPasswordRequest)
        {
            return await mAuthService.ResetPassword(aResetPasswordRequest);
        }

        [Authorize]
        [HttpPost("ChangePassword")]
        public async Task<ResultModel> ChangePassword(ChangePasswordRequestModel aChangePasswordRequestModel)
        {
            return await mAuthService.ChangePassword(aChangePasswordRequestModel, User);
        }
        #endregion

        #region Roles
        [Authorize(Roles = "Admin")]
        [HttpPost("AddUserToRole")]
        public async Task<ResultModel> AddUserToRole(UserRoleRequestModel aUserRoleRequestModel)
        {
            return await mAuthService.AddUserToRole(aUserRoleRequestModel);
        }

        [Authorize(Roles = "Admin")]
        [HttpPost("RemoveUserFromRole")]
        public async Task<ResultModel> RemoveUserFromRole(UserRoleRequestModel aUserRoleRequestModel)
        {
            return await mAuthService.RemoveUserFromRole(aUserRoleRequestModel);
        }
        #endregion
    }
}
