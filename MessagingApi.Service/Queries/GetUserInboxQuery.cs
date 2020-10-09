using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using MessagingApi.Data;
using MessagingApi.Data.Entities;
using MessagingApi.Service.ServiceResponses;

namespace MessagingApi.Service.Queries
{
    public class GetUserInboxQuery : IRequest<InboxServiceResponse>
    {
        public string UserId { get; set; }
        public GetUserInboxQuery(string userId)
        {
            UserId = userId;
        }

        public GetUserInboxQuery()
        {

        }


    }

    public class GetUserInboxHandler : IRequestHandler<GetUserInboxQuery, InboxServiceResponse>
    {
        private readonly IMongoDbRepository<Message> _messageRepository;

        public GetUserInboxHandler(IMongoDbRepository<Message> messageRepository)
        {
            _messageRepository = messageRepository;
        }

        public async Task<InboxServiceResponse> Handle(GetUserInboxQuery request, CancellationToken cancellationToken)
        {
            try
            {
                var messages = await _messageRepository.FilterByAsync(m => m.ToUserId == request.UserId);
                return new InboxServiceResponse(true, messages.ToList());
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                return new InboxServiceResponse(true, null);
            }
        }
    }
}
