using Matty.Framework;
using Matty.Framework.Abstractions;
using SmartHome.Core.Entities.User;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SmartHome.Core.Entities.SchedulingEntity
{
    [Table("tbl_scheduling_schedules")]
    public class ScheduleEntity : EntityBase<int>, ICreationAudit<AppUser, int>, IModificationAudit<AppUser, int?>
    {
        /// <summary>
        /// User display name
        /// </summary>
        [MaxLength(255)]
        public string Name { get; set; }

        [Required]
        public string JobName { get; set; }
        public string JobGroup { get; set; }

        /// <summary>
        /// Job status entity IDs are matching with JobStatus enum
        /// </summary>
        [Required]
        public int JobStatusEntityId { get; set; }
        public JobStatusEntity JobStatusEntity { get; set; }

        [Required]
        public JobType JobType { get; set; }
        public int JobTypeId { get; set; }

        [Required]
        public ScheduleType ScheduleType { get; set; }
        public int ScheduleTypeid { get; set; }
        public string SerializedJobSchedule { get; set; }

        [Required, MinLength(8), MaxLength(20)]
        public string CronExpression { get; set; }

        #region ICreationAudit impl
        public int CreatedById { get; set; }
        public AppUser CreatedBy { get; set; }
        public DateTime Created { get; set; }
        #endregion

        #region IModificationAudit impl
        public int? UpdatedById { get; set; }
        public AppUser UpdatedBy { get; set; }
        public DateTime? Updated { get; set; }
        #endregion

        public override string ToString()
        {
            return $"Name: {Name} JobType: {JobType.DisplayName} Cron: {CronExpression}";
        }
    }
}
