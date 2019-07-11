using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using SmartHome.API.Security;
using SmartHome.Core.Infrastructure;
using System;
using System.Globalization;
using System.Net;
using System.Threading.Tasks;

namespace SmartHome.API.Extensions
{
    public class ExceptionLoggingMiddleware
    {
        private readonly RequestDelegate _next;
        private static ILogger _logger;

        public ExceptionLoggingMiddleware(RequestDelegate next, ILogger<ExceptionLoggingMiddleware> logger)
        {
            _logger = logger;
            _next = next;
        }

        public async Task InvokeAsync(HttpContext httpContext)
        {
            try
            {
                await _next(httpContext);
            }
            catch(SmartHomeUnauthorizedException ex)
            {
                await HandleExceptionAsync(ex, httpContext, HttpStatusCode.Forbidden);
            }
            catch (SmartHomeEntityNotFoundException ex)
            {
                await HandleExceptionAsync(ex, httpContext, HttpStatusCode.NotFound);
            }
            catch (SmartHomeException ex)
            {
                await HandleExceptionAsync(ex, httpContext, HttpStatusCode.BadRequest);
            }
            catch (Exception ex)
            {
                await HandleExceptionAsync(ex, httpContext, HttpStatusCode.InternalServerError);
            }
        }

        private static Task HandleExceptionAsync(Exception ex, HttpContext context, HttpStatusCode code)
        {
            context.Response.ContentType = "application/json";
            context.Response.StatusCode = (int)code;

            var correlationId = Guid.NewGuid();
            _logger.LogError(ex, "Global exception logger, correlationId: " + correlationId);


            var isTrusted = TrustFactory.GetDefaultTrustProvider().IsTrustedRequest(context.User);
            var uiMessage = isTrusted ? $"{ex.Message} {ex.InnerException?.Message}" : "System error occured";

            var response = new ServiceResult<object> {Metadata = {ProblemDetails = CreateProblemDetails(ex, context, correlationId, isTrusted) } };
            response.Alerts.Add(new Alert(uiMessage, MessageType.Exception));

            return context.Response.WriteAsync(JsonConvert.SerializeObject(response));
        }

        private static ProblemDetails CreateProblemDetails(Exception ex, HttpContext context, Guid correlationId, bool isTrusted)
        {
            var details = new ProblemDetails
            {
                Title = "Unhandled exception",
                Type = "Exception",
                Detail = isTrusted ? ex.Message : "API Internal Server Error.Please contact administrator.",
                Status = context.Response.StatusCode,
                Instance = correlationId.ToString()
            };

            details.Extensions.Add("timestamp", DateTime.UtcNow.ToString(CultureInfo.InvariantCulture));
            details.Extensions.Add("location", context.Request.Path);
            return details;
        }
    }
}
