using System;
using System.Collections.Generic;
using System.Text;

namespace MessagingApi.Service.Interfaces
{
    public interface IJwtSettings
    {
        public string SecretKey { get; set; }
    }
}
