using Newtonsoft.Json;

namespace SmartHome.Core.Dto
{
    public class ExecuteNodeCommandJobDto
    { 
        [JsonProperty("jobTypeId")]
        public int JobTypeId { get; }

        [JsonProperty("nodeId")]
        public int NodeId { get; }

        [JsonProperty("cronExpression")]
        public string CronExpression { get; }

        public ExecuteNodeCommandJobDto(int jobTypeId, int nodeId, string cronExpression)
        {
            JobTypeId = jobTypeId;
            NodeId = nodeId;
            CronExpression = cronExpression;
        }
    }
}
