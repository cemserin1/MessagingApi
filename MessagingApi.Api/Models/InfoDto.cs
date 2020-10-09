using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MessagingApi.Api.Models
{
    public class InfoDto
    {
        public int Code { get; set; }
        public string Message { get; set; }
        public bool ShowToUser { get; set; }
    }
}
