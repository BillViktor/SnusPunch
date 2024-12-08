namespace SnusPunch.Shared.Models.Auth
{
    public class SnusPunchUserDto
    {
        public string? ProfilePictureUrl { get; set; }
        public string UserName { get; set; }
        public string? FavouriteSnus { get; set; }
        public int SnusPunches { get; set; }
        public int Friends { get; set; }
    }
}
