using System;
using SmartHome.Core.Domain;
using SmartHome.Core.MessageHanding;

namespace SmartHome.Contracts.GenericRest
{
    public class Mappings : NodeDataMapperBase, INodeDataMapper
    {
        public Mappings()
        {
            base.ValidProperties = SystemMagnitudes.Properties;
        }
    }
}
