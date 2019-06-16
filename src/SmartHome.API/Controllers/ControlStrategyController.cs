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
    public class ControlStrategyController : Controller
    {
        private readonly IControlStrategyService _controlStrategyService;

        public ControlStrategyController(IHttpContextAccessor contextAccessor, IControlStrategyService service)
        {
            _controlStrategyService = service;
            _controlStrategyService.Principal = contextAccessor.HttpContext.User;
        }

        [HttpGet]
        [ProducesResponseType(typeof(ServiceResult<IEnumerable<ControlStrategyDto>>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ServiceResult<IEnumerable<ControlStrategyDto>>), StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> GetAll()
        {
            var serviceResult = await _controlStrategyService.GetAll();

            return ControllerResponseHelper.GetDefaultResponse(serviceResult);
        }

        [HttpPost]
        [ProducesResponseType(typeof(ServiceResult<ControlStrategyDto>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ServiceResult<ControlStrategyDto>), StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> Create(ControlStrategyDto controlStrategy)
        {
            var serviceResult = await _controlStrategyService.CreateStrategy(controlStrategy);

            return ControllerResponseHelper.GetDefaultResponse(serviceResult);
        }

        // TODO
        [HttpPut("{id}")]
        [ProducesResponseType(typeof(ServiceResult<ControlStrategyDto>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ServiceResult<ControlStrategyDto>), StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> Update(ControlStrategyDto controlStrategy, int id)
        { 
            throw new NotImplementedException("UPDATE");
        }

        [HttpDelete("{id}")]
        [ProducesResponseType(typeof(ServiceResult<ControlStrategyDto>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ServiceResult<ControlStrategyDto>), StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> DeleteById(int id)
        {
            // TODO
            // mock:
            var serviceResult = new ServiceResult<object>(_controlStrategyService.Principal)
            {
                Data = new Object(),
                Alerts = new List<Alert> { new Alert("Successfully deleted", MessageType.Success) }
            };

            return ControllerResponseHelper.GetDefaultResponse(serviceResult);
        }
    }
}