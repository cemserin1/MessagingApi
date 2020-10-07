namespace MessagingApi.Data.Entities
{
    [BsonCollection("users")]
    public class User : Document
    {
        public string UserName { get; set; }

        public string Password { get; set; }

        public bool IsActive { get; set; }

    }
}
