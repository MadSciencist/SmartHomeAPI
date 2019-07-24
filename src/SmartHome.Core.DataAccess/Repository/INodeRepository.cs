using System.Collections.Generic;
using SmartHome.Core.Domain.Entity;
using System.Threading.Tasks;

namespace SmartHome.Core.DataAccess.Repository
{
    public interface INodeRepository : IGenericRepository<Node>
    {
        new Task<IEnumerable<Node>> GetAll();
        Task<IEnumerable<string>> GetAllClientIdsAsync();
        new Task<Node> GetByIdAsync(int id);
        Task<Node> GetByClientIdAsync(string clientId);
    }
}