using System;
using System.Collections.Generic;
using System.Reflection.Metadata;
using System.Text;
using MessagingApi.Data.Entities;
using MessagingApi.Service.Models;

namespace MessagingApi.Service.ServiceResponses
{
    public class InboxServiceResponse : BaseResponseDto
    {
        public Inbox Inbox { get; set; }

        public InboxServiceResponse(Inbox inbox)
        {
            Inbox = inbox;
        }

        public InboxServiceResponse()
        {
            
        }
    }
}
