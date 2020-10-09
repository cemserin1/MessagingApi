using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MessagingApi.Api.Models
{
    public class ErrorDto : BaseResponseDto
    {
        public ErrorDto(string message)
        {
            Message = message;
        }
        public string Message { get; set; }
    }
}
