﻿using SmartHome.Core.Entities.DictionaryEntity;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SmartHome.Core.DataAccess.Repository
{
    public interface IDictionaryRepository : IGenericRepository<Dictionary>
    {
        Task<IEnumerable<string>> GetNames();
    }
}
