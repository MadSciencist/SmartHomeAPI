using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SmartHome.API.Dto;
using SmartHome.API.Utils;
using SmartHome.Core.Domain.DictionaryEntity;
using SmartHome.Core.Infrastructure;
using SmartHome.Core.Services;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SmartHome.API.Controllers
{
    [Authorize]
    [ApiController]
    [ApiVersion("1")]
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

        [HttpGet]
        [ProducesResponseType(typeof(ServiceResult<IEnumerable<string>>), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetNames()
        {
            var dictionaries = await _dictionaryService.GetDictionaryNames();

            return Ok(dictionaries);
        }

        [HttpGet("{name}")]
        [ProducesResponseType(typeof(ServiceResult<Dictionary>), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetByName(string name)
        {
            var dictionary = await _dictionaryService.GetDictionaryByName(name);

            if (dictionary == null)
                return NotFound();

            return Ok(dictionary);
        }

        [HttpPut("{dictName}/{entryId}")]
        [ProducesResponseType(typeof(ServiceResult<Dictionary>), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> UpdateEntry(string dictName, int entryId, DictionaryEntryDto entry)
        {
            var serviceDictionaryEntry = new DictionaryValue
            {
                InternalValue = entry.InternalValue,
                DisplayValue = entry.DisplayValue,
                IsActive = entry.IsActive ?? true
            };

            var serviceResult = await _dictionaryService.UpdateEntry(dictName, entryId, serviceDictionaryEntry);

            return ControllerResponseHelper.GetDefaultResponse(serviceResult);
        }

        [HttpDelete("{dictName}/{entryId}")]
        [ProducesResponseType(typeof(ServiceResult<Dictionary>), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> DeleteEntry(string dictName, int entryId)
        {
            var serviceResult = await _dictionaryService.DeleteEntry(dictName, entryId);

            return ControllerResponseHelper.GetDefaultResponse(serviceResult);
        }

        [HttpPost("{dictName}")]
        [ProducesResponseType(typeof(ServiceResult<Dictionary>), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> AddNewEntry(string dictName, DictionaryEntryDto entry)
        {
            var serviceDictionaryEntry = new DictionaryValue
            {
                InternalValue = entry.InternalValue,
                DisplayValue = entry.DisplayValue,
                IsActive = entry.IsActive ?? true
            };

            var serviceResult = await _dictionaryService.AddNewEntry(dictName, serviceDictionaryEntry);

            return ControllerResponseHelper.GetDefaultResponse(serviceResult, StatusCodes.Status201Created);
        }
    }
}
