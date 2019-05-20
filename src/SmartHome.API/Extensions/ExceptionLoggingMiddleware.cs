using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using SmartHome.Core.Infrastructure;
using System;
using System.Net;
using System.Threading.Tasks;
using SmartHome.API.Dto;

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

            var errorDetails = new ErrorDetailsDto
            {
                CorrelationId = correlationId,
                StatusCode = context.Response.StatusCode,
                Message = isSecure ? "API Internal Server Error.Please contact administrator." : ex.Message,
                Time = DateTime.UtcNow,
                Location = new ErrorDetailsLocationDto
                {
                    Path = context.Request.Path,
                    Method = context.Request.Method
                }
            };

            var responseDto = new ServiceResult<object> {Data = errorDetails};
            responseDto.Alerts.Add(new Alert("System error occured", MessageType.Exception));

            return context.Response.WriteAsync(JsonConvert.SerializeObject(responseDto));
        }
    }
}
