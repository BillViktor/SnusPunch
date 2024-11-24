namespace SnusPunch.Shared.Models.Entry
{
    public class EntryDto
    {
        public int Id { get; set; }
        public string? PhotoUrl { get; set; }
        public string? Description { get; set; }
        public string SnusName { get; set; }
        public string UserName { get; set; }
        public string? UserProfilePictureUrl { get; set; }
        public int Likes { get; set; }
        public int Comments { get; set; }
        public DateTime CreateDate { get; set; } 
        public bool LikedByUser { get; set; }

        public string GetTimeAgo()
        {
            var sTimeDifference = DateTime.Now - CreateDate;

            if (sTimeDifference.TotalMinutes < 1)
            {
                return "Alldeles nyss";
            }
            else if (sTimeDifference.TotalMinutes < 60)
            {
                var sMinutes = (int)sTimeDifference.TotalMinutes;
                return $"{sMinutes} {(sMinutes == 1 ? "minut" : "minuter")} sedan";
            }
            else if (sTimeDifference.TotalHours < 24)
            {
                var sHours = (int)sTimeDifference.TotalHours;
                return $"{sHours} {(sHours == 1 ? "timme" : "timmar")} sedan";
            }
            else if (sTimeDifference.TotalDays < 30)
            {
                var sDays = (int)sTimeDifference.TotalDays;
                return $"{sDays} {(sDays == 1 ? "dag" : "dagar")} sedan";
            }
            else if (sTimeDifference.TotalDays < 365)
            {
                var sMonths = (int)(sTimeDifference.TotalDays / 30);
                return $"{sMonths} {(sMonths == 1 ? "månad" : "månader")} sedan";
            }
            else
            {
                var sYears = (int)(sTimeDifference.TotalDays / 365);
                return $"{sYears} {(sYears == 1 ? "år" : "år")} sedan";
            }
        }

        public string GetLikesString()
        {
            return Likes == 1 ? "1 like" : $"{Likes} likes";
        }

        public string GetCommentsString()
        {
            return Comments == 1 ? "1 kommentar" : $"{Comments} kommentarer";
        }
    }
}
