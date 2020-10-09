using System;
using System.Collections.Generic;
using System.Text;

namespace MessagingApi.Service.ServiceResponses
{
    public class BlockUserResponse
    {
        public BlockUserResponse()
        {
            
        }

        public BlockUserResponse(bool isSuccess)
        {
            IsSuccess = isSuccess;
        }

        public bool IsSuccess { get; set; }

    }
}
