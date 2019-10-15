using Newtonsoft.Json;
using Quartz;
using System;

namespace SmartHome.Core.Scheduling
{
    [JsonObject(MemberSerialization.Fields)]
    public class JobSchedule
    {
        public Type JobType { get; }
        public string CronExpression { get; }

        /// <summary>
        /// Represents single job.
        /// </summary>
        /// <param name="type">Type of job executor</param>
        /// <param name="cronExpression">Cron expression</param>
        public JobSchedule(Type type, string cronExpression)
        {
            JobType = type;
            CronExpression = cronExpression;
        }

        /// <summary>
        /// Gets identifier to uniquely identify the job.
        /// </summary>
        /// <returns></returns>
        public virtual string GetIdentity()
        {
            return JobType.FullName;
        }

        /// <summary>
        /// Parameter data which will be passed into job executor
        /// </summary>
        /// <returns></returns>
        public virtual JobDataMap GetJobData()
        {
            return null;
        }

        /// <summary>
        /// Creates new job with given identity.
        /// </summary>
        /// <returns></returns>
        public virtual IJobDetail CreateJob()
        {
            return JobBuilder
                .Create(JobType)
                .WithIdentity(GetIdentity())
                .WithDescription(GetIdentity())
                .SetJobData(GetJobData())
                .Build();
        }

        /// <summary>
        /// Creates new trigger with given identity and provided cron expression
        /// </summary>
        /// <returns></returns>
        public virtual ITrigger CreateTrigger()
        {
            return TriggerBuilder
                .Create()
                .WithIdentity($"{GetIdentity()}.trigger")
                .WithCronSchedule(CronExpression)
                .WithDescription($"{GetIdentity()}.trigger")
                .Build();
        }
    }
}
