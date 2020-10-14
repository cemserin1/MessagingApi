using System;
using System.Collections.Generic;
using System.Text;
using MessagingApi.Service.Models;

namespace MessagingApi.Service.ServiceResponses
{
    public class ApiResponse<T> : BaseResponseDto
    {
        public T Data { get; set; }
       
    }
}
