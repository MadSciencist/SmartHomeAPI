using Matty.Framework.Utils;
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
        private Dictionary<string, string> _mappings;
        private Dictionary<string, Type> _converters;

        // ReSharper disable once UnassignedGetOnlyAutoProperty
        // Autowired by Autofac
        // Dirty hack: the auto-wiring happens AFTER finishing constructor
        // So that's why initialize method was put here.
        // I did not wanted to pollute the contract classes
        // with a must of explicitly calling base constructor.
        private IPhysicalPropertyService _physicalPropertyService;
        public IPhysicalPropertyService PhysicalPropertyService
        {
            get => _physicalPropertyService;
            set
            {
                var initialize = (_physicalPropertyService == null);
                _physicalPropertyService = value;
                if (initialize)
                {
                    Initialize();
                }
            }
        }

        /// <summary>
        /// Is executed right after the constructor
        /// </summary>
        private void Initialize()
        {
            InitializeMapping();
            InitializeConverters();
        }


        #region INodeDataMapper impl
        public bool IsPropertyValid(string property)
        {
            // We assume that valid property is one which has mapping
            return _mappings.Keys.Any(x => x == property);
        }

        public string GetMapping(string property)
        {
            return DictionaryUtils.GetValue(_mappings, property);
        }

        public Type GetConverter(string magnitude)
        {
            return DictionaryUtils.GetValue(_converters, magnitude);
        }

        public async Task<PhysicalProperty> GetPhysicalPropertyByContractMagnitudeAsync(string magnitude)
        {
            var systemMagnitude = _mappings[magnitude];
            if (systemMagnitude == null) return null;
            return await PhysicalPropertyService.GetByMagnitudeAsync(systemMagnitude);
        }

        public async Task<IEnumerable<PhysicalProperty>> GetAllContractPhysicalProperties()
        {
            var mappedMagnitudes = _mappings.Select(x => x.Value);
            return await PhysicalPropertyService.GetFilteredByMagnitudes(mappedMagnitudes);
        }
        #endregion

        protected void AddMappings(Dictionary<string, string> mappings)
        {
            _mappings = mappings;
        }

        protected void AddConverters(Dictionary<string, Type> converters)
        {
            _converters = converters;
        }

        // Required to override
        protected abstract void InitializeMapping();

        protected virtual void InitializeConverters()
        {
        }
    }
}
