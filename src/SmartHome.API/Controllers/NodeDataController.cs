using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using SmartHome.API.Utils;
using SmartHome.Core.Domain.Enums;
using SmartHome.Core.Services;
using System;
using System.Threading.Tasks;

namespace SmartHome.API.Controllers
{
    [Authorize]
    [ApiController]
    [ApiVersion("1")]
    [Route("api/[controller]")]
    [Produces("application/json")]
    public class NodeDataController : Controller
    {
        private readonly INodeDataService _nodeDataService;
        private readonly IConfiguration _config;

        public NodeDataController(INodeDataService nodeDataService, IHttpContextAccessor contextAccessor, IConfiguration config)
        {
            _nodeDataService = nodeDataService;
            _config = config;
            _nodeDataService.Principal = contextAccessor.HttpContext.User;
        }

        [AllowAnonymous]
        [HttpGet("node/{nodeId}")]
        public async Task<IActionResult> GetPaged(int nodeId, [FromQuery] string[] properties, int? page, int? pageSize,
            DateTime? from, DateTime? to, DataOrder? orderByDate)
        {
            int pageInt = page ?? _config.GetValue<int>("Defaults:Paging:PageNumber");
            int pageSizeInt = pageSize ?? _config.GetValue<int>("Defaults:Paging:PageSize");
            DateTime dateFrom = from ?? _config.GetValue<DateTime>("Defaults:Paging:DateFrom");
            DateTime dateTo = to ?? DateTime.Now;
            DataOrder order = orderByDate ?? DataOrder.Asc;

            var serviceResult = await _nodeDataService.GetNodeData(nodeId, pageInt, pageSizeInt, properties, dateFrom, dateTo, order);

            return ControllerResponseHelper.GetDefaultResponse(serviceResult);
        }

        //[AllowAnonymous]
        ////Todo maybe extra entpoint with basic auth?
        //[HttpPost("add")]
        //public async Task<IActionResult> PostData()
        //{
        //    var data = await _nodeDataService.AddSingleAsync(EDataRequestReason.User, new NodeDataMagnitude
        //    {
        //        Magnitude = "temperature",
        //        Unit = "celc",
        //        Value = "25"
        //    });

        //    var response = new ResponseDtoContainer<NodeData>
        //    {
        //        Data = data
        //    };

        //    return Ok();
        //}
    }
}


