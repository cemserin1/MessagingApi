namespace MessagingApi.Data.Entities
{
    public class Message : Document
    {
        public string FromUserId { get; set; }

        public string ToUserId  { get; set; }

        public string MessageContent { get; set; }
    }
}
