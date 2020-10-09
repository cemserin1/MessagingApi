using System.Collections.Generic;

namespace MessagingApi.Data.Entities
{
    [BsonCollection("UserProfile")]

    public class UserProfile : Document
    {   
        public List<string> BlockedUsers { get; set; }

        public UserProfile()
        {
            BlockedUsers = new List<string>();
        }
    }
}
