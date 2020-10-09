using System;
using System.Collections.Generic;
using System.Text;

namespace MessagingApi.Service.ServiceRequests
{
    public class InboxServiceRequest
    {
        public string UserId { get; set; }

        public InboxServiceRequest()
        {
            
        }

        public InboxServiceRequest(string userId)
        {
            UserId = userId;
        }
    }
}
