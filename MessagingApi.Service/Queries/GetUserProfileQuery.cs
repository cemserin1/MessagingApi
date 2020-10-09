using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using MessagingApi.Data;
using MessagingApi.Data.Entities;
using MessagingApi.Service.ServiceResponses;
using MongoDB.Bson;

namespace MessagingApi.Service.Queries
{
    public class GetUserProfileQuery : IRequest<UserProfileServiceResponse>
    {
        public string UserId { get; set; }

        public GetUserProfileQuery(string userId)
        {
            UserId = userId;
        }

    }

    public class GetUserProfileHandler : IRequestHandler<GetUserProfileQuery, UserProfileServiceResponse>
    {
        private readonly IMongoDbRepository<UserProfile> _userProfileRepository;

        public GetUserProfileHandler(IMongoDbRepository<UserProfile> userProfileRepository)
        {
            _userProfileRepository = userProfileRepository;
        }

        public async Task<UserProfileServiceResponse> Handle(GetUserProfileQuery request, CancellationToken cancellationToken)
        {
            var selectedUser = await _userProfileRepository.FindOneAsync(u => u.Id == ObjectId.Parse(request.UserId) );
            return selectedUser == null ? new UserProfileServiceResponse(false, null) : new UserProfileServiceResponse(true, selectedUser);
        }
    }
}
