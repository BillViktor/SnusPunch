using Blazored.Modal;
using Blazored.Modal.Services;
using Microsoft.AspNetCore.Components;
using SnusPunch.Shared.Models.Snus;
using SnusPunch.Shared.Models.Statistics;
using SnusPunch.Web.Components;
using SnusPunch.Web.ViewModels.Snus;

namespace SnusPunch.Web.Pages.Statistics
{
    public partial class StatisticsPage
    {
        [CascadingParameter] public IModalService Modal { get; set; } = default!;
        [Inject] StatisticsViewModel StatisticsViewModel { get; set; }

        private StatisticsTimePeriodResponseDto mStatisticsTimePeriodResponseDto = null;
        private string mInterval = "Idag";
        private DateTime mStartDate = DateTime.Today;
        private DateTime mEndDate = DateTime.Now;

        protected override async Task OnInitializedAsync()
        {
            mStatisticsTimePeriodResponseDto = await StatisticsViewModel.GetStatisticsForTimePeriod(mStartDate, mEndDate);
        }

        private async Task OnIntervalChanged(ChangeEventArgs aChangeEventArgs)
        {
            string sString = aChangeEventArgs.Value.ToString();

            switch(sString)
            {
                case "Today":
                    mStartDate = DateTime.Today;
                    mEndDate = DateTime.Now;
                    mInterval = "Idag";
                    break;
                case "Last7":
                    mStartDate = DateTime.Today.AddDays(-7);
                    mEndDate = DateTime.Today.AddSeconds(-1);
                    mInterval = "Senaste 7 dagarna";
                    break;
                case "Last30":
                    mStartDate = DateTime.Today.AddDays(-30);
                    mEndDate = DateTime.Today.AddSeconds(-1);
                    mInterval = "Senaste 30 dagarna";
                    break;
                case "ThisWeek":
                    mStartDate = DateTime.Today.AddDays(-(DateTime.Today.DayOfWeek - DayOfWeek.Monday));
                    mEndDate = DateTime.Now;
                    mInterval = "Denna vecka";
                    break;
                case "ThisMonth":
                    mStartDate = new DateTime(DateTime.Today.Year, DateTime.Today.Month, 1);
                    mEndDate = DateTime.Now;
                    mInterval = "Denna månad";
                    break;
                case "Interval":
                default:
                    mInterval = "Eget intervall";
                    break;
            }

            await Update();
        }

        private async Task Update()
        {
            if (mInterval == "Eget intervall")
            {
                var sOptions = new ModalOptions
                {
                    DisableBackgroundCancel = true,
                    Size = ModalSize.Medium,
                    Position = ModalPosition.Middle
                };
                var sModal = Modal.Show<ChooseIntervalComponent>("Välj intervall", sOptions);
                var sResult = await sModal.Result;

                if (!sResult.Cancelled)
                {
                    var sData = sResult.Data as StatisticsTimePeriodRequestDto;

                    mStartDate = sData.StartDate;
                    mEndDate = sData.EndDate;
                }
            }

            mStatisticsTimePeriodResponseDto = await StatisticsViewModel.GetStatisticsForTimePeriod(mStartDate, mEndDate);
        }
    }
}
