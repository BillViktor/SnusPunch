namespace SnusPunch.Shared.Models.Auth
{
    public class UpdatePrivacySettingsRequestModel
    {
        public PrivacySettingEnum FriendPrivacySetting { get; set; }
        public PrivacySettingEnum EntryPrivacySetting { get; set; }
    }
}
