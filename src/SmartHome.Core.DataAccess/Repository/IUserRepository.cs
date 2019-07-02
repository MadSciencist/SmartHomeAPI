using System.Collections.Generic;
using System.Threading.Tasks;

namespace SmartHome.Core.DataAccess.Repository
{
    public interface IUserRepository
    {
        Task<IEnumerable<int>> GetAllUserNodes(int userId);
        Task<IEnumerable<int>> GetUserCreatedNodes(int userId);
        Task<IEnumerable<int>> GetUserEligibleNodes(int userId);
    }
}