using System.Linq;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json.Linq;
using SmartHome.API.DTO;
using SmartHome.API.Utils;
using SmartHome.Core.Dto;
using SmartHome.Core.Infrastructure;
using SmartHome.Core.Services;
using System.Net;
using System.Threading.Tasks;

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
            _nodeService.ClaimsOwner = contextAccessor.HttpContext.User;
        }

        [HttpPost("create")]
        [ProducesResponseType(typeof(ServiceResult<NodeDto>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ServiceResult<NodeDto>), StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> Create(NodeDto node)
        {
            var serviceResult = await _nodeService.CreateNode(node);
            serviceResult.HideExceptionMessages();

            if (serviceResult.Alerts.Any(x => x.MessageType == MessageType.Error))
                return BadRequest(serviceResult);

            if (serviceResult.Alerts.Any(x => x.MessageType == MessageType.Exception))
                return StatusCode(StatusCodes.Status500InternalServerError, serviceResult);

            return Ok(serviceResult);
        }

        [HttpPost("{nodeId}/command/{command}")]
        public async Task<IActionResult> ExecuteCommand(int nodeId, string command, JObject commandParams)
        {
            var serviceResult = await _nodeService.Control(nodeId, command, commandParams);
            serviceResult.HideExceptionMessages();

            if (serviceResult.Alerts.Any(x => x.MessageType == MessageType.Error))
                return BadRequest(serviceResult);

            if (serviceResult.Alerts.Any(x => x.MessageType == MessageType.Exception))
                return StatusCode(StatusCodes.Status500InternalServerError, serviceResult);

            return Ok(serviceResult);
        }
    }
}
