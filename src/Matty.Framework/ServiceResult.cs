using Matty.Framework.Enums;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.Security.Claims;

namespace Matty.Framework
{
    /// <summary>
    /// Container class for business logic response
    /// </summary>
    /// <typeparam name="T">Type of Data property.</typeparam>
    public class ServiceResult<T>
    {
        [JsonProperty("data", NullValueHandling = NullValueHandling.Include)]
        public T Data { get; set; }

        [JsonProperty("alerts", NullValueHandling = NullValueHandling.Include)]
        public List<Alert> Alerts { get; private set; }

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

        public ServiceResult(List<Alert> alerts)
        {
            Alerts = alerts;
            Metadata = new ResultMetadata();
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

        public void AddAlert(string message, MessageType type) => Alerts.Add(new Alert(message, type));

        public void AddAlert(Alert alert) => Alerts.Add(alert);

        public void AddAlerts(IEnumerable<Alert> alerts) => Alerts.AddRange(alerts);

        public void AddSuccessMessage(string message) => AddAlert(message, MessageType.Success);

        public void AddErrorMessage(string message) => AddAlert(message, MessageType.Error);
    }
}
