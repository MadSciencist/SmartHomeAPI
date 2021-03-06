﻿using Autofac;
using Microsoft.Extensions.Caching.Memory;
using SmartHome.Core.Entities.Entity;
using SmartHome.Core.Repositories;
using SmartHome.Core.Services.Abstractions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SmartHome.Core.Services
{
    public class PhysicalPropertyService : ServiceBase, IPhysicalPropertyService
    {
        private readonly IPhysicalPropertyRepository _propertyRepository;
        private readonly IMemoryCache _cache;
        private const string CacheKey = "_physicalProperty";
        private TimeSpan CacheLifeTime => TimeSpan.FromHours(12);

        public PhysicalPropertyService(ILifetimeScope container, IPhysicalPropertyRepository propertyRepository, IMemoryCache cache) : base(container)
        {
            _propertyRepository = propertyRepository;
            _cache = cache;
        }

        public async Task<IEnumerable<PhysicalProperty>> GetAll()
        {
            return GetFromCache() ?? await StoreInCache();
        }

        public async Task<IEnumerable<PhysicalProperty>> GetFilteredByMagnitudes(IEnumerable<string> magnitudes)
        {
            var properties = GetFromCache() ?? await StoreInCache();

            return properties.Where(property => magnitudes.Any(mag => mag == property.Magnitude));
        }

        private IEnumerable<PhysicalProperty> GetFromCache()
        {
            return _cache.TryGetValue<List<PhysicalProperty>>(CacheKey, out var properties) ? properties : null;
        }

        private async Task<IEnumerable<PhysicalProperty>> StoreInCache()
        {
            var properties = await _propertyRepository.GetAllAsync();
            return _cache.Set(CacheKey, properties, CacheLifeTime);
        }

        public async Task<PhysicalProperty> GetByMagnitudeAsync(string magnitude)
        {
            var properties = GetFromCache() ?? await StoreInCache();

            return properties.FirstOrDefault(x => x.Magnitude == magnitude);
        }
    }
}
