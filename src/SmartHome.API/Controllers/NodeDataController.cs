using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json.Linq;
using SmartHome.API.Utils;
using SmartHome.Core.Domain.Enums;
using SmartHome.Core.Dto;
using SmartHome.Core.MessageHanding;
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
        private readonly IMessageProcessor<RestMessageDto> _messageProcessor;
        private readonly IConfiguration _config;

        public NodeDataController(INodeDataService nodeDataService,
            IMessageProcessor<RestMessageDto> messageProcessor,
            IHttpContextAccessor contextAccessor,
            IConfiguration config)
        {
            _nodeDataService = nodeDataService;
            _messageProcessor = messageProcessor;
            _config = config;
            _nodeDataService.Principal = contextAccessor.HttpContext.User;
        }


        /// <summary>
        /// Get node data
        /// </summary>
        /// <param name="nodeId">Id of node</param>
        /// <param name="properties">Collection of included properties</param>
        /// <param name="page">Page number</param>
        /// <param name="pageSize">Size of page</param>
        /// <param name="from">Date from (UTC)</param>
        /// <param name="to">Date to (UTC)</param>
        /// <param name="orderByDate">Order by time (ASC, DESC)</param>
        /// <returns></returns>
        //[HttpGet("node/{nodeId}")]
        //public async Task<IActionResult> GetPaged(int nodeId, [FromQuery] string[] properties, int? page, int? pageSize,
        //    DateTime? from, DateTime? to, DataOrder? orderByDate)
        //{
        //    int pageInt = page ?? _config.GetValue<int>("Defaults:Paging:PageNumber");
        //    int pageSizeInt = pageSize ?? _config.GetValue<int>("Defaults:Paging:PageSize");
        //    DateTime dateFrom = from ?? _config.GetValue<DateTime>("Defaults:Paging:DateFrom");
        //    DateTime dateTo = to ?? DateTime.Now;
        //    DataOrder order = orderByDate ?? DataOrder.Asc;

        //    var serviceResult = await _nodeDataService.GetNodeData(nodeId, pageInt, pageSizeInt, properties, dateFrom, dateTo, order);

        //    return serviceResult.Data is null ? NotFound() : ControllerResponseHelper.GetDefaultResponse(serviceResult);
        //}

        [HttpGet("node/{nodeId}")]
        public async Task<IActionResult> GetPaged(int nodeId, [FromQuery] string[] properties, int? page, int? pageSize,
            DateTime? from, DateTime? to, DataOrder? orderByDate)
        {
            int pageInt = page ?? _config.GetValue<int>("Defaults:Paging:PageNumber");
            int pageSizeInt = pageSize ?? _config.GetValue<int>("Defaults:Paging:PageSize");
            DateTime dateFrom = from ?? _config.GetValue<DateTime>("Defaults:Paging:DateFrom");
            DateTime dateTo = to ?? DateTime.Now;
            DataOrder order = orderByDate ?? DataOrder.Asc;
            int maxCount = _config.GetValue<int>("Defaults:MaxCount");

            var paged = from is null && to is null;

            var serviceResult = await _nodeDataService.GetNodeDatas(nodeId, pageInt, pageSizeInt, properties, dateFrom, dateTo, order, maxCount, paged);

            return serviceResult.Data is null ? NotFound() : ControllerResponseHelper.GetDefaultResponse(serviceResult);
        }

        //TODO special node endpoint
        //Todo maybe extra endpoint with basic auth?
        [AllowAnonymous]
        //[Authorize(Policy = "sensor")]
        [HttpPost("clientId/{clientId}")]
        public async Task<IActionResult> AddNewNodeData(string clientId, JObject payload)
        {
            await _messageProcessor.Process(new RestMessageDto
            {
                ClientId = clientId,
                Payload = payload,
            });

            return Ok();
        }
    }
}


