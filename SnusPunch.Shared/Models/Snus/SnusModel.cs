using System.ComponentModel.DataAnnotations;

namespace SnusPunch.Shared.Models.Snus
{
    public class SnusModel : TableProperties
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Namn är obligatoriskt!")]
        public string Name { get; set; }

        public int PortionCount { get; set; }

        [Required(ErrorMessage = "Pris är obligatoriskt!"), Range(0, double.MaxValue, ErrorMessage = "Priset kan inte vara negativt!")]
        public decimal PriceInSek { get; set; }

        public decimal PricePerPortion { get; set; }

        public double WeightInGrams { get; set; }

        public double NicotineInMgPerGram { get; set; }

        public double NicotinePerPortion { get; set; }


        #region Constructors
        //Default
        public SnusModel() { }

        //Copy Constructor
        public SnusModel(SnusModel aSnusModel)
        {
            Id = aSnusModel.Id;
            Name = aSnusModel.Name;
            PortionCount = aSnusModel.PortionCount;
            PriceInSek = aSnusModel.PriceInSek;
            PricePerPortion = aSnusModel.PricePerPortion;
            WeightInGrams = aSnusModel.WeightInGrams;
            NicotineInMgPerGram = aSnusModel.NicotineInMgPerGram;
            NicotinePerPortion = aSnusModel.NicotinePerPortion;
        }
        #endregion
    }
}
