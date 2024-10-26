namespace SnusPunch.Shared.Models.Snus
{
    public class SnusModel : TableProperties
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int PortionCount { get; set; }
        public decimal PriceInSek { get; set; }
        public double WeightInGrams { get; set; }
        public double NicotineInMgPerGram { get; set; }

        public decimal PricePerPortion
        {
            get
            {
                return PortionCount != 0 ? PriceInSek / PortionCount : -1;
            }
        }

        public double NicotinePerPortion
        {
            get
            {
                return PortionCount != 0 ? NicotineInMgPerGram * WeightInGrams / PortionCount : -1;
            }
        }
    }
}
