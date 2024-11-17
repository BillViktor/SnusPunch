using System.ComponentModel.DataAnnotations;

namespace SnusPunch.Shared.Models.Snus
{
    public class AddSnusDto
    {
        [Required(ErrorMessage = "Namn är obligatoriskt!")]
        public string Name { get; set; }

        public int PortionCount { get; set; }

        [Required(ErrorMessage = "Pris är obligatoriskt!"), Range(0, double.MaxValue, ErrorMessage = "Priset kan inte vara negativt!")]
        public decimal PriceInSek { get; set; }

        public decimal PricePerPortion { get; set; }

        public double WeightInGrams { get; set; }

        public double NicotineInMgPerGram { get; set; }

        public double NicotinePerPortion { get; set; }
    }
}
