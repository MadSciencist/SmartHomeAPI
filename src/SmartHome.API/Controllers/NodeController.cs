using System;
using Matty.Framework;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json.Linq;
using SmartHome.API.Utils;
using SmartHome.Core.Dto;
using SmartHome.Core.Services.Abstractions;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SmartHome.API.Controllers
{
    [ApiController]
    [Authorize]
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

        /// <summary>
        /// List all nodes
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [ProducesResponseType(typeof(ServiceResult<IEnumerable<NodeDto>>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ServiceResult<IEnumerable<NodeDto>>), StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> GetAll()
        {
            var serviceResult = await _nodeService.GetAll();

            return serviceResult.Data is null ? NotFound() : ControllerResponseHelper.GetDefaultResponse(serviceResult);
        }

        /// <summary>
        /// Add new node
        /// </summary>
        /// <param name="node"></param>
        /// <returns></returns>
        [HttpPost]
        [ProducesResponseType(typeof(ServiceResult<NodeDto>), StatusCodes.Status201Created)]
        [ProducesResponseType(typeof(ServiceResult<NodeDto>), StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> Create(NodeDto node)
        {
            var serviceResult = await _nodeService.CreateNode(node);

            return ControllerResponseHelper.GetDefaultResponse(serviceResult, StatusCodes.Status201Created);
        }

        /// <summary>
        /// Execute a command on a node.
        /// </summary>
        /// <param name="nodeId"></param>
        /// <param name="command"></param>
        /// <param name="commandParams"></param>
        /// <returns></returns>
        [HttpPost("{nodeId}/command/{command}")]
        public async Task<IActionResult> ExecuteCommand(int nodeId, string command, JObject commandParams = null)
        {
            var serviceResult = await _nodeService.ExecuteCommand(nodeId, command, commandParams);

            return ControllerResponseHelper.GetDefaultResponse(serviceResult, StatusCodes.Status202Accepted);
        }

        /// <summary>
        /// Get parameter schema for given command.
        /// </summary>
        /// <param name="nodeId"></param>
        /// <param name="command"></param>
        /// <returns></returns>
        [HttpGet("{nodeId}/command/{command}")]
        public async Task<IActionResult> GetRequestBodyForCommand(int nodeId, string command)
        {
            var serviceResult = await _nodeService.GetCommandParamSchema(nodeId, command);

            return ControllerResponseHelper.GetDefaultResponse(serviceResult);
        }

        /// <summary>
        /// Delete existing node.
        /// </summary>
        /// <returns></returns>
        [HttpDelete("{nodeId}")]
        public IActionResult DeleteNode()
        {
            throw new NotImplementedException(nameof(DeleteNode));
        }

        /// <summary>
        /// Update existing node.
        /// </summary>
        /// <returns></returns>
        [HttpPut("{nodeId}")]
        public IActionResult UpdateNode(NodeDto nodeDto)
        {
            throw new NotImplementedException(nameof(UpdateNode));
        }
    }
}
