using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MediatR;
using MessagingApi.Service.Commands;
using MessagingApi.Service.ServiceRequests;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace MessagingApi.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserBlockController : ControllerBase
    {
        private readonly IMediator _mediator;

        public UserBlockController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpPost]
        [Authorize]
        public async Task<IActionResult> Post([FromBody] BlockServiceRequest request)
        {
            var userId = User.Claims.First(c => c.Type == "UserId").Value;
            request.BlockingUserId = userId.ToString();
            var result = await _mediator.Send(new BlockUserCommand(request));
            return Ok(result);
        }
    }
}
