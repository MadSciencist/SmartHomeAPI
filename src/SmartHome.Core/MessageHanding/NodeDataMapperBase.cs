using SmartHome.Core.Domain.Models;
using System.Collections.Generic;
using System.Linq;

namespace SmartHome.Core.MessageHanding
{
    public abstract class NodeDataMapperBase : INodeDataMapper
    {
        public virtual IDictionary<string, string> Mapping { get; protected set; }
        public virtual ICollection<PhysicalProperty> ValidProperties { get; protected set; }

        public virtual bool IsPropertyValid(string property)
        {
            return ValidProperties.Any(x => x.Magnitude == property);
        }
    }
}
