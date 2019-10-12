using SmartHome.Core.Entities.Entity;
using SmartHome.Core.Entities.User;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SmartHome.Core.Entities.SchedulingEntity
{
    [Table("tbl_scheduling_schedules")]
    public class SchedulesPersistence : EntityBase
    {
        [MaxLength(255)]
        public string Name { get; set; }

        [Required]
        public JobType JobType { get; set; }
        public int JobTypeId { get; set; }

        [Required, MinLength(8), MaxLength(20)]
        public string CronExpression { get; set; }

        public AppUser CreatedBy { get; set; }

        [Required]
        public int CreatedById { get; set; }

        [Required]
        public DateTime Created { get; set; }

        /// <summary>
        /// List of semicolon separated key-value pairs
        /// </summary>
        public string JobParams { get; set; }

        public override string ToString()
        {
            return $"Name: {Name} JobType: {JobType.DisplayName} Cron: {CronExpression}";
        }
    }
}
