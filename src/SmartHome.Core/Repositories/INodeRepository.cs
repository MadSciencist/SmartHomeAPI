using Matty.Framework.Abstractions;
using SmartHome.Core.Entities.Entity;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SmartHome.Core.Repositories
{
    public interface INodeRepository : IGenericRepository<Node>
    {
        Task<IEnumerable<string>> GetAllClientIdsAsync();
        Task<Node> GetByClientIdAsync(string clientId);
    }
}