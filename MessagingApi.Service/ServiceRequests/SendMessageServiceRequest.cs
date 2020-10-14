using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Reflection.Metadata;
using System.Text;
using System.Text.Json.Serialization;

namespace MessagingApi.Service.ServiceRequests
{
    public class SendMessageServiceRequest
    {
        [JsonIgnore]
        public string From { get; set; }
        [Required]
        public string To { get; set; }
        public string Content { get; set; }
    }
}
