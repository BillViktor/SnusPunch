namespace SnusPunch.Shared.Models.Snus
{
    public class SnusModel : TableProperties
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int PortionCount { get; set; }
        public decimal? PriceInSek { get; set; }
        public double? WeightInGrams { get; set; }
        public double? NicotineInMgPerGram { get; set; }
    }
}
