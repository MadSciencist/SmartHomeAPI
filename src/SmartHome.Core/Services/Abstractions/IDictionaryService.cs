using SmartHome.Core.Entities.DictionaryEntity;
using SmartHome.Core.Infrastructure;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SmartHome.Core.Services.Abstractions
{
    public interface IDictionaryService : IServiceBase
    {
        Task<ServiceResult<List<string>>> GetDictionaryNames();
        Task<ServiceResult<Dictionary>> GetDictionaryByName(string name);
        Task<ServiceResult<Dictionary>> AddNewEntry(string name, DictionaryValue entry);
        Task<ServiceResult<Dictionary>> DeleteEntry(string dictionaryName, int entryId);
        Task<ServiceResult<Dictionary>> UpdateEntry(string dictionaryName, int entryId, DictionaryValue entry);
    }
}