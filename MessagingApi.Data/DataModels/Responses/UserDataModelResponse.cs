using MessagingApi.Data.Entities;

namespace MessagingApi.Data.DataModels.Responses
{
    public class UserDataModelResponse
    {
        public string UserId { get; set; }

        public string UserName { get; set; }

        public string Password { get; set; }

        public bool IsActive { get; set; }

        public UserDataModelResponse(User user)
        {
            UserName = user.UserName;
            Password = user.Password;
            IsActive = user.IsActive;
            UserId = user.Id.ToString();
        }
    }
}
