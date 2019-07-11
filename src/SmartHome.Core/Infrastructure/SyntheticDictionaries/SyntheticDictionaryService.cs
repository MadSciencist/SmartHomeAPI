using SmartHome.Core.Domain.DictionaryEntity;
using SmartHome.Core.Infrastructure.AssemblyScanning;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using SmartHome.Core.Utils;

namespace SmartHome.Core.Infrastructure.SyntheticDictionaries
{
    // TODO refactor, some caching?
    /// <summary>
    /// Synthetic dictionaries are concepts of enumerating entities present in system
    /// Which are not actually stored in DB dictionary-related tables
    /// </summary>
    public class SyntheticDictionaryService
    {
        public ICollection<Dictionary> Dictionaries { get; private set; }

        public void Initialize()
        {
            FillDictionaries();
        }
        
        public bool HasDictionary(string name)
            => Dictionaries.Any(x => x.Name.CompareInvariant(name));

        public IEnumerable<string> GetNames()
            => Dictionaries.Select(x => x.Name);

        public Dictionary GetDictionary(string name) 
            => Dictionaries.SingleOrDefault(x => x.Name.CompareInvariant(name));

        private void FillDictionaries()
        {
            Dictionaries = Dictionaries ?? new List<Dictionary>();

            // Add new DeviceType dict which will contain assemblies metadata (not names)
            IDictionary<string, IEnumerable<Type>> executorTypes = AssemblyScanner.GetCommandExecutors();

            foreach (var executorType in executorTypes)
            {
                var asmLocation = executorType.Value.FirstOrDefault()?.Assembly?.Location;
                if (asmLocation is null) continue;

                var info = FileVersionInfo.GetVersionInfo(asmLocation);
                Dictionaries.Add(new Dictionary
                {
                    Name = $"{info.ProductName}-commands",
                    Description = info.Comments,
                    Metadata = "synthetic=true",
                    ReadOnly = true,
                    Values = executorType.Value.Select(x => new DictionaryValue
                    {
                        InternalValue = x.Name,
                        DisplayValue = x.GetAttributes<DisplayNameAttribute>()?.FirstOrDefault()?.DisplayName,
                        Id = 0,
                        IsActive = true
                    }).ToList()
                });
            }
        }
    }
}
