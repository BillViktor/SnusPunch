using SnusPunch.Shared.Models;

namespace SnusPunch.Data.Models.Identity
{
    public class SnusPunchFriendModel : TableProperties
    {
        public string SnusPunchUserModelOneId { get; set; }
        public SnusPunchUserModel SnusPunchUserModelOne { get; set; }

        public string SnusPunchUserModelTwoId { get; set; }
        public SnusPunchUserModel SnusPunchUserModelTwo { get; set; }
    }
}
