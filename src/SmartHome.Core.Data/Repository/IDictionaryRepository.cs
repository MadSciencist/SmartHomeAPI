using System.Collections.Generic;
using System.Threading.Tasks;
using SmartHome.Core.Entities.DictionaryEntity;

namespace SmartHome.Core.Data.Repository
{
    public interface IDictionaryRepository : IGenericRepository<Dictionary>
    {
        Task<IEnumerable<string>> GetNames();
    }
}
