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
    public class LoginController : ControllerBase
    {
        private readonly IMediator _mediator;

        public LoginController(IMediator mediator)
        {
            _mediator = mediator;
        }

        
        [HttpPost]
        [AllowAnonymous]
        public async Task<IActionResult> Post([FromBody] LoginServiceRequest request)
        {
            var result = await _mediator.Send(new UserLoginCommand(request));

            return Ok(result);
        }
    }
}
