using Autofac;
using Microsoft.EntityFrameworkCore;
using SmartHome.Core.Entities.DictionaryEntity;
using SmartHome.Core.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace SmartHome.Core.Data.Repository
{
    public class DictionaryRepository : GenericRepository<Dictionary>, IDictionaryRepository
    {
        public DictionaryRepository(ILifetimeScope container) : base(container)
        {
        }

        public override async Task<IEnumerable<Dictionary>> GetAllAsync()
        {
            return await Context.Dictionaries
                .Include(x => x.Values)
                .ToListAsync();
        }

        public override async Task<Dictionary> GetByIdAsync(int id)
        {
            return await Context.Dictionaries
                .Include(x => x.Values)
                .SingleOrDefaultAsync(x => x.Id == id);
        }

        public override async Task<Dictionary> GetFiltered(Expression<Func<Dictionary, bool>> predicate)
        {
            return await Context.Dictionaries
                .Include(x => x.Values)
                .FirstOrDefaultAsync(predicate);
        }

        public async Task<IEnumerable<string>> GetNames()
        {
            return await Context.Dictionaries
                .Select(x => x.Name)
                .Distinct()
                .ToListAsync();
        }
    }
}
