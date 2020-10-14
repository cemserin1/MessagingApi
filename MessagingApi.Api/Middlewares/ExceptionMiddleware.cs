using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FluentValidation;
using MessagingApi.Service.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.Extensions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Abstractions;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.DependencyInjection;
using Serilog;

namespace MessagingApi.Api.Middlewares
{
    public class ExceptionMiddleware
    {
        private readonly RequestDelegate _next;
        private ILogger _logger = Log.ForContext<ExceptionMiddleware>();
        private static readonly RouteData EmptyRouteData = new RouteData();

        private static readonly ActionDescriptor EmptyActionDescriptor = new ActionDescriptor();


        public ExceptionMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task Invoke(HttpContext context)
        {
            try
            {
                await _next(context);
            }
            catch (ValidationException validationException)
            {
                ErrorDto errorDto;
                var errors = new List<InfoDto>();
                if (validationException.Errors.Any())
                {
                    foreach (var error in validationException.Errors)
                    {
                        errors.Add(new InfoDto
                        {
                            Code = 0,
                            Message = error.ErrorMessage,
                            ShowToUser = true
                        });
                    }
                    errorDto = new ErrorDto(null);
                }
                else
                {
                    errorDto = new ErrorDto("Bad Request");
                }

                errorDto.InfoList = errors;
                var executor = context.RequestServices.GetService<IActionResultExecutor<ObjectResult>>();
                var routeData = context.GetRouteData() ?? EmptyRouteData;

                var actionContext = new ActionContext(context, routeData, EmptyActionDescriptor);
                await WriteLog(context, validationException, 0);
                await executor.ExecuteAsync(actionContext, new ObjectResult(errorDto));

            }
            catch (CustomException customException)
            {
                var infoDto = new InfoDto
                {
                    Message = customException.Message,
                    ShowToUser = customException.ShowToUser
                };
                var executor = context.RequestServices.GetService<IActionResultExecutor<ObjectResult>>();
                var routeData = context.GetRouteData() ?? EmptyRouteData;
                var actionContext = new ActionContext(context, routeData, EmptyActionDescriptor);
                await WriteLog(context, customException, 0);
                await executor.ExecuteAsync(actionContext, new ObjectResult(infoDto));
            }
            catch (Exception ex)
            {
                await WriteLog(context, ex, 0);
            }
        }

        private async Task WriteLog(HttpContext context, Exception exception, int statusCode)
        {
            var request = context.Request;
            context.Request.EnableBuffering();

            // Leave the body open so the next middleware can read it.
            using (var reader = new StreamReader(
                context.Request.Body,
                leaveOpen: true))
            {
                var body = await reader.ReadToEndAsync();
                // Do some processing with body…

                // Reset the request body stream position so the next middleware can read it
                context.Request.Body.Position = 0;
            }
            var authenticationType = string.Empty;
            var userId = string.Empty;
            var clientName = string.Empty;
            var reqBody = await new StreamReader(request.Body).ReadToEndAsync();
            var req = await new StreamReader(context.Request.BodyReader.AsStream()).ReadToEndAsync();
            var requestPathAndQuery = request.GetEncodedPathAndQuery();
            _logger = _logger.ForContext("MachineName", Environment.MachineName)
                .ForContext("RequestHost", request.Host.Host)
                .ForContext("RequestProtocol", request.Protocol)
                .ForContext("RequestMethod", request.Method)
                .ForContext("ResponseStatusCode", statusCode)
                .ForContext("RequestPath", request.Path)
                .ForContext("RequestPathAndQuery", requestPathAndQuery)
                .ForContext("Exception", exception, true)
                .ForContext("RequestHeaders", request.Headers.ToDictionary(h => h.Key, h => (object)h.Value.ToString()), true)
                .ForContext("Exception", exception, true);
            if (!string.IsNullOrEmpty(authenticationType))
            {
                _logger = _logger.ForContext("UserId", userId)
                    .ForContext("AuthenticationType", authenticationType)
                    .ForContext("ClientName", clientName);
            }
            var errorTemplate = $"HTTP {request.Method} {requestPathAndQuery} responded {statusCode}";
            _logger.Error(exception, errorTemplate);
            await Task.FromResult(true);
        }

        public static Task WriteResultAsync<TResult>(HttpContext context, TResult result) where TResult : IActionResult
        {
            if (context == null)
            {
                throw new ArgumentNullException(nameof(context));
            }

            var executor = context.RequestServices.GetService<IActionResultExecutor<TResult>>();

            if (executor == null)
            {
                throw new InvalidOperationException($"No result executor for '{typeof(TResult).FullName}' has been registered.");
            }

            var routeData = context.GetRouteData() ?? EmptyRouteData;

            var actionContext = new ActionContext(context, routeData, EmptyActionDescriptor);

            return executor.ExecuteAsync(actionContext, result);
        }
    }

}
