namespace MessagingApi.Data.Entities
{
    public class Error : Document
    {
        public int Code { get; set; }
        public string Message { get; set; }
        public bool IsUserFriendly { get; set; }
    }
}
