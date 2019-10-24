using Matty.Framework;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SmartHome.API.Utils;
using SmartHome.Core.Dto;
using SmartHome.Core.Services.Abstractions;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SmartHome.API.Controllers
{
    [ApiController]
    [Authorize]
    [Route("api/[controller]")]
    [Produces("application/json")]
    public class PhysicalPropertiesController : ControllerBase
    {
        private readonly IPhysicalPropertyService _physicalPropertyService;

        public PhysicalPropertiesController(IHttpContextAccessor contextAccessor, IPhysicalPropertyService physicalPropertyService)
        {
            _physicalPropertyService = physicalPropertyService;
            _physicalPropertyService.Principal = contextAccessor.HttpContext.User;
        }

        /// <summary>
        /// List all Physical Properties.
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [ProducesResponseType(typeof(ServiceResult<IEnumerable<PhysicalPropertyDto>>), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetAll()
        {
            var serviceResult = await _physicalPropertyService.GetAll();

            return ControllerResponseHelper.GetDefaultResponse(serviceResult);
        }
    }
}