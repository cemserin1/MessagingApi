using System;
using System.Collections.Generic;
using System.Text;
using MessagingApi.Data.Entities;

namespace MessagingApi.Service.ServiceResponses
{
    public class UserProfileServiceResponse
    {
        public bool IsSuccess { get; set; }

        public UserProfile UserProfile { get; set; }

        public UserProfileServiceResponse()
        {
            
        }

        public UserProfileServiceResponse(bool isSuccess, UserProfile userProfile)
        {
            IsSuccess = isSuccess;
            UserProfile = userProfile;
        }
    }
}
