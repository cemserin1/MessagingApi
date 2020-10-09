using System;
using System.Collections.Generic;
using System.Reflection.Metadata;
using System.Text;
using MessagingApi.Data.Entities;

namespace MessagingApi.Service.ServiceResponses
{
    public class InboxServiceResponse
    {
        public bool IsSuccess { get; set; }
        public List<Message> Messages { get; set; }

        public InboxServiceResponse()
        {
            Messages = new List<Message>();
        }

        public InboxServiceResponse(bool isSuccess, List<Message> messages)
        {
            Messages = messages;
            IsSuccess = isSuccess;
        }
    }
}
