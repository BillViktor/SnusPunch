using SnusPunch.Shared.Models.Entry;

namespace SnusPunch.Shared.Models.Auth
{
    public class SnusPunchUserProfileDto
    {
        public string UserName { get; set; }
        public string? ProfilePictureUrl { get; set; }

        //Snus
        public string? FavouriteSnus { get; set; }
        public int EntryCount { get; set; }
        public List<EntryDto> Entries { get; set; } = new List<EntryDto>();

        //Friends
        public int FriendsCount { get; set; }
        public List<SnusPunchUserDto> Friends { get; set; } = new List<SnusPunchUserDto>();

        //Photo Entries
        public List<EntryDto> PhotoEntries { get; set; } = new List<EntryDto>();

        public FriendshipStatusEnum FriendshipStatusEnum { get; set; } = FriendshipStatusEnum.None;
    }
}
