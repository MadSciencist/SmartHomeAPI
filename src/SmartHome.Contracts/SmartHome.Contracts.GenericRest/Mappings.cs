using SmartHome.Core.MessageHanding;
using System.Linq;

namespace SmartHome.Contracts.GenericRest
{
    public class Mappings : NodeDataMapperBase
    {
        protected override void InitializeMapping()
        {
            // All physical properties
            AddMappings(PhysicalPropertyService.GetAll().Result.Data.ToDictionary(x => x.Magnitude, x => x.Magnitude));
        }
    }
}
