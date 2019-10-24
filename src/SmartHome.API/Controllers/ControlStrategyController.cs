using Matty.Framework;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SmartHome.API.Utils;
using SmartHome.Core.Dto;
using SmartHome.Core.Services.Abstractions;
using System;
using System.Collections.Generic;
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
        [ProducesResponseType(typeof(ServiceResult<IEnumerable<ControlStrategyDto>>), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetAll()
        {
            var serviceResult = await _controlStrategyService.GetAll();

            return ControllerResponseHelper.GetDefaultResponse(serviceResult);
        }

        /// <summary>
        /// Add new strategy
        /// </summary>
        /// <param name="node"></param>
        /// <returns></returns>
        [HttpPost]
        [ProducesResponseType(typeof(ServiceResult<ControlStrategyDto>), StatusCodes.Status201Created)]
        [ProducesResponseType(typeof(ServiceResult<ControlStrategyDto>), StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> Create(ControlStrategyDto node)
        {
            var serviceResult = await _controlStrategyService.Create(node);

            return ControllerResponseHelper.GetDefaultResponse(serviceResult, StatusCodes.Status201Created);
        }

        /// <summary>
        /// Update strategy
        /// </summary>
        /// <param name="id"></param>
        /// <param name="node"></param>
        /// <returns></returns>
        [HttpPut("{id:int}")]
        [ProducesResponseType(typeof(ServiceResult<ControlStrategyDto>), StatusCodes.Status201Created)]
        [ProducesResponseType(typeof(ServiceResult<ControlStrategyDto>), StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> Edit(int id, ControlStrategyDto node)
        {
            var serviceResult = await _controlStrategyService.Update(id, node);

            return ControllerResponseHelper.GetDefaultResponse(serviceResult);
        }

        /// <summary>
        ///  Delete strategy
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpDelete("{id:int}")]
        [ProducesResponseType(typeof(ServiceResult<object>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ServiceResult<object>), StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> Delete(int id)
        {
            var serviceResult = await _controlStrategyService.Delete(id);

            return ControllerResponseHelper.GetDefaultResponse(serviceResult);
        }
    }
}
