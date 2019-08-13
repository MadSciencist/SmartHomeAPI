using SmartHome.Core.Domain;
using SmartHome.Core.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;

namespace SmartHome.Core.MessageHanding
{
    public abstract class NodeDataMapperBase : INodeDataMapper
    {
        public IDictionary<string, string> Mapping { get; protected set; }
        public IDictionary<string, Type> Converters { get; protected set; }

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
            return Mapping.Keys.Any(x => x == property);
        }

        public PhysicalProperty GetPhysicalPropertyByContractMagnitude(string magnitude)
        {
            var systemMagnitude = Mapping[magnitude];
            return SystemMagnitudes.Properties.SingleOrDefault(x => x.Magnitude == systemMagnitude);
        }

        public ICollection<PhysicalProperty> GetAllContractPhysicalProperties()
        {
            var intersection = SystemMagnitudes.Properties.Select(x => x.Magnitude).Intersect(Mapping.Values);
            return SystemMagnitudes.Properties.Where(x => intersection.Contains(x.Magnitude)).ToList();
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
