using System.Collections.Generic;
using System.Threading.Tasks;
using SmartHome.Core.Entities.Entity;

namespace SmartHome.Core.Data.Repository
{
    public interface INodeRepository : IGenericRepository<Node>
    {
        Task<IEnumerable<string>> GetAllClientIdsAsync();
        Task<Node> GetByClientIdAsync(string clientId);
    }
}