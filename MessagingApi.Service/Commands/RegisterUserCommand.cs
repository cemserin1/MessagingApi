using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using FluentValidation;
using MediatR;
using MessagingApi.Data;
using MessagingApi.Data.Entities;
using MessagingApi.Service.Models;
using MessagingApi.Service.ServiceRequests;
using MessagingApi.Service.ServiceResponses;

namespace MessagingApi.Service.Commands
{
    public class RegisterUserCommand : IRequest<UserSignupServiceResponse>
    {

        public string UserName { get; set; }
        public string Password { get; set; }


        public RegisterUserCommand(UserServiceRequest request)
        {
            UserName = request.UserName;
            Password = request.Password;
        }
    }

    public class RegisterUserCommandValidator : AbstractValidator<RegisterUserCommand>
    {
        public RegisterUserCommandValidator()
        {
            RuleFor(x => x.UserName).Must(x => x == null || x.Length >= 6)
                .WithMessage("username_must_be_at_least_6_characters");
            RuleFor(x => x.Password).Must(x => x == null || x.Length >= 6)
                .WithMessage("password_must_be_at_least_6_characters");
        }
    }

    public class RegisterUserCommandHandler : IRequestHandler<RegisterUserCommand, UserSignupServiceResponse>
    {
        private readonly IMediator _mediator;
        private readonly IMongoDbRepository<User> _userRepository;
        private readonly IMongoDbRepository<UserProfile> _userProfileRepository;

        public RegisterUserCommandHandler(IMediator mediator, IMongoDbRepository<User> userRepository, IMongoDbRepository<UserProfile> userProfileRepository)
        {
            _mediator = mediator;
            _userRepository = userRepository;
            _userProfileRepository = userProfileRepository;
        }


        public async Task<UserSignupServiceResponse> Handle(RegisterUserCommand request, CancellationToken cancellationToken)
        {
            //check if username exists
            UserDetailsServiceResponse selectedUser = await _mediator.Send(new GetUserDetailsCommand(request.UserName), cancellationToken);
            if (selectedUser != null)
            {
                throw new CustomException("user_name_already_exists", true);
            }
            User user = new User()
            {
                UserName = request.UserName,
                Password = request.Password,
                IsActive = true
            };
            await _userRepository.InsertOneAsync(user);
            //create a user profile as well
            await _userProfileRepository.InsertOneAsync(new UserProfile()
            {
                BlockedUsers = new List<string>(),
                Id = user.Id,
                User = user
            });

            return new UserSignupServiceResponse() { ShowToUser = true, Message = "user_added_successfully" };
        }
    }
}
