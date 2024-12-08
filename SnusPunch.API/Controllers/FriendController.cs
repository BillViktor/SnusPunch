using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SnusPunch.Services.Snus;
using SnusPunch.Shared.Models.Auth;
using SnusPunch.Shared.Models.Pagination;
using SnusPunch.Shared.Models.ResultModel;

namespace SnusPunch.API.Controllers
{
    [ApiController]
    [Authorize]
    [Route("[controller]")]
    public class FriendController : ControllerBase
    {
        private readonly ILogger<FriendController> mLogger;
        private readonly FriendService mFriendService;

        public FriendController(ILogger<FriendController> aLogger, FriendService aFriendService)
        {
            mLogger = aLogger;
            mFriendService = aFriendService;
        }

        [HttpPost("SendFriendRequest/{aUserName}")]
        public async Task<ResultModel> SendFriendRequest(string aUserName)
        {
            return await mFriendService.SendFriendRequest(aUserName, User);
        }

        [HttpDelete("RemoveFriendRequest/{aUserName}")]
        public async Task<ResultModel> RemoveFriendRequest(string aUserName)
        {
            return await mFriendService.RemoveFriendRequest(aUserName, User);
        }

        [HttpPut("DenyFriendRequest/{aUserName}")]
        public async Task<ResultModel> DenyFriendRequest(string aUserName)
        {
            return await mFriendService.DenyFriendRequest(aUserName, User);
        }

        [HttpPut("AcceptFriendRequest/{aUserName}")]
        public async Task<ResultModel> AcceptFriendRequest(string aUserName)
        {
            return await mFriendService.AcceptFriendRequest(aUserName, User);
        }

        [HttpDelete("RemoveFriend/{aUserName}")]
        public async Task<ResultModel> RemoveFriend(string aUserName)
        {
            return await mFriendService.RemoveFriend(aUserName, User);
        }

        [HttpPost("GetFriendsForUser/{aUserName}")]
        public async Task<ResultModel<PaginationResponse<SnusPunchUserDto>>> GetFriendsForUser(PaginationParameters aPaginationParameters, string aUserName)
        {
            return await mFriendService.GetFriendsForUser(aPaginationParameters, aUserName);
        }
    }
}
