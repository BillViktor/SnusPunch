namespace SnusPunch.Shared.Models.Auth
{
    public enum FriendshipStatusEnum
    {
        None,
        Received, //User has received a FQ from the user in question
        Pending, //User has a pending FQ to the user 
        Rejected, //User has been rejected 
        Friends
    }
}
