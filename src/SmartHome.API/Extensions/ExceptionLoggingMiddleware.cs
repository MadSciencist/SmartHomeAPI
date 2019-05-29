using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using SmartHome.Core.Infrastructure;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Net;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using SmartHome.API.DTO;

namespace SmartHome.API.Extensions
{
    public class ExceptionLoggingMiddleware
    {
        private readonly RequestDelegate _next;
        private static ILogger _logger;

        public ExceptionLoggingMiddleware(RequestDelegate next, ILoggerFactory logger)
        {
            _logger = logger.CreateLogger(nameof(ExceptionLoggingMiddleware));
            _next = next;
        }

        public async Task InvokeAsync(HttpContext httpContext)
        {
            try
            {
                await _next(httpContext);
            }
            catch (SmartHomeException ex)
            {
                await HandleExceptionAsync(ex, false, httpContext);
            }
            catch (Exception ex)
            {
                await HandleExceptionAsync(ex, true, httpContext);
            }
        }

        private static Task HandleExceptionAsync(Exception ex, bool isSecure, HttpContext context)
        {
            context.Response.ContentType = "application/json";
            context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;

            var correlationId = Guid.NewGuid();
            _logger.LogError(ex, "Global exception logger, correlationId: " + correlationId);

            var details = new ProblemDetails
            {
                Title = "Unhandled exception",
                Type = "Exception",
                Detail = isSecure ? "API Internal Server Error.Please contact administrator." : ex.Message,
                Status = context.Response.StatusCode,
                Instance = correlationId.ToString()
            };

            details.Extensions.Add("timestamp", DateTime.UtcNow.ToString(CultureInfo.InvariantCulture));
            details.Extensions.Add("location", context.Request.Path);

            var response = new ServiceResult<object> {Metadata = {ProblemDetails = details}};
            response.Alerts.Add(new Alert("System error occured", MessageType.Exception));

            return context.Response.WriteAsync(JsonConvert.SerializeObject(response));
        }
    }
}
