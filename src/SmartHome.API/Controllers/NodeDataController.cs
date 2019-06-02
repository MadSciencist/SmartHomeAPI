﻿using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SmartHome.Core.Services;
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
            _nodeDataService.Principal = contextAccessor.HttpContext.User;
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


