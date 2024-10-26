using Microsoft.AspNetCore.Mvc;
using SnusPunch.Data.Repository;
using SnusPunch.Services.Snus;
using SnusPunch.Shared.Models.ResultModel;
using SnusPunch.Shared.Models.Snus;

namespace SnusPunch.API.Controllers
{
    [ApiController]
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

        [HttpPost("AddSnus")]
        public async Task<ResultModel<SnusModel>> AddSnus(SnusModel aSnusModel)
        {
            return await mSnusService.AddSnus(aSnusModel);
        }

        [HttpGet("GetSnus")]
        public async Task<ResultModel<List<SnusModel>>> GetSnus()
        {
            return await mSnusService.GetSnus();
        }

        [HttpPut("UpdateSnus")]
        public async Task<ResultModel<SnusModel>> UpdateSnus(SnusModel aSnusModel)
        {
            return await mSnusService.UpdateSnus(aSnusModel);
        }

        [HttpDelete("RemoveSnus/{aSnusModelId}")]
        public async Task<ResultModel> RemoveSnus(int aSnusModelId)
        {
            return await mSnusService.RemoveSnus(aSnusModelId);
        }
    }
}