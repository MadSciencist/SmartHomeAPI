using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SmartHome.API.DTO;
using SmartHome.Core.DataAccess.Repository;
using SmartHome.Core.Domain.Entity;
using SmartHome.Core.Domain.Enums;

namespace SmartHome.API.Controllers
{
    [Authorize]
    [ApiController]
    [Route("api/[controller]")]
    [Produces("application/json")]
    public class NodeDataController : Controller
    {
        private readonly INodeDataRepository _dataRepository;

        public NodeDataController(INodeDataRepository dataRepository)
        {
            _dataRepository = dataRepository;
        }

        [AllowAnonymous]
        [HttpPost("add")]
        public async Task<IActionResult> PostData()
        {
            var data = await _dataRepository.AddSingleAsync(EDataRequestReason.User, new NodeDataMagnitudes
            {
                Magnitude = "temperature",
                Unit = "celc",
                Value = "25"
            });

            var response = new DtoContainer<NodeData>
            {
                Data = data
            };

            return Ok(response);
        }
    }
}


