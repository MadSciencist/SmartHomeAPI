using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using SmartHome.API.Services.Crud;

namespace SmartHome.API.Controllers
{
    [Authorize]
    [ApiController]
    [Route("api/[controller]")]
    public class NodeController : ControllerBase
    {
        private readonly ICrudNodeService _crudNodeService;

        public NodeController(ICrudNodeService crudNodeService)
        {
            _crudNodeService = crudNodeService;
        }
        [HttpGet("create")]
        public async Task<IActionResult> Create()
        {
            await _crudNodeService.CreateNode(this.User, "TestNodeName", "TestIdentifier", "aaaa", "sensor");
            return Ok();
        }
    }
}
