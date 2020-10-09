using System;
using System.Collections.Generic;
using System.Reflection.Metadata;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using MessagingApi.Data;
using MessagingApi.Data.Entities;
using MessagingApi.Service.Queries;
using MessagingApi.Service.ServiceRequests;
using MessagingApi.Service.ServiceResponses;

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
        private readonly IMongoDbRepository<Message> _messageRepository;
        private readonly IMediator _mediator;

        public SendMessageCommandHandler(IMongoDbRepository<Message> messageRepository, IMediator mediator)
        {
            _messageRepository = messageRepository;
            _mediator = mediator;
        }

        public async Task<SendMessageServiceResponse> Handle(SendMessageCommand request, CancellationToken cancellationToken)
        {

            if (request.From == request.To)
            {
                //cannot send message to yourself
                return new SendMessageServiceResponse(false);
            }
            try
            {
                var userFrom = await _mediator.Send(new GetUserDetailsCommand(request.From), cancellationToken);
                var userTo = await _mediator.Send(new GetUserDetailsCommand(request.To), cancellationToken);

                if (userTo == null)
                {
                    //this user does not exist
                    return new SendMessageServiceResponse(false);
                }
                var userProfileFrom = await _mediator.Send(new GetUserProfileQuery(userFrom.UserId), cancellationToken);
                var userProfileTo = await _mediator.Send(new GetUserProfileQuery(userTo.UserId), cancellationToken);
                if (userProfileFrom.UserProfile != null && userProfileFrom.UserProfile.BlockedUsers.Contains(userTo.UserId))
                {
                    //you blocked this user. Unblock first to send message.
                    return new SendMessageServiceResponse(false);
                }
                if (userProfileTo.UserProfile != null && userProfileTo.UserProfile.BlockedUsers.Contains(userFrom.UserId))
                {
                    //You cannot send messages to this user.
                    return new SendMessageServiceResponse(false);
                }
                await _messageRepository.InsertOneAsync(new Message() { FromUserId = userFrom.UserId, ToUserId = userTo.UserId, MessageContent = request.Content });
                return new SendMessageServiceResponse(true);
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                return new SendMessageServiceResponse(false);
            }
            throw new NotImplementedException();
        }
    }
}
