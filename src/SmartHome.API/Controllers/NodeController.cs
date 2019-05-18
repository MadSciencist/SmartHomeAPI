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
        [ProducesResponseType(typeof(ResponseDtoContainer<CreateNodeDto>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ResponseDtoContainer<object>), StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> Create(CreateNodeDto node)
        {
            var response = new ResponseDtoContainer<object>();

            try
            {
                response.Data = await _nodeService.CreateNode(node);
                return Ok(response);
            }
            catch (SmartHomeException ex)
            {
                response.Errors.Add(ExceptionLogHelper.CreateErrorDetails(ex.Message, (int)HttpStatusCode.BadRequest, HttpContext));
                return BadRequest(response);
            }
        }

        [HttpPost("{nodeId}/command/{command}")]
        public async Task<IActionResult> ExecuteCommand(int nodeId, string command, JObject commandParams)
        {
            var response = new ResponseDtoContainer<object>();

            try
            {
                response.Data = await _nodeService.Control(nodeId, command, commandParams);
                return Ok(response);
            }
            catch (SmartHomeException ex)
            {
                response.Errors.Add(ExceptionLogHelper.CreateErrorDetails(ex.Message, (int) HttpStatusCode.BadRequest, HttpContext));
                return BadRequest(response);
            }
        }
        
        //[HttpGet("{nodeId}/commands")]
        //public async Task<IActionResult> GetCommands(int nodeId)
        //{
        //    return Ok(_nodeService.GetNodeCommands(this.User, nodeId));
        //}


    }
}
