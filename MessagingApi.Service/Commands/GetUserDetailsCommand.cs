using System.Threading;
using System.Threading.Tasks;
using MediatR;
using MessagingApi.Data;
using MessagingApi.Data.Entities;
using MessagingApi.Service.ServiceResponses;
using Microsoft.Extensions.Caching.Distributed;

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
            var resp = await _userRepository.FindOneAsync(u => u.UserName == request.UserName);
            return resp == null ? null : new UserDetailsServiceResponse(resp);
        }
    }
}
