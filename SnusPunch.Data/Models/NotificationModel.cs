using SnusPunch.Data.Models.Identity;
using SnusPunch.Shared.Models;

namespace SnusPunch.Data.Models
{
    public class NotificationModel : TableProperties
    {
        public int Id { get; set; }
        
        //Användaren det avser
        public string? SnusPunchUserModelId { get; set; }
        public SnusPunchUserModel? SnusPunchUserModel { get; set; }

        //TODO
        //WIP
    }
}
