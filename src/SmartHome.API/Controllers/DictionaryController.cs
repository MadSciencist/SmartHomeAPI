using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SmartHome.Core.Domain.DictionaryEntity;
using SmartHome.Core.Services;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SmartHome.API.Controllers
{
    [Authorize]
    [ApiController]
    [Route("api/[controller]")]
    [Produces("application/json")]
    public class DictionaryController : ControllerBase
    {
        private readonly IDictionaryService _dictionaryService;

        public DictionaryController(IHttpContextAccessor contextAccessor, IDictionaryService dictionaryService)
        {
            _dictionaryService = dictionaryService;
            _dictionaryService.Principal = contextAccessor.HttpContext.User;
        }

        [HttpGet("getNames")]
        [ProducesResponseType(typeof(IEnumerable<string>), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetNames()
        {
            var dictionaries = await _dictionaryService.GetDictionaryNames();

            return Ok(dictionaries);
        }

        [HttpGet("getByName/{name}")]
        [ProducesResponseType(typeof(Dictionary), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetByName(string name)
        {
            var dictionary = await _dictionaryService.GetDictionaryByName(name);

            if (dictionary == null)
                return NotFound();

            return Ok(dictionary);
        }
    }
}
