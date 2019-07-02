using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using SmartHome.Core.Domain.User;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SmartHome.Core.DataAccess.Repository
{
    public class UserRepository : GenericRepository<AppUser>, IUserRepository
    {
        public UserRepository(AppDbContext context, ILoggerFactory loggerFactory) : base(context, loggerFactory)
        { 
        }

        public async Task<IEnumerable<int>> GetAllUserNodes(int userId)
        {
            var user = await base.AsQueryableNoTrack()
                .Include(x => x.CreatedNodes)
                .Include(x => x.EligibleNodes)
                .FirstOrDefaultAsync(x => x.Id == userId);

            var nodes = user.CreatedNodes.Union(user.EligibleNodes.Select(x => x.Node));
            return nodes.Select(x => x.Id);
        }

        public async Task<IEnumerable<int>> GetUserCreatedNodes(int userId)
        {
            var user = await base.AsQueryableNoTrack()
                .Include(x => x.CreatedNodes)
                .FirstOrDefaultAsync(x => x.Id == userId);

            return user.CreatedNodes?.Select(x => x.Id);
        }

        public async Task<IEnumerable<int>> GetUserEligibleNodes(int userId)
        {
            var user = await base.AsQueryableNoTrack()
                .Include(x => x.EligibleNodes)
                .FirstOrDefaultAsync(x => x.Id == userId);

            return user.EligibleNodes?.Select(x => x.NodeId);
        }
    }
}
