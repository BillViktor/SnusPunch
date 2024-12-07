using SnusPunch.Data.Models.Identity;
using SnusPunch.Shared.Models;

namespace SnusPunch.Data.Models.Entry
{
    public class EntryCommentLikeModel : TableProperties
    {
        public int Id { get; set; }

        //Entry
        public int EntryCommentId { get; set; }
        public EntryCommentModel EntryCommentModel { get; set; }

        //User
        public string? SnusPunchUserModelId { get; set; }
        public SnusPunchUserModel? SnusPunchUserModel { get; set; }
    }
}
