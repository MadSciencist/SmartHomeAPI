using SmartHome.Core.Domain.DictionaryEntity;
using SmartHome.Core.Services;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using SmartHome.Core.Infrastructure.AssemblyScanning;

namespace SmartHome.Core.Infrastructure.SyntheticDictionaries
{
    // TODO refactor, some caching?
    public class SyntheticDictionaryService
    {
        public ICollection<Dictionary> Dictionaries { get; private set; }

        public void Initialize()
        {
            FillDictionaries();
        }

        private void FillDictionaries()
        {
            Dictionaries = Dictionaries ?? new List<Dictionary>();

            // Add new DeviceType dict which will contain assemblies metadata (not names)
            var executorTypes = AssemblyScanner.GetCommandExecutors();

            foreach (var executorType in executorTypes)
            {
                var asmLocation = executorType.First().Value?.Assembly?.Location;
                if (asmLocation is null) continue;

                var info = FileVersionInfo.GetVersionInfo(asmLocation);
                Dictionaries.Add(new Dictionary
                {
                    Name = $"{info.ProductName}-commands",
                    Description = info.Comments,
                    Metadata = "synthetic,command",
                    ReadOnly = true,
                    Values = executorType.Values.Select(x => new DictionaryValue
                    {
                        InternalValue = x.Name,
                        DisplayValue = x.GetAttributes<DisplayNameAttribute>()?.FirstOrDefault()?.DisplayName,
                        Id = 0,
                        IsActive = true
                    }).ToList()
                });
            }

        }

        public bool HasDictionary(string name)
        {
            return Dictionaries.Any(x => string.Equals(x.Name, name, StringComparison.InvariantCultureIgnoreCase));
        }

        public IEnumerable<string> GetNames()
        {
            return Dictionaries.Select(x => x.Name);
        }

        public Dictionary GetDictionary(string name)
        {
            return Dictionaries.SingleOrDefault(x =>
                string.Equals(x.Name, name, StringComparison.InvariantCultureIgnoreCase));
        }
    }
}
