using Microsoft.AspNetCore.Http;
using SmartHome.API.Dto;
using System;

namespace SmartHome.API.Utils
{
    // TODO move actual logging here, so we wont miss any correlations
    public class ExceptionLogHelper
    {
        public static ErrorDetailsDto CreateErrorDetails(string message)
        {
            return new ErrorDetailsDto
            {
                CorrelationId = Guid.NewGuid(),
                Time = DateTime.UtcNow,
                Message = message
            };
        }

        public static ErrorDetailsDto CreateErrorDetails(string message, int statusCode)
        {
            return new ErrorDetailsDto
            {
                CorrelationId = Guid.NewGuid(),
                Time = DateTime.UtcNow,
                StatusCode = statusCode,
                Message = message
            };
        }

        public static ErrorDetailsDto CreateErrorDetails(string message, int statusCode, ErrorDetailsLocationDto location)
        {
            return new ErrorDetailsDto
            {
                CorrelationId = Guid.NewGuid(),
                Time = DateTime.UtcNow,
                StatusCode = statusCode,
                Message = message,
                Location = location
            };
        }

        public static ErrorDetailsDto CreateErrorDetails(string message, int statusCode, string path, string method)
        {
            return new ErrorDetailsDto
            {
                CorrelationId = Guid.NewGuid(),
                Time = DateTime.UtcNow,
                StatusCode = statusCode,
                Message = message,
                Location = new ErrorDetailsLocationDto
                {
                    Method = method,
                    Path = path
                }
            };
        }

        public static ErrorDetailsDto CreateErrorDetails(string message, int statusCode, HttpContext context)
        {
            return new ErrorDetailsDto
            {
                CorrelationId = Guid.NewGuid(),
                Time = DateTime.UtcNow,
                StatusCode = statusCode,
                Message = message,
                Location = new ErrorDetailsLocationDto
                {
                    Path = context.Request.Path,
                    Method = context.Request.Method
                }
            };
        }
    }
}
