using System;
using System.Collections.Generic;
using System.Reflection.Metadata;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
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

    public class BlockUserCommandHandler : IRequestHandler<BlockUserCommand, BlockUserResponse>
    {
        private readonly IMediator _mediator;
        private readonly IMongoDbRepository<User> _userRepository;
        private readonly IMongoDbRepository<UserProfile> _userProfileRepository;
        public BlockUserCommandHandler(IMediator mediator, IMongoDbRepository<User> userRepository, IMongoDbRepository<UserProfile> userProfileRepository)
        {
            _mediator = mediator;
            _userRepository = userRepository;
            _userProfileRepository = userProfileRepository;
        }

        public async Task<BlockUserResponse> Handle(BlockUserCommand request, CancellationToken cancellationToken)
        {
            //check if blocked user exists
            var user = await _mediator.Send(new GetUserDetailsCommand(request.UserBlocked), cancellationToken);
            if (user == null)
            {
                return new BlockUserResponse(false);
            }
            try
            {
                var userProfile = await _userProfileRepository.FindOneAsync(u => u.Id == ObjectId.Parse(request.BlockingUserId));
                if (userProfile == null)
                {
                    UserProfile profile = new UserProfile { Id = ObjectId.Parse(request.BlockingUserId) };
                    profile.BlockedUsers.Add(user.UserId);
                    await _userProfileRepository.InsertOneAsync(profile);
                }
                else
                {
                    var selectedUserProfile = await _userProfileRepository.FindOneAsync(u => u.Id.ToString() == request.BlockingUserId);
                    if (selectedUserProfile.BlockedUsers.Contains(request.UserBlocked))
                    {
                        //user's already blocked
                        return new BlockUserResponse(false);
                    }
                    selectedUserProfile.BlockedUsers.Add(user.UserId);
                    await _userProfileRepository.ReplaceOneAsync(selectedUserProfile);
                }
                return new BlockUserResponse(true);

            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                return new BlockUserResponse(false);
            }
        }
    }
}
