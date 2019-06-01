﻿using SmartHome.Core.Domain.DictionaryEntity;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SmartHome.Core.Services
{
    public interface IDictionaryService : IServiceBase
    {
        Task<IEnumerable<string>> GetDictionaryNames();
        Task<Dictionary> GetDictionaryByName(string name);
    }
}