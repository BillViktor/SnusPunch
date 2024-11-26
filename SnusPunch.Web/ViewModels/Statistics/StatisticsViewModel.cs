using SnusPunch.Shared.Models.Pagination;
using SnusPunch.Shared.Models.Snus;
using SnusPunch.Shared.Models.Statistics;
using SnusPunch.Web.Clients.Snus;

namespace SnusPunch.Web.ViewModels.Snus
{
    public class StatisticsViewModel : BaseViewModel
    {
        private readonly StatisticsClient mStatisticsClient;

        public StatisticsViewModel(StatisticsClient aStatisticsClient)
        {
            mStatisticsClient = aStatisticsClient;
        }

        public async Task<StatisticsTimePeriodResponseDto> GetStatisticsForTimePeriod(DateTime aStartDate, DateTime aEndDate)
        {
            StatisticsTimePeriodResponseDto sStatisticsTimePeriodResponseDto = null;
            IsBusy = true;

            StatisticsTimePeriodRequestDto sStatisticsTimePeriodRequestDto = new StatisticsTimePeriodRequestDto
            {
                StartDate = aStartDate,
                EndDate = aEndDate,
            };
            var sResult = await mStatisticsClient.GetStatisticsForTimePeriod(sStatisticsTimePeriodRequestDto);

            if (!sResult.Success)
            {
                Errors.AddRange(sResult.Errors);
            }
            else
            {
                sStatisticsTimePeriodResponseDto = sResult.ResultObject;
            }

            IsBusy = false;
            return sStatisticsTimePeriodResponseDto;
        }
    }
}
