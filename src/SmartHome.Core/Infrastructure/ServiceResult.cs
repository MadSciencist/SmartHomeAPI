using Newtonsoft.Json;
using System.Collections.Generic;

namespace SmartHome.Core.Infrastructure
{
    public class ServiceResult<T> where T : class, new()
    {
        [JsonProperty("data")]
        public T Data { get; set; }

        [JsonProperty("alerts")]
        public ICollection<Alert> Alerts { get; set; }

        [JsonProperty("metadata")]
        public ResultMetadata Metadata { get; set; }

        public ServiceResult()
        {
            Alerts = new List<Alert>();
            Metadata = new ResultMetadata();
        }

        public ServiceResult<T> HideExceptionMessages()
        {
            foreach (var alert in Alerts)
            {
                if (alert.MessageType == MessageType.Exception)
                {
                    alert.Message = "System error occured";
                }
            }

            return this;
        }
    }
}
