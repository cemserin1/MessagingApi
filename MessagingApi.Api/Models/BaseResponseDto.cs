using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MessagingApi.Api.Models
{
    public abstract class BaseResponseDto
    {
        protected BaseResponseDto()
        {
            InfoList = new List<InfoDto>();
        }
        public List<InfoDto> InfoList { get; set; }
    }
}
