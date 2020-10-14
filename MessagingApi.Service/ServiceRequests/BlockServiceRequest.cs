using System.Text.Json.Serialization;

namespace MessagingApi.Service.ServiceRequests
{
    public class BlockServiceRequest
    {
        public string UserName { get; set; }
        [JsonIgnore]
        public string BlockingUserId { get; set; }

        public BlockServiceRequest()
        {
        }

        public BlockServiceRequest(string userName, string blockingUserId)
        {
            UserName = userName;
            BlockingUserId = blockingUserId;
        }
    }
}
