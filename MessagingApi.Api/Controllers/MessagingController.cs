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
    public class MessagingController : ControllerBase
    {
        private readonly IMediator _mediator;

        public MessagingController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpPost]
        [Authorize]
        public async Task<IActionResult> Post([FromBody] SendMessageServiceRequest request)
        {
            var fromUserName = User.Claims.First(c => c.Type == "UserName").Value;
            request.From = fromUserName;
            var result = await _mediator.Send(new SendMessageCommand(request));
            return Ok(result);
        }

    }
}
