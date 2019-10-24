using System;
using System.Collections.Generic;

namespace SmartHome.Core.Dto
{
    public class ControlStrategyDto
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Connector { get; set; }
        public string Description { get; set; }
        public bool IsActive { get; set; }
        public IEnumerable<PhysicalPropertyDto> PhysicalProperties { get; set; }
        public int CreatedById { get; set; }
        public string CreatedBy { get; set; }
        public DateTime Created { get; set; }
        public int? LastUpdatedById { get; set; }
        public string LastUpdatedBy { get; set; }
        public DateTime? LastUpdated { get; set; }
    }
}
