using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SmartHome.API.DTO;
using SmartHome.Core.BusinessLogic;
using SmartHome.Core.Domain.Entity;
using SmartHome.Core.Domain.Enums;
using System.Threading.Tasks;

namespace SmartHome.API.Controllers
{
    [Authorize]
    [ApiController]
    [Route("api/[controller]")]
    [Produces("application/json")]
    public class NodeDataController : Controller
    {
        private readonly INodeDataService _nodeDataService;

        public NodeDataController(INodeDataService nodeDataService, IHttpContextAccessor contextAccessor)
        {
            _nodeDataService = nodeDataService;
            _nodeDataService.ClaimsOwner = contextAccessor.HttpContext.User;
        }

        [AllowAnonymous]
        [HttpPost("add")]
        public async Task<IActionResult> PostData()
        {
            //var data = await _nodeDataService.AddSingleAsync(EDataRequestReason.User, new NodeDataMagnitude
            //{
            //    Magnitude = "temperature",
            //    Unit = "celc",
            //    Value = "25"
            //});

            //var response = new ResponseDtoContainer<NodeData>
            //{
            //    Data = data
            //};

            return Ok();
        }
    }
}


