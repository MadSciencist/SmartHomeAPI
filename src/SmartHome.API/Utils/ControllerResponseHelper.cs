using System;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using SmartHome.API.Security;
using SmartHome.Core.Infrastructure;
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

        public static IActionResult GetDefaultResponse<T>(ServiceResult<T> serviceResult) where T : class
        {
            ValidateAndThrowIfNeeded(serviceResult);

            ProcessExceptionsMessageVisibility(serviceResult);

            return InferResponseType(serviceResult);
        }

        private static IActionResult InferResponseType<T>(ServiceResult<T> serviceResult) where T : class
        {
            if (serviceResult.Alerts.Any(x => x.MessageType == MessageType.Error))
            {
                return new BadRequestObjectResult(serviceResult);
            }

            if (serviceResult.Alerts.Any(x => x.MessageType == MessageType.Exception))
            {
                return new ContentResult
                {
                    StatusCode = StatusCodes.Status500InternalServerError,
                    Content = JsonConvert.SerializeObject(serviceResult)
                };
            }

            return new OkObjectResult(serviceResult);
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
            if (serviceResult?.Data == null)
            {
                throw new ArgumentNullException("Service output is null");
            }

            if (serviceResult.Principal == null)
            {
                throw new ArgumentNullException(nameof(serviceResult.Principal));
            }
        }
    }
}
