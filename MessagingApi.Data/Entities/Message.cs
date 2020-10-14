namespace MessagingApi.Data.Entities
{
    [BsonCollection("Messages")]

    public class Message : Document
    {
        public string FromUserId { get; set; }

        public string MessageContent { get; set; }
    }
}
