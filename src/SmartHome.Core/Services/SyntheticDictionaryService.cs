using Autofac;
using Microsoft.Extensions.Caching.Memory;
using SmartHome.Core.Entities.DictionaryEntity;
using SmartHome.Core.Infrastructure.AssemblyScanning;
using SmartHome.Core.Infrastructure.Attributes;
using SmartHome.Core.MessageHanding;
using SmartHome.Core.Utils;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using Autofac.Core;
using SmartHome.Core.DataAccess.Repository;
using SmartHome.Core.Entities.SchedulingEntity;

namespace SmartHome.Core.Services
{
    /// <summary>
    /// Synthetic dictionaries are concepts of enumerating entities present in system
    /// Which are not actually stored in DB dictionary-related tables
    /// </summary>
    public class SyntheticDictionaryService
    {
        public ICollection<Dictionary> Dictionaries { get; private set; }

        private readonly ILifetimeScope _container;
        private readonly IMemoryCache _cache;
        private const string CacheKey = "_syntheticDict";

        public SyntheticDictionaryService(ILifetimeScope container, IMemoryCache cache)
        {
            _container = container;
            _cache = cache;
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
            if (_cache.TryGetValue<List<Dictionary>>(CacheKey, out var dict))
            {
                Dictionaries = dict;
                return;
            }

            Dictionaries = new List<Dictionary>();
            AddContractsDict();
            AddCommandExecutorsDict();
            AddContractPropertiesDict();
            AddJobTypesDict();

            _cache.Set(CacheKey, Dictionaries, TimeSpan.FromMinutes(30));
        }

        private void AddContractPropertiesDict()
        {
            IDictionary<string, IEnumerable<Type>> mappers = AssemblyScanner.GetDataMappers();
            foreach (var mapper in mappers)
            {
                var asmLocation = mapper.Value.FirstOrDefault()?.Assembly?.Location;
                if (asmLocation is null) continue;

                var info = FileVersionInfo.GetVersionInfo(asmLocation);

                var mapperName = mapper.Value.First()?.FullName;
                var dataMapper = _container.ResolveNamed<object>(mapperName) as INodeDataMapper;
                if (dataMapper is null) throw new InvalidOperationException($"{asmLocation} has no mapper");

                Dictionaries.Add(new Dictionary
                {
                    Name = $"{info.ProductName}-properties",
                    Description = info.Comments,
                    Metadata = "synthetic=true",
                    ReadOnly = true,
                    Values = dataMapper.GetAllContractPhysicalProperties().Select(x => new DictionaryValue
                    {
                        DisplayValue = string.IsNullOrEmpty(x.Description) ? x.Magnitude : x.Description,
                        InternalValue = x.Magnitude,
                        Metadata = $"unit={x.Unit}",
                        IsActive = true
                    }).ToList()
                });
            }
        }

        private void AddContractsDict()
        {
            ICollection<FileVersionInfo> contracts = AssemblyScanner.GetContractAssembliesInfo();

            Dictionaries.Add(new Dictionary
            {
                Name = "contracts",
                Description = "Available device types",
                Metadata = "synthetic=true",
                ReadOnly = true,
                Values = contracts.Select(x => new DictionaryValue
                {
                    DisplayValue = x.Comments,
                    InternalValue = x.ProductName,
                    IsActive = true
                }).ToList()
            });
        }

        private void AddCommandExecutorsDict()
        {
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
                        DisplayValue = x.GetAttribute<DisplayTextAttribute>()?.Text,
                        IsActive = true
                    }).ToList()
                });
            }
        }

        private void AddJobTypesDict()
        {
            var jobRepo = _container.Resolve<IGenericRepository<JobType>>();
            var jobs = jobRepo.GetAll();

            Dictionaries.Add(new Dictionary
            {
                Name = nameof(JobType),
                Description = string.Empty,
                Metadata = "synthetic=true",
                ReadOnly = true,
                Values = jobs.Select(job => new DictionaryValue
                {
                    InternalValue = job.Id.ToString(),
                    DisplayValue = job.DisplayName,
                    IsActive = true
                }).ToList()
            });
        }
    }
}
