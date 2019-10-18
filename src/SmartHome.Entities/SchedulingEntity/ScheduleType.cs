using Matty.Framework;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SmartHome.Core.Entities.SchedulingEntity
{
    [Table("tbl_scheduling_schedule_type")]
    public class ScheduleType : EntityBase<int>
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.None)]
        public override int Id { get; set; }

        [Required, MaxLength(255)]
        public string FullyQualifiedName { get; set; }

        [Required, MaxLength(255)]
        public string AssemblyName { get; set; }

        [Required, MaxLength(255)]
        public string DisplayName { get; set; }

        public Type GetScheduleType()
        {
            return Type.GetType($"{FullyQualifiedName}, {AssemblyName}");
        }
    }
}
