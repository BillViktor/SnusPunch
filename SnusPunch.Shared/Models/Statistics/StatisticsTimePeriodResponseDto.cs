namespace SnusPunch.Shared.Models.Statistics
{
    public class StatisticsTimePeriodResponseDto
    {
        public int SnusCount { get; set; }
        public decimal TotalCostInSek { get; set; }
        public double TotalNicotineInMg { get; set; }
        public double AvgSnusCountPerDay { get; set; }
        public decimal AvgCostPerDayInSek { get; set; }
        public double AvgNicotinePerDayInMg { get; set; }
        public string? MostUsedSnus { get; set; }
        public int? MostUsedSnusCount { get; set; }
    }
}
