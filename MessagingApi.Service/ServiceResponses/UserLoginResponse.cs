using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Text;
using MessagingApi.Service.Models;

namespace MessagingApi.Service.ServiceResponses
{
    public class UserLoginResponse : BaseResponseDto
    {
        public string UserName { get; set; }

        public string Token { get; set; }

     
    }
}
