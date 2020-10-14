using System.Collections.Generic;

namespace MessagingApi.Service.Models
{
    public abstract class BaseResponseDto
    {
        public string Message { get; set; }
        public bool ShowToUser { get; set; }

        protected BaseResponseDto()
        {
            InfoList = new List<InfoDto>();
        }
        public List<InfoDto> InfoList { get; set; }
    }
}
