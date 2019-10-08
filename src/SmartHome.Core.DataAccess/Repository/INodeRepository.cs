using SmartHome.Core.Entities.Entity;
using System.Collections.Generic;
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