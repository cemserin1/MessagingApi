﻿using System;
using System.Collections.Generic;
using System.Reflection.Metadata;
using System.Text;

namespace MessagingApi.Service.ServiceRequests
{
    public class UserServiceRequest
    {
        public UserServiceRequest()
        {
            
        }
        public UserServiceRequest(string userName, string password)
        {
            UserName = userName;
            Password = password;
        }
        
        public string UserName { get; set; }

        public string Password { get; set; }

    }
}
