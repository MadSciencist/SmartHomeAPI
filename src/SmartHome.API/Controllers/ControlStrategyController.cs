using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SmartHome.Core.Dto;
using SmartHome.Core.Infrastructure;
using SmartHome.Core.Services;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using SmartHome.API.Utils;

namespace SmartHome.API.Controllers
{
    [Authorize]
    [ApiController]
    [Route("api/[controller]")]
    [Produces("application/json")]
    public class ControlStrategyController : Controller
    {
        private readonly IControlStrategyService _controlStrategyService;

        public ControlStrategyController(IHttpContextAccessor contextAccessor, IControlStrategyService service)
        {
            _controlStrategyService = service;
            _controlStrategyService.Principal = contextAccessor.HttpContext.User;
        }

        [HttpPost("create")]
        [ProducesResponseType(typeof(ServiceResult<NodeDto>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ServiceResult<NodeDto>), StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> Create(ControlStrategyDto controlStrategy)
        {
            var serviceResult = await _controlStrategyService.CreateStrategy(controlStrategy);

            return ControllerResponseHelper.GetDefaultResponse(serviceResult);
        }
    }
}