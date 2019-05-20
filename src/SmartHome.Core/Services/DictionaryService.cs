using Microsoft.EntityFrameworkCore;
using SmartHome.Core.DataAccess.Repository;
using SmartHome.Core.Domain.DictionaryEntity;
using SmartHome.Core.Utils;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SmartHome.Core.Services
{
    public class DictionaryService : ServiceBase, IUserAuditable, IDictionaryService
    {
        private readonly IGenericRepository<Dictionary> _dictRepository;

        public DictionaryService(IGenericRepository<Dictionary> dictRepository)
        {
            _dictRepository = dictRepository;
        }

        public async Task<IEnumerable<string>> GetDictionaryNames()
        {
            return await _dictRepository.AsQueryableNoTrack()
                .Distinct()
                .Select(x => x.Name)
                .ToListAsync();
        }

        public async Task<Dictionary> GetDictionaryByName(string name)
        {
            return await _dictRepository.AsQueryableNoTrack()
                .Include(x => x.Values)
                .FirstOrDefaultAsync(x => x.Name == name);
        }
    }
}
