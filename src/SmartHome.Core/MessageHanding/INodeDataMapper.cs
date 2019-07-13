using SmartHome.Core.Domain.Models;
using System.Collections.Generic;

namespace SmartHome.Core.MessageHanding
{
    public interface INodeDataMapper
    {
        IDictionary<string, string> Mapping { get; }
        ICollection<PhysicalProperty> ValidProperties { get; }

        bool IsPropertyValid(string property);
    }
}
