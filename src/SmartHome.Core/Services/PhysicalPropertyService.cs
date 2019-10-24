using Autofac;
using Microsoft.Extensions.Caching.Memory;
using SmartHome.Core.Entities.Entity;
using SmartHome.Core.Repositories;
using SmartHome.Core.Services.Abstractions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Matty.Framework;
using SmartHome.Core.Dto;

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

        public async Task<ServiceResult<IEnumerable<PhysicalPropertyDto>>> GetAll()
        {
            var physicalProperties = GetFromCache() ?? await StoreInCache();

            return new ServiceResult<IEnumerable<PhysicalPropertyDto>>(Principal)
            {
                Data = Mapper.Map<IEnumerable<PhysicalPropertyDto>>(physicalProperties)
            };
        }

        public async Task<ServiceResult<IEnumerable<PhysicalPropertyDto>>> GetFilteredByMagnitudes(IEnumerable<string> magnitudes)
        {
            var properties = GetFromCache() ?? await StoreInCache();
            var filtered = properties.Where(property => magnitudes.Any(mag => mag == property.Magnitude));

            return new ServiceResult<IEnumerable<PhysicalPropertyDto>>(Principal)
            {
                Data = Mapper.Map<IEnumerable<PhysicalPropertyDto>>(filtered)
            };
        }

        public async Task<ServiceResult<PhysicalPropertyDto>> GetByMagnitudeAsync(string magnitude)
        {
            var properties = GetFromCache() ?? await StoreInCache();
            var matched = properties.FirstOrDefault(x => x.Magnitude == magnitude);

            return new ServiceResult<PhysicalPropertyDto>(Principal)
            {
                Data = Mapper.Map<PhysicalPropertyDto>(matched)
            };
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
    }
}
