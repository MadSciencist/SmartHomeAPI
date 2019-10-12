using Newtonsoft.Json;

namespace SmartHome.Core.Dto
{
    public class ScheduleNodeCommandJobDto
    {
        [JsonProperty("name")]
        public string Name { get; set; }

        [JsonProperty("jobTypeId")]
        public int JobTypeId { get; }

        [JsonProperty("nodeId")]
        public int NodeId { get; }

        [JsonProperty("cronExpression")]
        public string CronExpression { get; }

        [JsonProperty("command")]
        public string Command { get; }

        [JsonProperty("commandParams")]
        public object CommandParams { get; }

        public ScheduleNodeCommandJobDto(string name, string command, object commandParams, int jobTypeId, int nodeId, string cronExpression)
        {
            Name = name;
            Command = command;
            JobTypeId = jobTypeId;
            NodeId = nodeId;
            CronExpression = cronExpression;
        }
    }
}
