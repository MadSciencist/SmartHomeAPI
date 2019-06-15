using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SmartHome.API.Utils;
using SmartHome.Core.Dto;
using SmartHome.Core.Infrastructure;
using SmartHome.Core.Services;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SmartHome.API.Controllers
{
    [Authorize]
    [ApiController]
    [ApiVersion("1")]
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

        [HttpGet]
        [ProducesResponseType(typeof(ServiceResult<IEnumerable<CommandEntityDto>>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ServiceResult<IEnumerable<CommandEntityDto>>), StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> GetAll()
        {
            var serviceResult = await _commandService.GetCommands();
            
            return ControllerResponseHelper.GetDefaultResponse(serviceResult);
        }

        [HttpPost]
        [ProducesResponseType(typeof(ServiceResult<CommandEntityDto>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ServiceResult<CommandEntityDto>), StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> Create(CommandEntityDto dto)
        {
            var serviceResult  = await _commandService.CreateCommand(dto.Alias, dto.ExecutorClassName);

            return ControllerResponseHelper.GetDefaultResponse(serviceResult);
        }

        [HttpPut("{id}")]
        [ProducesResponseType(typeof(ServiceResult<CommandEntityDto>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ServiceResult<CommandEntityDto>), StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> UpdateById(CommandEntityDto dto, int id)
        {
            // TODO
            throw new NotImplementedException(nameof(UpdateById));
        }

        [HttpDelete("{id}")]
        [ProducesResponseType(typeof(ServiceResult<CommandEntityDto>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ServiceResult<CommandEntityDto>), StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> DeleteById(int id)
        {
            // TODO
            // mock:
            var serviceResult = new ServiceResult<object>(_commandService.Principal)
            {
                Data = new Object(),
                Alerts = new List<Alert> {new Alert("Successfully deleted", MessageType.Success)}
            };

            return ControllerResponseHelper.GetDefaultResponse(serviceResult);
        }
    }
}
