using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using SmartHome.API.DTO;
using SmartHome.API.Utils;
using System;
using System.Net;
using System.Threading.Tasks;

namespace SmartHome.API.Extensions
{
    public class ExceptionLoggingMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger _logger;

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
            catch (Exception ex)
            {
                var correlationId = Guid.NewGuid();
                _logger.LogError(ex, "Global exception logger, correlationId: " + correlationId);
                await HandleExceptionAsync(httpContext, correlationId);
            }
        }

        private static Task HandleExceptionAsync(HttpContext context, Guid correlationId)
        {
            context.Response.ContentType = "application/json";
            context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;

            var errorDetails = new ErrorDetailsDto
            {
                CorrelationId = correlationId,
                StatusCode = context.Response.StatusCode,
                Message = "API Internal Server Error. Please contact administrator.",
                Time = DateTime.UtcNow,
                Location = new ErrorDetailsLocationDto
                {
                    Path = context.Request.Path,
                    Method = context.Request.Method
                }
            };

            var responseDto = new DtoContainer<object>();
            responseDto.Errors.Add(errorDetails);

            return context.Response.WriteAsync(JsonConvert.SerializeObject(responseDto));
        }
    }
}
