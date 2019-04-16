using Autofac;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SmartHome.API.Persistence;
using SmartHome.DeviceController;
using System.Threading.Tasks;
using SmartHome.API.Services;

namespace SmartHome.API.Controllers
{
    [Authorize]
    [ApiController]
    [Route("api/[controller]")]
    [Produces("application/json")]
    public class NodeController : ControllerBase
    {
        private readonly INodeService _nodeService;
        private readonly AppIdentityDbContext _context;
        private readonly ILifetimeScope _container;

        public NodeController(INodeService crudNodeService, AppIdentityDbContext context, ILifetimeScope container)
        {
            _nodeService = crudNodeService;
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
        [HttpGet("{test}")]
        public async Task<IActionResult> A(string test)
        {
            var strategy = _container.ResolveNamed(test, typeof(IControlStrategy)) as IControlStrategy;
            
            strategy.Execute();
            return Ok();
        }



        //[AllowAnonymous]
        //[HttpGet("test")]
        //public async Task<IActionResult> A()
        //{
        //    var nodesWithIncludedUsers = _context.Nodes.Include(x => x.AllowedUsers).ThenInclude(x => x.User);
        //    return Ok(usnodesWithIncludedUsersers);
        //}
    }
}
