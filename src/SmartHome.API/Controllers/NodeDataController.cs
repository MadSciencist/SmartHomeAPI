using System;
using System.Collections.Generic;
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
    [ApiVersion("1")]
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
        [HttpGet("node/{nodeId}")]
        public async Task<IActionResult> GetPaged(int nodeId, int? page, int? pageSize, [FromQuery] string[] properties, DateTime? from, DateTime? to)
        {
            int pageInt = page ?? 1;
            int pageSizeInt = pageSize ?? 1000;
            DateTime dateFrom = from ?? new DateTime(2000, 1, 1);
            DateTime dateTo = to ?? DateTime.Now;

            var serviceResult = await _nodeDataService.GetNodeData(nodeId, pageInt, pageSizeInt, properties, dateFrom, dateTo);

            return ControllerResponseHelper.GetDefaultResponse(serviceResult);
        }

        [AllowAnonymous]
        [HttpGet("node/test")]
        public async Task<IActionResult> Test()
        {
            var dict = new Dictionary<string, ICollection<int>>();
            dict.Add("First", new List<int>());
            dict.Add("Sec", new List<int>());

            return Ok(new {d = dict, meta = new {a = 1, b = 2}});
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


