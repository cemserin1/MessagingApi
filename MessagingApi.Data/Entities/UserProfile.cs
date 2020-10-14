using System.Collections.Generic;

namespace MessagingApi.Data.Entities
{
    [BsonCollection("UserProfile")]

    public class UserProfile : Document
    {
        public User User { get; set; }

        public List<string> BlockedUsers { get; set; }

        public Inbox Inbox { get; set; }

        public UserProfile()
        {
            BlockedUsers = new List<string>();
            Inbox = new Inbox();
            User = new User();
        }
    }
}
