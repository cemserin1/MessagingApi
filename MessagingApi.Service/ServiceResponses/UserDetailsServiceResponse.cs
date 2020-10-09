using System;
using System.Collections.Generic;
using System.Text;
using MessagingApi.Data.Entities;

namespace MessagingApi.Service.ServiceResponses
{
    public class UserDetailsServiceResponse
    {
        public string UserName { get; set; }
        public string Password { get; set; }
        public bool IsActive { get; set; }
        public string UserId { get; set; }
        public DateTime DateCreated { get; set; }

        public UserDetailsServiceResponse()
        {
            
        }
        public UserDetailsServiceResponse(User u)
        {
            UserName = u.UserName;
            Password = u.Password;
            IsActive = u.IsActive;
            DateCreated = u.CreatedAt;
            UserId = u.Id.ToString();
        }
    }
}
