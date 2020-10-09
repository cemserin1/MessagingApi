using System;
using System.Net;

namespace MessagingApi.Api.Models
{
    [Serializable]
    public class CustomException : Exception
    {
        public readonly HttpStatusCode StatusCode;
        public readonly bool ShowToUser;

        public CustomException(string message, bool showToUser = false, HttpStatusCode statusCode = HttpStatusCode.BadRequest) : base(message)
        {
            StatusCode = statusCode;
            ShowToUser = showToUser;
        }
    }
}
