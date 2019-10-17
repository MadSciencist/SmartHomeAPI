using Autofac;
using SmartHome.Core.Entities.Entity;
using SmartHome.Core.Services.Abstractions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SmartHome.Core.MessageHanding
{
    public abstract class NodeDataMapperBase : INodeDataMapper
    {
        public IPhysicalPropertyService PhysicalPropertyService { get; set; }

        private Dictionary<string, string> _mappings;
        private Dictionary<string, Type> _converters;

        public void AddMappings(Dictionary<string, string> mappings)
        {
            _mappings = mappings;
        }

        public void AddConverters(Dictionary<string, Type> converters)
        {
            _converters = converters;
        }

        #region ctor
        protected NodeDataMapperBase()
        {
            // ReSharper disable VirtualMemberCallInConstructor
            InitializeMapping();
            InitializeConverters();
        }
        #endregion

        #region INodeDataMapper impl
        public bool IsPropertyValid(string property)
        {
            // We assume that valid property is one which has mapping
            return _mappings.Keys.Any(x => x == property);
        }

        public string GetMapping(string property)
        {   // todo error handling
            return _mappings[property];
        }

        public Type GetConverter(string magnitude)
        {// todo error handling
            return _converters[magnitude];
        }

        public async Task<PhysicalProperty> GetPhysicalPropertyByContractMagnitudeAsync(string magnitude)
        {
            var systemMagnitude = _mappings[magnitude];
            return await PhysicalPropertyService.GetByMagnitudeAsync(systemMagnitude);
        }

        public ICollection<PhysicalProperty> GetAllContractPhysicalProperties()
        {
            //var intersection = SystemMagnitudes.Properties.Select(x => x.Magnitude).Intersect(Mapping.Values);
            //return SystemMagnitudes.Properties.Where(x => intersection.Contains(x.Magnitude)).ToList();
            return null;
        }
        #endregion

        // Required to override
        protected abstract void InitializeMapping();

        // It is possible that no conversion is required
        protected virtual void InitializeConverters()
        {
        }
    }
}
