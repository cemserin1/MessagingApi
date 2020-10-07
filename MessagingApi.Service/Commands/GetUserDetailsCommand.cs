using System.Threading;
using System.Threading.Tasks;
using MediatR;
using MessagingApi.Data;
using MessagingApi.Data.Entities;
using MessagingApi.Service.ServiceResponses;

namespace MessagingApi.Service.Commands
{

    public class GetUserDetailsCommand : IRequest<UserDetailsServiceResponse>
    {
        public string UserName { get; set; }

        public GetUserDetailsCommand(string userName)
        {
            UserName = userName;
        }
    }

    public class GetUserDetailsCommandHandler : IRequestHandler<GetUserDetailsCommand, UserDetailsServiceResponse>
    {
        private readonly IMongoDbRepository<User> _userRepository;

        public GetUserDetailsCommandHandler(IMongoDbRepository<User> userRepository)
        {
            _userRepository = userRepository;
        }


        public async Task<UserDetailsServiceResponse> Handle(GetUserDetailsCommand request, CancellationToken cancellationToken)
        {
            //await _userRepository.InsertOneAsync(new User() { IsActive = true, Password = "12345", UserName = "armut" });
            var resp = _userRepository.FindOneAsync(u => u.UserName == "armut").Result;
            return new UserDetailsServiceResponse(resp);
        }
    }
}
