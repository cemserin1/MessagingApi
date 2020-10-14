using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Reflection.Metadata;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using FluentValidation;
using MediatR;
using MessagingApi.Data;
using MessagingApi.Data.Entities;
using MessagingApi.Service.Queries;
using MessagingApi.Service.ServiceRequests;
using MessagingApi.Service.ServiceResponses;
using MongoDB.Bson;

namespace MessagingApi.Service.Commands
{
    public class SendMessageCommand : IRequest<SendMessageServiceResponse>
    {
        public string From { get; set; }
        public string To { get; set; }
        public string Content { get; set; }

        public SendMessageCommand()
        {
        }
        public SendMessageCommand(SendMessageServiceRequest request)
        {
            From = request.From;
            To = request.To;
            Content = request.Content;
        }
    }

    public class SendMessageCommandHandler : IRequestHandler<SendMessageCommand, SendMessageServiceResponse>
    {
        private readonly IMongoDbRepository<UserProfile> _userProfileRepository;
        private readonly IMediator _mediator;

        public SendMessageCommandHandler(IMongoDbRepository<UserProfile> userProfileRepository, IMediator mediator)
        {
            _userProfileRepository = userProfileRepository;
            _mediator = mediator;
        }

        public class SendMessageCommandValidator : AbstractValidator<SendMessageCommand>
        {
            public SendMessageCommandValidator()
            {
                RuleFor(x => x.Content)
                    .Cascade(CascadeMode.Stop)
                    .NotEmpty().WithMessage("content_cannot_be_empty")
                    .Length(0, 1000).WithMessage("content_cannot_be_longer_than_1000_characters");
                RuleFor(x => x.From).NotEqual(x => x.To).WithMessage("cannot_send_message_to_yourself");
                //RuleFor(x => x.Content).Must(string.IsNullOrEmpty)
                //    .WithMessage("content_cannot_be_empty");
                //RuleFor(x => x.Content).Must(x => x.Length > 1000)
                //    .WithMessage("content_cannot_be_longer_than_1000_characters");
                RuleFor(x => x.To).Must(x => x.Length >= 6)
                    .WithMessage("to_user_must_be_at_least_6_characters");
            }
        }

        public async Task<SendMessageServiceResponse> Handle(SendMessageCommand request, CancellationToken cancellationToken)
        {
            var userFrom = await _mediator.Send(new GetUserDetailsCommand(request.From), cancellationToken);
            var userTo = await _mediator.Send(new GetUserDetailsCommand(request.To), cancellationToken);
            if (userTo == null)
            {
                //this user does not exist
                return new SendMessageServiceResponse() { Message = "user_does_not_exist" };
            }
            var userProfileFrom = await _mediator.Send(new GetUserProfileQuery(userFrom.UserId), cancellationToken);
            var userProfileTo = await _mediator.Send(new GetUserProfileQuery(userTo.UserId), cancellationToken);

            if (userProfileFrom.UserProfile.BlockedUsers.Contains(userTo.UserId))
            {
                //you blocked this user. Unblock first to send message.
                return new SendMessageServiceResponse() { Message = "user_is_blocked" };
            }
            if (userProfileTo.UserProfile != null && userProfileTo.UserProfile.BlockedUsers.Contains(userFrom.UserId))
            {
                //You cannot send messages to this user.
                return new SendMessageServiceResponse() { Message = "you_are_blocked_by_this_user" };
            }

            AddMessage(userProfileFrom.UserProfile, userProfileTo.UserProfile, request.Content);
            AddMessage(userProfileTo.UserProfile, userProfileFrom.UserProfile, request.Content);


            return new SendMessageServiceResponse() { Message = "Success", ShowToUser = true };
        }


        private void AddMessage(UserProfile from, UserProfile to, string content)
        {
            var conversation = from.Inbox.ConversationList.FirstOrDefault(c => c.FromUser== to.User.UserName);
            if (conversation == null)
            {
                from.Inbox.ConversationList.Add(new Conversation()
                {
                    ConversationId = ObjectId.GenerateNewId().ToString(),
                    FromUser = to.User.UserName,
                    Messages = new List<Message>()
                });
            }
            Message msg = new Message()
            {
                Id = ObjectId.GenerateNewId(),
                FromUserId = from.User.Id.ToString(),
                MessageContent = content
            };
            from.Inbox.ConversationList.FirstOrDefault(c => c.FromUser== to.User.UserName)?.Messages.Add(msg);
            _userProfileRepository.ReplaceOneAsync(from);
        }
    }
}
