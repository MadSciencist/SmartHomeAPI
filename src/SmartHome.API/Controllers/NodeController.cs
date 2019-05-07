using System;
using Autofac;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SmartHome.Core.Control;
using SmartHome.Core.Persistence;
using System.Threading.Tasks;
using SmartHome.API.DTO;
using SmartHome.Core.BusinessLogic;
using SmartHome.Domain.Entity;
using Newtonsoft.Json.Linq;
using SmartHome.Core.Infrastructure;
using SmartHome.API.Utils;
using System.Net;

namespace SmartHome.API.Controllers
{
    [Authorize]
    [ApiController]
    [Route("api/[controller]")]
    [Produces("application/json")]
    public class NodeController : ControllerBase
    {
        private readonly INodeService _nodeService;
        private readonly AppDbContext _context;
        private readonly ILifetimeScope _container;

        public NodeController(INodeService nodeService, AppDbContext context, ILifetimeScope container)
        {
            _nodeService = nodeService;
            // _nodeService.ClaimsPrincipal = this.User; //TODO assigns null?
            _nodeService.MyProperty = "asdadsasd";

            _context = context;
            _container = container;
        }

        [HttpPost("{nodeId}/command/{command}")]
        public async Task<IActionResult> ExecuteCommand(int nodeId, string command, JObject commandParams)
        {
            var response = new DtoContainer<object>();

            try
            {
                response.Data = await _nodeService.Control(this.User, nodeId, command, commandParams);
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
        //    await _nodeService.CreateNode(this.User, "TestNodeName", "TestIdentifier", "aaaa", "sensor");
        //    //ModelState.AddModelError();

        //    return Ok();
        //}

        //[HttpPost("{nodeId}/getState")]
        //public async Task<IActionResult> GetState(int nodeId)
        //{
        //    //var response = await _nodeService.Control(this.User, nodeId, new NodeCommand();
        //    return Ok();
        //}
    }
}
