using SmartHome.Core.Domain;
using SmartHome.Core.MessageHanding;

namespace SmartHome.Contracts.GenericRest
{
    public class Mappings : NodeDataMapperBase, INodeDataMapper
    {
        public Mappings()
        {
            // Use same properties as system - because this is generic handler
            base.ValidProperties = SystemMagnitudes.Properties;
        }
    }
}
