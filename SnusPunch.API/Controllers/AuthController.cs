using Microsoft.AspNetCore.Mvc;
using SnusPunch.Services.Snus;
using SnusPunch.Shared.Models.Pagination;
using SnusPunch.Shared.Models.ResultModel;
using SnusPunch.Shared.Models.Snus;

namespace SnusPunch.API.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class AuthController : ControllerBase
    {
        private readonly ILogger<AuthController> mLogger;
        private readonly AuthService mAuthService;

        public AuthController(ILogger<AuthController> aLogger, AuthService aAuthService)
        {
            mLogger = aLogger;
            mAuthService = aAuthService;
        }

        [HttpPost("Register")]
        public async Task<ResultModel> Register()
        {
            return new ResultModel();
        }
    }
}
