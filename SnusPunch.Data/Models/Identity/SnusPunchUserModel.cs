using Microsoft.AspNetCore.Identity;
using SnusPunch.Data.Models.Entry;
using SnusPunch.Shared.Models;
using SnusPunch.Shared.Models.Snus;

namespace SnusPunch.Data.Models.Identity
{
    public class SnusPunchUserModel : IdentityUser
    {
        public int? FavoriteSnusId { get; set; }
        public string? ProfilePicturePath { get; set; }
        public SnusModel FavoriteSnus { get; set; }
        public virtual ICollection<IdentityUserRole<string>> UserRoles { get; set; } = new List<IdentityUserRole<string>>();
        public List<EntryModel> Entries { get; set; } = new List<EntryModel>();
        public List<SnusPunchFriendModel> FriendsAddedByUser { get; set; } = new List<SnusPunchFriendModel>();
        public List<SnusPunchFriendModel> FriendsAddedByOthers { get; set; } = new List<SnusPunchFriendModel>();
        public List<SnusPunchFriendModel> Friends 
        {
            get
            {
                var sList = FriendsAddedByUser;
                sList.AddRange(FriendsAddedByOthers);
                return sList;
            }
        } 
        public DateTime CreateDate { get; set; }
        public DateTime? UpdateDate { get; set; }
    }
}
