namespace MessagingApi.Service.Models
{
    public class ErrorDto : BaseResponseDto
    {
        public ErrorDto(string message)
        {
            Message = message;
        }
        public new string Message { get; set; }
    }
}
