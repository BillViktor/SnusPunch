using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SnusPunch.Services.Entry;
using SnusPunch.Services.Snus;
using SnusPunch.Shared.Models.Entry;
using SnusPunch.Shared.Models.Pagination;
using SnusPunch.Shared.Models.ResultModel;
using SnusPunch.Shared.Models.Snus;
using System.Security.Claims;

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

        [HttpPost("GetEntriesPaginated")]
        public async Task<ResultModel<PaginationResponse<EntryDto>>> GetEntriesPaginated(PaginationParameters aPaginationParameters)
        {
            return await mEntryService.GetEntriesPaginated(aPaginationParameters);
        }

        [HttpPost("AddEntry")]
        public async Task<ResultModel<EntryDto>> AddEntry(AddEntryDto aAddEntryDto)
        {
            return await mEntryService.AddEntry(aAddEntryDto, User);
        }
    }
}