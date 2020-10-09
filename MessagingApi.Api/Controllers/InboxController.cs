using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MediatR;
using MessagingApi.Service.ServiceRequests;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace MessagingApi.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class InboxController : ControllerBase
    {
        private readonly IMediator _mediator;

        public InboxController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpPost]
        [Authorize]
        public async Task<IActionResult> Post([FromBody] InboxServiceRequest request)
        {
            var result = await _mediator.Send(request);
            return Ok(result);
        }
    }
}
