using System;
using System.Collections.Generic;
using System.Text;

namespace MessagingApi.Service.ServiceResponses
{
    public class UserLoginResponse
    {
        public string UserName { get; set; }

        public string Token { get; set; }

        public UserLoginResponse()
        {
            
        }
        public UserLoginResponse(string userName, string token)
        {
            UserName = userName;
            Token = token;
        }
    }
}
