using System;
using System.Collections.Generic;
using System.Text;

namespace MessagingApi.Service.ServiceResponses
{
    public class SendMessageServiceResponse
    {
        public bool IsSuccess { get; set; }

        public SendMessageServiceResponse()
        {
            
        }

        public SendMessageServiceResponse(bool isSuccess)
        {
            IsSuccess = isSuccess;
        }
    }
}
