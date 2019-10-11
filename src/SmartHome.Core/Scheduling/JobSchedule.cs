using System;

namespace SmartHome.Core.Scheduling
{
    public class JobSchedule
    {
        public Type JobType { get; }
        public string CronExpression { get; }

        /// <summary>
        /// Represents single job
        /// </summary>
        /// <param name="type">Type of job executor</param>
        /// <param name="cronExpression">Cron expression</param>
        public JobSchedule(Type type, string cronExpression)
        {
            JobType = type;
            CronExpression = cronExpression;
        }

        public virtual string GetIdentity()
        {
            return JobType.FullName;
        }
    }
}
