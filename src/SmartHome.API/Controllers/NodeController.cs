using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json.Linq;
using SmartHome.API.Utils;
using SmartHome.Core.Dto;
using SmartHome.Core.Infrastructure;
using SmartHome.Core.Services;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SmartHome.API.Controllers
{
    [ApiController]
    [ApiVersion("1")]
    [Authorize(Policy = "Admin")]
    [Authorize(Policy = "User")]
    [Route("api/[controller]")]
    [Produces("application/json")]
    public class NodeController : ControllerBase
    {
        private readonly INodeService _nodeService;

        public NodeController(IHttpContextAccessor contextAccessor, INodeService nodeService)
        {
            _nodeService = nodeService;
            _nodeService.Principal = contextAccessor.HttpContext.User;
        }

        [HttpGet]
        [ProducesResponseType(typeof(ServiceResult<IEnumerable<NodeDto>>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ServiceResult<IEnumerable<NodeDto>>), StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> GetAll()
        {
            var serviceResult = await _nodeService.GetAll();

            return ControllerResponseHelper.GetDefaultResponse(serviceResult);
        }

        [HttpPost]
        [ProducesResponseType(typeof(ServiceResult<NodeDto>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ServiceResult<NodeDto>), StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> Create(NodeDto node)
        {
            var serviceResult = await _nodeService.CreateNode(node);

            return ControllerResponseHelper.GetDefaultResponse(serviceResult, StatusCodes.Status201Created);
        }

        [HttpPost("{nodeId}/command/{command}")]
        public async Task<IActionResult> ExecuteCommand(int nodeId, string command, JObject commandParams)
        {
            var serviceResult = await _nodeService.Control(nodeId, command, commandParams);

            return ControllerResponseHelper.GetDefaultResponse(serviceResult, StatusCodes.Status202Accepted);
        }
    }
}
