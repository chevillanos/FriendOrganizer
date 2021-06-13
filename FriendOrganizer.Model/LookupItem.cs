namespace FriendOrganizer.Model
{
    // Generic class for returning a single entity
    public class LookupItem
    {
        public int Id { get; set; }
        public string DisplayMember { get; set; }
    }

    public class NullLookupItem : LookupItem
    {
        public new int? Id { get; set; }
    }
}
