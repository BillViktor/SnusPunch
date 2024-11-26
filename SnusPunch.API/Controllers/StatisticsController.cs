using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SnusPunch.Services.Statistics;
using SnusPunch.Shared.Models.ResultModel;
using SnusPunch.Shared.Models.Statistics;

namespace SnusPunch.API.Controllers
{
    [ApiController]
    [Authorize]
    [Route("[controller]")]
    public class StatisticsController : ControllerBase
    {
        private readonly ILogger<StatisticsController> mLogger;
        private readonly StatisticsService mStatisticsService;

        public StatisticsController(ILogger<StatisticsController> aLogger, StatisticsService aStatisticsService)
        {
            mLogger = aLogger;
            mStatisticsService = aStatisticsService;
        }

        [HttpPost("GetStatisticsForTimePeriod")]
        public async Task<ResultModel<StatisticsTimePeriodResponseDto>> GetStatisticsForTimePeriod(StatisticsTimePeriodRequestDto aStatisticsTimePeriodRequestDto)
        {
            return await mStatisticsService.GetStatisticsForTimePeriod(aStatisticsTimePeriodRequestDto, User);
        }
    }
}
