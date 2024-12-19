namespace SnusPunch.Shared.Models.Notification
{
    public class NotificationDto
    {
        public string UserName { get; set; }
        public string ProfilePictureUrl { get; set; }
        public NotificationActionEnum NotificationType { get; set; }
        public bool NotificationViewed { get; set; }
        public int EntityId { get; set; }
        public DateTime CreateDate { get; set; }

        public override string ToString()
        {
            if(NotificationType == NotificationActionEnum.EntryLike)
            {
                return "har gillat ditt inlägg";
            }
            else if (NotificationType == NotificationActionEnum.CommentLike)
            {
                return "har gillat din kommentar";
            }
            else if (NotificationType == NotificationActionEnum.Comment)
            {
                return "har kommenterat ditt inlägg";
            }

            return "";
        }

        public string GetTimeAgo()
        {
            var sTimeDifference = DateTime.Now - CreateDate;

            if (sTimeDifference.TotalMinutes < 1)
            {
                return "Nu";
            }
            else if (sTimeDifference.TotalMinutes < 60)
            {
                var sMinutes = (int)sTimeDifference.TotalMinutes;
                return $"{sMinutes} min";
            }
            else if (sTimeDifference.TotalHours < 24)
            {
                var sHours = (int)sTimeDifference.TotalHours;
                return $"{sHours} tim";
            }
            else if (sTimeDifference.TotalDays < 30)
            {
                var sDays = (int)sTimeDifference.TotalDays;
                return $"{sDays} d";
            }
            else if (sTimeDifference.TotalDays < 365)
            {
                var sMonths = (int)(sTimeDifference.TotalDays / 30);
                return $"{sMonths} mån";
            }
            else
            {
                var sYears = (int)(sTimeDifference.TotalDays / 365);
                return $"{sYears} år";
            }
        }
    }
}
