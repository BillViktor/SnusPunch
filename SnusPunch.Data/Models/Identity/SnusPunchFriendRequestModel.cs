using SnusPunch.Shared.Models;

namespace SnusPunch.Data.Models.Identity
{
    public class SnusPunchFriendRequestModel : TableProperties
    {
        public string SnusPunchUserModelOneId { get; set; }
        public SnusPunchUserModel SnusPunchUserModelOne { get; set; }

        public string SnusPunchUserModelTwoId { get; set; }
        public SnusPunchUserModel SnusPunchUserModelTwo { get; set; }

        public bool Denied { get; set; } = false;
    }
}
