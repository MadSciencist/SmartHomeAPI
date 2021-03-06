﻿using Matty.Framework;
using SmartHome.Core.Entities.Enums;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SmartHome.Core.Entities.SchedulingEntity
{
    [Table("tbl_scheduling_job_status")]
    public class JobStatusEntity : EntityBase<int>
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.None)]
        public override int Id { get; set; }

        public string Name { get; set; }

        public JobStatus AsEnum() => Enum.Parse<JobStatus>(Id.ToString());
    }
}
