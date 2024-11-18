using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SnusPunch.Services.Snus;
using SnusPunch.Shared.Models.Auth;
using SnusPunch.Shared.Models.Pagination;
using SnusPunch.Shared.Models.ResultModel;
using SnusPunch.Shared.Models.Snus;

namespace SnusPunch.API.Controllers
{
    [ApiController]
    [Authorize]
    [Route("[controller]")]
    public class UserController : ControllerBase
    {
        private readonly ILogger<UserController> mLogger;
        private readonly UserService mUserService;

        public UserController(ILogger<UserController> aLogger, UserService aUserService)
        {
            mLogger = aLogger;
            mUserService = aUserService;
        }

        [Authorize]
        [HttpPost("GetUsersPaginated")]
        public async Task<ResultModel<PaginationResponse<SnusPunchUserDto>>> AddSnus(PaginationParameters aPaginationParameters)
        {
            return await mUserService.GetUsersPaginated(aPaginationParameters);
        }
    }
}
