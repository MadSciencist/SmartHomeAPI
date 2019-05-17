using SmartHome.Core.Domain.Entity;
using System.Threading.Tasks;

namespace SmartHome.Core.DataAccess.Repository
{
    public interface INodeRepository : IGenericRepository<Node>
    {
        Task<Node> GetByClientIdAsync(string clientId);
    }
}