namespace SnusPunch.Shared.Models.Auth
{
    public class UserInfoModel
    {
        public string? ProfilePictureUrl { get; set; }
        public string UserName { get; set; } 
        public string Email { get; set; } 
        public bool IsEmailConfirmed { get; set; }
        public List<RoleClaimModel> RoleClaims { get; set; } = new List<RoleClaimModel>();
        public int? FavouriteSnusId { get; set; }
        public string? FavouriteSnusName { get; set; }
        public int FriendRequests { get; set; } = 0;
        public int UnreadNotifications { get; set; } = 0;
        public int UnreadMessages { get; set; } = 0;
        public PrivacySettingEnum FriendPrivacySettingEnum { get; set; }
        public PrivacySettingEnum EntryPrivacySettingEnum { get; set; }
    }
}
