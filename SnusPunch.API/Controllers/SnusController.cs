using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SnusPunch.Services.Snus;
using SnusPunch.Shared.Models.Pagination;
using SnusPunch.Shared.Models.ResultModel;
using SnusPunch.Shared.Models.Snus;
using System.Security.Claims;

namespace SnusPunch.API.Controllers
{
    [ApiController]
    [Authorize]
    [Route("[controller]")]
    public class SnusController : ControllerBase
    {
        private readonly ILogger<SnusController> mLogger;
        private readonly SnusService mSnusService;

        public SnusController(ILogger<SnusController> aLogger, SnusService aSnusService)
        {
            mLogger = aLogger;
            mSnusService = aSnusService;
        }

        [Authorize(Roles = "Admin")]
        [HttpPost("AddSnus")]
        public async Task<ResultModel<SnusModel>> AddSnus(SnusModel aSnusModel)
        {
            return await mSnusService.AddSnus(aSnusModel);
        }

        [HttpGet("GetSnusDto")]
        public async Task<ResultModel<List<SnusDto>>> GetSnusDto()
        {
            return await mSnusService.GetSnusDto();
        }

        [HttpGet("GetSnusById/{aSnusModelId}")]
        public async Task<ResultModel<SnusModel>> GetSnusById(int aSnusModelId)
        {
            return await mSnusService.GetSnusById(aSnusModelId);
        }

        [HttpPost("GetSnusPaginated")]
        public async Task<ResultModel<PaginationResponse<SnusModel>>> GetSnusPaginated(PaginationParameters aPaginationParameters)
        {
            return await mSnusService.GetSnusPaginated(aPaginationParameters);
        }

        [Authorize(Roles = "Admin")]
        [HttpPut("UpdateSnus")]
        public async Task<ResultModel<SnusModel>> UpdateSnus(SnusModel aSnusModel)
        {
            return await mSnusService.UpdateSnus(aSnusModel);
        }

        [Authorize(Roles = "Admin")]
        [HttpDelete("RemoveSnus/{aSnusModelId}")]
        public async Task<ResultModel> RemoveSnus(int aSnusModelId)
        {
            return await mSnusService.RemoveSnus(aSnusModelId);
        }

        [HttpPut("SetFavouriteSnus/{aSnusModelId}")]
        public async Task<ResultModel> SetFavouriteSnus(int aSnusModelId)
        {
            return await mSnusService.SetFavouriteSnus(aSnusModelId, User);
        }
    }
}
