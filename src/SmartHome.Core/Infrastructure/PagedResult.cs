using Newtonsoft.Json;
using System.Collections.Generic;

namespace SmartHome.Core.Infrastructure
{
    public class PagedResult<T> : PagedResultBase where T : class
    {
        [JsonIgnore]
        public IList<T> Results { get; set; }

        public PagedResult()
        {
            Results = new List<T>();
        }
    }
}
