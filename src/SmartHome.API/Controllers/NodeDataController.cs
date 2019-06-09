using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SmartHome.API.Utils;
using SmartHome.Core.Services;
using System.Threading.Tasks;
using SmartHome.Core.Domain.Entity;
using SmartHome.Core.Infrastructure;

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
            _nodeDataService.Principal = contextAccessor.HttpContext.User;
        }

        [AllowAnonymous]
        [HttpPost("node/{nodeId}/paged")]
        public async Task<IActionResult> Create(int nodeId, int page, int pageSize)
        {
            ServiceResult<PaginatedList<NodeData>> serviceResult = await _nodeDataService.GetNodeData(nodeId, page, pageSize);

            return ControllerResponseHelper.GetDefaultResponse(serviceResult);
        }



        [AllowAnonymous]
        //Todo maybe extra entpoint with basic auth?
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


