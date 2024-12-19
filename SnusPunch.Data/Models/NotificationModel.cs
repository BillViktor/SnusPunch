using SnusPunch.Data.Models.Entry;
using SnusPunch.Data.Models.Identity;
using SnusPunch.Shared.Models;
using SnusPunch.Shared.Models.Notification;

namespace SnusPunch.Data.Models
{
    public class NotificationModel : TableProperties
    {
        public int Id { get; set; }

        //The user that "sent" the notification
        public string SnusPunchUserModelIdOne { get; set; }
        public SnusPunchUserModel SnusPunchUserModelOne { get; set; }

        //The user that received the notification
        public string SnusPunchUserModelIdTwo { get; set; }
        public SnusPunchUserModel SnusPunchUserModelTwo { get; set; }

        //Action
        public NotificationActionEnum NotificationActionEnum {  get; set; }

        //Has the user seen the notification?
        public bool NotificationViewed { get; set; }

        //Related entry
        public int EntityId { get; set; }

        public override string ToString()
        {
            if(NotificationActionEnum == NotificationActionEnum.EntryLike)
            {
                return $"{SnusPunchUserModelOne.UserName} har gillat ditt inlägg.";
            }
            else if (NotificationActionEnum == NotificationActionEnum.CommentLike)
            {
                return $"{SnusPunchUserModelOne.UserName} har gillat din kommentar.";
            }
            else if(NotificationActionEnum == NotificationActionEnum.Comment)
            {
                return $"{SnusPunchUserModelOne.UserName} har kommenterat ditt inlägg.";
            }

            return "";
        }
    }
}
