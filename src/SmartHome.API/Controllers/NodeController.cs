using Autofac;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SmartHome.Core.Control;
using SmartHome.Core.Persistence;
using SmartHome.Core.Services;
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
        private readonly AppDbContext _context;
        private readonly ILifetimeScope _container;

        public NodeController(INodeService nodeService, AppDbContext context, ILifetimeScope container)
        {
            _nodeService = nodeService;
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

        [AllowAnonymous]
        [HttpPost("getState/{nodeId}")]
        public async Task<IActionResult> Control(int nodeId)
        {
            var response = await _nodeService.Control(nodeId, new ControlCommand{ CommandType = EControlCommand.GetState });
            return Ok(response);
        }
    }
}
