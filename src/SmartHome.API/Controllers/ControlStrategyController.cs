using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SmartHome.API.Utils;
using SmartHome.Core.Services.Abstractions;
using System.Threading.Tasks;

namespace SmartHome.API.Controllers
{
    [ApiController]
    [Authorize]
    [Route("api/[controller]")]
    [Produces("application/json")]
    public class ControlStrategyController : ControllerBase
    {
        private readonly IControlStrategyService _controlStrategyService;

        public ControlStrategyController(IHttpContextAccessor contextAccessor, IControlStrategyService controlStrategyService)
        {
            _controlStrategyService = controlStrategyService;
            _controlStrategyService.Principal = contextAccessor.HttpContext.User;
        }

        /// <summary>
        /// List all control strategies
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var serviceResult = await _controlStrategyService.GetAll();

            return ControllerResponseHelper.GetDefaultResponse(serviceResult);
        }
    }
}
