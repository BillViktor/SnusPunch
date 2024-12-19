using Microsoft.AspNetCore.Identity;
using SnusPunch.Data.Models.Entry;
using SnusPunch.Shared.Models.Auth;
using SnusPunch.Shared.Models.Snus;

namespace SnusPunch.Data.Models.Identity
{
    public class SnusPunchUserModel : IdentityUser
    {
        public int? FavoriteSnusId { get; set; }
        public string? ProfilePicturePath { get; set; }
        public SnusModel FavoriteSnus { get; set; }
        public List<EntryModel> Entries { get; set; } = new List<EntryModel>();

        //Unfortenately these two have to be seperated
        public ICollection<SnusPunchFriendModel> FriendsAddedByUser { get; set; }
        public ICollection<SnusPunchFriendModel> FriendsAddedByOthers { get; set; }

        public PrivacySettingEnum FriendPrivacySetting { get; set; } = PrivacySettingEnum.All;
        public PrivacySettingEnum EntryPrivacySetting { get; set; } = PrivacySettingEnum.All;

        public DateTime CreateDate { get; set; }
        public DateTime? UpdateDate { get; set; }
    }
}
