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
        public async Task<ResultModel<string>> Info()
        {
            return await mAuthService.Info(User);
        }

        [Authorize]
        [HttpGet("Roles")]
        public async Task<ResultModel<List<RoleClaimModel>>> Roles()
        {
            return await mAuthService.Roles(User);
        }

        [HttpPost("VerifyEmail")]
        public async Task<ResultModel> VerifyEmail(VerifyEmailRequest VerifyEmailRequest)
        {
            return await mAuthService.VerifyEmail(VerifyEmailRequest.UserId, VerifyEmailRequest.Token);
        }

        [HttpPost("ResendConfirmationEmail")]
        public async Task<ResultModel> ResendConfirmationEmail(ResendVerificationRequestModel aResendVerificationRequest)
        {
            return await mAuthService.ResendVerificationEmail(aResendVerificationRequest.UserId);
        }

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
    }
}
