using SnusPunch.Data.Models.Identity;
using SnusPunch.Shared.Models;

namespace SnusPunch.Data.Models.Entry
{
    public class EntryLikeModel : TableProperties
    {
        public int Id { get; set; }

        //Entry
        public int EntryId { get; set; }
        public EntryModel EntryModel { get; set; }

        //User
        public string? SnusPunchUserModelId { get; set; }
        public SnusPunchUserModel? SnusPunchUserModel { get; set; }
    }
}
