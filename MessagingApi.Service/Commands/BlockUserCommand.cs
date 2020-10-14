using System;
using System.Collections.Generic;
using System.Reflection.Metadata;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using FluentValidation;
using MediatR;
using MessagingApi.Data;
using MessagingApi.Data.Entities;
using MessagingApi.Service.ServiceRequests;
using MessagingApi.Service.ServiceResponses;
using MongoDB.Bson;

namespace MessagingApi.Service.Commands
{
    public class BlockUserCommand : IRequest<BlockUserResponse>
    {
        public string UserBlocked { get; set; }
        public string BlockingUserId { get; set; }

        public BlockUserCommand(BlockServiceRequest request)
        {
            UserBlocked = request.UserName;
            BlockingUserId = request.BlockingUserId;
        }
    }

    public class BlockUserCommandValidator : AbstractValidator<BlockUserCommand>
    {
        public BlockUserCommandValidator()
        {
            RuleFor(x => x.UserBlocked).Must(x => x == null || x.Length >= 6)
                .WithMessage("username_must_be_at_least_6_characters");
        }
    }

    public class BlockUserCommandHandler : IRequestHandler<BlockUserCommand, BlockUserResponse>
    {
        private readonly IMediator _mediator;
        private readonly IMongoDbRepository<UserProfile> _userProfileRepository;
        public BlockUserCommandHandler(IMediator mediator, IMongoDbRepository<UserProfile> userProfileRepository)
        {
            _mediator = mediator;
            _userProfileRepository = userProfileRepository;
        }

        public async Task<BlockUserResponse> Handle(BlockUserCommand request, CancellationToken cancellationToken)
        {
            var user = await _mediator.Send(new GetUserDetailsCommand(request.UserBlocked), cancellationToken);
            if (user == null)
            {
                return new BlockUserResponse() { Message = "user_does_not_exist" };
            }
            var userProfile = await _userProfileRepository.FindOneAsync(u => u.Id == ObjectId.Parse(request.BlockingUserId));
            if (userProfile.BlockedUsers.Contains(user.UserId))
            {
                return new BlockUserResponse() { Message = "user_is_already_blocked" };
            }
            userProfile.BlockedUsers.Add(user.UserId);
            await _userProfileRepository.ReplaceOneAsync(userProfile);
            return new BlockUserResponse() { Message = "Success" };
        }
    }
}
