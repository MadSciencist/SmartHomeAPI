using Autofac;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SmartHome.Core.Control;
using SmartHome.Core.Persistence;
using System.Threading.Tasks;
using SmartHome.Core.BusinessLogic;

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

        [HttpGet("create")]
        public async Task<IActionResult> Create()
        {
            await _nodeService.CreateNode(this.User, "TestNodeName", "TestIdentifier", "aaaa", "sensor");
            //ModelState.AddModelError();

            return Ok();
        }

        [HttpPost("getState/{nodeId}")]
        public async Task<IActionResult> GetState(int nodeId)
        {
            var response = await _nodeService.Control(this.User, nodeId, new ControlCommand { CommandType = EControlCommand.GetState });
            return Ok(response);
        }

        [HttpPost("setState/{nodeId}")]
        public async Task<IActionResult> SetState(int nodeId)
        {
            var command = new ControlCommand
            {
                CommandType = EControlCommand.SetState,
                Payload = ""
            };
            var response = await _nodeService.Control(this.User, nodeId, command);
            return Ok(response);
        }
    }
}
