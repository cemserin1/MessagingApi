using System;
using System.Collections.Generic;
using System.Text;
using MessagingApi.Data.Entities;
using MessagingApi.Service.Models;

namespace MessagingApi.Service.ServiceResponses
{
    public class SendMessageServiceResponse : BaseResponseDto
    {
        public DateTime MessageDate { get; set; }

        public SendMessageServiceResponse()
        {
            
        }

        public SendMessageServiceResponse(DateTime messageDate)
        {
            MessageDate = messageDate;
        }
    }
}
