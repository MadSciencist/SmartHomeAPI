using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SmartHome.API.Dto;
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
    public class CommandController
    {
        private readonly ICommandService _commandService;

        public CommandController(IHttpContextAccessor contextAccessor, ICommandService service)
        {
            _commandService = service;
            _commandService.Principal = contextAccessor.HttpContext.User;
        }

        [HttpPost("create")]
        [ProducesResponseType(typeof(ServiceResult<CommandEntityDto>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ServiceResult<CommandEntityDto>), StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> Create(CreateCommandDto dto)
        {
            var serviceResult  = await _commandService.CreateCommand(dto.CommandName, dto.ExecutorClassName);

            return ControllerResponseHelper.GetDefaultResponse(serviceResult);
        }
    }
}
