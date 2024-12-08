namespace SnusPunch.Shared.Models.Auth
{
    public enum PrivacySettingEnum
    {
        Private,
        Friends,
        All
    }

    public static class PrivacySettingHelper
    {
        public static string ToString(PrivacySettingEnum aPrivacySettingEnum)
        {
            if(aPrivacySettingEnum == PrivacySettingEnum.Private)
            {
                return "Privat";
            }
            else if (aPrivacySettingEnum == PrivacySettingEnum.Friends)
            {
                return "Vänner";
            }
            else if (aPrivacySettingEnum == PrivacySettingEnum.All)
            {
                return "Alla";
            }
            else
            {
                return "Odefinierad";
            }
        }
    }
}
