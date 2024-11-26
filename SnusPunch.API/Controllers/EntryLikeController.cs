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
    public class EntryLikeController : ControllerBase
    {
        private readonly ILogger<EntryLikeController> mLogger;
        private readonly EntryLikeService mEntryLikeService;

        public EntryLikeController(ILogger<EntryLikeController> aLogger, EntryLikeService aEntryLikeService)
        {
            mLogger = aLogger;
            mEntryLikeService = aEntryLikeService;
        }

        [HttpPost("LikeEntry/{aEntryModelId}")]
        public async Task<ResultModel> LikeEntry(int aEntryModelId)
        {
            return await mEntryLikeService.LikeEntry(aEntryModelId, User);
        }

        [HttpDelete("UnlikeEntry/{aEntryModelId}")]
        public async Task<ResultModel> UnlikeEntry(int aEntryModelId)
        {
            return await mEntryLikeService.UnlikeEntry(aEntryModelId, User);
        }

        [HttpPost("GetEntryLikesPaginated/{aEntryModelId}")]
        public async Task<ResultModel<PaginationResponse<EntryLikeDto>>> GetEntryLikesPaginated(PaginationParameters aPaginationParameters, int aEntryModelId)
        {
            return await mEntryLikeService.GetEntryLikesPaginated(aPaginationParameters, aEntryModelId);
        }
    }
}
