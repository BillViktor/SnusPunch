namespace SnusPunch.Shared.Models.Entry
{
    public class EntryDto
    {
        public string? PhotoUrl { get; set; }
        public string? Description { get; set; }
        public string SnusName { get; set; }
        public string UserName { get; set; }
        public string? UserProfilePictureUrl { get; set; }
        public DateTime CreateDate { get; set; }

        public string GetTimeAgo()
        {
            var sDifference = DateTime.Now - CreateDate;
            var sDifferenceInMinutes = sDifference.Minutes;
            var sDifferenceInHours = sDifference.Hours;

            if(sDifferenceInMinutes < 0)
            {
                return "Alldeless nyss";
            }
            else if(sDifferenceInMinutes < 60)
            {
                return $"{sDifferenceInMinutes} {(sDifferenceInMinutes == 1 ? "minut" : "minuter")} sedan";
            }
            else if(sDifferenceInHours >= 1)
            {
                return $"{sDifferenceInMinutes} {(sDifferenceInHours == 1 ? "timme" : "timmar")} sedan";
            }
            else if (sDifferenceInHours >= 24)
            {
                return $"{sDifferenceInMinutes} {(sDifferenceInHours == 24 ? "dag" : "dagar")} sedan";
            }

            return "";
        }
    }
}
