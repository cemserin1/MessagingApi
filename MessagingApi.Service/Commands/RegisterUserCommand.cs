using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using MessagingApi.Data;
using MessagingApi.Data.Entities;
using MessagingApi.Service.ServiceRequests;
using MessagingApi.Service.ServiceResponses;

namespace MessagingApi.Service.Commands
{
    public class RegisterUserCommand : IRequest<bool>
    {
        public UserServiceRequest User { get; set; }

        public RegisterUserCommand(UserServiceRequest _request, IMediator mediator)
        {
            this.User = _request;
        }
    }

    public class RegisterUserCommandHandler : IRequestHandler<RegisterUserCommand, bool>
    {
        private readonly IMediator _mediator;
        private readonly IMongoDbRepository<User> _userRepository;

        public RegisterUserCommandHandler(IMediator mediator, IMongoDbRepository<User> userRepository)
        {
            _mediator = mediator;
            _userRepository = userRepository;
        }


        public async Task<bool> Handle(RegisterUserCommand request, CancellationToken cancellationToken)
        {
            //check if username exists
            UserDetailsServiceResponse selectedUser = await _mediator.Send(new GetUserDetailsCommand(request.User.UserName), cancellationToken);
            if (selectedUser == null)
            {
                return false;
            }
            try
            {
                await _userRepository.InsertOneAsync(new User()
                {
                    UserName = request.User.UserName,
                    Password = request.User.Password,
                    IsActive = true
                });
                return true;
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                return false;
            }
        }
    }
}
