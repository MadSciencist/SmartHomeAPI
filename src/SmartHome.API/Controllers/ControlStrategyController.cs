using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SmartHome.API.Utils;
using SmartHome.Core.Dto;
using SmartHome.Core.Infrastructure;
using SmartHome.Core.Services;
using System.Threading.Tasks;

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
        [ProducesResponseType(typeof(ServiceResult<ControlStrategyDto>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ServiceResult<ControlStrategyDto>), StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> Create(ControlStrategyDto controlStrategy)
        {
            var serviceResult = await _controlStrategyService.CreateStrategy(controlStrategy);

            return ControllerResponseHelper.GetDefaultResponse(serviceResult);
        }

        [HttpPost("{strategyId}/attachCommand/{commandId}")]
        [ProducesResponseType(typeof(ServiceResult<CommandEntityDto>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ServiceResult<CommandEntityDto>), StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> AttachCommand(int strategyId, int commandId)
        {
            var serviceResult = await _controlStrategyService.AttachAvailableCommand(strategyId, commandId);

            return ControllerResponseHelper.GetDefaultResponse(serviceResult);
        }
    }
}