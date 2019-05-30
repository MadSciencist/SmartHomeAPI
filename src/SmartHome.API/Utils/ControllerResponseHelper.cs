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

        public static IActionResult GetDefaultResponse<T>(ServiceResult<T> serviceResult) where T : class, new()
        {
            if (serviceResult.Principal == null)
            {
                throw new ArgumentNullException(nameof(serviceResult.Principal));
            }

            var isTrusted = TrustProvider.IsTrustedRequest(serviceResult.Principal);

            if (!isTrusted)
            {
                serviceResult.HideExceptionMessages();
            }

            if (serviceResult.Alerts.Any(x => x.MessageType == MessageType.Error))
            {
                return new BadRequestObjectResult(serviceResult);
            }

            if (serviceResult.Alerts.Any(x => x.MessageType == MessageType.Exception))
            {
                return  new ContentResult
                {
                    StatusCode = StatusCodes.Status500InternalServerError,
                    Content = JsonConvert.SerializeObject(serviceResult)
                };
            }

            return new OkObjectResult(serviceResult);
        }
    }
}
