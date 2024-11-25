namespace SnusPunch.Shared.Models.Entry
{
    public enum EntryFilterEnum
    {
        All,
        Friends, //Not implemented yet,
        Self
    }

    public static class EntryFilterEnumHelper
    {
        public static string GetEntryFilterEnumName(EntryFilterEnum aEntryFilterEnum)
        {
            switch (aEntryFilterEnum)
            {
                case EntryFilterEnum.All:
                    return "Allas inlägg";
                case EntryFilterEnum.Friends:
                    return "Vänners inlägg";
                case EntryFilterEnum.Self:
                    return "Dina inlägg";
                default:
                    return "Odefinierat";
            }
        }
    }
}
