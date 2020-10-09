using System;
using System.Collections.Generic;
using System.Text;
using MessagingApi.Service.Interfaces;

namespace MessagingApi.Service.Helpers.JWT
{
    public class JwtSettings : IJwtSettings
    {
        public string SecretKey { get; set; }
    }
}
