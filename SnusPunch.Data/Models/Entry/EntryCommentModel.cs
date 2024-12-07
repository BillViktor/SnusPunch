using SnusPunch.Data.Models.Identity;
using SnusPunch.Shared.Models;

namespace SnusPunch.Data.Models.Entry
{
    public class EntryCommentModel : TableProperties
    {
        public int Id { get; set; }

        public string Comment { get; set; }

        //Entry
        public int EntryId { get; set; }
        public EntryModel EntryModel { get; set; }

        //User
        public string? SnusPunchUserModelId { get; set; }
        public SnusPunchUserModel? SnusPunchUserModel { get; set; }

        // Parent comment (for nested replies)
        public int? ParentCommentId { get; set; }
        public EntryCommentModel? ParentComment { get; set; }

        //Replies
        public List<EntryCommentModel> Replies { get; set; } = new List<EntryCommentModel>();

        //Likes
        public List<EntryCommentLikeModel> CommentLikes { get; set; } = new List<EntryCommentLikeModel>();
    }
}
