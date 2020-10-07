namespace MessagingApi.Data.DataModels.Requests
{
    public class UserDataModelRequest
    {
        public string UserId { get; set; }

        public UserDataModelRequest(string userId)
        {
            UserId = userId;
        }
    }
}
