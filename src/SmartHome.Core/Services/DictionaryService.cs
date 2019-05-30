﻿using Microsoft.EntityFrameworkCore;
using SmartHome.Core.DataAccess.Repository;
using SmartHome.Core.Domain.DictionaryEntity;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SmartHome.Core.Services
{
    public class DictionaryService : ServiceBase<object, Dictionary>, IDictionaryService
    {

        public DictionaryService(IGenericRepository<Dictionary> dictRepository) : base(dictRepository)
        {
        }

        public async Task<IEnumerable<string>> GetDictionaryNames()
        {
            return await GenericRepository.AsQueryableNoTrack()
                .Distinct()
                .Select(x => x.Name)
                .ToListAsync();
        }

        public async Task<Dictionary> GetDictionaryByName(string name)
        {
            return await GenericRepository.AsQueryableNoTrack()
                .Include(x => x.Values)
                .FirstOrDefaultAsync(x => x.Name == name);
        }
    }
}
