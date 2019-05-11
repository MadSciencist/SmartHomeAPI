using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SmartHome.Domain.DictionaryEntity;
using System.Collections.Generic;
using System.Linq;
using SmartHome.Core.DataAccess.Repository;

namespace SmartHome.API.Controllers
{
    //[Authorize]
    [ApiController]
    [Route("api/[controller]")]
    [Produces("application/json")]
    public class DictionaryController : ControllerBase
    {
        private readonly IGenericRepository<Dictionary> _dictRepository;

        public DictionaryController(IGenericRepository<Dictionary> dictRepository)
        {
            _dictRepository = dictRepository;
        }

        [HttpGet("getAll")]
        [ProducesResponseType(typeof(IEnumerable<Dictionary>), StatusCodes.Status200OK)]
        public IActionResult GetAll()
        {
            IEnumerable<Dictionary> dictionaries = _dictRepository.AsQueryableNoTrack()
                .Include(x => x.Values);

            return Ok(dictionaries);
        }

        [HttpGet("getNames")]
        [ProducesResponseType(typeof(IEnumerable<string>), StatusCodes.Status200OK)]
        public IActionResult GetNames()
        {
            IEnumerable<string> dictionaries = _dictRepository.AsQueryableNoTrack()
                .Distinct()
                .Select(x => x.Name);

            return Ok(dictionaries);
        }

        [HttpGet("getByName/{name}")]
        [ProducesResponseType(typeof(Dictionary), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public IActionResult GetByName(string name)
        {
            Dictionary dictionary = _dictRepository.AsQueryableNoTrack()
                .Include(x => x.Values)
                .FirstOrDefault(x => x.Name == name);

            if (dictionary == null)
            {
                return NotFound();
            }

            return Ok(dictionary);
        }
    }
}
