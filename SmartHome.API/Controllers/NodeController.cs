using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using SmartHome.API.Persistence.App;
using SmartHome.API.Services.Crud;

namespace SmartHome.API.Controllers
{
    [Authorize]
    [ApiController]
    [Route("api/[controller]")]
    public class NodeController : ControllerBase
    {
        private readonly ICrudNodeService _crudNodeService;
        private readonly AppDbContext _context;

        public NodeController(ICrudNodeService crudNodeService, AppDbContext context)
        {
            _crudNodeService = crudNodeService;
            _context = context;
        }

        [HttpGet("create")]
        public async Task<IActionResult> Create()
        {
            await _crudNodeService.CreateNode(this.User, "TestNodeName", "TestIdentifier", "aaaa", "sensor");
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
