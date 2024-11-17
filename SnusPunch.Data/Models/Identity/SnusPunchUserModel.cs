using Microsoft.AspNetCore.Identity;
using SnusPunch.Shared.Models.Snus;

namespace SnusPunch.Data.Models.Identity
{
    public class SnusPunchUserModel : IdentityUser
    {
        public int? FavoriteSnusId { get; set; }
        public SnusModel FavoriteSnus { get; set; }
    }
}
