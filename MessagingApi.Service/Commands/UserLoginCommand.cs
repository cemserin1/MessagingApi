using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Reflection.Metadata;
using System.Security.Claims;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using FluentValidation;
using MediatR;
using MessagingApi.Data;
using MessagingApi.Data.Entities;
using MessagingApi.Service.Helpers.JWT;
using MessagingApi.Service.Interfaces;
using MessagingApi.Service.ServiceRequests;
using MessagingApi.Service.ServiceResponses;
using Microsoft.IdentityModel.Tokens;

namespace MessagingApi.Service.Commands
{
    public class UserLoginCommand : IRequest<UserLoginResponse>
    {
        public string UserName { get; set; }
        public string Password { get; set; }

        public UserLoginCommand(LoginServiceRequest request)
        {
            UserName = request.UserName;
            Password = request.Password;
        }
    }

    public class UserLoginCommandValidator : AbstractValidator<UserLoginCommand>
    {
        public UserLoginCommandValidator()
        {
            RuleFor(x => x.UserName).NotEmpty().NotNull()
                .WithMessage($"{nameof(UserLoginCommand.UserName)} cannot be empty.");

            RuleFor(x => x.Password).NotEmpty().NotNull()
                .WithMessage($"{nameof(UserLoginCommand.Password)} cannot be empty.");


        }
    }

    public class UserLoginCommandHandler : IRequestHandler<UserLoginCommand, UserLoginResponse>
    {
        private readonly IMongoDbRepository<User> _userRepository;
        private readonly IJwtSettings _jwtSettings;

        public UserLoginCommandHandler(IMongoDbRepository<User> userRepository, IJwtSettings jwtSettings)
        {
            _userRepository = userRepository;
            _jwtSettings = jwtSettings;
        }



        public async Task<UserLoginResponse> Handle(UserLoginCommand request, CancellationToken cancellationToken)
        {
            var user = await _userRepository.FindOneAsync(u =>
                u.UserName == request.UserName && u.Password == request.Password);
            if (user == null)
            {
                return null;
            }

            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(_jwtSettings.SecretKey);
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new[]
                {
                    new Claim("UserId", user.Id.ToString()),
                    new Claim("UserName",user.UserName),
                }),
                Expires = DateTime.UtcNow.AddMinutes(120),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
            };
            var token = tokenHandler.CreateToken(tokenDescriptor);
            string generatedToken = tokenHandler.WriteToken(token);
            return new UserLoginResponse(request.UserName, generatedToken);

        }
    }
}
