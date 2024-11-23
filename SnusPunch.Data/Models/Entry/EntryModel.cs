using SnusPunch.Data.Models.Identity;
using SnusPunch.Shared.Models;
using SnusPunch.Shared.Models.Snus;

namespace SnusPunch.Data.Models.Entry
{
    public class EntryModel : TableProperties
    {
        public int Id { get; set; }
        public string? Description { get; set; }
        public string? PhotoUrl { get; set; }

        //User
        public string SnusPunchUserModelId { get; set; }
        public SnusPunchUserModel SnusPunchUserModel { get; set; }

        //Snus
        public int? SnusId { get; set; }
        public SnusModel? Snus { get; set; }

        //Likes
        public List<EntryLikeModel> Likes { get; set; } = new List<EntryLikeModel>();

        //Kommentarer
        public List<EntryCommentModel> Comments { get; set; } = new List<EntryCommentModel>();
    }
}
