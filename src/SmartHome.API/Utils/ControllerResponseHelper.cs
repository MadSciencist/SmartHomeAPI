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
        static ITrustProvider trustProvider;

        static ControllerResponseHelper()
        {
            trustProvider = new AdminTrustProvider();
        }

        public static IActionResult GetDefaultResponse<T>(ServiceResult<T> serviceResult) where T : class, new()
        {
            if(!trustProvider.IsTrusted(serviceResult.Principal))
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
