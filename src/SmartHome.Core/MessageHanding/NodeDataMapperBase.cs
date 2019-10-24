using Matty.Framework.Utils;
using SmartHome.Core.Dto;
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
        public string GetMapping(string property)
        {
            return DictionaryUtils.GetValue(_mappings, property);
        }

        public Type GetConverter(string magnitude)
        {
            return DictionaryUtils.GetValue(_converters, magnitude);
        }

        public async Task<PhysicalPropertyDto> GetPhysicalPropertyByMagnitudeAsync(string magnitude)
        {
            var serviceResult = await PhysicalPropertyService.GetByMagnitudeAsync(magnitude);
            return serviceResult.Data;
        }

        public async Task<IEnumerable<PhysicalPropertyDto>> GetAllContractPhysicalProperties()
        {
            var mappedMagnitudes = _mappings.Select(x => x.Value);
            var serviceResult = await PhysicalPropertyService.GetFilteredByMagnitudes(mappedMagnitudes);
            return serviceResult.Data;
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
