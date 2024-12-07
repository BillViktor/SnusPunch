using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using SnusPunch.Data.Models.Entry;
using SnusPunch.Data.Repository;
using SnusPunch.Services.Entry;
using SnusPunch.Shared.Models.Entry;
using SnusPunch.Shared.Models.Pagination;
using SnusPunch.Shared.Models.ResultModel;
using System.Security.Claims;

namespace SnusPunch.API.Controllers
{
    [ApiController]
    [Authorize]
    [Route("[controller]")]
    public class EntryCommentController : ControllerBase
    {
        private readonly ILogger<EntryCommentController> mLogger;
        private readonly EntryCommentService mEntryCommentService;

        public EntryCommentController(ILogger<EntryCommentController> aLogger, EntryCommentService aEntryCommentService)
        {
            mLogger = aLogger;
            mEntryCommentService = aEntryCommentService;
        }

        [HttpPost("GetEntryCommentsPaginated/{aEntryModelId}")]
        public async Task<ResultModel<PaginationResponse<EntryCommentDto>>> GetEntryCommentsPaginated(PaginationParameters aPaginationParameters, int aEntryModelId)
        {
            return await mEntryCommentService.GetEntryCommentsPaginated(aPaginationParameters, aEntryModelId, User);
        }

        [HttpPost("GetEntryCommentRepliesPaginated/{aEntryCommentModelId}")]
        public async Task<ResultModel<PaginationResponse<EntryCommentDto>>> GetEntryCommentRepliesPaginated(PaginationParameters aPaginationParameters, int aEntryCommentModelId)
        {
            return await mEntryCommentService.GetEntryCommentRepliesPaginated(aPaginationParameters, aEntryCommentModelId, User);
        }

        [HttpPost("AddEntryComment")]
        public async Task<ResultModel<EntryCommentDto>> AddEntryComment(AddEntryCommentDto aAddEntryCommentDto)
        {
            return await mEntryCommentService.AddEntryComment(aAddEntryCommentDto, User);
        }

        [Authorize(Roles = "Admin")]
        [HttpDelete("AdminRemoveEntryComment/{aEntryCommentModelId}")]
        public async Task<ResultModel> AdminRemoveEntryComment(int aEntryCommentModelId)
        {
            return await mEntryCommentService.RemoveEntryComment(aEntryCommentModelId);
        }

        [HttpDelete("RemoveEntryComment/{aEntryCommentModelId}")]
        public async Task<ResultModel> RemoveEntryComment(int aEntryCommentModelId)
        {
            return await mEntryCommentService.RemoveEntryComment(aEntryCommentModelId, User);
        }
    }
}
