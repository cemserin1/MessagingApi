using System;
using System.Collections.Generic;
using System.Text;

namespace MessagingApi.Service.ServiceRequests
{
    public class UserServiceRequest
    {
        public UserServiceRequest(string userName, string password, bool isActive)
        {
            UserName = userName;
            Password = password;
            IsActive = isActive;
        }

        public string UserName { get; set; }

        public string Password { get; set; }

        public bool IsActive { get; set; }
    }
}
