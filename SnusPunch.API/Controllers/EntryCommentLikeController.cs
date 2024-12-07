using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SnusPunch.Services.Entry;
using SnusPunch.Shared.Models.Entry.Likes;
using SnusPunch.Shared.Models.Pagination;
using SnusPunch.Shared.Models.ResultModel;

namespace SnusPunch.API.Controllers
{
    [ApiController]
    [Authorize]
    [Route("[controller]")]
    public class EntryCommentLikeController : ControllerBase
    {
        private readonly ILogger<EntryCommentLikeController> mLogger;
        private readonly EntryCommentLikeService mEntryCommentLikeService;

        public EntryCommentLikeController(ILogger<EntryCommentLikeController> aLogger, EntryCommentLikeService aEntryCommentLikeService)
        {
            mLogger = aLogger;
            mEntryCommentLikeService = aEntryCommentLikeService;
        }

        [HttpPost("LikeComment/{aEntryCommentModelId}")]
        public async Task<ResultModel> LikeComment(int aEntryCommentModelId)
        {
            return await mEntryCommentLikeService.LikeComment(aEntryCommentModelId, User);
        }

        [HttpDelete("UnlikeComment/{aEntryCommentModelId}")]
        public async Task<ResultModel> UnlikeComment(int aEntryCommentModelId)
        {
            return await mEntryCommentLikeService.UnlikeComment(aEntryCommentModelId, User);
        }

        [HttpPost("GetCommentLikesPaginated/{aEntryCommentModelId}")]
        public async Task<ResultModel<PaginationResponse<EntryLikeDto>>> GetCommentLikesPaginated(PaginationParameters aPaginationParameters, int aEntryCommentModelId)
        {
            return await mEntryCommentLikeService.GetCommentLikesPaginated(aPaginationParameters, aEntryCommentModelId);
        }
    }
}
