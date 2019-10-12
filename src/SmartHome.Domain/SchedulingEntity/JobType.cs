using SmartHome.Core.Entities.Entity;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SmartHome.Core.Entities.SchedulingEntity
{
    [Table("tbl_scheduling_job_type")]
    public class JobType : EntityBase
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.None)]
        public override int Id { get; set; }

        [Required, MaxLength(255)]
        public string FullyQualifiedName { get; set; }

        [Required, MaxLength(255)]
        public string AssemblyName { get; set; }

        [Required, MaxLength(255)]
        public string DisplayName { get; set; }

        public Type GetJobType()
        {
            return Type.GetType($"{FullyQualifiedName}, {AssemblyName}");
        }
    }
}
