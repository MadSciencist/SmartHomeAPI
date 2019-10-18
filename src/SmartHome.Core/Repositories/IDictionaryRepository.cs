using Matty.Framework.Abstractions;
using SmartHome.Core.Entities.DictionaryEntity;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SmartHome.Core.Repositories
{
    public interface IDictionaryRepository : ITransactionalRepository<Dictionary, int>
    {
        Task<IEnumerable<string>> GetNames();
    }
}
