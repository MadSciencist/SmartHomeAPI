using Newtonsoft.Json;
using System.Collections.Generic;
using System.Security.Claims;

namespace SmartHome.Core.Infrastructure
{
    public class ServiceResult<T> where T : class
    {
        [JsonProperty("data", NullValueHandling = NullValueHandling.Include)]
        public T Data { get; set; }

        [JsonProperty("alerts", NullValueHandling = NullValueHandling.Include)]
        public ICollection<Alert> Alerts { get; set; }

        [JsonProperty("metadata", NullValueHandling = NullValueHandling.Include)]
        public ResultMetadata Metadata { get; set; }

        [JsonIgnore]
        public ClaimsPrincipal Principal { get; set; }

        [JsonIgnore]
        public int? ResponseStatusCodeOverride { get; set; }


        #region ctors
        public ServiceResult(ClaimsPrincipal principal) : this()
        {
            Principal = principal;
        }

        public ServiceResult()
        {
            Alerts = new List<Alert>();
            Metadata = new ResultMetadata();
        }
        #endregion

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
