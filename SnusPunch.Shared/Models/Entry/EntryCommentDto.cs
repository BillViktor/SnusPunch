using SnusPunch.Shared.Models.Pagination;

namespace SnusPunch.Shared.Models.Entry
{
    public class EntryCommentDto
    {
        public int Id { get; set; }
        public int? ParentId { get; set; }
        public string Comment { get; set; }
        public string UserName { get; set; }
        public string ProfilePictureUrl { get; set; }
        public bool LikedByUser { get; set; }
        public int Likes { get; set; }
        public int ReplyCount { get; set; } 
        public DateTime CreateDate { get; set; }

        //Replies
        public PaginationParameters ReplyPaginationParemeters { get; set; } = new PaginationParameters
        {
            SortPropertyName = "CreateDate",
            SortOrder = SortOrderEnum.Ascending,
            PageSize = 5
        };
        public PaginationMetaData ReplyPaginationMetaData { get; set; } = null;
        public List<EntryCommentDto> Replies { get; set; } = new List<EntryCommentDto>();

        //Reply
        public bool ShowReplyInput { get; set; } = false;
        public string ReplyInput { get; set; }


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

        public string GetLikesString()
        {
            return Likes == 1 ? "1 like" : $"{Likes} likes";
        }

        public string GetCommentsString()
        {
            return $"{ReplyCount} svar";
        }
    }
}
