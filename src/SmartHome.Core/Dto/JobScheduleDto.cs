using System;

namespace SmartHome.Core.Dto
{
    public class JobScheduleDto
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string CronExpression { get; set; }
        public int JobStatusId { get; set; }
        public string JobStatus { get; set; }
        public int JobTypeId { get; set; }
        public string JobType { get; set; }
        public int CreatedById { get; set; }
        public object JobParams { get; set; }
        public string CreatedBy { get; set; }
        public DateTime Created { get; set; }
        public int? LastUpdatedById { get; set; }
        public string LastUpdatedBy { get; set; }
        public DateTime? LastUpdated { get; set; }
    }
}
