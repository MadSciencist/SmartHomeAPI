using SmartHome.Core.Domain.DictionaryEntity;
using SmartHome.Core.Utils;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SmartHome.Core.Services
{
    public interface IDictionaryService : IUserAuditable
    {
        Task<IEnumerable<string>> GetDictionaryNames();
        Task<Dictionary> GetDictionaryByName(string name);
    }
}