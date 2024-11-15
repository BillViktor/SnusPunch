using Microsoft.AspNetCore.Mvc;
using SnusPunch.Services.Email;
using SnusPunch.Services.Snus;
using SnusPunch.Shared.Models.Auth;
using SnusPunch.Shared.Models.ResultModel;

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
        public async Task<ResultModel> Register(RegisterModel aRegisterModel)
        {
            return await mAuthService.Register(aRegisterModel);
        }
    }
}
