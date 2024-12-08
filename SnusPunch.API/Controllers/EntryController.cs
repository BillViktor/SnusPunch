using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SnusPunch.Services.Entry;
using SnusPunch.Shared.Models.Entry;
using SnusPunch.Shared.Models.Pagination;
using SnusPunch.Shared.Models.ResultModel;

namespace SnusPunch.API.Controllers
{
    [ApiController]
    [Authorize]
    [Route("[controller]")]
    public class EntryController : ControllerBase
    {
        private readonly ILogger<EntryController> mLogger;
        private readonly EntryService mEntryService;

        public EntryController(ILogger<EntryController> aLogger, EntryService aEntryService)
        {
            mLogger = aLogger;
            mEntryService = aEntryService;
        }

        [HttpPost("GetEntriesPaginated/{aFetchEmptyPunches}/{aEntryFilterEnum}")]
        public async Task<ResultModel<PaginationResponse<EntryDto>>> GetEntriesPaginated(PaginationParameters aPaginationParameters, bool aFetchEmptyPunches, EntryFilterEnum aEntryFilterEnum)
        {
            return await mEntryService.GetEntriesPaginated(aPaginationParameters, aFetchEmptyPunches, aEntryFilterEnum, User);
        }

        [HttpPost("GetPhotoEntriesForUser/{aUserName}")]
        public async Task<ResultModel<PaginationResponse<EntryDto>>> GetPhotoEntriesForUser(PaginationParameters aPaginationParameters, string aUserName)
        {
            return await mEntryService.GetPhotoEntriesForUser(aPaginationParameters, aUserName, User);
        }

        [HttpGet("GetEntryById/{aEntryId}")]
        public async Task<ResultModel<EntryDto>> GetEntryById(int aEntryId)
        {
            return await mEntryService.GetEntryById(aEntryId, User);
        }

        [HttpPost("AddEntry")]
        public async Task<ResultModel<EntryDto>> AddEntry(AddEntryDto aAddEntryDto)
        {
            return await mEntryService.AddEntry(new AddEntryWithImageDto { FormFile = null, Description = aAddEntryDto.Description, SnusId = aAddEntryDto.SnusId}, User);
        }

        [HttpPost("AddEntryWithImage")]
        public async Task<ResultModel<EntryDto>> AddEntryWithImage(AddEntryWithImageDto aAddEntryDto)
        {
            return await mEntryService.AddEntry(aAddEntryDto, User);
        }

        [HttpDelete("RemoveEntry/{aEntryModelId}")]
        public async Task<ResultModel> RemoveEntry(int aEntryModelId)
        {
            return await mEntryService.RemoveEntry(aEntryModelId, User);
        }

        [Authorize(Roles = "Admin")]
        [HttpDelete("AdminRemoveEntry/{aEntryModelId}")]
        public async Task<ResultModel> AdminRemoveEntry(int aEntryModelId)
        {
            return await mEntryService.RemoveEntry(aEntryModelId);
        }
    }
}
