using System;
using System.Collections.Generic;
using System.Text;

namespace MessagingApi.Service.ServiceRequests
{
    public class LoginServiceRequest
    {
        public string UserName { get; set; }

        public string Password { get; set; }

        public LoginServiceRequest()
        {
            
        }
        public LoginServiceRequest(string userName, string password)
        {
            UserName = userName;
            Password = password;
        }
    }
}
