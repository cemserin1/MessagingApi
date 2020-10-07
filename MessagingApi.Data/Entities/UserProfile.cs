using System.Collections.Generic;

namespace MessagingApi.Data.Entities
{
    public class UserProfile : User
    {
        public List<string> BlockedUsers { get; set; }
    }
}
