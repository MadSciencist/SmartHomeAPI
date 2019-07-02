using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SmartHome.API.Security;
using SmartHome.Core.Domain.Enums;
using SmartHome.Core.Infrastructure;
using System;
using System.Linq;

namespace SmartHome.API.Utils
{
    public static class ControllerResponseHelper
    {
        public static ITrustProvider TrustProvider { get; set; }

        static ControllerResponseHelper()
        {
            TrustProvider = TrustFactory.GetDefaultTrustProvider();
        }

        public static IActionResult GetDefaultResponse<T>(ServiceResult<T> serviceResult,
            int overrideOkStatus = StatusCodes.Status200OK) where T : class
        {
            ValidateAndThrowIfNeeded(serviceResult);

            ProcessExceptionsMessageVisibility(serviceResult);

            return InferResponseType(serviceResult, overrideOkStatus);
        }

        private static IActionResult InferResponseType<T>(ServiceResult<T> serviceResult, int okStatus) where T : class
        {
            if (serviceResult.ResponseStatusCodeOverride.HasValue)
            {
                var response = new ObjectResult(serviceResult) { StatusCode = serviceResult.ResponseStatusCodeOverride.Value };
                return response;
            }

            if (serviceResult.Alerts.Any(x => x.MessageType == MessageType.Error))
            {
                return new BadRequestObjectResult(serviceResult);
            }

            if (serviceResult.Alerts.Any(x => x.MessageType == MessageType.Exception))
            {
                var response = new ObjectResult(serviceResult) {StatusCode = StatusCodes.Status500InternalServerError};
                return response;
            }

            return new ObjectResult(serviceResult) { StatusCode = okStatus };
        }

        private static void ProcessExceptionsMessageVisibility<T>(ServiceResult<T> serviceResult) where T : class
        {
            var isTrusted = TrustProvider.IsTrustedRequest(serviceResult.Principal);

            if (!isTrusted)
            {
                serviceResult.HideExceptionMessages();
            }
        }

        private static void ValidateAndThrowIfNeeded<T>(ServiceResult<T> serviceResult) where T : class
        {
            if (serviceResult == null)
            {
                throw new ArgumentNullException(nameof(ServiceResult<T>));
            }

            if (serviceResult.Principal == null)
            {
                throw new ArgumentNullException(nameof(serviceResult.Principal));
            }
        }
    }
}
