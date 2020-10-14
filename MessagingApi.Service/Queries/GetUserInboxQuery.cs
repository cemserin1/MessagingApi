using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using MessagingApi.Data;
using MessagingApi.Data.Entities;
using MessagingApi.Service.Models;
using MessagingApi.Service.ServiceResponses;
using MongoDB.Bson;

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
        private readonly IMongoDbRepository<UserProfile> _userProfileRepository;

        public GetUserInboxHandler(IMongoDbRepository<UserProfile> userProfileRepository)
        {
            _userProfileRepository = userProfileRepository;
        }

        public async Task<InboxServiceResponse> Handle(GetUserInboxQuery request, CancellationToken cancellationToken)
        {
            var selectedUser = await _userProfileRepository.FindOneAsync(u => u.Id == new ObjectId(request.UserId));
            if (selectedUser == null)
            {
                throw new CustomException("user_not_found", true);
            }
            return new InboxServiceResponse() { Inbox = selectedUser.Inbox ?? new Inbox(), Message = "Success" };
        }
    }
}
