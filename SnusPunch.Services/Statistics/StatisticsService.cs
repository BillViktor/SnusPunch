using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using SnusPunch.Data.Models.Identity;
using SnusPunch.Data.Repository;
using SnusPunch.Services.Entry;
using SnusPunch.Shared.Models.ResultModel;
using SnusPunch.Shared.Models.Snus;
using SnusPunch.Shared.Models.Statistics;
using System.Security.Claims;

namespace SnusPunch.Services.Statistics
{
    public class StatisticsService
    {
        private readonly ILogger<EntryService> mLogger;
        private readonly SnusPunchRepository mSnusPunchRepository;
        private readonly UserManager<SnusPunchUserModel> mUserManager;

        public StatisticsService(ILogger<EntryService> aLogger, SnusPunchRepository aSnusPunchRepository, UserManager<SnusPunchUserModel> aUserManager)
        {
            mLogger = aLogger;
            mSnusPunchRepository = aSnusPunchRepository;
            mUserManager = aUserManager;
        }

        public async Task<ResultModel<StatisticsTimePeriodResponseDto>> GetStatisticsForTimePeriod(StatisticsTimePeriodRequestDto aStatisticsTimePeriodRequestDto, ClaimsPrincipal aClaimsPrincipal)
        {
            ResultModel<StatisticsTimePeriodResponseDto> sResultModel = new ResultModel<StatisticsTimePeriodResponseDto>();

            try
            {
                if(aStatisticsTimePeriodRequestDto.StartDate >= aStatisticsTimePeriodRequestDto.EndDate)
                {
                    sResultModel.Success = false;
                    sResultModel.AddError("Startdatum kan inte vara större än slutdatum.");
                    return sResultModel;
                }

                var sUser = await mUserManager.GetUserAsync(aClaimsPrincipal);

                if (sUser == null)
                {
                    sResultModel.AddError("Användaren hittades ej");
                    sResultModel.Success = false;
                    return sResultModel;
                }

                sResultModel.ResultObject = await mSnusPunchRepository.GetStatisticsForTimePeriod(aStatisticsTimePeriodRequestDto.StartDate, aStatisticsTimePeriodRequestDto.EndDate, sUser.Id);
            }
            catch (Exception aException)
            {
                mLogger.LogError(aException, "Exception at GetStatisticsForTimePeriod in StatisticsService");
                sResultModel.Success = false;
                sResultModel.AddExceptionError(aException);
            }

            return sResultModel;
        }
    }
}
