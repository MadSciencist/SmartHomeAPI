using Matty.Framework.Enums;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json.Linq;
using SmartHome.API.Utils;
using SmartHome.Core.Dto;
using SmartHome.Core.MessageHanding;
using SmartHome.Core.Services.Abstractions;
using System;
using System.Linq;
using System.Threading.Tasks;
using Matty.Framework;

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

        [HttpGet("node/{nodeId}")]
        [ProducesResponseType(typeof(ServiceResult<NodeDataResultDto>), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetSingleMagnitude(int nodeId, string magnitude, int? limit)
        {
            int limitInt = limit ?? _config.GetValue<int>("Defaults:MaxCount");

            var serviceResult = await _nodeDataService.GetNodeDataByMagnitude(nodeId, magnitude, limitInt);

            if (!serviceResult.Data.Values.Any())
                serviceResult.ResponseStatusCodeOverride = StatusCodes.Status404NotFound;

            return ControllerResponseHelper.GetDefaultResponse(serviceResult);
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


