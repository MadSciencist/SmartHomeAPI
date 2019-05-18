using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json.Linq;
using SmartHome.API.DTO;
using SmartHome.API.Utils;
using SmartHome.Core.Infrastructure;
using System.Net;
using System.Threading.Tasks;
using SmartHome.Core.Services;

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

        [HttpPost("{nodeId}/command/{command}")]
        public async Task<IActionResult> ExecuteCommand(int nodeId, string command, JObject commandParams)
        {
            var response = new ResponseDtoContainer<object>();

            try
            {
                response.Data = await _nodeService.Control(nodeId, command, commandParams);
            }
            catch (SmartHomeException ex)
            {
                response.Errors.Add(ExceptionLogHelper.CreateErrorDetails(ex.Message, (int)HttpStatusCode.BadRequest, HttpContext));
                return BadRequest(response);
            }

            return Ok(response);
        }
        
        //[HttpGet("{nodeId}/commands")]
        //public async Task<IActionResult> GetCommands(int nodeId)
        //{
        //    return Ok(_nodeService.GetNodeCommands(this.User, nodeId));
        //}

        //[HttpGet("create")]
        //public async Task<IActionResult> Create()
        //{
        //    await _nodeService.CreateNode("TestNodeName", "TestIdentifier", "aaaa", "sensor");
        //    //ModelState.AddModelError();

        //    return Ok();
        //}

        //[HttpPost("{nodeId}/getState")]
        //public async Task<IActionResult> GetState(int nodeId)
        //{
        //    //var response = await _nodeService.Control(nodeId, new NodeCommand();
        //    return Ok();
        //}
    }
}
