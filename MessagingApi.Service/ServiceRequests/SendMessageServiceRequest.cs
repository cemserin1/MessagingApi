using System;
using System.Collections.Generic;
using System.Reflection.Metadata;
using System.Text;

namespace MessagingApi.Service.ServiceRequests
{
    public class SendMessageServiceRequest
    {
        public string From { get; set; }
        public string To { get; set; }
        public string Content { get; set; }
    }
}
