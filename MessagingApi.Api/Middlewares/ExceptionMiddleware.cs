using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using FluentValidation;
using MessagingApi.Api.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.Extensions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Abstractions;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Net.Http.Headers;
using Serilog;

namespace MessagingApi.Api.Middlewares
{
    public class ExceptionMiddleware
    {
        private readonly RequestDelegate _next;
        private Serilog.ILogger _logger = Log.ForContext<ExceptionMiddleware>();
        private static readonly RouteData EmptyRouteData = new RouteData();

        private static readonly ActionDescriptor EmptyActionDescriptor = new ActionDescriptor();
        private static readonly HashSet<string> CorsHeaderNames = new HashSet<string>(StringComparer.OrdinalIgnoreCase)
        {
            HeaderNames.AccessControlAllowCredentials,
            HeaderNames.AccessControlAllowHeaders,
            HeaderNames.AccessControlAllowMethods,
            HeaderNames.AccessControlAllowOrigin,
            HeaderNames.AccessControlExposeHeaders,
            HeaderNames.AccessControlMaxAge,
        };

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
                ErrorDto ErrorDto;

                var errors = new List<InfoDto>();
                if (validationException.Errors.Any())
                {
                    foreach (var error in validationException.Errors)
                    {

                        errors.Add(new InfoDto
                        {
                            Code = 0,
                            Message = error.ErrorMessage,
                            ShowToUser = false
                        });
                    }

                    ErrorDto = new ErrorDto(null);
                }
                else
                {
                    ErrorDto = new ErrorDto("Bad Request");
                }

                ErrorDto.InfoList = errors;
                var executor = context.RequestServices.GetService<IActionResultExecutor<ObjectResult>>();
                var routeData = context.GetRouteData() ?? EmptyRouteData;

                var actionContext = new ActionContext(context, routeData, EmptyActionDescriptor);

                await executor.ExecuteAsync(actionContext, new ObjectResult(ErrorDto));


            }
            catch (CustomException customException)
            {
                var infoDto = new InfoDto
                {
                    Message = customException.Message,
                    ShowToUser = customException.ShowToUser,

                };


            }
            catch (Exception ex)
            {
            }
        }

        private static async Task ClearResponseAndBuildErrorDto(HttpContext context, ErrorDto errorDto, int statusCode)
        {
            var headers = new HeaderDictionary();

            // Make sure problem responses are never cached.
            headers.Append(HeaderNames.CacheControl, "no-cache, no-store, must-revalidate");
            headers.Append(HeaderNames.Pragma, "no-cache");
            headers.Append(HeaderNames.Expires, "0");

            foreach (var header in context.Response.Headers)
            {
                // Because the CORS middleware adds all the headers early in the pipeline,
                // we want to copy over the existing Access-Control-* headers after resetting the response.
                if (CorsHeaderNames.Contains(header.Key))
                {
                    headers.Add(header);
                }
            }

            context.Response.Clear();
            context.Response.StatusCode = statusCode;

            foreach (var header in headers)
            {
                context.Response.Headers.Add(header);
            }
        }

        private async Task WriteLog(HttpContext context, Exception exception, int statusCode)
        {
            var request = context.Request;
            var authenticationType = string.Empty;
            var userId = string.Empty;
            var clientName = string.Empty;




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
