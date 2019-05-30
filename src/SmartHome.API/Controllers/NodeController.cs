using System;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json.Linq;
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
    public class NodeController : ControllerBase
    {
        private readonly INodeService _nodeService;

        public NodeController(IHttpContextAccessor contextAccessor, INodeService nodeService)
        {
            _nodeService = nodeService;
            _nodeService.Principal = contextAccessor.HttpContext.User;
        }

        [HttpPost("create")]
        [ProducesResponseType(typeof(ServiceResult<NodeDto>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ServiceResult<NodeDto>), StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> Create(NodeDto node)
        {
            var serviceResult = await _nodeService.CreateNode(node);

            return ControllerResponseHelper.GetDefaultResponse(serviceResult);
        }

        [HttpPost("{nodeId}/command/{command}")]
        public async Task<IActionResult> ExecuteCommand(int nodeId, string command, JObject commandParams)
        {
            var serviceResult = await _nodeService.Control(nodeId, command, commandParams);

            return ControllerResponseHelper.GetDefaultResponse(serviceResult);
        }
    }
}
