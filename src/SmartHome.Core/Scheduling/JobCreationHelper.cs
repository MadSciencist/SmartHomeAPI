using Quartz;

namespace SmartHome.Core.Scheduling
{
    public class JobCreationHelper
    {
        public static IJobDetail CreateJob(JobSchedule schedule)
        {
            return JobBuilder
                .Create(schedule.JobType)
                .WithIdentity(schedule.GetIdentity())
                .WithDescription(schedule.GetIdentity())
                .Build();
        }

        public static ITrigger CreateTrigger(JobSchedule schedule)
        {
            return TriggerBuilder
                .Create()
                .WithIdentity($"{schedule.GetIdentity()}.trigger")
                .WithCronSchedule(schedule.CronExpression)
                .WithDescription($"{schedule.GetIdentity()}.trigger")
                .Build();
        }
    }
}
