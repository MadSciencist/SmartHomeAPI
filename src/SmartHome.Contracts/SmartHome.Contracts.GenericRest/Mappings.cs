using SmartHome.Core.Entities;
using SmartHome.Core.MessageHanding;
using System.Linq;

namespace SmartHome.Contracts.GenericRest
{
    public class Mappings : NodeDataMapperBase
    {
        protected override void InitializeMapping()
        {
            // Use same properties as system - because this is generic handler
            base.Mapping = SystemMagnitudes.Properties.ToDictionary(x => x.Magnitude, x => x.Magnitude);
        }
    }
}
